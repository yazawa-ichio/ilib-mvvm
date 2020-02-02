using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		IViewElement[] m_Elements;
		IViewElement[] IView.Elements => m_Elements;

		public IViewModel DataContext => m_ViewModel;

		private void Awake()
		{
			Prepare();
			Init();
		}

		private void OnDestroy()
		{
			m_Binding?.Dispose();
			m_Binding = null;
			OnDestroyImpl();
		}

		public void Prepare(bool force = false)
		{
			if (m_Prepare && !force) return;
			m_Prepare = true;
			List<IViewElement> elements = new List<IViewElement>();
			GetComponentsInChildren(true, elements);
			if (m_LightBinds != null)
			{
				elements.AddRange(m_LightBinds.Select(x => x.Resolve()));
			}
			foreach (var view in GetComponentsInChildren<IView>(true))
			{
				var viewComponent = view as View;
				if (viewComponent == this) continue;
				view.Prepare();
				if (view.Elements == null) continue;
				foreach (var remove in view.Elements)
				{
					elements.Remove(remove);
				}
			}
			m_Elements = elements.ToArray();
		}

		private void Update()
		{
			TryUpdate();
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

	}


}
