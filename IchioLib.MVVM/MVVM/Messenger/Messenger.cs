using System;
using System.Collections;
using System.Collections.Generic;

namespace ILib.MVVM
{
	public class Messenger : IMessenger
	{
		public static IMessenger Default { get; private set; } = new Messenger();

		class HolderBase
		{
			public WeakReference<object> Recipient = null;
			public bool Removed = false;
		}

		class Holder : HolderBase
		{
			public Action Action = null;
		}

		class Holder<T> : HolderBase
		{
			public Action<T> Action = null;
		}

		class RemoveHolder
		{
			public string Name;
			public HolderBase Holder;
		}

		int m_Lock;
		Queue<RemoveHolder> m_DelayRemove = new Queue<RemoveHolder>(4);
		Dictionary<string, List<HolderBase>> m_Events = new Dictionary<string, List<HolderBase>>();

		public void Register<T>(object recipient, string name, Action<T> action)
		{
			if (recipient == null) throw new ArgumentNullException("recipient");
			var holders = GetList(name);
			var holder = new Holder<T>
			{
				Recipient = new WeakReference<object>(recipient),
				Action = action
			};
			holders.Add(holder);
		}

		public void Unregister<T>(object recipient, string name, Action<T> action)
		{
			bool refresh = false;
			List<HolderBase> list = GetList(name);
			for (int i = 0; i < list.Count; i++)
			{
				HolderBase holder = list[i];
				if (holder.Removed)
				{
					refresh = true;
					continue;
				}
				object r;
				if (!holder.Recipient.TryGetTarget(out r))
				{
					refresh = true;
					continue;
				}
				if (r != recipient)
				{
					continue;
				}
				var h = holder as Holder<T>;
				if (h != null && h.Action == action)
				{
					holder.Recipient = null;
					holder.Removed = true;
					if (m_Lock > 0)
					{
						m_DelayRemove.Enqueue(new RemoveHolder { Name = name, Holder = holder });
					}
					else
					{
						list.RemoveAt(i);
					}
					break;
				}
			}
			if (refresh)
			{
				Refresh();
			}
		}

		public void Unregister(object recipient)
		{
			foreach (var list in m_Events.Values)
			{
				for (int i = 0; i < list.Count; i++)
				{
					HolderBase holder = list[i];
					if (holder.Removed)
					{
						continue;
					}
					object r;
					if (!holder.Recipient.TryGetTarget(out r))
					{
						continue;
					}
					if (r == recipient)
					{
						holder.Recipient = null;
						holder.Removed = true;
					}
				}
			}
			Refresh();
		}


		public void Send<T>(string name, T message)
		{
			try
			{
				m_Lock++;
				var list = GetList(name);
				for (int i = 0; i < list.Count; i++)
				{
					var item = list[i];
					if (item.Removed) continue;
					var holder = item as Holder<T>;
					holder?.Action.Invoke(message);
				}
			}
			finally
			{
				m_Lock--;
				if (m_DelayRemove.Count > 0)
				{
					Refresh();
				}
			}
		}

		public void Refresh()
		{
			if (m_Lock > 0) return;
			while (m_DelayRemove.Count > 0)
			{
				var remove = m_DelayRemove.Dequeue();
				GetList(remove.Name).Remove(remove.Holder);
			}
			foreach (var list in m_Events.Values)
			{
				for (int i = list.Count -1; i >= 0; i--)
				{
					HolderBase holder = list[i];
					object r;
					if (holder.Removed || holder.Recipient.TryGetTarget(out r) || r == null)
					{
						list.RemoveAt(i);
					}
				}
			}
		}

		List<HolderBase> GetList(string name)
		{
			List<HolderBase> holders;
			if (!m_Events.TryGetValue(name, out holders))
			{
				m_Events[name] = holders = new List<HolderBase>();
			}
			return holders;
		}

	}
}

