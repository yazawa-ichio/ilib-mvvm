using System;

namespace ILib.MVVM.Message
{
	public class ReferenceHandle : IDisposable
	{
		public static readonly ReferenceHandle Empty = new ReferenceHandle(null, null);

		object m_Reference;
		HolderBase m_Holder;
		ReferenceHandle[] m_Handles;

		public bool Disposed => m_Reference == null;

		internal ReferenceHandle(object obj, HolderBase holder)
		{
			m_Reference = obj;
			m_Holder = holder;
		}

		internal ReferenceHandle(ReferenceHandle[] handles)
		{
			m_Handles = handles;
		}

		~ReferenceHandle()
		{
			Dispose();
		}

		public void Dispose()
		{
			m_Reference = null;
			if (m_Holder != null)
			{
				m_Holder.Removed = true;
				m_Holder = null;
			}
			if (m_Handles != null)
			{
				foreach (var h in m_Handles)
				{
					h?.Dispose();
				}
				m_Handles = null;
			}
		}

		public static bool IsActive(WeakReference<ReferenceHandle> handle)
		{
			if (handle.TryGetTarget(out ReferenceHandle target))
			{
				return !target.Disposed;
			}
			return false;
		}

	}

}