using System.Collections.Generic;

namespace ILib.MVVM
{
	public class BindingEventHandler : IViewEventHandler
	{
		IViewEventHandler m_Handler;
		LinkedList<IViewEventHandler> m_Handlers;

		public void Add(IViewEventHandler handler)
		{
			if (m_Handler == null)
			{
				m_Handler = handler;
			}
			else
			{
				var list = m_Handlers ?? (m_Handlers = new LinkedList<IViewEventHandler>());
				list.AddLast(handler);
			}
		}
		public void Remove(IViewEventHandler handler)
		{
			if (m_Handler == handler)
			{
				m_Handler = null;
			}
			else
			{
				m_Handlers?.Remove(handler);
			}
		}

		void IViewEventHandler.OnViewEvent(string name)
		{
			m_Handler?.OnViewEvent(name);
			if (m_Handlers != null)
			{
				foreach (var handler in m_Handlers)
				{
					handler.OnViewEvent(name);
				}
			}
		}

		void IViewEventHandler.OnViewEvent<T>(string name, T args)
		{
			m_Handler?.OnViewEvent(name, args);
			if (m_Handlers != null)
			{
				foreach (var handler in m_Handlers)
				{
					handler.OnViewEvent(name, args);
				}
			}
		}
	}
}
