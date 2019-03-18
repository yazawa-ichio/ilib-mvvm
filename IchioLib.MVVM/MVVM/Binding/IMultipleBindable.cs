using System.Collections;
using System.Collections.Generic;

namespace ILib.MVVM
{
	public interface IMultipleBindable : IViewElement
	{
		void OnPrepare();
		IEnumerable<IBindable> GetBindables();
	}
}
