using System;
using System.Collections;
using System.Collections.Generic;

namespace ILib.MVVM
{

	public class GeneralViewModel : ViewModelBase
	{

		public T Get<T>(string path)
		{
			return GetImpl<T>(path);
		}

		public void Set<T>(string path, T val)
		{
			SetImpl(path, val);
		}

		public BindingCommand Command(string name, Action onViewEvent, Func<bool> canExecute = null)
		{
			var binding = new BindingCommand(name, this);
			binding.Set(new DelegateCommand(onViewEvent, canExecute));
			return binding;
		}

		public BindingCommand Command<T>(T name, Action onViewEvent, Func<bool> canExecute = null)
		{
			return Command(EventKeyToStrConv.ToStr(name), onViewEvent, canExecute);
		}

		public BindingCommand Command(string name, ICommand cmd)
		{
			var binding = new BindingCommand(name, this);
			binding.Set(cmd);
			return binding;
		}

		public BindingCommand Command<T>(T name, ICommand cmd)
		{
			return Command(EventKeyToStrConv.ToStr(name), cmd);
		}

		public BindingCommand<T> Command<T>(string name, Action<T> onViewEvent, Func<bool> canExecute = null)
		{
			var binding = new BindingCommand<T>(name, this);
			binding.Set(new DelegateCommand<T>(onViewEvent, canExecute));
			return binding;
		}

		public BindingCommand<U> Command<T, U>(T name, Action<U> onViewEvent, Func<bool> canExecute = null)
		{
			return Command(EventKeyToStrConv.ToStr(name), onViewEvent, canExecute);
		}

		public BindingCommand<T> Command<T>(string name, ICommand<T> cmd)
		{
			var binding = new BindingCommand<T>(name, this);
			binding.Set(cmd);
			return binding;
		}

		public BindingCommand<U> Command<T, U>(T name, ICommand<U> cmd)
		{
			return Command(EventKeyToStrConv.ToStr(name), cmd);
		}

	}

}
