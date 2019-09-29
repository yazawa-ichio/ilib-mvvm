using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ILib.MVVM.Drawer
{
	[CustomEditor(typeof(View), editorForChildClasses:true)]
	public class ViewDrawer : Editor
	{
		static Dictionary<int, BindingViewer> m_Viewers = new Dictionary<int, BindingViewer>();

		[SerializeField]
		BindingViewer m_BindingViewer;
		[SerializeField]
		bool m_DefaultView;

		private void OnEnable()
		{
			var view = target as View;
			if (m_BindingViewer == null)
			{
				if (!m_Viewers.TryGetValue(view.GetInstanceID(), out m_BindingViewer))
				{
					m_BindingViewer = new BindingViewer();
				}
			}
			m_BindingViewer.Setup(target as View);
			m_Viewers[view.GetInstanceID()] = m_BindingViewer; 
		}

		private void OnDisable()
		{
			 
		}

		public override void OnInspectorGUI()
		{
			m_DefaultView = EditorGUILayout.Toggle("DefaultView", m_DefaultView);
			if (m_DefaultView)
			{
				base.OnInspectorGUI();
			}
			else
			{
				m_BindingViewer.Draw();
			}
		}

	}

}
