using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{
	public class GraphicColorArgument : EventArgument<Color>
	{
		[SerializeField]
		Graphic m_Target = default;

		public override Color GetValue()
		{
			return m_Target.color;
		}

	}

}
