using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ILib.MVVM
{
	using Message;

	public sealed partial class Messenger
	{
		public static Messenger Default { get; private set; } = new Messenger();

		int m_Lock;
		bool m_Refresh;
		List<HolderBase> m_Events = new List<HolderBase>();
		public IMessengerHook Hook { get; set; }

		public void Register<T>(object recipient, string eventName, Action<T> action)
		{
			RegisterImpl(recipient, eventName, action);
		}
		
		public void Register<T, U>(object recipient, T eventName, Action<U> action)
		{
			RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}

		public void Register<T>(object recipient, object eventName, Action<T> action)
		{
			RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}

		public void Register(object recipient, string eventName, Action action)
		{
			RegisterImpl(recipient, eventName, action);
		}

		public void Register<T>(object recipient, T eventName, Action action)
		{
			RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}
		
		public void Register(object recipient, object eventName, Action action)
		{
			RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}

		public ReferenceHandle WeakRegister<T>(object recipient, string eventName, Action<T> action)
		{
			return RegisterImpl(recipient, eventName, action, true);
		}

		public ReferenceHandle WeakRegister<T, U>(object recipient, T eventName, Action<U> action)
		{
			return RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action, true);
		}

		public ReferenceHandle WeakRegister<T>(object recipient, object eventName, Action<T> action)
		{
			return RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action, true);
		}

		public ReferenceHandle WeakRegister(object recipient, string eventName, Action action)
		{
			return RegisterImpl(recipient, eventName, action, true);
		}

		public ReferenceHandle WeakRegister<T>(object recipient, T eventName, Action action)
		{
			return RegisterImpl(recipient, EventKeyToStrConv.ToStr(eventName), action, true);
		}

		public ReferenceHandle WeakRegister(object recipient, object eventName, Action action)
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

		public void Unregister<T>(object recipient, string eventName, Action<T> action)
		{
			for (int i = m_Events.Count - 1; i >= 0; i--)
			{
				if (m_Events[i] is Holder<T> holder && holder.IsSame(recipient, eventName, action))
				{
					Remove(i);
				}
			}
		}

		public void Unregister<T, U>(object recipient, T eventName, Action<U> action)
		{
			Unregister(recipient, EventKeyToStrConv.ToStr(eventName), action);
		}

		public void Unregister(object recipient, string eventName, Action action)
		{
			for (int i = m_Events.Count - 1; i >= 0; i--)
			{
				if (m_Events[i] is Holder holder && holder.IsSame(recipient, eventName, action))
				{
					Remove(i);
				}
			}
		}

		public void Unregister<T>(object recipient, T eventName, Action action)
		{
			Unregister(recipient, EventKeyToStrConv.ToStr(eventName), action);
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

		public void Send<T>(string eventName, T message)
		{
			Hook?.OnSend(eventName, message);
			SendImpl(eventName, message);
		}

		public void Send<T, U>(T eventName, U message)
		{
			Hook?.OnSend(eventName, message);
			SendImpl(EventKeyToStrConv.ToStr(eventName), message);
		}

		public void Send<T>(object eventName, T message)
		{
			Hook?.OnSend(eventName, message);
			SendImpl(EventKeyToStrConv.ToStr(eventName), message);
		}

		public void Send(string eventName)
		{
			Hook?.OnSend(eventName);
			SendImpl(eventName);
		}

		public void Send<T>(T eventName)
		{
			Hook?.OnSend(eventName);
			SendImpl(EventKeyToStrConv.ToStr(eventName));
		}

		public void Refresh()
		{
			if (m_Lock > 0)
			{
				m_Refresh = true;
			}
			else
			{
				m_Refresh = false;
				m_Events.RemoveAll(x => !x.IsActive());
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

