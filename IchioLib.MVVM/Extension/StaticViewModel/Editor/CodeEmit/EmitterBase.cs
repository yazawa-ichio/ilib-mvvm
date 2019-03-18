using System.Collections;
using System.Collections.Generic;

//namespace ILib.CodeEmit
namespace ILib.MVVM.CodeEmit
{

	public abstract class EmitterBase
	{
		public abstract void Emit(CodeWriter writer);
	}

}
