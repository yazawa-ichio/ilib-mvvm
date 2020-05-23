using UnityEngine;

namespace ILib.MVVM
{
	[DefaultExecutionOrder(1000)]
	internal class ViewUpdaterInstance : MonoBehaviour
	{
		static ViewUpdaterInstance s_Instance;
		public static void TryInit()
		{
			if (s_Instance != null) return;
			GameObject obj = new GameObject("ViewUpdaterInstance");
			GameObject.DontDestroyOnLoad(obj);
			s_Instance = obj.AddComponent<ViewUpdaterInstance>();
		}

		private void LateUpdate()
		{
			ViewUpdater.Update();
		}

	}

}