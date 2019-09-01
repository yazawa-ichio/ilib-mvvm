using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{
	public abstract class BindBase<T> : MonoBehaviour, IBindable<T>
	{
		[SerializeField]
		string m_Path = null;
		public string Path => m_Path;
		public virtual bool IsActive => this != null;

		IBindingProperty<T> m_Property;
		protected int m_Hash;

		bool m_UpdateLock = false;

		protected void Set(T val, bool update = false)
		{
			if (m_UpdateLock) return;
			if (m_Property != null)
			{
				m_Property.Value = val;
				if (update)
				{
					m_Hash = m_Property.Hash;
					m_UpdateLock = true;
					try
					{
						UpdateValue(val);
					}
					finally
					{
						m_UpdateLock = false;
					}
				}
			}
		}

		public System.Type BindType()
		{
			return typeof(T);
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			if (prop is IBindingProperty<T>)
			{
				m_Property = (IBindingProperty<T>)prop;
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
			UpdateValue(m_Property.Value);
			return;
		}

		protected abstract void UpdateValue(T val);
	}


}
