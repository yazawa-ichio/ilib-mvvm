using UnityEngine;

namespace ILib.MVVM
{
	public class FloatToBoolConverter : Converter<float, bool>
	{

		[SerializeField]
		ComparisonOperatorType m_Type = ComparisonOperatorType.Equal;
		[SerializeField]
		float m_Value = 0;

		public override bool Convert(float input)
		{
			switch (m_Type)
			{
				case ComparisonOperatorType.Equal:
					return m_Value == input;
				case ComparisonOperatorType.NotEqual:
					return m_Value != input;
				case ComparisonOperatorType.GreaterThan:
					return m_Value < input;
				case ComparisonOperatorType.GreaterThanOrEqual:
					return m_Value <= input;
				case ComparisonOperatorType.LessThan:
					return m_Value > input;
				case ComparisonOperatorType.LessThanOrEqual:
					return m_Value >= input;
			}
			return false;
		}

	}
}