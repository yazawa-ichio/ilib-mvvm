using System.Collections.Generic;

//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{
	public abstract class MemberEmitter : EmitterBase
	{
		public string Summary;
		public string Accessibility = "public";
		public List<AttributeEmitter> Attributes = new List<AttributeEmitter>();

		public void AddAttribute(string attr)
		{
			AttributeEmitter emitter = new AttributeEmitter();
			emitter.Name = attr;
			Attributes.Add(emitter);
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
			EmitMember(writer);
		}

		public abstract void EmitMember(CodeWriter writer);

	}

	public class FieldEmitter : MemberEmitter
	{
		public string Type;
		public string Name;
		public string InitValue = "null";

		public override void EmitMember(CodeWriter writer)
		{
			writer.AppendTab();
			writer.Append($"{Accessibility} {Type} {Name}");
			if (string.IsNullOrEmpty(InitValue))
			{
				writer.Append($" = {InitValue}");
			}
			writer.AppendLine(";");
		}
	}

	public class PropertyEmitter : MemberEmitter
	{
		public string Type;
		public string Name;
		public EmitterBase Getter;
		public EmitterBase Setter;

		public override void EmitMember(CodeWriter writer)
		{
			writer.WriteLine($"{Accessibility} {Type} {Name}");
			using (writer.Bracket())
			{
				if (Getter != null) Getter.Emit(writer);
				if (Setter != null) Setter.Emit(writer);
			}
		}
	}

	public class ShortPropertyEmitter : MemberEmitter
	{
		public string Type;
		public string Name;
		public bool SettterProtected;
		public string InitValue;

		public override void EmitMember(CodeWriter writer)
		{
			string setter = SettterProtected ? "protected set;" : "set;";
			writer.AppendTab();
			writer.Append($"{Accessibility} {Type} {Name} {{ get; {setter} }}");
			if (string.IsNullOrEmpty(InitValue))
			{
				writer.Append($" = {InitValue};");
			}
			writer.AppendLine("");
		}
	}

	public class LazyPropertyEmitter : MemberEmitter
	{
		public string Type;
		public string Name;
		public string Create = "";

		public override void EmitMember(CodeWriter writer)
		{
			writer.WriteLine($"{Accessibility} {Type} {Name}");
			using (writer.Bracket())
			{
				writer.WriteLine("get");
				using (writer.Bracket())
				{
					writer.WriteLine($"if(m_{Name} == null) m_{Name} = {Create};");
					writer.WriteLine($"return m_{Name};");
				}
			}
			writer.WriteLine($"{Type} m_{Name};");
		}
	}

}
