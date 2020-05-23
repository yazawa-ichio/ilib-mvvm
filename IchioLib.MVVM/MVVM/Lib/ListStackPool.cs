using System;
using System.Collections.Generic;

namespace ILib.MVVM
{
	public class ListStackPool<T>
	{
		public struct Scope : IDisposable
		{
			ListStackPool<T> m_Pool;
			public List<T> List { get; private set; }
			public Scope(ListStackPool<T> pool)
			{
				m_Pool = pool;
				List = pool.Get();
			}

			public void Dispose()
			{
				m_Pool.Return(List);
				List = null;
			}
		}


		Stack<List<T>> m_Stack = new Stack<List<T>>();

		public List<T> Get()
		{
			if (m_Stack.Count > 0) return m_Stack.Pop();
			return new List<T>();
		}

		public void Return(List<T> list)
		{
			list.Clear();
			m_Stack.Push(list);
		}

		public void Clear()
		{
			m_Stack.Clear();
		}

		public Scope Use()
		{
			return new Scope(this);
		}

	}

}