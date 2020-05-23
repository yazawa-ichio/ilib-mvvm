namespace ILib.MVVM
{
	using ILib.MVVM.Message;
	using System;

	public static class Messenger
	{
		public static IMessenger Default { get; private set; } = Create();

		public static IMessenger Create() => new MessengerImpl();

		public static IMessengerHook Hook { get => Default.Hook; set => Default.Hook = value; }

		public static void Refresh()
		{
			Default.Refresh();
		}

		public static void Register<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action)
		{
			Default.Register(recipient, eventName, action);
		}

		public static void Register<TMessage>(object recipient, object eventName, Action<TMessage> action)
		{
			Default.Register(recipient, eventName, action);
		}

		public static void Register<TEvent>(object recipient, TEvent eventName, Action action)
		{
			Default.Register(recipient, eventName, action);
		}

		public static void RegisterHandle(object target)
		{
			Default.RegisterHandle(target);
		}

		public static void Send<TEvent, TMessage>(TEvent eventName, TMessage message)
		{
			Default.Send(eventName, message);
		}

		public static void Send<TEvent>(TEvent eventName)
		{
			Default.Send(eventName);
		}

		public static void Unregister<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action)
		{
			Default.Unregister(recipient, eventName, action);
		}

		public static void Unregister<TMessage>(object recipient, object eventName, Action<TMessage> action)
		{
			Default.Unregister(recipient, eventName, action);
		}

		public static void Unregister<TEvent>(object recipient, TEvent eventName, Action action)
		{
			Default.Unregister(recipient, eventName, action);
		}

		public static void Unregister(object recipient)
		{
			Default.Unregister(recipient);
		}

		public static ReferenceHandle WeakRegister<TEvent, TMessage>(object recipient, TEvent eventName, Action<TMessage> action)
		{
			return Default.WeakRegister(recipient, eventName, action);
		}

		public static ReferenceHandle WeakRegister<TMessage>(object recipient, object eventName, Action<TMessage> action)
		{
			return Default.WeakRegister(recipient, eventName, action);
		}

		public static ReferenceHandle WeakRegister<TEvent>(object recipient, TEvent eventName, Action action)
		{
			return Default.WeakRegister(recipient, eventName, action);
		}

		public static ReferenceHandle WeakRegisterHandle(object target)
		{
			return Default.WeakRegisterHandle(target);
		}
	}
}