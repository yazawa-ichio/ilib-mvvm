using System.Collections;

namespace ILib.MVVM
{
	public class ReactiveProperty<T> : System.IDisposable
	{
		public readonly string Path;
		IViewModel m_VM;

		public T Value
		{
			get { return m_VM.Get<T>(Path); }
			set { m_VM.Set<T>(Path, value); }
		}

		public event System.Action<T> OnChanged;

		public ReactiveProperty(string path, IViewModel vm)
		{
			Path = path;
			m_VM = vm;
			m_VM.Property.Subscribe<T>(Path, OnNotifyChanged);
		}

		public ReactiveProperty(string path, IViewModel vm, T val)
		{
			Path = path;
			m_VM = vm;
			Value = val;
			m_VM.Property.Subscribe<T>(Path, OnNotifyChanged);
		}

		void OnNotifyChanged(T val)
		{
			OnChanged?.Invoke(val);
		}

		public void Dispose()
		{
			OnChanged = null;
			m_VM.Property.Unsubscribe<T>(Path, OnNotifyChanged);
		}

		public void SetDirty()
		{
			m_VM.SetDirty(Path);
		}

		public static explicit operator T(ReactiveProperty<T> property)
		{
			return property.Value;
		}

	}

}