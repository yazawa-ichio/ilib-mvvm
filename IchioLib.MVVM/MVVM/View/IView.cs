using System.Collections.Generic;

namespace ILib.MVVM
{
	public interface IView
	{
		bool IsActive { get; }
		[System.Obsolete]
		IViewElement[] Elements { get; }
		IViewModel DataContext { get; }
		void Prepare(bool force = false);
		void Attach(IViewModel vm);
		void TryUpdate();

		void GetElements(List<IViewElement> elements);
	}
}