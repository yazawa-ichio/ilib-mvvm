using System;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class IntEventButtonBind : LightEventBind<int, Button>
	{
		int m_Val;
		protected override void OnInit()
		{
			m_Target.onClick.AddListener(OnClick);
		}
		void OnClick()
		{
			Event(m_Val);
		}
		protected override void UpdateValue(int val)
		{
			m_Val = val;
		}
	}
}
