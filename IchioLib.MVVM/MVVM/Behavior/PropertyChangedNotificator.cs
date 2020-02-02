using System;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace ILib.MVVM.Behaviors
{
	public class PropertyChangedNotificator : IBindable, IDisposable
	{
		IBindingProperty m_Property;
		public string Path { get; private set; }
		public bool IsActive => m_ActiveCheck?.Invoke() ?? false;
		public object Value => m_Property != null ? m_Property.GetValue<object>() : null;
		int m_Hash;
		Func<bool> m_ActiveCheck;
		public event Action OnChanged;

		public PropertyChangedNotificator(string path, Object owner)
		{
			Path = path;
			m_ActiveCheck = () => owner != null;
		}

		public PropertyChangedNotificator(string path, Func<bool> activeCheck)
		{
			Path = path;
			m_ActiveCheck = activeCheck;
		}

		public Type BindType()
		{
			return null;
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			m_Property = prop;
		}

		void IBindable.Unbind(IBindingProperty prop)
		{
			if (m_Property == prop) m_Property = null;
		}

		void IBindable.TryUpdate()
		{
			if (m_Property == null || m_Property.Hash == m_Hash) return;
			m_Hash = m_Property.Hash;
			OnChanged?.Invoke();
			return;
		}

		public IConverter GetConverter()
		{
			return null;
		}

		public void Dispose()
		{
			m_ActiveCheck = null;
		}
	}


	public class PropertyChangedNotificator<T> : IBindable<T>, IDisposable
	{
		IBindingProperty<T> m_Property;
		public string Path { get; private set; }
		public bool IsActive => m_ActiveCheck?.Invoke() ??  false;
		public T Value => m_Property != null ? m_Property.Value : default;
		public
		int m_Hash;
		Func<bool> m_ActiveCheck;
		public event Action<T> OnChanged;

		public PropertyChangedNotificator(string path, Object owner)
		{
			Path = path;
			m_ActiveCheck = () => owner != null;
		}

		public PropertyChangedNotificator(string path, Func<bool> activeCheck)
		{
			Path = path;
			m_ActiveCheck = activeCheck;
		}

		public Type BindType()
		{
			return typeof(T);
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			if (prop is IBindingProperty<T> ret)
			{
				m_Property = ret;
			}
		}

		void IBindable.Unbind(IBindingProperty prop)
		{
			if (m_Property == prop)
			{
				m_Property = null;
			}
		}

		void IBindable.TryUpdate()
		{
			if (m_Property == null || m_Property.Hash == m_Hash) return;
			m_Hash = m_Property.Hash;
			OnChanged?.Invoke(m_Property.Value);
			return;
		}

		public IConverter GetConverter()
		{
			return null;
		}

		public void Dispose()
		{
			m_ActiveCheck = null;
		}

	}



}
