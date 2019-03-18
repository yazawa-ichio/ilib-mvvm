namespace ILib.MVVM
{
	public interface IViewEventHandler : IViewElement
	{
		void OnViewEvent(string name);
		void OnViewEvent<T>(string name, T args);
	}
}
