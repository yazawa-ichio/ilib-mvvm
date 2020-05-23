namespace ILib.MVVM
{

	public static class ViewUtil
	{

		public static ListStackPool<IViewElement> ElementListStack { get; private set; } = new ListStackPool<IViewElement>();

		public static ListStackPool<IView> ViewListStack { get; private set; } = new ListStackPool<IView>();

		public static ListStackPool<IBindingProperty> BindingPropertyListStack { get; private set; } = new ListStackPool<IBindingProperty>();

		public static ListStackPool<IViewElement>.Scope UseElementList() => ElementListStack.Use();

		public static ListStackPool<IView>.Scope UseViewList() => ViewListStack.Use();

		public static ListStackPool<IBindingProperty>.Scope UseBindingPropertyListStack() => BindingPropertyListStack.Use();

	}

}