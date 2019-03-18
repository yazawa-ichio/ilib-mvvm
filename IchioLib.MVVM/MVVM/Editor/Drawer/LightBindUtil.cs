using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ILib.MVVM.Drawer
{
	public static class LightBindUtil
	{
		public readonly static string[] DispNames;
		public readonly static string[] TypeNames;
		public readonly static System.Type[] TargetTypes;

		static LightBindUtil()
		{
			var field = typeof(LightBindEntry).GetField("s_Dic", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
			var entries = (field.GetValue(null) as Dictionary<string, LightBindEntry>).Values.ToArray();
			DispNames = Enumerable.Concat(new string[] { "None" }, entries.Select(x => x.Type.Name)).ToArray();
			TypeNames = Enumerable.Concat(new string[] { "" }, entries.Select(x => x.TypeName)).ToArray();
			TargetTypes = Enumerable.Concat(new System.Type[] { typeof(Object) }, entries.Select(x => GetTragetType(x.Type))).ToArray();
		}

		static System.Type GetTragetType(System.Type type)
		{
			var cur = type;
			while (cur != null)
			{
				if (cur.IsGenericType && typeof(LightBind<,>) == cur.GetGenericTypeDefinition())
				{
					var genericTypes = cur.GenericTypeArguments;
					return genericTypes[1];
				}
				cur = cur.BaseType;
			}

			return typeof(Object);
		}

		public static void AddCreateMenuItem(GenericMenu menu, SerializedProperty arrayProp, System.Action<int> onCreate, bool cancel = false)
		{
			for (int i = 0; i < TypeNames.Length; i++)
			{
				menu.AddItem(new GUIContent(DispNames[i]), false, (obj) =>
				{
					arrayProp.serializedObject.Update();
					int arraySize = arrayProp.arraySize;
					arrayProp.InsertArrayElementAtIndex(arraySize);
					var prop = arrayProp.GetArrayElementAtIndex(arraySize);
					prop.FindPropertyRelative("m_Path").stringValue = "New";
					prop.FindPropertyRelative("m_Type").stringValue = obj.ToString();
					prop.FindPropertyRelative("m_Target").objectReferenceValue = null;
					arrayProp.serializedObject.ApplyModifiedProperties();
					onCreate(arraySize);
				}, TypeNames[i]);
			}
			if (cancel)
			{
				menu.AddItem(new GUIContent("Cancel"), false, () => { });
			}
		}
	}
}
