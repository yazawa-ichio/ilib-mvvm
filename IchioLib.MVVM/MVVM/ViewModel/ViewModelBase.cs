using System;
using System.Collections.Generic;

namespace ILib.MVVM
{
	public abstract class ViewModelBase : IViewModel
	{
		LinkedList<IBinding> m_Bindings = new LinkedList<IBinding>();

		BindingPropertyCollection m_Properties = new BindingPropertyCollection();
		BindingEventCollection m_Events = new BindingEventCollection();

		public IMessenger Messenger
		{
			get { return GetImpl<IMessenger>("Messanger") ?? ILib.MVVM.Messenger.Default; }
			set { SetImpl("Messanger", value); }
		}

		T IViewModel.Get<T>(string path) => GetImpl<T>(path);

		protected T GetImpl<T>(string path)
		{
			return m_Properties.Get<T>(path);
		}

		void IViewModel.Set<T>(string path, T val) => SetImpl<T>(path, val);

		protected void SetImpl<T>(string path, T val)
		{
			IBindingProperty<T> newProperty;
			if (m_Properties.Set(path, val, out newProperty))
			{
				BindNewProperty(path, newProperty);
			}
		}

		public void SetDirty(string path)
		{
			m_Properties.SetDirty(path);
		}

		public void SetAllDirty()
		{
			m_Properties.SetAllDirty();
		}

		void IViewModel.Register(IBinding binding)
		{
			m_Bindings.AddLast(binding);
			m_Properties.Bind(binding);
			binding.Bind(m_Events);
		}

		void IViewModel.Unregister(IBinding binding)
		{
			m_Bindings.Remove(binding);
			m_Properties.Unbind(binding);
			binding.Unbind(m_Events);
		}

		void BindNewProperty(string path, IBindingProperty property)
		{
			foreach (var binding in m_Bindings)
			{
				binding.Bind(path, property);
			}
		}

		void IViewModel.SubscribeChanged<T>(string path, Action<T> notify)
		{
			m_Properties.Subscribe(path, notify);
		}

		void IViewModel.UnsubscribeChanged<T>(string path, Action<T> notify)
		{
			m_Properties.Unsubscribe(path, notify);
		}

		void IViewModel.SubscribeViewEvent(string name, Action onViewEvent)
		{
			m_Events.Add(name, onViewEvent);
		}

		void IViewModel.UnsubscribeViewEvent(string name, Action onViewEvent)
		{
			m_Events.Remove(name, onViewEvent);
		}

		void IViewModel.SubscribeViewEvent<T>(string name, Action<T> onViewEvent)
		{
			m_Events.Add(name, onViewEvent);
		}

		void IViewModel.UnsubscribeViewEvent<T>(string name, Action<T> onViewEvent)
		{
			m_Events.Remove(name, onViewEvent);
		}

#if UNITY_EDITOR
		IEnumerable<IBindingProperty> IViewModel.GetProperties()
		{
			return m_Properties.GetAll();
		}

		IEnumerable<IBindingEvent> IViewModel.GetEvents()
		{
			return m_Events.GetAll();
		}
#endif

	}

}
