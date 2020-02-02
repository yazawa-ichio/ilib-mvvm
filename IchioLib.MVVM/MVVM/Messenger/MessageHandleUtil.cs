using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ILib.MVVM.Message
{
	internal static class MessageHandleUtil
	{

		static Dictionary<Type, MessageHandleAttribute[]> s_Cache = new Dictionary<Type, MessageHandleAttribute[]>();
		static Dictionary<Type, RegisterDelegate> s_Register = new Dictionary<Type, RegisterDelegate>();

		public static MessageHandleAttribute[] GetAttributes(Type type)
		{
			MessageHandleAttribute[] ret = null;
			if (!s_Cache.TryGetValue(type, out ret))
			{
				ret = GetAttributesImpl(type).ToArray();
			}
			return ret;
		}

		static IEnumerable<MessageHandleAttribute> GetAttributesImpl(Type type)
		{
			var cur = type;
			do
			{
				foreach (var method in cur.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
				{
					if (Attribute.IsDefined(method, typeof(MessageHandleAttribute)))
					{
						foreach (var attr in method.GetCustomAttributes<MessageHandleAttribute>())
						{
							if (attr.m_Register == null)
							{
								attr.m_Method = method;
								attr.m_Register = GetRegister(method);
							}
							yield return attr;
						}
					}
				}
				cur = cur.BaseType;
			} while (cur != null);
		}

		static RegisterDelegate GetRegister(MethodInfo info)
		{
			var prms = info.GetParameters();
			if (prms.Length == 0)
			{
				return Holder.Register;
			}
			else if (prms.Length == 1)
			{
				var prmType = prms[0].ParameterType;
				if (s_Register.TryGetValue(prmType, out var ret))
				{
					return ret;
				}
				var holder = typeof(Holder<>).MakeGenericType(prmType);
				var method = holder.GetMethod("Register", BindingFlags.Public | BindingFlags.Static);
				var action = Delegate.CreateDelegate(typeof(RegisterDelegate), method);
				return s_Register[prmType] = (RegisterDelegate)action;
			}
			else
			{
				throw new InvalidOperationException("関数のパラメーターの引数は1つのみしか指定できません");
			}
		}

	}
}

