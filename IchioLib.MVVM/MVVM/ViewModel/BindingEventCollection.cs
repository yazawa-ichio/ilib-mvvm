using System.Collections;
using System.Collections.Generic;
namespace ILib.MVVM
{
	public class BindingEventCollection : IViewEventHandler , IEnumerable<IBindingEvent>
	{
		Dictionary<string, BindingEventBase> m_Events = new Dictionary<string, BindingEventBase>(4);

		public void Add(string name, System.Action onViewEvent)
		{
			var bindingEvent = Get<BindingEvent>(name);
			if (bindingEvent == null)
			{
				bindingEvent = new BindingEvent(name);
				SetNewEvent(name, bindingEvent);
			}
			bindingEvent.OnEvent += onViewEvent;
		}

		public void Remove(string name, System.Action onViewEvent)
		{
			var bindingEvent = Get<BindingEvent>(name);
			if (bindingEvent != null)
			{
				bindingEvent.OnEvent -= onViewEvent;
			}
		}

		public void Add<T>(string name, System.Action<T> onViewEvent)
		{
			var bindingEvent = Get<BindingEvent<T>>(name);
			if (bindingEvent == null)
			{
				bindingEvent = new BindingEvent<T>(name);
				SetNewEvent(name, bindingEvent);
			}
			bindingEvent.OnEvent += onViewEvent;
		}

		public void Remove<T>(string name, System.Action<T> onViewEvent)
		{
			var bindingEvent = Get<BindingEvent<T>>(name);
			if (bindingEvent != null)
			{
				bindingEvent.OnEvent -= onViewEvent;
			}
		}

		public void Invoke(string name)
		{
			Get<BindingEvent>(name)?.Invoke();
		}

		public void Invoke<T>(string name, T args)
		{
			Get<BindingEvent<T>>(name)?.Invoke(args);
		}

		T Get<T>(string name) where T : BindingEventBase
		{
			BindingEventBase bindingEvent;
			if (m_Events.TryGetValue(name, out bindingEvent))
			{
				while (bindingEvent != null)
				{
					var ret = bindingEvent as T;
					if (ret != null) return ret;
					bindingEvent = bindingEvent.Next;
				}
			}
			return null;
		}

		void SetNewEvent(string name, BindingEventBase newEvent)
		{
			BindingEventBase bindingEvent;
			if (!m_Events.TryGetValue(name, out bindingEvent))
			{
				m_Events[name] = newEvent;
			}
			else
			{
				while (bindingEvent.Next != null)
				{
					bindingEvent = bindingEvent.Next;
				}
				bindingEvent.Next = newEvent;
			}
		}

		void IViewEventHandler.OnViewEvent(string name)
		{
			Invoke(name);
		}
		void IViewEventHandler.OnViewEvent<T>(string name, T args)
		{
			Invoke(name, args);
		}

		IEnumerator<IBindingEvent> IEnumerable<IBindingEvent>.GetEnumerator()
		{
			foreach (var e in m_Events.Values)
			{
				var cur = e;
				do
				{
					yield return cur;
				} while ((cur = cur.Next) != null);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (var e in m_Events.Values)
			{
				var cur = e;
				do
				{
					yield return cur;
				} while ((cur = cur.Next) != null);
			}
		}
	}
}
