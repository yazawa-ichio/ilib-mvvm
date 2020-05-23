namespace ILib.MVVM
{
	public class BoolInverseConverter : Converter<bool, bool>
	{
		public override bool Convert(bool input)
		{
			return !input;
		}
	}
}