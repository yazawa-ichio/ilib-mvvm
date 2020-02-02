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
		IConverter m_Converter;
		bool m_CheckConverter;

		public System.Type BindType()
		{
			return GetConverter()?.GetTargetType() ?? typeof(IViewModel);
		}

		public IConverter GetConverter()
		{
			if (m_CheckConverter) return m_Converter;
			m_CheckConverter = true;
			return m_Converter = GetComponent<IConverter>();
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			IBindingProperty ret = null;
			if (m_Converter.TryConvert(prop, ref ret) && ret.IsAssignable<IViewModel>())
			{
				m_Property = ret;
				return;
			}
			if (prop.IsAssignable<IViewModel>())
			{
				m_Property = prop;
			}
		}

		void IBindable.Unbind(IBindingProperty prop)
		{
			if (m_Converter != null && m_Converter.Unbind(prop))
			{
				m_Property = null;
				OnUnbind();
			}
			if (m_Property == prop)
			{
				m_Property = null;
				OnUnbind();
			}
		}

		protected virtual void OnUnbind() { }

		void IBindable.TryUpdate()
		{
			m_Converter?.TryUpdate();
			if (m_Property == null || m_Property.Hash == m_Hash) return;
			m_Hash = m_Property.Hash;
			Attach(m_Property.GetValue<IViewModel>());
		}

	}

}
