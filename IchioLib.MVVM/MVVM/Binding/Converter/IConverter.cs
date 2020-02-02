namespace ILib.MVVM
{
	public interface IConverter
	{
		System.Type GetTargetType();
		void TryUpdate();
		bool TryConvert<T>(IBindingProperty property, ref IBindingProperty<T> output);
		bool TryConvert(IBindingProperty property, ref IBindingProperty output);
		bool Unbind(IBindingProperty property);
	}
}