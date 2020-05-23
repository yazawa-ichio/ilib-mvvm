using System.Collections.Generic;
namespace ILib.MVVM
{

	public class EventBroker : IViewEventDispatcher
	{
		List<BindingEventBase> m_Events = new List<BindingEventBase>();

		public void Subscribe(string name, System.Action onViewEvent)
		{
			var bindingEvent = Get<BindingEvent>(name);
			if (bindingEvent == null)
			{
				bindingEvent = new BindingEvent(name);
				SetNewEvent(bindingEvent);
			}
			bindingEvent.OnEvent += onViewEvent;
		}

		public void Subscribe<T>(T name, System.Action onViewEvent)
		{
			Subscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Subscribe(object name, System.Action onViewEvent)
		{
			Subscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Unsubscribe(string name, System.Action onViewEvent)
		{
			var bindingEvent = Get<BindingEvent>(name);
			if (bindingEvent != null)
			{
				bindingEvent.OnEvent -= onViewEvent;
			}
		}

		public void Unsubscribe<T>(T name, System.Action onViewEvent)
		{
			Unsubscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Unsubscribe(object name, System.Action onViewEvent)
		{
			Unsubscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Subscribe<T>(string name, System.Action<T> onViewEvent)
		{
			var bindingEvent = Get<BindingEvent<T>>(name);
			if (bindingEvent == null)
			{
				bindingEvent = new BindingEvent<T>(name);
				SetNewEvent(bindingEvent);
			}
			bindingEvent.OnEvent += onViewEvent;
		}

		public void Subscribe<T, U>(T name, System.Action<U> onViewEvent)
		{
			Subscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Subscribe<T>(object name, System.Action<T> onViewEvent)
		{
			Subscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Unsubscribe<T>(string name, System.Action<T> onViewEvent)
		{
			var bindingEvent = Get<BindingEvent<T>>(name);
			if (bindingEvent != null)
			{
				bindingEvent.OnEvent -= onViewEvent;
			}
		}

		public void Unsubscribe<T, U>(T name, System.Action<U> onViewEvent)
		{
			Unsubscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Unsubscribe<T>(object name, System.Action<T> onViewEvent)
		{
			Unsubscribe(EventKeyToStrConv.ToStr(name), onViewEvent);
		}

		public void Publish(string name)
		{
			Get<BindingEvent>(name)?.Invoke();
		}

		public void Publish<T>(T name)
		{
			Get<BindingEvent>(EventKeyToStrConv.ToStr(name))?.Invoke();
		}

		public void Publish<T>(string name, T args)
		{
			Get<BindingEvent<T>>(name)?.Invoke(args);
		}

		public void Publish<T, U>(T name, U args)
		{
			Get<BindingEvent<U>>(EventKeyToStrConv.ToStr(name))?.Invoke(args);
		}

		T Get<T>(string name) where T : BindingEventBase
		{
			if (m_Events == null) return null;
			foreach (var e in m_Events)
			{
				if (e is T target && target.Name == name)
				{
					return target;
				}
			}
			return null;
		}

		void SetNewEvent(BindingEventBase newEvent)
		{
			if (m_Events == null)
			{
				m_Events = new List<BindingEventBase>();
			}
			m_Events.Add(newEvent);
		}

		void IViewEventDispatcher.Dispatch(string name)
		{
			Publish(name);
		}

		void IViewEventDispatcher.Dispatch<T>(string name, T args)
		{
			Publish(name, args);
		}

#if UNITY_EDITOR
		public IEnumerable<IBindingEvent> GetAll()
		{
			return m_Events;
		}
#endif
	}
}