using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class ToggleBind : LightBind<bool, Toggle>
	{
		protected override void OnInit()
		{
			m_Target.onValueChanged.AddListener(x => Set(x));
		}

		protected override void UpdateValue(bool val)
		{
			if (m_Target.isOn != val)
			{
				m_Target.isOn = val;
			}
		}
	}
}
