using System;
using System.Collections.Generic;

namespace ILib.MVVM
{

	public delegate void NewBindingPropertyDelegate(string path, IBindingProperty property);

	public class BindingPropertyCollection
	{

		Dictionary<string, BindingProperty> m_Properties = new Dictionary<string, BindingProperty>();

		public event NewBindingPropertyDelegate OnNewProperty;

		public void Subscribe<T>(string path, Action<T> notify)
		{
			Get<T>(path).OnChanged += notify;
		}

		public void Unsubscribe<T>(string path, Action<T> notify)
		{
			Get<T>(path).OnChanged -= notify;
		}

		public IBindingProperty<T> Get<T>(string path)
		{
			if (m_Properties.TryGetValue(path, out BindingProperty property))
			{
				var last = property;
				while (property != null)
				{
					if (property is BindingProperty<T> ret)
					{
						//データがすでにある
						return ret;
					}
					last = property;
					property = property.Next;
				}
				{
					var ret = NewProperty<T>(path);
					last.Next = ret;
					return ret;
				}
			}
			else
			{
				var ret = NewProperty<T>(path);
				m_Properties[path] = ret;
				return ret;
			}
		}

		BindingProperty<T> NewProperty<T>(string path)
		{
			var ret = new BindingProperty<T>(path);
			OnNewProperty?.Invoke(path, ret);
			return ret;
		}

		public void SetDirty(string path)
		{
			BindingProperty property;
			if (m_Properties.TryGetValue(path, out property))
			{
				while (property != null)
				{
					property.SetDirty();
					property = property.Next;
				}
			}
		}

		public void SetAllDirty()
		{
			foreach (var _property in m_Properties.Values)
			{
				var property = _property;
				while (property != null)
				{
					property.SetDirty();
					property = property.Next;
				}
			}
		}

		public List<IBindingProperty> GetAll(List<IBindingProperty> ret)
		{
			foreach (var _property in m_Properties.Values)
			{
				var property = _property;
				while (property != null)
				{
					ret.Add(property);
					property = property.Next;
				}
			}
			return ret;
		}

	}

}