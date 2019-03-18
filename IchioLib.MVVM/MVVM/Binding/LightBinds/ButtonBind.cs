using System;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class ButtonBind : LightSimpleEventBind<bool, Button>
	{
		protected override void OnInit()
		{
			m_Target.onClick.AddListener(OnClick);
		}
		void OnClick()
		{
			Event();
		}

		protected override void UpdateValue(bool val)
		{
			m_Target.interactable = val;
		}

		public override Type EventType()
		{
			return null;
		}
	}

}
