using UnityEngine;

namespace ILib.MVVM
{
	public class PostionBind : LightBind<Vector3, Transform>
	{
		protected override void UpdateValue(Vector3 val)
		{
			m_Target.localPosition = val;
		}
	}

}
