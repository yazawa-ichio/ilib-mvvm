using System;

namespace ILib.MVVM.Message
{

	internal abstract class HolderBase
	{
		public string EventName { get; set; }

		public WeakReference<object> Recipient = null;

		public bool Removed = false;

		public virtual bool IsActive()
		{
			if (Removed) return false;

			if (!Recipient.TryGetTarget(out var recipient)) return false;

			// Equals(null)でUnityの破棄判定を得られる
			return recipient != null && !recipient.Equals(null);
		}

		public bool IsRecipient(object obj)
		{
			if (Recipient.TryGetTarget(out var recipient) && recipient == obj)
			{
				return true;
			}
			return false;
		}

		public bool HasEvent(object obj, string eventName)
		{
			return IsActive() && EventName == eventName && IsRecipient(obj);
		}

	}



}

