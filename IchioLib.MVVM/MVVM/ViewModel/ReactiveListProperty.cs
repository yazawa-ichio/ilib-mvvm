using System.Collections.Generic;

namespace ILib.MVVM
{
	public class ReactiveListProperty<T> : System.IDisposable
	{
		public readonly string Path;
		IViewModel m_VM;

		List<T> m_List;
		List<T> GetList()
		{
			if (m_List == null)
			{
				m_List = new List<T>();
				m_VM.Set<List<T>>(Path, m_List);
			}
			return m_List;
		}

		public List<T> Value
		{
			set { m_VM.Set<List<T>>(Path, value); m_List = value; }
		}

		public event System.Action<List<T>> OnChanged;


		public ReactiveListProperty(string path, IViewModel vm)
		{
			Path = path;
			m_VM = vm;
			m_VM.SubscribeChanged<List<T>>(Path, OnNotifyChanged);
		}

		public ReactiveListProperty(string path, IViewModel vm, List<T> val)
		{
			Path = path;
			m_VM = vm;
			Value = val;
			m_VM.SubscribeChanged<List<T>>(Path, OnNotifyChanged);

		}

		void OnNotifyChanged(List<T> val)
		{
			OnChanged?.Invoke(val);
		}

		public void Dispose()
		{
			m_VM.UnsubscribeChanged<List<T>>(Path, OnNotifyChanged);
		}

		public void SetDirty()
		{
			m_VM.SetDirty(Path);
		}


		public T this[int index]
		{
			get
			{
				return GetList()[index];
			}
			set
			{
				GetList()[index] = value;
				SetDirty();
			}
		}

		public void Add(T val)
		{
			var list = GetList();
			list.Add(val);
			SetDirty();
		}

		public void Remove(T val)
		{
			if (m_List != null && m_List.Remove(val))
			{
				SetDirty();
			}
		}

		public void Clear()
		{
			if (m_List == null) return;
			if (m_List.Count > 0)
			{
				m_List.Clear();
				SetDirty();
			}
		}

		public void AddRange(IEnumerable<T> collection)
		{
			var list = GetList();
			list.AddRange(collection);
			SetDirty();
		}

		public void RemoveAll(System.Predicate<T> match)
		{
			if (m_List == null) return;
			if (m_List.RemoveAll(match) > 0)
			{
				SetDirty();
			}
		}



	}

}
