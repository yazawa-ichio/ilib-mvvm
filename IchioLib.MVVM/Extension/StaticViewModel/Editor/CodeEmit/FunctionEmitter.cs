using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{
	public class FunctionEmitter : EmitterBase
	{
		public class Argument
		{
			public string Name;
			public string Type;
			public string InitValue;
		}

		public string Summary;
		public string Accessibility = "public";
		public string Return = "void";
		public string Name;
		public List<AttributeEmitter> Attributes = new List<AttributeEmitter>();
		public List<Argument> Args = new List<Argument>();

		public List<EmitterBase> Emitters = new List<EmitterBase>();

		public void AddArg(string type, string name, string initValue = "")
		{
			Args.Add(new Argument
			{
				Name = name,
				Type = type,
				InitValue = initValue,
			});
		}

		public void WriteLine(string line)
		{
			Emitters.Add(new WriteLineEmitter(line));
		}

		public void Append(string message)
		{
			Emitters.Add(new AppendEmitter(message));
		}

		public void Append(System.Action<CodeWriter> action)
		{
			Emitters.Add(new DelegateEmitter(action));
		}

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
			writer.Append($"{Accessibility} {Return} {Name} (");
			for (int i = 0; i < Args.Count; i++)
			{
				if (i > 0) writer.Append(", ");
				var arg = Args[i];
				if (string.IsNullOrEmpty(arg.InitValue))
				{
					writer.Append($"{arg.Type} {arg.Name}");
				}
				else
				{
					writer.Append($"{arg.Type} {arg.Name} = {arg.InitValue}");
				}
			}
			writer.AppendLine(")");
			using (writer.Bracket())
			{
				foreach (var emitter in Emitters)
				{
					emitter.Emit(writer);
				}
			}
		}
	}
}