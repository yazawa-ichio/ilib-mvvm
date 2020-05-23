using ILib.MVVM.Message;
using System;

namespace ILib.MVVM
{
	public interface IMessenger
	{
		IMessengerHook Hook { get; set; }

		void Register<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action);

		void Register<TMessage>(object recipient, object eventName, Action<TMessage> action);

		void Register<TEvent>(object recipient, TEvent eventName, Action action);

		void RegisterHandle(object target);

		ReferenceHandle WeakRegister<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action);

		ReferenceHandle WeakRegister<TMessage>(object recipient, object eventName, Action<TMessage> action);

		ReferenceHandle WeakRegister<TEvent>(object recipient, TEvent eventName, Action action);

		ReferenceHandle WeakRegisterHandle(object target);

		void Unregister<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action);

		void Unregister<TMessage>(object recipient, object eventName, Action<TMessage> action);

		void Unregister<TEvent>(object recipient, TEvent eventName, Action action);

		void Unregister(object recipient);

		void Send<TEvent, TMessage>(TEvent eventName, TMessage message);

		void Send<TEvent>(TEvent eventName);

		void Refresh();

	}

}