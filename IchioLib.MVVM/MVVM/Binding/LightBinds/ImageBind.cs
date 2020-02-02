using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{
	[UnityEngine.Scripting.Preserve]
	public class ImageBind : LightBind<Sprite,Image>
	{
		protected override void UpdateValue(Sprite val)
		{
			m_Target.sprite = val;
		}
	}

}
