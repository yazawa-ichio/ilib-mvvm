using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{
	using Behaviors;

	public abstract class Behavior : MonoBehaviour, IBehavior
	{

		[SerializeField]
		string m_MessangerPath = "Messanger";

		public string MessangerPath => m_MessangerPath;

		IMessenger m_Messanger = Messenger.Default;
		IMessenger IBehavior.Messanger => m_Messanger;
		Action<IMessenger> m_OnSetMessanger = null;

		void OnDestroy()
		{
			m_Messanger?.Unregister(this);
			OnDestroyImpl();
		}

		protected virtual void OnDestroyImpl() { }

		protected void Register<T>(string name, Action<T> action)
		{
			if (m_Messanger != null)
			{
				m_Messanger.Register(this, name, action);
			}
			m_OnSetMessanger += (msg) => msg.Register(this, name, action);
		}

		internal void SetMessanger(IMessenger messenger)
		{
			m_Messanger?.Unregister(this);
			m_Messanger = messenger;
			m_OnSetMessanger?.Invoke(messenger);
		}

		void IMultipleBindable.OnPrepare() => OnPrepare();

		protected virtual void OnPrepare()
		{
			
		}

		protected virtual IEnumerable<IBindable> GetBindables()
		{
			yield break;
		}

		IEnumerable<IBindable> IMultipleBindable.GetBindables()
		{
			yield return new MessangerBind(this, m_MessangerPath);
			foreach (var bind in GetBindables())
			{
				if (bind != null)
				{
					yield return bind;
				}
			}
		}

	}
}
