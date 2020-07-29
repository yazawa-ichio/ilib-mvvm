using System;
using System.Collections.Generic;

namespace ILib.MVVM
{

	public static class ViewUpdater
	{
		static List<IView> s_Views = new List<IView>();
		static Predicate<IView> s_CheckActive;

		static ViewUpdater()
		{
			ViewUpdaterInstance.TryInit();
			s_CheckActive = (x) => x.IsActive;
		}

		public static void Register(IView view)
		{
			s_Views.Add(view);
		}

		static void UpdateImpl()
		{
			bool remove = false;
			for (int i = 0; i < s_Views.Count; i++)
			{
				var view = s_Views[i];
				if (view.IsActive)
				{
					view.TryUpdate();
				}
				else
				{
					remove = true;
				}
			}
			if (remove)
			{
				s_Views.RemoveAll(s_CheckActive);
			}
		}

		public static void Update()
		{
			UpdateImpl();
		}

	}

}