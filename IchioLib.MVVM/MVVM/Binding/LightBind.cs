using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ILib.MVVM
{
	public interface ILightBind
	{
		void Init(Object obj, string path);
	}

	public abstract class LightBind<TValue, UTarget> : ILightBind, IBindable<TValue> where UTarget : Object
	{
		public string Path { get; private set; }
		public virtual bool IsActive => m_Target != null;

		IBindingProperty<TValue> m_Property;
		protected UTarget m_Target;
		protected int m_Hash;

		protected TValue Value
		{
			get
			{
				if (m_Property != null) return m_Property.Value;
				return default;
			}
		}

		public Type BindType()
		{
			return typeof(TValue);
		}

		bool m_UpdateLock = false;

		protected void Set(TValue val, bool update = false)
		{
			if (m_UpdateLock) return;
			if (m_Property != null)
			{
				m_Property.Value = val;
				if (update)
				{
					m_Hash = m_Property.Hash;
					m_UpdateLock = true;
					UpdateValue(val);
					m_UpdateLock = false;
				}
			}
		}

		public void Init(Object obj, string path)
		{
			m_Target = (UTarget)obj;
			Path = path;
			OnInit();
		}

		protected virtual void OnInit() { }

		void IBindable.Bind(IBindingProperty prop)
		{
			if (prop is IBindingProperty<TValue>)
			{
				m_Property = (IBindingProperty<TValue>)prop;
			}
		}

		void IBindable<TValue>.Bind(IBindingProperty<TValue> prop)
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

		protected abstract void UpdateValue(TValue val);


	}

	public abstract class LightEventBind<TValue, UTarget> : LightBind<TValue, UTarget>, IViewEvent where UTarget : Object
	{
		string IViewEvent.Name => Path;

		IViewEventHandler m_Handler;

		void IViewEvent.Bind(IViewEventHandler handler)
		{
			m_Handler = handler;
		}

		public virtual Type EventType()
		{
			return typeof(TValue);
		}

		protected void Event() => Event(Path);

		protected void Event(string name)
		{
			m_Handler?.OnViewEvent(name);
		}

		protected void Event(TValue val) => Event(Path, val);

		protected void Event<T>(string name, T val)
		{
			m_Handler?.OnViewEvent(name, val);
		}

		protected void Event(EventArgument argument)
		{
			argument.Do(Path, m_Handler);
		}

	}

}
