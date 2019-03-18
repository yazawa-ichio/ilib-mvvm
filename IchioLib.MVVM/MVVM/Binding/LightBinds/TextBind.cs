using UnityEngine.UI;

namespace ILib.MVVM
{
	public class TextBind : LightBind<string, Text>
	{
		protected override void UpdateValue(string val)
		{
			m_Target.text = val;
		}
	}

}
