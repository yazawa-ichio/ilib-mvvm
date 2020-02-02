using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{
	public interface IBindable : IViewElement
	{
		string Path { get; }
		bool IsActive { get; }
		void TryUpdate();
		System.Type BindType();
		IConverter GetConverter();
		void Bind(IBindingProperty prop);
		void Unbind(IBindingProperty prop);
	}

	public interface IBindable<T> : IBindable
	{
		//void Bind(IBindingProperty<T> prop);
	}

}
