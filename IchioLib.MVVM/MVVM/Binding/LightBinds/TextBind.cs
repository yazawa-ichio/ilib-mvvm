using UnityEngine.UI;

namespace ILib.MVVM
{
	[UnityEngine.Scripting.Preserve]
	public class TextBind : LightBind<string, Text>
	{
		protected override void UpdateValue(string val)
		{
			m_Target.text = val;
		}
	}

}