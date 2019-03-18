using UnityEngine;
using UnityEditor;

namespace ILib.MVVM.StaticVM
{
	public static class ToolMenu
	{
		[MenuItem("CONTEXT/View/StaticVM CreateConfig")]
		static void CreateConfig()
		{
			if (ConfigAsset.I == null)
			{
				ConfigAsset.CreateConfig();
			}
			else
			{
				Selection.activeObject = ConfigAsset.I;
			}
		}

		[MenuItem("CONTEXT/View/StaticVM Generate", true)]
		static bool GenerateValidate(MenuCommand command)
		{
			return command.context is IView;
		}

		[MenuItem("CONTEXT/View/StaticVM Generate")]
		static void Generate(MenuCommand command)
		{
			var view = command.context as IView;
			var wizard = ScriptableWizard.CreateInstance<GenerateWizard>();
			wizard.Setup(view, command.context.name);
			wizard.ShowUtility();
		}

		[MenuItem("CONTEXT/View/StaticVM Quick Generate", true)]
		static bool QuickGenerateValidate(MenuCommand command)
		{
			return command.context is IView && ConfigAsset.I != null;
		}

		[MenuItem("CONTEXT/View/StaticVM Quick Generate")]
		static void QuickGenerate(MenuCommand command)
		{
			var view = command.context as IView;
			var config = ConfigAsset.I.GetConfig();
			var name = command.context.name;
			string output = System.IO.Path.Combine(config.Output, name + config.NameSuffix + ".cs");
			new Generator(view, name, config).Run(output);
			AssetDatabase.Refresh();
		}

		public static void AllQuickGenerate(string[] paths)
		{
			var config = ConfigAsset.I.GetConfig();
			foreach (var path in paths)
			{
				if (!path.StartsWith("Assets")) continue;
				GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
				var view = obj.GetComponent<IView>();
				if (view == null) continue;

				var name = System.IO.Path.GetFileNameWithoutExtension(path);
				string output = System.IO.Path.Combine(config.Output, name + config.NameSuffix + ".cs");
				new Generator(view, name, config).Run(output);
			}
			AssetDatabase.Refresh();
		}

	}
}
