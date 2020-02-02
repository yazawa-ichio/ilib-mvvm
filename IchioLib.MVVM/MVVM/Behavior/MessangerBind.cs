using System;

namespace ILib.MVVM.Behaviors
{
	internal class MessangerBind : IBindable
	{
		int m_Hash;
		WeakReference<Behavior> m_Behavior;
		IBindingProperty<Messenger> m_Prop;
		public string Path { get; private set; }
		public bool IsActive => m_Behavior != null;

		public MessangerBind(Behavior behavior, string path)
		{
			m_Behavior = new System.WeakReference<Behavior>(behavior);
			Path = path;
		}

		public Type BindType()
		{
			return typeof(Messenger);
		}

		public void Bind(IBindingProperty prop)
		{
			if (prop is IBindingProperty<Messenger> target)
			{
				m_Prop = target;
			}
		}

		public void Unbind(IBindingProperty prop)
		{
			if (prop == m_Prop)
			{
				m_Prop = null;
			}
		}

		public void TryUpdate()
		{
			if (m_Prop == null || m_Prop.Hash == m_Hash) return;
			m_Hash = m_Prop.Hash;
			Behavior behavior;
			if (m_Behavior.TryGetTarget(out behavior))
			{
				behavior.SetMessanger(m_Prop.Value ?? Messenger.Default);
			}
			return;
		}


		IConverter IBindable.GetConverter()
		{
			return null;
		}

	}

}
