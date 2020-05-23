using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{
	[UnityEngine.Scripting.Preserve]
	public class ColorBind : LightBind<Color, Graphic>
	{
		protected override void UpdateValue(Color val)
		{
			m_Target.color = val;
		}
	}

}