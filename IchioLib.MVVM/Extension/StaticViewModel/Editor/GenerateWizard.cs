using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ILib.MVVM.StaticVM
{

	public class GenerateWizard : ScriptableWizard
	{
		static readonly string PreviewShowKey = "ILib.MVVM.GenerateWizard.PreviewShow";

		IView m_Target;
		string m_Name;
		ConfigAsset m_ConfigAsset;
		Config m_Config = new Config();
		bool m_PreviewShow;
		string m_Preview;
		Vector3 m_ScrollPos;

		public void Setup(IView view, string name)
		{
			m_Target = view;
			m_Name = name;
			m_ConfigAsset = ConfigAsset.I;
			if (m_ConfigAsset != null)
			{
				m_Config = m_ConfigAsset.GetConfig();
			}
			UpdatePreviewScripte();
			m_PreviewShow = EditorPrefs.GetBool(PreviewShowKey, true);
		}

		private void UpdatePreviewScripte()
		{
			m_Preview = new Generator(m_Target, m_Name, m_Config).DryRun();
		}

		protected override bool DrawWizardGUI()
		{
			if (m_Target == null)
			{
				Close();
				return false;
			}
			var ret = base.DrawWizardGUI();
			DrawConfigMenu();
			DrawPreview();
			return ret;
		}

		void DrawConfigMenu()
		{
			using (var scope = new EditorGUI.ChangeCheckScope())
			{
				using (new GUILayout.HorizontalScope())
				{
					GUILayout.Label("Generate Setting");
					if (m_ConfigAsset != null)
					{
						GUILayout.Label("ConfigAsset", GUILayout.ExpandWidth(false));
						if (GUILayout.Button("Load", GUILayout.ExpandWidth(false)))
						{
							m_Config = m_ConfigAsset.GetConfig();
							GUI.FocusControl("");
							Event.current.Use();
						}
						if (GUILayout.Button("Apply", GUILayout.ExpandWidth(false)))
						{
							m_ConfigAsset.UpdateConfig(m_Config);
							EditorUtility.SetDirty(m_ConfigAsset);
						}
					}
					else
					{
						GUILayout.Label("ConfigAsset", GUILayout.ExpandWidth(false));
						if (GUILayout.Button("Create", GUILayout.ExpandWidth(false)))
						{
							ConfigAsset.CreateConfig();
							m_ConfigAsset = ConfigAsset.I;
							if (m_ConfigAsset != null)
							{
								m_Config = m_ConfigAsset.GetConfig();
								UpdatePreviewScripte();
							}
						}
					}
				}
				using (new GUILayout.VerticalScope("Box"))
				{
					m_Name = EditorGUILayout.TextField("Name", m_Name);
					m_Config.Output = EditorGUILayout.TextField("Output Dir", m_Config.Output);
					m_Config.NameSpace = EditorGUILayout.TextField("NameSpace", m_Config.NameSpace);
					m_Config.NameSuffix = EditorGUILayout.TextField("NameSuffix", m_Config.NameSuffix);
					m_Config.ReactivePropertyMode = EditorGUILayout.Toggle("ReactivePropertyMode", m_Config.ReactivePropertyMode);
					m_Config.CommandMode = EditorGUILayout.Toggle("CommandMode", m_Config.CommandMode);
					m_Config.CommandSuffix = EditorGUILayout.TextField("CommandSuffix", m_Config.CommandSuffix);
					m_Config.EventValueSuffix = EditorGUILayout.TextField("EventValueSuffix", m_Config.EventValueSuffix);
				}
				if (scope.changed)
				{
					UpdatePreviewScripte();
				}
			}
		}

		void DrawPreview()
		{
			string contenxt = "Preview Output:" + System.IO.Path.Combine(m_Config.Output, m_Name + m_Config.NameSuffix + ".cs");
			var foldout = EditorGUILayout.Foldout(m_PreviewShow, contenxt, true);
			if (foldout != m_PreviewShow)
			{
				EditorPrefs.SetBool(PreviewShowKey, foldout);
				m_PreviewShow = foldout;
			}
			if (!m_PreviewShow) return;
			using (var scope = new GUILayout.ScrollViewScope(m_ScrollPos))
			{
				m_ScrollPos = scope.scrollPosition;
				GUILayout.TextArea(m_Preview);
			}
		}

		void OnWizardCreate()
		{
			string output = System.IO.Path.Combine(m_Config.Output, m_Name + m_Config.NameSuffix + ".cs");
			new Generator(m_Target, m_Name, m_Config).Run(output);
			AssetDatabase.Refresh();
		}
	}

}