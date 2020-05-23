namespace ILib.MVVM
{
	public interface IViewEventDispatcher : IViewElement
	{
		void Dispatch(string name);
		void Dispatch<T>(string name, T args);
	}
}