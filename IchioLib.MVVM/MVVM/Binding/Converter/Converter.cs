using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.MVVM
{

	public abstract class Converter<TInput, UOutput> : MonoBehaviour, IConverter
	{
		public virtual bool IsActive => this != null;
		protected ConvertProperty<UOutput> m_Output;
		protected IBindingProperty<TInput> m_Input;
		protected int m_Hash;
		protected bool m_ForceUpdate;

		public abstract UOutput Convert(TInput input);

		public virtual bool TryInverseConvert(UOutput value, ref TInput ret)
		{
			return false;
		}

		public Type GetTargetType()
		{
			return typeof(TInput);
		}

		public virtual bool TryConvert<T>(IBindingProperty property, ref IBindingProperty<T> output)
		{
			IBindingProperty ret = null;
			if (TryConvert(property, ref ret) && ret is IBindingProperty<T> target)
			{
				output = target;
				return true;
			}
			return false;
		}

		public virtual bool TryConvert(IBindingProperty property, ref IBindingProperty output)
		{
			var input = property as IBindingProperty<TInput>;
			if (input == null)
			{
				return false;
			}
			m_ForceUpdate = true;
			if (m_Output == null)
			{
				m_Output = new ConvertProperty<UOutput>(input.Path);
				m_Output.OnChanged += OnChanged;
			}
			m_Input = input;
			TryUpdate();
			output = m_Output;
			return true;
		}

		public virtual bool Unbind(IBindingProperty property)
		{
			if (m_Input == property)
			{
				m_Input = null;
				m_Output = null;
				return true;
			}
			return false;
		}

		public void TryUpdate()
		{
			if (m_Output == null || m_Input == null) return;
			if (!m_ForceUpdate && m_Input.Hash == m_Hash)
			{
				return;
			}
			m_ForceUpdate = false;
			m_Output.ConvertApply(Convert(m_Input.Value));
		}

		void OnChanged(UOutput output)
		{
			TInput input = default;
			if (TryInverseConvert(output, ref input))
			{
				m_Input.Value = input;
			}
		}

	}
}