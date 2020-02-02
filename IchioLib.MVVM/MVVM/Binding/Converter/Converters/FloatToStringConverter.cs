using UnityEngine;

namespace ILib.MVVM
{
	public class FloatToStringConverter : Converter<float, string>
	{
		[SerializeField]
		string m_Format = null;

		public override string Convert(float input)
		{
			if (string.IsNullOrEmpty(m_Format))
			{
				return input.ToString();
			}
			return string.Format(m_Format, input);
		}

	}

}