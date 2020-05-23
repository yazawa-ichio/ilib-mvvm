//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{
	public class WriteLineEmitter : EmitterBase
	{
		public string Message;
		public WriteLineEmitter(string message) => Message = message;
		public override void Emit(CodeWriter writer)
		{
			writer.WriteLine(Message);
		}
	}

}