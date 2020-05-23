using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	internal class BindingEventBase : IBindingEvent
	{
		public string Name { get; private set; }
		public BindingEventBase(string name)
		{
			Name = name;
		}
	}

	internal class BindingEvent : BindingEventBase
	{
		public event System.Action OnEvent;
		public BindingEvent(string name) : base(name) { }
		public void Invoke()
		{
			OnEvent?.Invoke();
		}
		public override string ToString()
		{
			if (OnEvent == null)
			{
				return "Empty";
			}
			return $"{OnEvent.Method}:({OnEvent.Target})";
		}
	}

	internal class BindingEvent<T> : BindingEventBase
	{
		public event System.Action<T> OnEvent;
		public BindingEvent(string name) : base(name) { }
		public void Invoke(T val)
		{
			OnEvent?.Invoke(val);
		}
		public override string ToString()
		{
			var target = OnEvent?.Target ?? null;
			return $"{OnEvent.Method}:({OnEvent.Target})";
		}
	}

}