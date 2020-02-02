using System;
using System.Collections;
using System.Collections.Generic;

namespace ILib.MVVM
{
	internal abstract class BindingProperty : IBindingProperty
	{
		public string Path { get; private set; }
		public int Hash { get; protected set; }
		internal BindingProperty Next;
		public BindingProperty(string path)
		{
			Path = path;
			Hash = 1;
		}

		public abstract Type GetBindType();
		public abstract bool IsAssignable<T>() where T : class;
		public abstract T GetValue<T>() where T : class;
		public abstract void SetValue<T>(object val) where T : class;

		public void SetDirty()
		{
			Hash++;
		}

	}

	internal class BindingProperty<TValue> : BindingProperty, IBindingProperty<TValue>
	{
		TValue m_Value;
		public TValue Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
				Hash++;
				OnChanged?.Invoke(value);
			}
		}

		public BindingProperty(string path) : base(path) { m_Value = default; }

		public event Action<TValue> OnChanged;

		public override Type GetBindType()
		{
			return typeof(TValue);
		}

		public override bool IsAssignable<T>()
		{
			return typeof(T).IsAssignableFrom(typeof(TValue));
		}

		public override T GetValue<T>()
		{
			return m_Value as T;
		}

		public override void SetValue<T>(object val)
		{
			Value = (TValue)val;
		}

		public override string ToString()
		{
			return $"{m_Value}";
		}

	}
}
