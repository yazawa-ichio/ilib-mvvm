using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	public class CollectionBind : AssignableBindBase<IList<IViewModel>>
	{
		[SerializeField]
		GameObject m_Preafb = null;

		List<GameObject> m_Instance = new List<GameObject>();

		protected override void UpdateValue(IList<IViewModel> val)
		{
			int count = val.Count;
			for (int i = m_Instance.Count - 1; i >= count; i--)
			{
				GameObject.Destroy(m_Instance[i]);
				m_Instance.RemoveAt(i);
			}
			for (int i = 0; i < count; i++)
			{
				if (m_Instance.Count == i)
				{
					m_Instance.Add(Instantiate(m_Preafb, transform));
				}
				Bind(i, m_Instance[i], val[i]);
			}
		}

		protected virtual void Bind(int index, GameObject instance, IViewModel vm)
		{
			var view = instance.GetComponent<IView>();
			view.Prepare();
			view.Attach(vm);
		}

	}
}
