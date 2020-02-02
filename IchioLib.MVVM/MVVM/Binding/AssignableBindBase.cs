using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{
	public abstract class AssignableBindBase<T> : MonoBehaviour, IBindable<T> where T : class
	{
		[SerializeField]
		string m_Path = null;
		public string Path => m_Path;
		public virtual bool IsActive => this != null;

		IBindingProperty m_Property;
		protected int m_Hash;
		protected bool m_ForceUpdate;
		IConverter m_Converter;
		bool m_CheckConverter;

		protected void Set(T val)
		{
			if (m_Property != null)
			{
				m_Property.SetValue<T>(val);
			}
		}

		public System.Type BindType()
		{
			return GetConverter()?.GetTargetType() ?? typeof(T);
		}

		public IConverter GetConverter()
		{
			if (m_CheckConverter) return m_Converter;
			m_CheckConverter = true;
			return m_Converter = GetComponent<IConverter>();
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			var conv = GetConverter();
			IBindingProperty ret = null;
			if (conv != null && conv.TryConvert(prop, ref ret) && ret.IsAssignable<T>())
			{
				m_Property = ret;
				m_ForceUpdate = true;
				return;
			}
			if (prop.IsAssignable<T>())
			{
				m_Property = prop;
				m_ForceUpdate = true;
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
			TryUpdate();
		}

		void TryUpdate()
		{
			m_Converter?.TryUpdate();
			if (m_Property == null || (!m_ForceUpdate && m_Property.Hash == m_Hash))
			{
				return;
			}
			m_ForceUpdate = false;
			m_Hash = m_Property.Hash;
			UpdateValue(m_Property.GetValue<T>());
		}

		protected abstract void UpdateValue(T val);
	}


}
