namespace ILib.MVVM
{
	public class DelegateCommand : ICommand
	{
		System.Action m_Action;
		System.Func<bool> m_CanExecute;

		public bool CanExecute => m_CanExecute != null ? m_CanExecute() : true;

		public DelegateCommand(System.Action action, System.Func<bool> canExecute = null)
		{
			m_Action = action;
			m_CanExecute = canExecute;
		}

		void ICommand.Execute() => m_Action?.Invoke();

	}

	public class DelegateCommand<T> : ICommand<T>
	{
		System.Action<T> m_Action;
		System.Func<bool> m_CanExecute;

		public bool CanExecute => m_CanExecute != null ? m_CanExecute() : true;

		public DelegateCommand(System.Action<T> action, System.Func<bool> canExecute = null)
		{
			m_Action = action;
			m_CanExecute = canExecute;
		}

		void ICommand<T>.Execute(T args) => m_Action?.Invoke(args);
	}

}
