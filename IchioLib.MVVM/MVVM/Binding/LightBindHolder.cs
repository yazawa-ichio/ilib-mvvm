using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{


	[System.Serializable]
	public class LightBindHolder
	{
		[SerializeField]
		string m_Path = "";

		public string Path { get { return m_Path; } set { m_Path = value; } }

		[SerializeField]
		string m_Type = "";

		public System.Type Type
		{
			set { m_Type = LightBindEntry.GetKey(value); }
			get { return LightBindEntry.GetEntry(m_Type)?.Type ?? null; }
		}


		[SerializeField]
		Object m_Target = null;
		public Object Target => m_Target;

		public IBindable Resolve()
		{
			var bind = LightBindEntry.Instantiate(m_Type);
			bind.Init(m_Target, m_Path);
			return bind as IBindable;
		}

	}


	public class LightBindEntry
	{
		public string TypeName;
		public System.Type Type;

		static Dictionary<string, LightBindEntry> s_Dic = new Dictionary<string, LightBindEntry>();

		static LightBindEntry()
		{
			foreach (var type in GetLightBindType())
			{
				string key = ToKey(type);
				s_Dic[key] = new LightBindEntry { TypeName = key, Type = type };
			}
		}

		static IEnumerable<System.Type> GetLightBindType()
		{
			foreach (var assemblie in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assemblie.GetTypes())
				{
					if (type.IsAbstract) continue;
					if (typeof(ILightBind).IsAssignableFrom(type) && typeof(IBindable).IsAssignableFrom(type))
					{
						yield return type;
					}
				}
			}
		}

		static string ToKey(System.Type type)
		{
			return type.FullName;
		}

		public static LightBindEntry GetEntry(string key)
		{
			LightBindEntry entry;
			s_Dic.TryGetValue(key, out entry);
			return entry;
		}


		public static ILightBind Instantiate(string key)
		{
			var type = s_Dic[key].Type;
			return System.Activator.CreateInstance(type) as ILightBind;
		}

		public static string GetKey(System.Type type)
		{
			return ToKey(type);
		}

	}


}
