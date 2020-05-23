using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ILib.MVVM.Drawer
{
	public class LightBindViewer : IElementViewer
	{
		readonly static string[] s_DispNames = LightBindUtil.DispNames;
		readonly static string[] s_TypeNames = LightBindUtil.TypeNames;
		readonly static System.Type[] s_TargetTypes = LightBindUtil.TargetTypes;

		View m_View;
		SerializedProperty m_Root;
		SerializedProperty m_Prop;
		int m_Index;

		public LightBindViewer(View view, SerializedProperty root, int index)
		{
			m_View = view;
			m_Root = root;
			m_Prop = m_Root.GetArrayElementAtIndex(index);
			m_Index = index;
		}

		public void DrawInspector()
		{
			var typeProp = m_Prop.FindPropertyRelative("m_Type");
			var pathProp = m_Prop.FindPropertyRelative("m_Path");
			var targetProp = m_Prop.FindPropertyRelative("m_Target");


			GUILayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup(true);
			{
				EditorGUILayout.ObjectField("LightBind", m_View, typeof(MonoBehaviour), allowSceneObjects: true);

			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();

			var index = System.Array.IndexOf(s_TypeNames, typeProp.stringValue);
			var selectIndex = EditorGUILayout.Popup("Type", index, s_DispNames);
			if (index != selectIndex)
			{
				typeProp.stringValue = selectIndex <= 0 ? "" : s_TypeNames[selectIndex];
				index = selectIndex;
			}
			EditorGUILayout.PropertyField(pathProp);


			EditorGUILayout.ObjectField(targetProp, s_TargetTypes[selectIndex]);
			var target = targetProp.objectReferenceValue;
			if (target != null && (selectIndex == 0 || !s_TargetTypes[selectIndex].IsAssignableFrom(target.GetType())))
			{
				targetProp.objectReferenceValue = null;
			}
		}

		public string GetName()
		{
			var path = m_Prop.FindPropertyRelative("m_Path").stringValue;
			var typeStr = m_Prop.FindPropertyRelative("m_Type").stringValue;
			var entry = LightBindEntry.GetEntry(typeStr);
			string typeName = entry?.Type.Name ?? "?" + typeStr;
			return $"{path}({typeName})";
		}

		public bool CanRemove()
		{
			return true;
		}

		public void Remove()
		{
			m_Root.serializedObject.Update();
			m_Root.DeleteArrayElementAtIndex(m_Index);
			m_Root.serializedObject.ApplyModifiedProperties();
		}

		public bool IsBindable()
		{
			return true;
		}

		public bool DrawBinding(bool selected)
		{
			bool ret;
			var c = GUI.backgroundColor;
			GUI.backgroundColor = new Color(1f, 1f, 1f, 0.75f);
			if (selected)
			{
				GUILayout.BeginHorizontal("SelectionRect");
			}
			else
			{
				GUILayout.BeginHorizontal();
			}
			{
				ret = GUILayout.Button(GetName(), "Label");
			}
			GUILayout.EndHorizontal();
			GUI.backgroundColor = c;
			return ret;
		}

	}

}