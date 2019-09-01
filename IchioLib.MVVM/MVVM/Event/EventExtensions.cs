using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	public static class EventExtensions
	{
		public static void SubscribeViewEvent<T>(this IViewModel self, T key, System.Action onViewEvent)
		{
			var str = EventKeyToStrConv.ToStr(key);
			self.SubscribeViewEvent(str, onViewEvent);
		}

		public static void UnsubscribeViewEvent<T>(this IViewModel self, T key, System.Action onViewEvent)
		{
			var str = EventKeyToStrConv.ToStr(key);
			self.UnsubscribeViewEvent(str, onViewEvent);
		}

		public static void SubscribeViewEvent<T, U>(this IViewModel self, T key, System.Action<U> onViewEvent)
		{
			var str = EventKeyToStrConv.ToStr(key);
			self.SubscribeViewEvent(str, onViewEvent);
		}

		public static void UnsubscribeViewEvent<T, U>(this IViewModel self, T key, System.Action<U> onViewEvent)
		{
			var str = EventKeyToStrConv.ToStr(key);
			self.UnsubscribeViewEvent(str, onViewEvent);
		}
	}

}
