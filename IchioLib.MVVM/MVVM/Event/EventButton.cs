using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{

	public class EventButton : MonoBehaviour, IViewEvent
	{
		public string Name => m_Event;

		[SerializeField, EventName]
		string m_Event = default;
		[SerializeField]
		Button m_Button = default;
		EventArgument m_Aargument = default;

		IViewEventHandler m_Handler;

		void Awake()
		{
			if (m_Button == null)
			{
				m_Button = GetComponent<Button>();
			}
			m_Aargument = GetComponent<EventArgument>();
			m_Button.onClick.AddListener(OnClick);
		}

		protected virtual void OnClick()
		{
			if (m_Aargument != null)
			{
				m_Aargument.Do(Name, m_Handler);
			}
			else
			{
				m_Handler?.OnViewEvent(Name);
			}
		}

		void IViewEvent.Bind(IViewEventHandler handler)
		{
			m_Handler = handler;
		}

		public virtual System.Type EventType()
		{
			return m_Aargument?.GetEventType() ?? null;
		}

	}

}
