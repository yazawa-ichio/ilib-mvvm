using System;
using System.Collections.Generic;
using System.Linq;

namespace ILib.MVVM.Message
{

	internal sealed class MessengerImpl : IMessenger
	{
		int m_Lock;
		bool m_Refresh;
		List<HolderBase> m_Events = new List<HolderBase>();
		public IMessengerHook Hook { get; set; }

		public void Register<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action)
		{
			RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}

		public void Register<TMessage>(object recipient, object eventName, Action<TMessage> action)
		{
			RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}

		public void Register<TEvent>(object recipient, TEvent eventName, Action action)
		{
			RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}

		public ReferenceHandle WeakRegister<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action)
		{
			return RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action, true);
		}

		public ReferenceHandle WeakRegister<TMessage>(object recipient, object eventName, Action<TMessage> action)
		{
			return RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action, true);
		}

		public ReferenceHandle WeakRegister<TEvent>(object recipient, TEvent eventName, Action action)
		{
			return RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action, true);
		}

		public void RegisterHandle(object target)
		{
			MessageHandleAttribute.Register(this, target, false);
		}

		public ReferenceHandle WeakRegisterHandle(object target)
		{
			return MessageHandleAttribute.Register(this, target, true);
		}

		public void Unregister<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action)
		{
			var eventNameStr = EventKeyToStrConv.ToStr(eventName);
			for (int i = m_Events.Count - 1; i >= 0; i--)
			{
				if (m_Events[i] is Holder<TMessage> holder && holder.IsSame(recipient, eventNameStr, action))
				{
					Remove(i);
				}
			}
		}

		public void Unregister<TMessage>(object recipient, object eventName, Action<TMessage> action)
		{
			var eventNameStr = EventKeyToStrConv.ToStr(eventName);
			for (int i = m_Events.Count - 1; i >= 0; i--)
			{
				if (m_Events[i] is Holder<TMessage> holder && holder.IsSame(recipient, eventNameStr, action))
				{
					Remove(i);
				}
			}
		}

		public void Unregister<TEvent>(object recipient, TEvent eventName, Action action)
		{
			var eventNameStr = EventKeyToStrConv.ToStr(eventName);
			for (int i = m_Events.Count - 1; i >= 0; i--)
			{
				if (m_Events[i] is Holder holder && holder.IsSame(recipient, eventNameStr, action))
				{
					Remove(i);
				}
			}
		}

		public void Unregister(object recipient)
		{
			for (int i = m_Events.Count - 1; i >= 0; i--)
			{
				var holder = m_Events[i];
				if (holder.IsActive() && holder.IsRecipient(recipient))
				{
					Remove(i);
				}
			}
		}


		public void Send<TEvent, TMessage>(TEvent eventName, TMessage message)
		{
			Hook?.OnSend(eventName, message);
			SendImpl(EventKeyToStrConv.ToStr(eventName), message);
		}

		public void Send<TEvent>(TEvent eventName)
		{
			Hook?.OnSend(eventName);
			SendImpl(EventKeyToStrConv.ToStr(eventName));
		}

		Predicate<HolderBase> m_RemoveMatch = x => !x.IsActive();
		public void Refresh()
		{
			if (m_Lock > 0)
			{
				m_Refresh = true;
			}
			else
			{
				m_Refresh = false;
				m_Events.RemoveAll(m_RemoveMatch);
			}
		}

		ReferenceHandle RegisterImpl<T>(object recipient, string eventName, Action<T> action, bool weakreference = false)
		{
			if (recipient == null) throw new ArgumentNullException("recipient");
			if (GetHolders<IHolder<T>>(recipient, eventName).Any(x => x.IsSame(recipient, action)))
			{
				//イベント登録済み
				return ReferenceHandle.Empty;
			}
			if (!weakreference)
			{
				var holder = new Holder<T>
				{
					EventName = eventName,
					Recipient = new WeakReference<object>(recipient),
					Action = action
				};
				m_Events.Add(holder);
				return ReferenceHandle.Empty;
			}
			else
			{
				var holder = new WeakReferenceHolder<T>
				{
					EventName = eventName,
					Recipient = new WeakReference<object>(recipient),
					Action = new WeakReference<Action<T>>(action)
				};
				var handle = new ReferenceHandle(action, holder);
				holder.Handle = new WeakReference<ReferenceHandle>(handle);
				m_Events.Add(holder);
				return handle;
			}
		}

		ReferenceHandle RegisterImpl(object recipient, string eventName, Action action, bool weakreference = false)
		{
			if (recipient == null) throw new ArgumentNullException("recipient");
			if (GetHolders<IHolder>(recipient, eventName).Any(x => x.IsSame(recipient, action)))
			{
				//イベント登録済み
				return ReferenceHandle.Empty;
			}
			if (!weakreference)
			{
				var holder = new Holder
				{
					EventName = eventName,
					Recipient = new WeakReference<object>(recipient),
					Action = action
				};
				m_Events.Add(holder);
				return ReferenceHandle.Empty;
			}
			else
			{
				var holder = new WeakReferenceHolder
				{
					EventName = eventName,
					Recipient = new WeakReference<object>(recipient),
					Action = new WeakReference<Action>(action)
				};
				var handle = new ReferenceHandle(action, holder);
				holder.Handle = new WeakReference<ReferenceHandle>(handle);
				m_Events.Add(holder);
				return handle;
			}
		}

		IEnumerable<T> GetHolders<T>(object recipient, string eventName)
		{
			foreach (var holder in m_Events)
			{
				if (holder is T ret && holder.HasEvent(recipient, eventName))
				{
					yield return ret;
				}
			}
		}

		void Remove(int index)
		{
			if (m_Lock > 0)
			{
				m_Events[index].Removed = true;
				m_Refresh = true;
			}
			else
			{
				m_Events.RemoveAt(index);
			}
		}

		void SendImpl<T>(string eventName, T message)
		{
			try
			{
				m_Lock++;
				for (int i = 0, count = m_Events.Count; i < count; i++)
				{
					var holder = m_Events[i] as IHolder<T>;
					if (holder == null || holder.EventName != eventName || !holder.IsActive())
					{
						continue;
					}
					holder.Invoke(message);
				}
			}
			finally
			{
				m_Lock--;
				if (m_Lock == 0 && m_Refresh)
				{
					Refresh();
				}
			}
		}

		void SendImpl(string eventName)
		{
			try
			{
				m_Lock++;
				for (int i = 0, count = m_Events.Count; i < count; i++)
				{
					var holder = m_Events[i] as IHolder;
					if (holder == null || holder.EventName != eventName || !holder.IsActive())
					{
						continue;
					}
					holder.Invoke();
				}
			}
			finally
			{
				m_Lock--;
				if (m_Lock == 0 && m_Refresh)
				{
					Refresh();
				}
			}
		}

	}
}