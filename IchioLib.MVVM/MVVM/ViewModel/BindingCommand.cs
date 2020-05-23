namespace ILib.MVVM
{

	public class BindingCommand : System.IDisposable
	{
		public readonly string Name;
		IViewModel m_VM;
		ICommand m_Value;

		public ICommand Value
		{
			get { return Get(); }
			set { Set(value); }
		}

		public BindingCommand(string name, IViewModel vm)
		{
			Name = name;
			m_VM = vm;
			m_VM.Event.Subscribe(name, Execute);
		}

		public ICommand Get()
		{
			return m_Value;
		}

		public void Set(ICommand cmd)
		{
			m_Value = cmd;
		}

		void Execute()
		{
			if (m_Value != null && m_Value.CanExecute)
			{
				m_Value.Execute();
			}
		}

		public void Dispose()
		{
			if (m_VM != null)
			{
				m_VM.Event.Unsubscribe(Name, Execute);
				m_VM = null;
			}
		}

		public override string ToString()
		{
			return m_Value?.ToString() ?? "Empty";
		}

	}


	public class BindingCommand<T> : System.IDisposable
	{
		public readonly string Name;
		IViewModel m_VM;
		ICommand<T> m_Value;

		public ICommand<T> Value
		{
			get { return Get(); }
			set { Set(value); }
		}

		public BindingCommand(string name, IViewModel vm)
		{
			Name = name;
			m_VM = vm;
			m_VM.Event.Subscribe<T>(name, Execute);
		}

		public ICommand<T> Get()
		{
			return m_Value;
		}

		public void Set(ICommand<T> cmd)
		{
			m_Value = cmd;
		}

		void Execute(T args)
		{
			if (m_Value != null && m_Value.CanExecute)
			{
				m_Value.Execute(args);
			}
		}

		public void Dispose()
		{
			if (m_VM != null)
			{
				m_VM.Event.Subscribe<T>(Name, Execute);
				m_VM = null;
			}
		}

		public override string ToString()
		{
			return m_Value?.ToString() ?? "Empty";
		}

	}

}