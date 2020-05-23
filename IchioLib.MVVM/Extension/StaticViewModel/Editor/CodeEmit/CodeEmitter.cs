using System.Collections;
using System.Collections.Generic;

//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{
	public class CodeEmitter : EmitterBase
	{
		public string Comment;
		public string Namespace;
		public List<string> Using = new List<string>();
		public List<ClassEmitter> Class = new List<ClassEmitter>();

		public void AddUsing(string val)
		{
			Using.Add(val);
		}

		public void AddClass(ClassEmitter emitter)
		{
			Class.Add(emitter);
		}

		public ClassEmitter AddClass()
		{
			ClassEmitter emitter = new ClassEmitter();
			Class.Add(emitter);
			return emitter;
		}

		public void Run(string path)
		{
			CodeWriter writer = new CodeWriter();
			Emit(writer);
			writer.Dump(path);
		}

		public string DryRun()
		{
			CodeWriter writer = new CodeWriter();
			Emit(writer);
			return writer.ToString();
		}

		public override void Emit(CodeWriter writer)
		{
			if (!string.IsNullOrEmpty(Comment))
			{
				writer.Comment(Comment);
			}

			foreach (var _using in Using)
			{
				writer.AppendLine($"using {_using};");
			}

			writer.AppendLine("");

			if (!string.IsNullOrEmpty(Namespace))
			{
				writer.AppendLine($"namespace {Namespace}");
			}

			using (!string.IsNullOrEmpty(Namespace) ? writer.Bracket() : null)
			{
				foreach (var emitter in Class)
				{
					writer.AppendLine("");
					emitter.Emit(writer);
					writer.AppendLine("");
				}
			}

		}
	}
}