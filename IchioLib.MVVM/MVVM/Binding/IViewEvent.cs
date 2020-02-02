namespace ILib.MVVM
{
	public interface IViewEvent : IViewElement
	{
		string Name { get; }
		System.Type EventType();
		void Bind(IViewEventDispatcher handler);
		EventArgument GetArgument();
	}
}
