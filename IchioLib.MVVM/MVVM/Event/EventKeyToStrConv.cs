using System.Collections.Generic;
namespace ILib.MVVM
{
	public static class EventKeyToStrConv
	{
		static class EnumCache<T>
		{
			public readonly static bool IsEnum;
			static string s_Prefix;
			readonly static Dictionary<T, string> s_Dic;

			static EnumCache()
			{
				IsEnum = typeof(T).IsEnum;
				if (IsEnum)
				{
					s_Dic = new Dictionary<T, string>();
					s_Prefix = typeof(T).FullName + "-";
				}
			}

			public static string Get(T val)
			{
				if (s_Dic.TryGetValue(val, out var key))
				{
					return key;
				}
				return s_Dic[val] = s_Prefix + val.ToString();
			}

		}

		public static string ToStr<T>(T key)
		{
			if (key is string str)
			{
				return str;
			}
			if (typeof(T) == typeof(object))
			{
				return key.GetType().FullName + "-" + key.ToString();
			}
			if (EnumCache<T>.IsEnum)
			{
				return EnumCache<T>.Get(key);
			}
			else
			{
				return key.GetType().FullName + "-" + key.ToString();
			}
		}
	}

}