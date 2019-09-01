using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{

	public class InputFieldArgument : EventAargument<string>
	{
		[SerializeField]
		InputField m_Target = default;

		public override string GetValue()
		{
			return m_Target.text;
		}

	}

}
