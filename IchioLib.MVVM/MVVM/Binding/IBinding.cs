namespace ILib.MVVM
{
	public interface IBinding : IViewElement
	{
		void TryUpdate();
		void Bind(string path, IBindingProperty prop);
		void Unbind(string path, IBindingProperty prop);
		void Bind(IViewEventHandler handler);
		void Unbind(IViewEventHandler handler);
	}
}
