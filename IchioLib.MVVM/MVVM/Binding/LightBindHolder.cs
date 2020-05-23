using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ILib.MVVM
{


	[Serializable]
	public class LightBindHolder : IDisposable
	{
		[SerializeField]
		string m_Path = "";

		[SerializeField]
		string m_Type = "";

		[SerializeField]
		Object m_Target = null;

		[NonSerialized]
		ILightBind m_LightBind = default;

		public IBindable Resolve()
		{
			if (m_LightBind == null)
			{
				m_LightBind = LightBindEntry.Get(m_Type);
				m_LightBind.Init(m_Target, m_Path);
			}
			return m_LightBind as IBindable;
		}

		public void Dispose()
		{
			if (m_LightBind != null)
			{
				LightBindEntry.Return(m_Type, m_LightBind);
				m_LightBind = null;
			}
		}

	}


	public class LightBindEntry
	{
		public string TypeName;
		public System.Type Type;
		System.Func<ILightBind> m_Instantiate;
		Stack<ILightBind> m_Pool = new Stack<ILightBind>();
		int m_MaxPoolCount;

		public ILightBind Get()
		{
			if (m_Pool.Count > 0)
			{
				return m_Pool.Pop();
			}
			return m_Instantiate();
		}

		public void Return(ILightBind lightBind)
		{
			if (m_Pool.Count < m_MaxPoolCount)
			{
				if (lightBind.OnReturn())
				{
					m_Pool.Push(lightBind);
				}
			}
		}

		static Dictionary<string, LightBindEntry> s_Dic = new Dictionary<string, LightBindEntry>();

		static LightBindEntry()
		{
			Register<ActiveBind>();
			Register<ImageBind>();
			Register<PostionBind>();
			Register<TextBind>();
			Register<ToggleBind>();
			Register<ButtonBind>();
			Register<InputFieldBind>();
			Register<SliderBind>();
			Register<IntEventButtonBind>();
			Register<FloatEventButtonBind>();
			Register<StringEventButtonBind>();
		}

		public static void Register<T>(int maxPoolCount = 64) where T : ILightBind, new()
		{
			var key = ToKey(typeof(T));
			if (!s_Dic.TryGetValue(key, out var entry))
			{
				s_Dic[key] = entry = new LightBindEntry();
				entry.TypeName = key;
				entry.Type = typeof(T);
			}
			entry.m_Instantiate = () => new T();
			entry.m_MaxPoolCount = maxPoolCount;
		}

		static string ToKey(Type type)
		{
			return type.FullName;
		}

		public static LightBindEntry GetEntry(string key)
		{
			LightBindEntry entry;
			if (s_Dic.TryGetValue(key, out entry))
			{
				return entry;
			}
			var type = System.Type.GetType(key);
			s_Dic[key] = entry = new LightBindEntry();
			entry.m_Instantiate = () => System.Activator.CreateInstance(type) as ILightBind;
			entry.Type = type;
			entry.TypeName = key;
			return entry;
		}


		public static ILightBind Get(string key)
		{
			return GetEntry(key).Get();
		}

		public static void Return(string key, ILightBind lightBind)
		{
			GetEntry(key).Return(lightBind);
		}

		public static string GetKey(Type type)
		{
			return ToKey(type);
		}


	}


}