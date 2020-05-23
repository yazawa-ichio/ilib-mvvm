using System;

namespace ILib.MVVM.Message
{
	internal interface IHolder
	{
		string EventName { get; }
		bool IsActive();
		void Invoke();
		bool IsSame(object recipient, Action action);
	}

	internal interface IHolder<T>
	{
		string EventName { get; }
		bool IsActive();
		void Invoke(T message);
		bool IsSame(object recipient, Action<T> action);
	}
}