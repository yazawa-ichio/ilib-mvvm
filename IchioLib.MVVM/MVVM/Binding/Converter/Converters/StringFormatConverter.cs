using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	public class StringFormatConverter : Converter<string, string>
	{
		[SerializeField]
		string m_Format = "{0}";

		public override string Convert(string input)
		{
			return string.Format(m_Format, input);
		}

	}

}