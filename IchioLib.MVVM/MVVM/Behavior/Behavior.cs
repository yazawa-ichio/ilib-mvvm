using System;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{
	using Behaviors;

	public abstract class Behavior : MonoBehaviour, IBehavior
	{

		[SerializeField]
		string m_MessengerPath = ViewModelBase.MessengerPath;

		public string MessengerPath => m_MessengerPath;

		protected IMessenger m_Messenger = Messenger.Default;
		IMessenger IBehavior.Messenger => m_Messenger;
		Action<IMessenger> m_OnSetMessenger = null;

		void OnDestroy()
		{
			m_Messenger?.Unregister(this);
			OnDestroyImpl();
		}

		protected virtual void OnDestroyImpl() { }

		protected void Register<T>(string name, Action<T> action)
		{
			if (m_Messenger != null)
			{
				m_Messenger.Register(this, name, action);
			}
			m_OnSetMessenger += (msg) => msg.Register(this, name, action);
		}

		internal void SetMessenger(IMessenger messenger)
		{
			m_Messenger?.Unregister(this);
			m_Messenger = messenger;
			m_OnSetMessenger?.Invoke(messenger);
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
			yield return new MessengerBind(this, m_MessengerPath);
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