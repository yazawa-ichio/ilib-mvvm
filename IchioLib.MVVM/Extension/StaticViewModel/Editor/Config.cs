using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM.StaticVM
{
	[System.Serializable]
	public class Config
	{
		public string Output = "Assets/";
		public string NameSpace = "App.MVVM";
		public string NameSuffix = "VM";
		public bool ReactivePropertyMode = true;
		public bool CommandMode = true;
		public string CommandSuffix = "";
		public string EventValueSuffix = "Value";
	}
}