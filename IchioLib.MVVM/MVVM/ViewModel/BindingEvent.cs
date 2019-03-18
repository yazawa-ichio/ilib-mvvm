using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	internal class BindingEventBase : IBindingEvent
	{
		public string Name { get; private set; }
		internal BindingEventBase Next;
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
			var target = OnEvent?.Target ?? null;
			return $"{target}";
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
			return $"{target}";
		}

	}

}
