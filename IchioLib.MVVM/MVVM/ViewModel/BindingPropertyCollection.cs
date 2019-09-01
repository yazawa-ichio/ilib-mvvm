using System.Collections;
using System.Collections.Generic;

namespace ILib.MVVM
{

	public class BindingPropertyCollection 
	{
		Dictionary<string, BindingProperty> m_Properties = new Dictionary<string, BindingProperty>();

		public T Get<T>(string path)
		{
			BindingProperty<T> property = GetProperty<T>(path);
			return property != null && property.IsValid ? property.Value : default;
		}

		public bool Set<T>(string path, T val, out IBindingProperty<T> newProperty)
		{
			BindingProperty<T> property = GetProperty<T>(path);
			if (property != null)
			{
				property.Value = val;
				if (!property.IsValid)
				{
					property.IsValid = true;
					newProperty = property;
					return true;
				}
				newProperty = null;
				return false;
			}
			else
			{
				newProperty = SetNewPropety(path, val, true);
				return true;
			}
		}

		public void Subscribe<T>(string path, System.Action<T> notify)
		{
			var prop = GetProperty<T>(path);
			if (prop == null)
			{
				prop = SetNewPropety(path, default(T), false);
			}
			prop.OnChanged += notify;
		}

		public void Unsubscribe<T>(string path, System.Action<T> notify)
		{
			var prop = GetProperty<T>(path);
			if (prop != null)
			{
				prop.OnChanged -= notify;
			}
		}

		BindingProperty<T> GetProperty<T>(string path)
		{
			BindingProperty property;
			if (m_Properties.TryGetValue(path, out property))
			{
				while (property != null)
				{
					var ret = property as BindingProperty<T>;
					if (ret != null) return ret;
					property = property.Next;
				}
			}
			return null;
		}

		BindingProperty<T> SetNewPropety<T>(string path, T val, bool isValid)
		{
			BindingProperty property;
			if (!m_Properties.TryGetValue(path, out property))
			{
				m_Properties[path] = property = new BindingProperty<T>(path, val);
			}
			else
			{
				while (property.Next != null)
				{
					property = property.Next;
				}
				property.Next = new BindingProperty<T>(path, val);
				property = property.Next;
			}
			property.IsValid = isValid;
			return property as BindingProperty<T>;
		}

		public void SetDirty(string path)
		{
			BindingProperty property;
			if (m_Properties.TryGetValue(path, out property))
			{
				while (property != null)
				{
					if (property.IsValid)
					{
						property.SetDirty();
					}
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
					if (property.IsValid)
					{
						property.SetDirty();
					}
					property = property.Next;
				}
			}
		}

		public void Bind(IBinding binding)
		{
			foreach (var _property in m_Properties.Values)
			{
				var property = _property;
				while (property != null)
				{
					if (property.IsValid)
					{
						binding.Bind(property.Path, property);
					}
					property = property.Next;
				}
			}
		}

		public void Unbind(IBinding binding)
		{
			foreach (var _property in m_Properties.Values)
			{
				var property = _property;
				while (property != null)
				{
					if (property.IsValid)
					{
						binding.Unbind(property.Path, property);
					}
					property = property.Next;
				}
			}
		}

#if UNITY_EDITOR
		public IEnumerable<IBindingProperty> GetAll() => m_Properties.Values;
#endif

	}

}
