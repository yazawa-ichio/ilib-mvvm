using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ILib.MVVM
{
	[UnityEngine.Scripting.Preserve]
	public interface ILightBind
	{
		void Init(Object obj, string path);
		bool OnReturn();
	}

	[UnityEngine.Scripting.Preserve]
	public abstract class LightBind<TValue, UTarget> : ILightBind, IBindable<TValue> where UTarget : Object
	{
		public string Path { get; private set; }
		public virtual bool IsActive => m_Target != null;

		IBindingProperty<TValue> m_Property;
		protected UTarget m_Target;
		protected int m_Hash;
		protected bool m_ForceUpdate;
		protected IConverter m_Converter;

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
			return GetConverter()?.GetTargetType() ?? typeof(TValue);
		}

		protected void Set(TValue val)
		{
			if (m_Property != null)
			{
				m_Property.Value = val;
			}
		}

		public void Init(Object obj, string path)
		{
			m_Target = (UTarget)obj;
			m_Converter = (m_Target as Component)?.GetComponent<IConverter>() ?? null;
			Path = path;
			OnInit();
		}

		public virtual bool OnReturn()
		{
			Path = default;
			m_Property = default;
			m_Target = default;
			m_Hash = default;
			m_ForceUpdate = default;
			m_Converter = default;
			return true;
		}

		public IConverter GetConverter()
		{
			return m_Converter;
		}

		protected virtual void OnInit() { }

		void IBindable.Bind(IBindingProperty prop)
		{
			if (m_Converter != null && m_Converter.TryConvert(prop, ref m_Property))
			{
				m_ForceUpdate = true;
				return;
			}
			if (prop is IBindingProperty<TValue>)
			{
				m_Property = (IBindingProperty<TValue>)prop;
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

		protected void TryUpdate()
		{
			m_Converter?.TryUpdate();
			if (m_Property == null || (!m_ForceUpdate && m_Property.Hash == m_Hash))
			{
				return;
			}
			m_ForceUpdate = false;
			m_Hash = m_Property.Hash;
			UpdateValue(m_Property.Value);
		}

		protected abstract void UpdateValue(TValue val);

	}

	[UnityEngine.Scripting.Preserve]
	public abstract class LightEventBind<TValue, UTarget> : LightBind<TValue, UTarget>, IViewEvent where UTarget : Object
	{
		string IViewEvent.Name => Path;

		IViewEventDispatcher m_Handler;

		void IViewEvent.Bind(IViewEventDispatcher handler)
		{
			m_Handler = handler;
		}

		public override bool OnReturn()
		{
			m_Handler = null;
			return base.OnReturn();
		}

		public virtual Type EventType()
		{
			return typeof(TValue);
		}

		protected void Event() => Event(Path);

		protected void Event(string name)
		{
			m_Handler?.Dispatch(name);
		}

		protected void Event(TValue val) => Event(Path, val);

		protected void Event<T>(string name, T val)
		{
			m_Handler?.Dispatch(name, val);
		}

		protected void Event(EventArgument argument)
		{
			argument.Do(Path, m_Handler);
		}

		public virtual EventArgument GetArgument() { return null; }
	}

}