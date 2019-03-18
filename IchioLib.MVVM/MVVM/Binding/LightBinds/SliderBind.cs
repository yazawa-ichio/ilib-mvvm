using System;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class SliderBind : LightEventBind<float, Slider>
	{
		protected override void OnInit()
		{
			m_Target.onValueChanged.AddListener(OnChanged);
		}

		void OnChanged(float val)
		{
			Set(val);
			Event(val);
		}

		protected override void UpdateValue(float val)
		{
			if (m_Target.value != val)
			{
				m_Target.value = val;
			}
		}
	}

}
