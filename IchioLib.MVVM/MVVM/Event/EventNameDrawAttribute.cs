#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ILib.MVVM
{
	[AttributeUsage(AttributeTargets.Field)]
	public class EventNameDrawAttribute : PropertyAttribute
	{
	}

	[AttributeUsage(AttributeTargets.Field), Obsolete("use EventNameDrawAttribute")]
	public class EventNameAttribute : EventNameDrawAttribute
	{
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(EventNameDrawAttribute))]
	class EventNameDrawer : PropertyDrawer
	{
		static string[] s_Keys;
		static string[] s_Values;

		static EventNameDrawer()
		{
			var dic = new Dictionary<string, string>();
			dic[""] = "None";
			foreach (var type in GetEventKeyTypes())
			{
				string baseName = type.FullName;
				foreach (var val in Enum.GetValues(type))
				{
					var key = EventKeyToStrConv.ToStr(val);
					dic[key] = baseName + "/" + val;
				}
			}
			s_Keys = dic.Keys.ToArray();
			s_Values = dic.Values.ToArray();
		}

		static IEnumerable<Type> GetEventKeyTypes()
		{
			foreach (var assemblie in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assemblie.GetTypes())
				{
					if (Attribute.IsDefined(type, typeof(EventKeyAttribute)))
					{
						yield return type;
					}
				}
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var key = property.stringValue;
			var index = Array.IndexOf(s_Keys, key);
			if (index < 0)
			{
				index = 0;
			}
			var ret = EditorGUI.Popup(position, label.text, index, s_Values);
			if (ret != index)
			{
				property.stringValue = s_Keys[ret];
			}
		}
	}

#endif

}