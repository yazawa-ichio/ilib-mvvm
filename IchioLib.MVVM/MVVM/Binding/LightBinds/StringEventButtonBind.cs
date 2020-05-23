using UnityEngine.UI;

namespace ILib.MVVM
{
	public class StringEventButtonBind : LightEventBind<string, Button>
	{
		string m_Val;
		protected override void OnInit()
		{
			m_Val = default;
			m_Target.onClick.AddListener(OnClick);
		}
		void OnClick()
		{
			Event(m_Val);
		}
		protected override void UpdateValue(string val)
		{
			m_Val = val;
		}
	}

}