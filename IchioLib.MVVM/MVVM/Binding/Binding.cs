using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ILib.MVVM
{

	public class Binding : IBinding, IMultipleBindable
	{
		class BindData
		{
			public IBindable Bind;
			public BindData Next;
		}

		IViewElement[] m_Elements;
		IBindable[] m_Binding = System.Array.Empty<IBindable>();
		Dictionary<string, BindData> m_BindData = new Dictionary<string, BindData>();
		BindingEventHandler m_ViewEventHandler = new BindingEventHandler();

		void IMultipleBindable.OnPrepare() { }

		IEnumerable<IBindable> GetBindables()
		{
			if (m_Elements != null)
			{
				foreach (var element in m_Elements)
				{
					var bind = element as IBindable;
					if (bind != null)
					{
						yield return bind;
					}
					var multipleBind = element as IMultipleBindable;
					if (multipleBind != null)
					{
						multipleBind.OnPrepare();
						foreach (var multipleBindItem in multipleBind.GetBindables())
						{
							yield return multipleBindItem;
						}
					}
				}
			}
		}

		IEnumerable<IBindable> IMultipleBindable.GetBindables() => GetBindables();

		public void Init(IViewElement[] elements)
		{
			m_Elements = elements;
			foreach (var elm in elements)
			{
				//イベントであれば登録
				(elm as IViewEvent)?.Bind(m_ViewEventHandler);
			}
			m_BindData.Clear();
			m_Binding = GetBindables().ToArray();
			foreach (var bind in m_Binding)
			{
				//binderをパスごとに連結
				BindData data;
				if (!m_BindData.TryGetValue(bind.Path, out data))
				{
					m_BindData[bind.Path] = data = new BindData();
				}
				else
				{
					var cur = data;
					while (cur.Next != null)
					{
						cur = cur.Next;
					}
					cur.Next = data = new BindData();
				}
				data.Bind = bind;
			}
		}

		public void TryUpdate()
		{
			//いったん面倒なのでnull突っ込む
			//本当は前詰めにすべき
			for (int i = 0; i < m_Binding.Length; i++)
			{
				IBindable bind = m_Binding[i];
				if (bind == null) continue;
				if (bind.IsActive)
				{
					bind.TryUpdate();
				}
				else
				{
					m_Binding[i] = null;
				}
			}
		}

		public void Bind(string path, IBindingProperty prop)
		{
			BindData data;
			if (m_BindData.TryGetValue(path, out data))
			{
				var cur = data;
				do
				{
					cur.Bind.Bind(prop);
				} while ((cur = cur.Next) != null);
			}
		}

		public void Unbind(string path, IBindingProperty prop)
		{
			BindData data;
			if (m_BindData.TryGetValue(path, out data))
			{
				var cur = data;
				do
				{
					cur.Bind.Unbind(prop);
				} while ((cur = cur.Next) != null);
			}
		}

		public void Bind(IViewEventHandler handler)
		{
			m_ViewEventHandler.Add(handler);
		}

		public void Unbind(IViewEventHandler handler)
		{
			m_ViewEventHandler.Remove(handler);
		}

	}

}
