namespace ILib.MVVM
{
	public interface IView
	{
		IViewElement[] Elements { get; }
		IViewModel DataContext { get; }
		void Prepare(bool force = false);
		void Attach(IViewModel vm);
	}
}
