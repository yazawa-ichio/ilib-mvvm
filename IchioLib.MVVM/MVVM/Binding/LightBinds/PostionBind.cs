using UnityEngine;

namespace ILib.MVVM
{
	[UnityEngine.Scripting.Preserve]
	public class PostionBind : LightBind<Vector3, Transform>
	{
		protected override void UpdateValue(Vector3 val)
		{
			m_Target.localPosition = val;
		}
	}

}