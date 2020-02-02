using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{
	public static class EventKeyToStrConv
	{
		public static string ToStr<T>(T key)
		{
			if (key is string str)
			{
				return str;
			}
			return key.GetType().FullName + "-" + key.ToString();
		}
	}

}
