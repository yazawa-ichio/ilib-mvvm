using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{

	public class EventButton : MonoBehaviour, IViewEvent
	{
		public string Name => m_Event;

		[SerializeField, EventNameDraw]
		string m_Event = default;
		[SerializeField]
		Button m_Button = default;
		EventArgument m_Aargument = default;

		IViewEventDispatcher m_Handler;
		bool m_Init;

		void Awake()
		{
			Init();
		}

		void Init()
		{
			if (m_Init) return;
			m_Init = true;
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
				m_Handler?.Dispatch(Name);
			}
		}

		void IViewEvent.Bind(IViewEventDispatcher handler)
		{
			m_Handler = handler;
		}

		public virtual System.Type EventType()
		{
			return m_Aargument?.GetEventType() ?? null;
		}

		public EventArgument GetArgument()
		{
			Init();
			return m_Aargument;
		}

	}

}