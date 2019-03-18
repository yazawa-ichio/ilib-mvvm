namespace ILib.MVVM
{
	public interface IMessenger
	{
		void Register<T>(object recipient, string name, System.Action<T> action);
		void Unregister<T>(object recipient, string name, System.Action<T> action);
		void Unregister(object recipient);
		void Send<T>(string name, T message);
		void Refresh();
	}
}
