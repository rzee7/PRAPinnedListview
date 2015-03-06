# PRA Pinned Header #
PRAPinnedListView  enables you to use Sticky Headers feature with header click, use this list view to make your app look awesome. You don't need to write lot's of code to use this just few simple steps and you can add it to your app. this contains a generic adapter also that means you also don't need to make a separate list adapter to bind list, just pass your item collection and your list will be bind prestty well. **:)**

![PRA List Preview](https://raw.githubusercontent.com/rzee7/PRAPinnedListview/master/Preview/PinnedList.gif)


# Features #
- Sticky Headers
- Inbuilt Adapter
- Header Click

## PRAListView ##

Ready to use!! Follow me >

- Add PRAPinnedListView reference to app.
- Add PRAListView to your Xaml file (e.g. Main.xaml)

	     <prapinnedlistview.PRAListView
		    android:id="@+id/list"
		    android:layout_width="match_parent"
		    android:layout_height="match_parent" />

- Access in your activity (e.g. MainActivity.cs).
			
			#region Pinned List View

            PRAService.Register<IListItemView<ItemHolder>>(this);
            listView = FindViewById<PRAListView>(Resource.Id.list);
            listView.SetItemSource<ItemHolder>(DataHandler.GetData());
            listView.PRAItemClick += listView_PRAItemClick;
            #endregion

> PRAListItem Click Cvent
		
		void listView_PRAItemClick(object sender, PRAListItemClickEventArg<object> e)
        {
            Toast.MakeText(this, string.Format("Hey! you clicked {0}", ((ItemHolder)e.Item).Title), ToastLength.Short).Show();
        }

- Implement **IListItemView** interface with your modal into your activity where you accessing PRAListView.


    	public class MainActivity : Activity, IListItemView<ItemHolder>

- Initialized both views method **GetItemHeaderView()** and **GetItemView()**.

		#region Get Header Item View

        public View GetItemHeaderView(int position, View convertView, ViewGroup parent, ItemHolder headerItem)
        {
            View view = convertView;
            view = view ?? LayoutInflater.Inflate(Resource.Layout.HeaderView, parent, false); //Header view
            view.FindViewById<TextView>(Resource.Id.txtTitle).Text = headerItem.Title;
            return view;
        }

        #endregion
 		
		#region Get Item View

        public View GetItemView(int position, View convertView, ViewGroup parent, ItemHolder item)
        {
            View view = convertView;
            view = view ?? LayoutInflater.Inflate(Resource.Layout.ItemView, parent, false); //Item view
            view.FindViewById<TextView>(Resource.Id.txtItemTitle).Text = item.Title;
            return view;
        }

        #endregion

Yo!

You have done your most time taken part here :) Now we just need to pass **Item Header View** and **Item View** these view will be customized as per your need :) Yeppeeeee!! we have done 

This lib ported from [Pinned Section](https://github.com/beworker/pinned-section-listview "Sergej Shafarenka") Thanks! [Sergej Shafarenka](https://github.com/beworker)
