using System.Collections.Generic;
namespace ILib.MVVM
{
	public interface IViewModel
	{
		void Register(IBinding binding);
		void Unregister(IBinding binding);
		void Set<T>(string path, T val);
		T Get<T>(string path);
		void SetDirty(string path);
		void SetAllDirty();
		void SubscribeChanged<T>(string path, System.Action<T> notify);
		void UnsubscribeChanged<T>(string path, System.Action<T> notify);
		void SubscribeViewEvent(string name, System.Action onViewEvent);
		void UnsubscribeViewEvent(string name, System.Action onViewEvent);
		void SubscribeViewEvent<T>(string name, System.Action<T> onViewEvent);
		void UnsubscribeViewEvent<T>(string name, System.Action<T> onViewEvent);
		IEnumerable<IBindingProperty> GetProperties();
		IEnumerable<IBindingEvent> GetEvents();
	}
}
