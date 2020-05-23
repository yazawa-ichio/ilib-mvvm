using System;

namespace ILib.MVVM
{
	public class ConvertProperty<TValue> : IBindingProperty<TValue>
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

		public string Path { get; private set; }

		public int Hash { get; private set; }

		public event Action<TValue> OnChanged;

		public ConvertProperty(string path)
		{
			Path = path;
			Hash = 1;
			m_Value = default;
		}

		public void ConvertApply(TValue value)
		{
			m_Value = value;
			Hash++;
		}

		public Type GetBindType()
		{
			return typeof(TValue);
		}

		public bool IsAssignable<T>() where T : class
		{
			return typeof(T).IsAssignableFrom(typeof(TValue));
		}

		public T GetValue<T>() where T : class
		{
			return m_Value as T;
		}

		public void SetValue<T>(object val) where T : class
		{
			Value = (TValue)val;
		}

		public override string ToString()
		{
			return $"{m_Value}";
		}

		public void SetDirty()
		{
			Hash++;
		}

	}
}