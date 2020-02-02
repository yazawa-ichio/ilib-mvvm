using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using ILib.CodeEmit;
using ILib.MVVM.CodeEmit;

namespace ILib.MVVM.StaticVM
{
	public class Generator
	{
		IView m_View;
		string m_Name;
		Config m_Config;

		public Generator(IView view, string name, Config config)
		{
			m_View = view;
			m_Name = name;
			m_Config = config;
		}

		public string DryRun()
		{
			return CreateEmitter().DryRun();
		}

		public void Run(string path)
		{
			CreateEmitter().Run(path);
		}

		CodeEmitter CreateEmitter()
		{
			CodeEmitter codeEmitter = new CodeEmitter();
			codeEmitter.Namespace = m_Config.NameSpace;
			codeEmitter.AddUsing("System");
			codeEmitter.AddUsing("System.Collections");
			codeEmitter.AddUsing("System.Collections.Generic");
			codeEmitter.AddUsing("ILib.MVVM");
			ClassEmitter classEmitter = codeEmitter.AddClass();
			classEmitter.Accessibility = "public partial";
			classEmitter.Name = m_Name + m_Config.NameSuffix;
			classEmitter.Extends.Add("ViewModelBase");

			m_View.Prepare(true);

			HashSet<string> path = new HashSet<string>();

			foreach (var elm in m_View.Elements)
			{
				if (elm is EventButton)
				{
					continue;
				}
				var bindable = elm as IBindable;
				if (bindable != null && path.Add(bindable.Path))
				{
					if (m_Config.ReactivePropertyMode)
					{
						ReactivePropertyEmit(bindable, classEmitter.Members);
					}
					else
					{
						PropertyEmit(bindable, classEmitter.Members);
					}
				}
				var viewEvent = elm as IViewEvent;
				if (viewEvent != null)
				{
					if (m_Config.CommandMode)
					{
						CommandEmit(viewEvent, classEmitter.Members);
					}
					else
					{
						EventEmit(viewEvent, classEmitter.Members);
					}
				}
			}
			return codeEmitter;
		}

		public static bool IsList(System.Type type)
		{
			return type.IsGenericType && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition());
		}

		public static string GetGenericTypeString(System.Type type)
		{
			return GetTypeString(type.GenericTypeArguments[0]);
		}

		public static string GetTypeString(System.Type type)
		{
			string fullName = type.FullName;
			switch (fullName)
			{
				case "System.Boolean": return "bool";
				case "System.Byte": return "byte";
				case "System.SByte": return "sbyte";
				case "System.Char": return "char";
				case "System.Decimal": return "decimal";
				case "System.Double": return "double";
				case "System.Single": return "float";
				case "System.Int32": return "int";
				case "System.UInt32": return "uint";
				case "System.Int64": return "long";
				case "System.UInt64": return "ulong";
				case "System.Object": return "object";
				case "System.Int16": return "short";
				case "System.UInt16": return "ushort";
				case "System.String": return "string";
			}
			if (type.IsGenericType && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
			{
				return $"{type.GenericTypeArguments[0].FullName.Replace("+", ".")}[]";
			}
			return type.FullName.Replace("+", ".");
		}

		void PropertyEmit(IBindable bindable, List<MemberEmitter> ret)
		{
			PropertyEmitter property = new PropertyEmitter();
			string name = bindable.Path.Replace("/", "").Replace(".", "");
			if (bindable is IViewEvent) name += m_Config.EventValueSuffix;
			string type = GetTypeString(bindable.BindType());
			property.Name = name;
			property.Type = type;
			property.Setter = new WriteLineEmitter($"set {{ SetImpl<{type}>(\"{bindable.Path}\", value); }}");
			property.Getter = new WriteLineEmitter($"get {{ return GetImpl<{type}>(\"{bindable.Path}\"); }}");
			property.Summary = $"BindingPath : {bindable.Path}\nTarget: {bindable.ToString()}";
			ret.Add(property);
		}

		void ReactivePropertyEmit(IBindable bindable, List<MemberEmitter> ret)
		{
			LazyPropertyEmitter property = new LazyPropertyEmitter();
			string name = bindable.Path.Replace("/", "").Replace(".", "");
			if (bindable is IViewEvent) name += m_Config.EventValueSuffix;
			
			property.Name = name;
			property.Summary = $"BindingPath : {bindable.Path}\nTarget: {bindable.ToString()}";

			if (!IsList(bindable.BindType()))
			{
				string type = GetTypeString(bindable.BindType());
				property.Type = $"ReactiveProperty<{type}>";
				property.Create = $"new ReactiveProperty<{type}>(\"{bindable.Path}\", this)";
			}
			else
			{
				string type = GetGenericTypeString(bindable.BindType());
				property.Type = $"ReactiveListProperty<{type}>";
				property.Create = $"new ReactiveListProperty<{type}>(\"{bindable.Path}\", this)";
			}
			ret.Add(property);
		}

		void CommandEmit(IViewEvent viewEvent, List<MemberEmitter> ret)
		{
			string name = viewEvent.Name.Replace("/", "").Replace(".", "");
			string genericType = (viewEvent.EventType() != null) ? $"<{GetTypeString(viewEvent.EventType())}>" : "";

			FieldEmitter field = new FieldEmitter();
			field.Accessibility = "private";
			field.Name = "m_" + name + m_Config.CommandSuffix;
			field.Type = "BindingCommand" + genericType;
			ret.Add(field);


			PropertyEmitter property = new PropertyEmitter();
			property.Name = name + m_Config.CommandSuffix;
			property.Type = "ICommand" + genericType;
			property.Summary = $"BindingPath : {viewEvent.Name}\nSender: {viewEvent.ToString()}";

			property.Getter = new DelegateEmitter(w =>
			{
				w.WriteLine("get");
				using (w.Bracket())
				{
					w.WriteLine($"return m_{name}{m_Config.CommandSuffix}?.Get() ?? null;");
				}
			});
			property.Setter = new DelegateEmitter(w =>
			{
				w.WriteLine("set");
				using (w.Bracket())
				{
					w.WriteLine($"if (m_{name}{m_Config.CommandSuffix} == null) m_{name}{m_Config.CommandSuffix} = new BindingCommand{genericType}(\"{viewEvent.Name}\", this);");
					w.WriteLine($"m_{name}{m_Config.CommandSuffix}.Set(value);");
				}
			});
			ret.Add(property);
		}

		void EventEmit(IViewEvent viewEvent, List<MemberEmitter> ret)
		{
			string name = viewEvent.Name.Replace("/", "").Replace(".", "");
			string genericType = (viewEvent.EventType() != null) ? $"<{GetTypeString(viewEvent.EventType())}>" : "";

			PropertyEmitter property = new PropertyEmitter();
			property.Name = "On" + name;
			property.Type = "event Action" + genericType;
			property.Summary = $"BindingPath : {viewEvent.Name}\nSender: {viewEvent.ToString()}";

			property.Getter = new DelegateEmitter(w =>
			{
				w.WriteLine("add");
				using (w.Bracket())
				{
					w.WriteLine($"m_Event.Subscribe{genericType}(\"{viewEvent.Name}\", value);");
				}
			});
			property.Setter = new DelegateEmitter(w =>
			{
				w.WriteLine("remove");
				using (w.Bracket())
				{
					w.WriteLine($"m_Event.Unsubscribe{genericType}(\"{viewEvent.Name}\", value);");
				}
			});
			ret.Add(property);
		}

	}
}
