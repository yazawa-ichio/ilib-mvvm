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

		bool m_UpdateLock = false;

		protected void Set(T val, bool update = false)
		{
			if (m_UpdateLock) return;
			if (m_Property != null)
			{
				m_Property.SetValue<T>(val);
				if (update)
				{
					m_Hash = m_Property.Hash;
					m_UpdateLock = true;
					UpdateValue(val);
					m_UpdateLock = false;
				}
			}
		}

		public System.Type BindType()
		{
			return typeof(T);
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			if (prop.IsAssignable<T>())
			{
				m_Property = prop;
			}
		}

		void IBindable<T>.Bind(IBindingProperty<T> prop)
		{
			m_Property = prop;
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
			UpdateValue(m_Property.GetValue<T>());
			return;
		}

		protected abstract void UpdateValue(T val);
	}


}
