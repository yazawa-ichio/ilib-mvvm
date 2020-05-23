using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	public class View : MonoBehaviour, IView
	{
		[SerializeField]
		LightBindHolder[] m_LightBinds = null;

		[System.NonSerialized]
		bool m_Prepare;

		IViewModel m_ViewModel;
		Binding m_Binding = new Binding();
		internal IViewElement[] m_Elements;
		bool IView.IsActive => IsActive;
		bool m_IsActive = true;
		protected bool IsActive => m_IsActive && gameObject != null && this != null;

		public IViewModel DataContext => m_ViewModel;

		IViewElement[] IView.Elements => m_Elements;

		private void Awake()
		{
			ViewUpdater.Register(this);
			Prepare();
			Init();
		}

		private void OnDestroy()
		{
			m_IsActive = false;
			m_Binding?.Dispose();
			m_Binding = null;
			foreach (var lightBind in m_LightBinds)
			{
				lightBind.Dispose();
			}
			OnDestroyImpl();
		}

		public void Prepare(bool force = false)
		{
			if (m_Prepare && !force) return;
			m_Prepare = true;

			using (var elementsScope = ViewUtil.UseElementList())
			{
				var elements = elementsScope.List;
				GetComponentsInChildren(true, elements);
				if (m_LightBinds != null)
				{
					foreach (var lightBind in m_LightBinds)
					{
						elements.Add(lightBind.Resolve());
					}
				}
				using (var viewListScope = ViewUtil.UseViewList())
				{
					var viewList = viewListScope.List;
					GetComponentsInChildren(true, viewList);
					foreach (var view in viewList)
					{
						var viewComponent = view as View;
						if (viewComponent == this) continue;
						view.Prepare();
						using (var childElementsScope = ViewUtil.UseElementList())
						{
							var childElements = childElementsScope.List;
							view.GetElements(childElements);
							foreach (var remove in childElements)
							{
								elements.Remove(remove);
							}
						}
					}
				}
				m_Elements = elements.ToArray();
			}
		}

		protected virtual void OnDestroyImpl() { }

		public virtual void TryUpdate()
		{
			m_Binding.TryUpdate();
		}

		private void Init()
		{
			m_Binding.Init(m_Elements);
		}

		public T Attach<T>(System.Action<T> action) where T : IViewModel, new()
		{
			T vm = new T();
			action?.Invoke(vm);
			Attach(vm);
			return vm;
		}

		public void Attach(IViewModel vm)
		{
			if (m_ViewModel != null)
			{
				m_Binding.Unbind(m_ViewModel);
			}
			m_ViewModel = vm;
			m_Binding.Bind(vm);
		}

		public void GetElements(List<IViewElement> elements)
		{
			if (!m_Prepare)
			{
				Prepare();
			}
			if (m_Elements == null) return;
			foreach (var element in m_Elements)
			{
				elements.Add(element);
			}
		}

	}


}