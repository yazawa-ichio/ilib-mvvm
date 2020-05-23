//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{
	public class DelegateEmitter : EmitterBase
	{
		public System.Action<CodeWriter> Action;
		public DelegateEmitter(System.Action<CodeWriter> action) => Action = action;
		public override void Emit(CodeWriter writer)
		{
			Action(writer);
		}
	}

}