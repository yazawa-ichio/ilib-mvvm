using System.Collections.Generic;
namespace ILib.MVVM
{
	public interface IViewModel
	{
		BindingPropertyCollection Property { get; }

		EventBroker Event { get; }

		void Set<T>(string path, T val);

		T Get<T>(string path);

		void SetDirty(string path);

		void SetAllDirty();

	}
}