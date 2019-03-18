using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	public class BindableView : View, IBindable<IViewModel>
	{
		[SerializeField]
		string m_Path = "";

		string IBindable.Path => m_Path;

		bool IBindable.IsActive => this != null;

		IBindingProperty m_Property;
		protected int m_Hash;

		public System.Type BindType()
		{
			return typeof(IViewModel);
		}

		void IBindable<IViewModel>.Bind(IBindingProperty<IViewModel> prop)
		{
			m_Property = prop;
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			if (prop.IsAssignable<IViewModel>())
			{
				m_Property = prop;
			}
		}

		void IBindable.Unbind(IBindingProperty prop)
		{
			if (m_Property == prop)
			{
				m_Property = null;
				OnUnbind();
			}
		}

		protected virtual void OnUnbind() { }

		void IBindable.TryUpdate()
		{
			if (m_Property == null || m_Property.Hash == m_Hash) return;
			m_Hash = m_Property.Hash;
			Attach(m_Property.GetValue<IViewModel>());
		}

	}

}
