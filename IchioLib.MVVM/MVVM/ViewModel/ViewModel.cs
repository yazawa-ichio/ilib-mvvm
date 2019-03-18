using System;
using System.Collections;
using System.Collections.Generic;

namespace ILib.MVVM
{

	public class ViewModel : ViewModelBase
	{

		public T Get<T>(string path)
		{
			return GetImpl<T>(path);
		}

		public void Set<T>(string path, T val)
		{
			SetImpl(path, val);
		}

		public BindingCommand Command(string name, System.Action onViewEvent, System.Func<bool> canExecute = null)
		{
			var binding = new BindingCommand(name, this);
			binding.Set(new DelegateCommand(onViewEvent, canExecute));
			return binding;
		}

		public BindingCommand Command(string name, ICommand cmd)
		{
			var binding = new BindingCommand(name, this);
			binding.Set(cmd);
			return binding;
		}

		public BindingCommand<T> Command<T>(string name, System.Action<T> onViewEvent, System.Func<bool> canExecute = null)
		{
			var binding = new BindingCommand<T>(name, this);
			binding.Set(new DelegateCommand<T>(onViewEvent, canExecute));
			return binding;
		}

		public BindingCommand<T> Command<T>(string name, ICommand<T> cmd)
		{
			var binding = new BindingCommand<T>(name, this);
			binding.Set(cmd);
			return binding;
		}

	}

}
