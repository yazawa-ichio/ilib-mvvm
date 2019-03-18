using UnityEngine;

namespace ILib.MVVM
{
	public class ActiveBind : LightBind<bool, GameObject>
	{
		protected override void UpdateValue(bool val)
		{
			if (m_Target.activeSelf != val) m_Target.SetActive(val);
		}
	}

}
