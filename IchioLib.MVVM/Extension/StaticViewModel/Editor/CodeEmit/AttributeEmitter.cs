using System.Collections.Generic;

//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{
	public class AttributeEmitter : EmitterBase
	{
		public string Name;
		public List<string> Args = new List<string>();

		public void AddArg(string val)
		{
			Args.Add(val);
		}

		public void AddProp(string prop, string value)
		{
			Args.Add($"{prop}={value}");
		}

		public override void Emit(CodeWriter writer)
		{
			writer.AppendTab();
			writer.Append("[");
			writer.Append(Name);
			if (Args.Count > 0)
			{
				writer.Append("(");
				for (int i = 0; i < Args.Count; i++)
				{
					if (i > 0) writer.Append(", ");
					writer.Append(Args[i]);
				}
				writer.Append(")");
			}
			writer.Append("]");
		}
	}
}
