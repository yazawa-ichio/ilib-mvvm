﻿using System;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class ButtonBind : LightEventBind<bool, Button>
	{
		EventArgument m_Aargument = default;

		protected override void OnInit()
		{
			m_Target.onClick.AddListener(OnClick);
			m_Aargument = m_Target.GetComponent<EventArgument>();
		}

		public override bool OnReturn()
		{
			m_Aargument = default;
			return base.OnReturn();
		}

		void OnClick()
		{
			if (m_Aargument != null)
			{
				Event(m_Aargument);
			}
			else
			{
				Event();
			}
		}

		protected override void UpdateValue(bool val)
		{
			m_Target.interactable = val;
		}

		public override Type EventType()
		{
			return m_Aargument?.GetEventType() ?? null;
		}

		public override EventArgument GetArgument()
		{
			return m_Aargument;
		}

	}

}