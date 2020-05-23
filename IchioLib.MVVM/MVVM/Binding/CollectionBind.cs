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
			for (int i = m_Instance.Count - 1; i >= val.Count; i--)
			{
				GameObject.Destroy(m_Instance[i]);
				m_Instance.RemoveAt(i);
			}
			for (int i = 0; i < val.Count; i++)
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

			using (var elementsScope = ViewUtil.UseElementList())
			{
				var elements = elementsScope.List;
				view.GetElements(elements);
				foreach (var elm in elements)
				{
					if (elm is IViewEvent viewEvent)
					{
						var argument = viewEvent.GetArgument();
						if (argument != null && argument is CollectionIndexArgument target)
						{
							target.Index = index;
						}
					}
				}
			}
		}
	}
}