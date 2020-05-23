namespace ILib.MVVM
{
	public abstract class ViewModelBase : IViewModel
	{
		public const string MessengerPath = "Messenger";

		protected BindingPropertyCollection m_Properties = new BindingPropertyCollection();

		BindingPropertyCollection IViewModel.Property => m_Properties;

		protected EventBroker m_Event = new EventBroker();

		public EventBroker Event => m_Event;

		public IMessenger Messenger
		{
			get { return GetImpl<IMessenger>(MessengerPath) ?? ILib.MVVM.Messenger.Default; }
			set { SetImpl(MessengerPath, value); }
		}

		T IViewModel.Get<T>(string path) => GetImpl<T>(path);

		protected T GetImpl<T>(string path)
		{
			return m_Properties.Get<T>(path).Value;
		}

		void IViewModel.Set<T>(string path, T val) => SetImpl<T>(path, val);

		protected void SetImpl<T>(string path, T val)
		{
			m_Properties.Get<T>(path).Value = val;
		}

		public void SetDirty(string path)
		{
			m_Properties.SetDirty(path);
		}

		public void SetAllDirty()
		{
			m_Properties.SetAllDirty();
		}

	}

}