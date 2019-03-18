using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace ILib.MVVM.Drawer
{
	[CustomPropertyDrawer(typeof(LightBindHolder))]
	public class LightBindHolderDrawer : PropertyDrawer
	{
		readonly static string[] s_DispNames = LightBindUtil.DispNames;
		readonly static string[] s_TypeNames = LightBindUtil.TypeNames;
		readonly static System.Type[] s_TargetTypes = LightBindUtil.TargetTypes;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) * 4;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			float height = position.height / 4; ;
			var rect = new Rect(position);
			rect.height = height;
			EditorGUI.LabelField(rect, label.text);

			using (new EditorGUI.IndentLevelScope())
			{
				rect.y += height;
				var typeProp = property.FindPropertyRelative("m_Type");
				var index = System.Array.IndexOf(s_TypeNames, typeProp.stringValue);
				var selectIndex = EditorGUI.Popup(rect, "Type", index, s_DispNames);
				if (index != selectIndex)
				{
					typeProp.stringValue = selectIndex <= 0 ? "" : s_TypeNames[selectIndex];
					index = selectIndex;
				}
				rect.y += height;
				EditorGUI.PropertyField(rect, property.FindPropertyRelative("m_Path"));
				rect.y += height;
				var targetProp = property.FindPropertyRelative("m_Target");
				EditorGUI.ObjectField(rect, targetProp, s_TargetTypes[selectIndex]);
				var target = targetProp.objectReferenceValue;
				if (target != null && (selectIndex == 0 || !s_TargetTypes[selectIndex].IsAssignableFrom(target.GetType())))
				{
					targetProp.objectReferenceValue = null;
				}
			}
			EditorGUI.EndProperty();
		}
	}
}
