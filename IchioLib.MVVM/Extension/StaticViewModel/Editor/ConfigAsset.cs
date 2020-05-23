using UnityEngine;
using UnityEditor;
using System.IO;

namespace ILib.MVVM.StaticVM
{
	public class ConfigAsset : ScriptableObject
	{
		static ConfigAsset s_instance;
		public static ConfigAsset I
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = Load();
				}
				return s_instance;
			}
		}

		static ConfigAsset Load()
		{
			foreach (var guid in AssetDatabase.FindAssets("t:ILib.MVVM.StaticVM.ConfigAsset"))
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath<ConfigAsset>(path);
				if (asset != null)
				{
					return asset;
				}
			}
			return null;
		}

		public static void CreateConfig()
		{
			string path = EditorUtility.SaveFilePanelInProject("Create StaticVM.Config", "StaticViewModelConfig", "asset", "Select SavePath");
			if (!string.IsNullOrEmpty(path))
			{
				var config = CreateInstance<ConfigAsset>();
				config.m_GenerateConfig.Output = Path.Combine(Path.GetDirectoryName(path), "Generated");
				AssetDatabase.CreateAsset(config, path);
				Selection.activeObject = config;
				s_instance = config;
			}
		}

		[SerializeField]
		Config m_GenerateConfig = new Config();

		public Config GetConfig()
		{
			return JsonUtility.FromJson<Config>(JsonUtility.ToJson(m_GenerateConfig));
		}

		public void UpdateConfig(Config config)
		{
			if (m_GenerateConfig == null) m_GenerateConfig = new Config();
			JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(config), m_GenerateConfig);
		}

	}
}