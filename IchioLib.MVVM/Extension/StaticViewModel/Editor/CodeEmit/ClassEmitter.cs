using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{

	public class ClassEmitter : EmitterBase
	{
		public string Summary;
		public string Accessibility = "public";
		public string Name = "";
		public List<string> Extends = new List<string>();
		public List<AttributeEmitter> Attributes = new List<AttributeEmitter>();

		public List<MemberEmitter> Members = new List<MemberEmitter>();
		public List<FunctionEmitter> Functions = new List<FunctionEmitter>();

		public override void Emit(CodeWriter writer)
		{
			if (!string.IsNullOrEmpty(Summary))
			{
				writer.ShortSummary(Summary);
			}
			foreach (var attr in Attributes)
			{
				attr.Emit(writer);
			}

			writer.AppendTab();
			writer.Append($"{Accessibility} class {Name}");
			if (Extends.Count > 0)
			{
				writer.Append($" : {Extends[0]}");
				for (int i = 1; i < Extends.Count; i++)
				{
					writer.Append($", {Extends[i]}");
				}
			}
			writer.AppendLine("");

			using (writer.Bracket())
			{
				writer.AppendLine("");
				foreach (var emitter in Members)
				{
					emitter.Emit(writer);
					writer.AppendLine("");
				}
				writer.AppendLine("");
				foreach (var emitter in Functions)
				{
					emitter.Emit(writer);
					writer.AppendLine("");
				}
				writer.AppendLine("");
			}
		}
	}

}