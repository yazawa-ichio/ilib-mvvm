using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class MessageButton : Behavior
	{
		[SerializeField, EventName]
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
			if (m_Messanger == null) return;
			if (m_Aargument != null)
			{
				m_Aargument.Do(m_Event, m_Messanger);
			}
			else
			{
				m_Messanger?.Send(m_Event);
			}
		}

	}
}