using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	public class IntToStringConverter : Converter<int, string>
	{
		[SerializeField]
		string m_Format = null;

		public override string Convert(int input)
		{
			if (string.IsNullOrEmpty(m_Format))
			{
				return input.ToString();
			}
			return string.Format(m_Format, input);
		}

	}
}