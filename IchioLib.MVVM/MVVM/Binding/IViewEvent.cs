namespace ILib.MVVM
{
	public interface IViewEvent : IViewElement
	{
		string Name { get; }
		System.Type EventType();
		void Bind(IViewEventHandler handler);
	}
}
