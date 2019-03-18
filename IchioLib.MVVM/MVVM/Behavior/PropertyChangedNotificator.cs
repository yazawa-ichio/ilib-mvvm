using System;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace ILib.MVVM.Behaviors
{
	public class PropertyChangedNotificator : IBindable
	{
		IBindingProperty m_Property;
		public string Path { get; private set; }
		public bool IsActive => m_ActiveCheck();
		public
		int m_Hash;
		Func<bool> m_ActiveCheck;
		public event Action OnChanged;

		public PropertyChangedNotificator(string path, object owner)
		{
			Path = path;
			var reference = new WeakReference<object>(owner);
			m_ActiveCheck = () =>
			{
				object obj;
				if (reference.TryGetTarget(out obj) && obj != null)
				{
					return true;
				}
				return false;
			};
		}

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
			return typeof(object);
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

	}


	public class PropertyChangedNotificator<T> : IBindable<T>
	{
		IBindingProperty<T> m_Property;
		public string Path { get; private set; }
		public bool IsActive => m_ActiveCheck();
		public
		int m_Hash;
		Func<bool> m_ActiveCheck;
		public event Action<T> OnChanged;

		public PropertyChangedNotificator(string path, object owner)
		{
			Path = path;
			var reference = new WeakReference<object>(owner);
			m_ActiveCheck = () =>
			{
				object obj;
				if (reference.TryGetTarget(out obj) && obj != null)
				{
					return true;
				}
				return false;
			};
		}

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

		void IBindable<T>.Bind(IBindingProperty<T> prop)
		{
			m_Property = prop;
		}

		void IBindable.Bind(IBindingProperty prop)
		{
			if (prop is IBindingProperty<T>)
			{
				m_Property = (IBindingProperty<T>)prop;
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

	}



}
