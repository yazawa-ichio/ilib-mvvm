using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.MVVM
{

	public abstract class EventButtonBase<T> : MonoBehaviour, IViewEvent
	{

		public string Name => EventKeyToStrConv.ToStr(m_Event);

		[SerializeField]
		T m_Event = default;
		[SerializeField]
		Button m_Button = default;

		protected IViewEventHandler m_Handler;

		void Awake()
		{
			if (m_Button == null)
			{
				m_Button = GetComponent<Button>();
			}
			m_Button.onClick.AddListener(OnClick);
		}

		protected virtual void OnClick()
		{
			m_Handler?.OnViewEvent(Name);
		}

		void IViewEvent.Bind(IViewEventHandler handler)
		{
			m_Handler = handler;
		}

		public virtual System.Type EventType()
		{
			return null;
		}
	}

}
