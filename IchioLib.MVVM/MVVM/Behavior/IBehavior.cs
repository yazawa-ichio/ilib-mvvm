namespace ILib.MVVM
{
	public interface IBehavior : IMultipleBindable
	{
		string MessengerPath { get; }
		IMessenger Messenger { get; }
	}
}