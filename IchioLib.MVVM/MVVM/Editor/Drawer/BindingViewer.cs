using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ILib.MVVM.Drawer
{

	[System.Serializable]
	public class BindingViewer
	{
		static string s_BindableListPrefKey = "ILIB.MVVM.Drawer-BindableList";
		static string s_ViewModelPrefKey = "ILIB.MVVM.Drawer-ViewModel";

		[SerializeField]
		int m_Index;

		View m_View;
		SerializedObject m_Serialized;
		SerializedProperty m_LightBinds;

		int m_LightBindsCount = -1;
		List<IViewElement> m_TempElements = new List<IViewElement>();
		HashSet<IViewElement> m_Elements;

		string[] m_ElementNames;
		List<IElementViewer> m_ElementViewers = new List<IElementViewer>();

		public void Setup(View view)
		{
			m_View = view;
			UpdateList();
		}

		bool IsChange()
		{
			if (Event.current.type != EventType.Layout)
			{
				return false;
			}

			if (m_LightBindsCount != m_LightBinds.arraySize)
			{
				return true;
			}
			m_TempElements.Clear();
			m_View.GetComponentsInChildren(true, m_TempElements);
			foreach (var elm in m_TempElements)
			{
				if (!m_Elements.Contains(elm))
				{
					return true;
				}
			}
			return false;
		}

		public void UpdateList()
		{
			m_Serialized = new SerializedObject(m_View);
			m_LightBinds = m_Serialized.FindProperty("m_LightBinds");
			m_LightBindsCount = m_LightBinds.arraySize;
			m_Elements = new HashSet<IViewElement>();
			m_View.GetComponentsInChildren(true, m_TempElements);
			m_Elements.UnionWith(m_TempElements);

			foreach (var childView in m_View.GetComponentsInChildren<View>(true))
			{
				if (childView == m_View) continue;
				m_TempElements.Clear();
				childView.GetComponentsInChildren(true, m_TempElements);
				foreach (var elm in m_TempElements)
				{
					m_Elements.Remove(elm);
				}
			}

			m_TempElements.Clear();

			m_ElementViewers.Clear();
			for (int i = 0; i < m_LightBindsCount; i++)
			{
				m_ElementViewers.Add(new LightBindViewer(m_View, m_LightBinds, i));
			}
			foreach (var _elm in m_Elements)
			{
				var elm = (MonoBehaviour)_elm;
				if (elm != null)
				{
					m_ElementViewers.Add(new ComponentElementViewer(elm));
				}
			}
			m_ElementNames = new string[m_ElementViewers.Count];

		}


		public void Draw()
		{
			if (m_View == null)
			{
				GUILayout.Label("None Binding.");
				return;
			}

			m_Serialized.Update();

			if (IsChange())
			{
				UpdateList();
			}

			for (int i = 0; i < m_ElementViewers.Count; i++)
			{
				m_ElementNames[i] = m_ElementViewers[i].GetName();
			}

			DrawElementInspector();
			DrawBindableList();
			DrawViewModel();

			m_Serialized.ApplyModifiedProperties();
		}

		void DrawElementInspector()
		{
			GUILayout.Label("Element Inspector");

			using (new GUILayout.VerticalScope("box"))
			{
				using (new GUILayout.HorizontalScope())
				{
					GUILayout.Label("Select Element", GUILayout.ExpandWidth(false));
					m_Index = EditorGUILayout.Popup(m_Index, m_ElementNames);

					if (GUILayout.Button("", "OL Plus", GUILayout.ExpandWidth(false)))
					{
						ShowAddMenu();
						return;
					}
					using (new EditorGUI.DisabledScope((m_ElementViewers.Count == 0) ? true : !m_ElementViewers[m_Index].CanRemove()))
					{
						if (GUILayout.Button("", "OL Minus", GUILayout.ExpandWidth(false)))
						{
							ShowRemoveMenu(m_Index);
							UpdateList();
						}
					}
				}
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				if (m_Index >= 0 && m_Index < m_ElementViewers.Count)
				{
					m_ElementViewers[m_Index].DrawInspector();
				}
				else
				{
					GUILayout.Label("None Select Elements");
				}
			}
		}

		void DrawBindableList()
		{
			var foldout = EditorPrefs.GetBool(s_BindableListPrefKey, true);
			var ret = EditorGUILayout.Foldout(EditorPrefs.GetBool(s_BindableListPrefKey, true), "Bindable List");
			if (foldout != ret)
			{
				foldout = ret;
				EditorPrefs.SetBool(s_BindableListPrefKey, ret);
			}
			if (!foldout)
			{
				return;
			}
			using (new GUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Path(Type)");
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				for (int i = 0; i < m_ElementViewers.Count; i++)
				{
					IElementViewer elm = m_ElementViewers[i];
					if (!elm.IsBindable()) continue;
					if (elm.DrawBinding(m_Index == i))
					{
						m_Index = i;
					}
				}
			}
		}

		void DrawViewModel()
		{
			var foldout = EditorPrefs.GetBool(s_ViewModelPrefKey, true);
			var ret = EditorGUILayout.Foldout(foldout, "ViewModel");
			if (ret != foldout)
			{
				foldout = ret;
				EditorPrefs.SetBool(s_ViewModelPrefKey, ret);
			}
			if (!ret)
			{
				return;
			}
			var vm = m_View.DataContext;
			if (vm == null)
			{
				GUILayout.Label("not attach ViewModel");
				return;
			}
			using (new GUILayout.VerticalScope("box"))
			{
				EditorGUILayout.LabelField("Property Path(Type)", "Value");
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				using (var scope = ViewUtil.UseBindingPropertyListStack())
				{
					foreach (var prop in vm.Property.GetAll(scope.List))
					{
						EditorGUILayout.LabelField($"{prop.Path}({prop.GetBindType().Name})", prop.ToString());
					}
				}
			}

			using (new GUILayout.VerticalScope("box"))
			{
				EditorGUILayout.LabelField("Event Name", "Target");
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				foreach (var e in vm.Event.GetAll())
				{
					EditorGUILayout.LabelField(e.Name, e.ToString());
				}
			}


		}

		void ShowAddMenu()
		{
			GenericMenu menu = new GenericMenu();
			LightBindUtil.AddCreateMenuItem(menu, m_LightBinds, (index) =>
			{
				m_Index = index;
			}, true);
			menu.ShowAsContext();
		}

		void ShowRemoveMenu(int index)
		{
			if (EditorUtility.DisplayDialog("確認", "削除しますか？", "OK"))
			{
				m_Serialized.Update();
				m_LightBinds.DeleteArrayElementAtIndex(index);
				if (m_Index > 0) m_Index = index - 1;
				m_Serialized.ApplyModifiedProperties();
			}
		}

	}

}