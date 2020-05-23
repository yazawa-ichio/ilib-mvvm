namespace ILib.MVVM
{
	public interface IBindingProperty
	{
		string Path { get; }
		int Hash { get; }
		T GetValue<T>() where T : class;
		void SetValue<T>(object val) where T : class;
		System.Type GetBindType();
		bool IsAssignable<T>() where T : class;
		void SetDirty();
	}

	public interface IBindingProperty<T> : IBindingProperty
	{
		T Value { get; set; }
		event System.Action<T> OnChanged;
	}
}