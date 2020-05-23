using System;
using System.Collections.Generic;
using System.Linq;

namespace ILib.MVVM
{

	public class Binding : IDisposable, IMultipleBindable, IViewEventDispatcher
	{
		IViewElement[] m_Elements;
		IBindable[] m_Binding = Array.Empty<IBindable>();
		int m_BindingCount = 0;
		List<IViewModel> m_ViewModels = new List<IViewModel>();
		bool m_Disposed;

		void IMultipleBindable.OnPrepare() { }

		IEnumerable<IBindable> GetBindables()
		{
			if (m_Elements != null)
			{
				foreach (var element in m_Elements)
				{
					var bind = element as IBindable;
					if (bind != null)
					{
						yield return bind;
					}
					var multipleBind = element as IMultipleBindable;
					if (multipleBind != null)
					{
						multipleBind.OnPrepare();
						foreach (var multipleBindItem in multipleBind.GetBindables())
						{
							yield return multipleBindItem;
						}
					}
				}
			}
		}

		IEnumerable<IBindable> IMultipleBindable.GetBindables() => GetBindables();

		public void Init(IViewElement[] elements)
		{
			m_Elements = elements;
			foreach (var elm in elements)
			{
				//イベントであれば登録
				(elm as IViewEvent)?.Bind(this);
			}
			m_Binding = GetBindables().ToArray();
			m_BindingCount = m_Binding.Length;
		}

		public void TryUpdate()
		{
			if (m_Disposed) return;
			for (int i = 0; i < m_BindingCount; i++)
			{
				IBindable bind = m_Binding[i];
				if (bind.IsActive)
				{
					bind.TryUpdate();
				}
				else
				{
					//左詰めする
					LeftShiftBindingArray(i);
				}
			}
		}

		void LeftShiftBindingArray(int index)
		{
			m_BindingCount--;
			for (int i = index; i < m_BindingCount; i++)
			{
				m_Binding[i] = m_Binding[i + 1];
			}
			m_Binding[m_BindingCount - 1] = null;
		}

		public void Bind(IViewModel model)
		{
			if (m_Disposed) return;
			m_ViewModels.Add(model);
			using (var scope = ViewUtil.UseBindingPropertyListStack())
			{
				foreach (var prop in model.Property.GetAll(scope.List))
				{
					Bind(prop.Path, prop);
				}
			}
			model.Property.OnNewProperty += Bind;
		}

		public void Unbind(IViewModel model)
		{
			if (m_Disposed) return;
			m_ViewModels.Remove(model);
			using (var scope = ViewUtil.UseBindingPropertyListStack())
			{
				foreach (var prop in model.Property.GetAll(scope.List))
				{
					Unbind(prop.Path, prop);
				}
			}
			model.Property.OnNewProperty -= Bind;
		}

		void Bind(string path, IBindingProperty prop)
		{
			if (m_Disposed) return;
			for (int i = 0; i < m_BindingCount; i++)
			{
				IBindable bind = m_Binding[i];
				if (bind.Path == path && bind.IsActive)
				{
					bind.Bind(prop);
				}
			}
		}

		void Unbind(string path, IBindingProperty prop)
		{
			if (m_Disposed) return;
			for (int i = 0; i < m_BindingCount; i++)
			{
				IBindable bind = m_Binding[i];
				if (bind.Path == path && bind.IsActive)
				{
					bind.Unbind(prop);
				}
			}
		}

		void IViewEventDispatcher.Dispatch(string name)
		{
			if (m_Disposed) return;
			foreach (var vm in m_ViewModels)
			{
				vm.Event.Publish(name);
			}
		}

		void IViewEventDispatcher.Dispatch<T>(string name, T args)
		{
			if (m_Disposed) return;
			foreach (var vm in m_ViewModels)
			{
				vm.Event.Publish(name, args);
			}
		}

		public void Dispose()
		{
			m_Disposed = true;
			for (int i = m_ViewModels.Count - 1; i >= 0; i--)
			{
				Unbind(m_ViewModels[i]);
			}
		}

	}

}