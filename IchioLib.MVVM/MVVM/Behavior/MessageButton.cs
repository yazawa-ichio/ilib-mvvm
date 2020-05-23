using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class MessageButton : Behavior
	{
		[SerializeField, EventNameDraw]
		string m_Event = default;
		[SerializeField]
		Button m_Button = default;
		EventArgument m_Aargument = default;

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
			if (m_Messenger == null) return;
			if (m_Aargument != null)
			{
				m_Aargument.Do(m_Event, m_Messenger);
			}
			else
			{
				m_Messenger?.Send(m_Event);
			}
		}

	}
}