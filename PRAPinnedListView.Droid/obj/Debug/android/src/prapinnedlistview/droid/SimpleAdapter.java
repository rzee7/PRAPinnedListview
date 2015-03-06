package prapinnedlistview.droid;


public class SimpleAdapter
	extends android.widget.ArrayAdapter
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemViewType:(I)I:GetGetItemViewType_IHandler\n" +
			"n_getView:(ILandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetView_ILandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"n_getViewTypeCount:()I:GetGetViewTypeCountHandler\n" +
			"";
		mono.android.Runtime.register ("PRAPinnedListView.Droid.SimpleAdapter, PRAPinnedListView.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SimpleAdapter.class, __md_methods);
	}


	public SimpleAdapter (android.content.Context p0, int p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == SimpleAdapter.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.Droid.SimpleAdapter, PRAPinnedListView.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1 });
	}


	public SimpleAdapter (android.content.Context p0, int p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == SimpleAdapter.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.Droid.SimpleAdapter, PRAPinnedListView.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SimpleAdapter (android.content.Context p0, int p1, java.lang.Object[] p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == SimpleAdapter.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.Droid.SimpleAdapter, PRAPinnedListView.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:T[], Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SimpleAdapter (android.content.Context p0, int p1, int p2, java.lang.Object[] p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == SimpleAdapter.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.Droid.SimpleAdapter, PRAPinnedListView.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:T[], Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public SimpleAdapter (android.content.Context p0, int p1, java.util.List p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == SimpleAdapter.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.Droid.SimpleAdapter, PRAPinnedListView.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Collections.Generic.IList`1<T>, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SimpleAdapter (android.content.Context p0, int p1, int p2, java.util.List p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == SimpleAdapter.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.Droid.SimpleAdapter, PRAPinnedListView.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Collections.Generic.IList`1<T>, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public int getItemViewType (int p0)
	{
		return n_getItemViewType (p0);
	}

	private native int n_getItemViewType (int p0);


	public android.view.View getView (int p0, android.view.View p1, android.view.ViewGroup p2)
	{
		return n_getView (p0, p1, p2);
	}

	private native android.view.View n_getView (int p0, android.view.View p1, android.view.ViewGroup p2);


	public int getViewTypeCount ()
	{
		return n_getViewTypeCount ();
	}

	private native int n_getViewTypeCount ();

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
