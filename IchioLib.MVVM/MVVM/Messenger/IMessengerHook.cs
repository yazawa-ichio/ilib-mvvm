namespace ILib.MVVM
{
	public interface IMessengerHook
	{
		void OnSend(string name);
		void OnSend<TEventName>(TEventName name);
		void OnSend<TMessage>(string name, TMessage args);
		void OnSend<TEventName, UMessage>(TEventName name, UMessage args);
	}
}

