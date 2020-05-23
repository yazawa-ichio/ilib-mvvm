using UnityEditor;
using UnityEngine;

namespace ILib.MVVM.Drawer
{
	public class ComponentElementViewer : IElementViewer
	{
		MonoBehaviour m_Element;
		Editor m_Editor;
		IBindable m_Bindable;

		public ComponentElementViewer(MonoBehaviour element)
		{
			m_Element = element;
			m_Editor = Editor.CreateEditor(element);
			m_Bindable = m_Element as IBindable;
		}

		public void DrawInspector()
		{
			EditorGUILayout.ObjectField("Bind GameObject", m_Element.gameObject, typeof(GameObject), allowSceneObjects: true);
			m_Editor.OnInspectorGUI();
		}

		public string GetName()
		{
			if (m_Bindable == null)
			{
				return $"{m_Element.name}({m_Element.GetType().Name})";
			}
			else
			{
				return $"{m_Bindable.Path}({m_Bindable.GetType().Name})";
			}
		}

		public bool CanRemove()
		{
			return false;
		}

		public void Remove()
		{
		}

		public bool IsBindable()
		{
			return m_Bindable != null;
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