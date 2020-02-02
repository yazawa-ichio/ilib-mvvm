using System;
using System.Reflection;

namespace ILib.MVVM
{
	using Message;

	internal delegate ReferenceHandle RegisterDelegate(Messenger messenger, object target, MessageHandleAttribute attribute, bool weakreference);

	[AttributeUsage(AttributeTargets.Method)]
	public class MessageHandleAttribute : Attribute
	{
		public string EventName { get; private set; }

		internal MethodInfo m_Method;

		internal RegisterDelegate m_Register;

		public MessageHandleAttribute(object eventName)
		{
			EventName = EventKeyToStrConv.ToStr(eventName);
		}

		public static void Preload(Type type)
		{
			MessageHandleUtil.GetAttributes(type);
		}

		internal static ReferenceHandle Register(Messenger messenger, object target, bool weakreference)
		{
			var attrs = MessageHandleUtil.GetAttributes(target.GetType());
			var handles = new ReferenceHandle[attrs.Length];
			for (int i = 0; i < attrs.Length; i++)
			{
				var attr = attrs[i];
				handles[i] = attr.m_Register(messenger, target, attr, weakreference);
			}
			return new ReferenceHandle(handles);
		}

	}
}

