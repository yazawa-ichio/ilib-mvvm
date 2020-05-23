namespace ILib.MVVM
{
	public interface ICommand
	{
		bool CanExecute { get; }
		void Execute();
	}

	public interface ICommand<T>
	{
		bool CanExecute { get; }
		void Execute(T args);
	}
}