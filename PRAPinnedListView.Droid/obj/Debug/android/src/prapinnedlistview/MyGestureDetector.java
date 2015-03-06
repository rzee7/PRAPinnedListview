package prapinnedlistview;


public class MyGestureDetector
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PRAPinnedListView.MyGestureDetector, PRAPinnedListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyGestureDetector.class, __md_methods);
	}


	public MyGestureDetector () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyGestureDetector.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.MyGestureDetector, PRAPinnedListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
