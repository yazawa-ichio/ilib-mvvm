using System;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class InputFieldBind : LightEventBind<string, InputField>
	{
		protected override void OnInit()
		{
			m_Target.onValueChanged.AddListener(OnChanged);
			m_Target.onEndEdit.AddListener(OnEnd);
		}

		void OnChanged(string val)
		{
			if (m_Target.text != val)
			{
				m_Target.text = val;
			}
			Event(val);
		}

		void OnEnd(string val)
		{
			if (m_Target.text != val)
			{
				m_Target.text = val;
			}
			Event(val);
		}

		protected override void UpdateValue(string val)
		{
			if (m_Target.text != val)
			{
				m_Target.text = val;
			}
		}

	}

}