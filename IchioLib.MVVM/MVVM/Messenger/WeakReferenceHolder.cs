using System;

namespace ILib.MVVM.Message
{

	internal class WeakReferenceHolder : HolderBase, IHolder
	{
		public WeakReference<Action> Action = null;
		public WeakReference<ReferenceHandle> Handle;

		public override bool IsActive()
		{
			return base.IsActive() && Action.TryGetTarget(out _) && ReferenceHandle.IsActive(Handle);
		}

		public void Invoke()
		{
			if (!Removed && Action.TryGetTarget(out var action))
			{
				action?.Invoke();
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

			if (!Action.TryGetTarget(out var action) || action != target)
			{
				return false;
			}

			return true;
		}

	}

	internal class WeakReferenceHolder<T> : HolderBase, IHolder<T>
	{
		public WeakReference<Action<T>> Action = null;
		public WeakReference<ReferenceHandle> Handle;

		public override bool IsActive()
		{
			return base.IsActive() && Action.TryGetTarget(out _) && ReferenceHandle.IsActive(Handle); ;
		}

		public void Invoke(T message)
		{
			if (!Removed && Action.TryGetTarget(out var action))
			{
				action?.Invoke(message);
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

			if (!Action.TryGetTarget(out var action) || action != target)
			{
				return false;
			}

			return true;
		}


	}

}

