using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{

	public class ButtonToggleBind : LightBind<bool, Button>
	{
		protected override void OnInit()
		{
			m_Target.onClick.AddListener(() => Set(!Value));
		}

		protected override void UpdateValue(bool val)
		{
		}
	}

}
