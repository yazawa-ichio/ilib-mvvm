using System;

namespace ILib.MVVM.Message
{
	internal class Holder : HolderBase, IHolder
	{
		public Action Action = null;

		public override bool IsActive()
		{
			return base.IsActive() && Action != null;
		}

		public void Invoke()
		{
			if (!Removed && Action != null)
			{
				Action();
			}
			else
			{
				Removed = true;
			}
		}

		public bool IsSame(object obj, string eventName, Action target)
		{
			return EventName == eventName && IsSame(obj, target);
		}

		public bool IsSame(object obj, Action target)
		{
			if (Removed) return false;

			if (!Recipient.TryGetTarget(out var recipient) || recipient != obj)
			{
				return false;
			}
			return Action == target;
		}

		[UnityEngine.Scripting.Preserve]
		public static ReferenceHandle Register(Messenger messenger, object target, MessageHandleAttribute attr, bool weakreference)
		{
			var action = (Action)Delegate.CreateDelegate(typeof(Action), target, attr.m_Method);
			if (!weakreference)
			{
				messenger.Register(target, attr.EventName, action);
				return ReferenceHandle.Empty;
			}
			else
			{
				return messenger.WeakRegister(target, attr.EventName, action);
			}
		}

	}


	internal class Holder<T> : HolderBase, IHolder<T>
	{
		public Action<T> Action = null;

		public override bool IsActive()
		{
			return base.IsActive() && Action != null;
		}

		public void Invoke(T message)
		{
			if (!Removed && Action != null)
			{
				Action(message);
			}
			else
			{
				Removed = true;
			}
		}

		public bool IsSame(object obj, string eventName, Action<T> target)
		{
			return EventName == eventName && IsSame(obj, target);
		}

		public bool IsSame(object obj, Action<T> target)
		{
			if (Removed) return false;

			if (!Recipient.TryGetTarget(out var recipient) || recipient != obj)
			{
				return false;
			}

			return Action == target;
		}

		[UnityEngine.Scripting.Preserve]
		public static ReferenceHandle Register(Messenger messenger, object target, MessageHandleAttribute attr, bool weakreference)
		{
			var action = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target, attr.m_Method);
			if (!weakreference)
			{
				messenger.Register(target, attr.EventName, action);
				return ReferenceHandle.Empty;
			}
			else
			{
				return messenger.WeakRegister(target, attr.EventName, action);
			}
		}

	}


}

