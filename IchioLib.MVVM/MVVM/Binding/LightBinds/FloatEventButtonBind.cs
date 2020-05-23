using UnityEngine.UI;

namespace ILib.MVVM
{
	public class FloatEventButtonBind : LightEventBind<float, Button>
	{
		float m_Val;
		protected override void OnInit()
		{
			m_Val = default;
			m_Target.onClick.AddListener(OnClick);
		}
		void OnClick()
		{
			Event(m_Val);
		}
		protected override void UpdateValue(float val)
		{
			m_Val = val;
		}
	}
}