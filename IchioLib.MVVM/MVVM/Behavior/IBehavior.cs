using System.Collections;
using System.Collections.Generic;

namespace ILib.MVVM
{
	public interface IBehavior : IMultipleBindable
	{
		string MessangerPath { get; }
		Messenger Messanger { get; }
	}
}
