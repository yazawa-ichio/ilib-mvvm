using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{
	public abstract class EventArgument : MonoBehaviour
	{
		public abstract System.Type GetEventType();
		public abstract void Do(string name, IViewEventHandler handler);
	}

	public abstract class EventArgument<T> : EventArgument
	{
		public abstract T GetValue();

		public override System.Type GetEventType() => typeof(T);

		public override void Do(string name, IViewEventHandler handler)
		{
			handler?.OnViewEvent<T>(name, GetValue());
		}

	}
}
