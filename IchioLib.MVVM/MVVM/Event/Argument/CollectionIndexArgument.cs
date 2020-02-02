namespace ILib.MVVM
{
	public class CollectionIndexArgument : EventArgument<int>
	{
		public int Index { get; set; }

		public override int GetValue()
		{
			return Index;
		}
	}
}
