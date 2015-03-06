using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Globalization;
using Java.Util;
using Android.Graphics;

namespace PRAPinnedListView.Droid
{
    [Activity(Label = "PRA Pinned Header", MainLauncher = true, Icon = "@drawable/praIcon")]
    public class MainActivity : Activity, IListItemView<ItemHolder>
    {
        #region Private Declaration

        private PRAListView listView;

        #endregion

        #region On Create Method

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            #region Pinned List View

            PRAService.Register<IListItemView<ItemHolder>>(this);
            listView = FindViewById<PRAListView>(Resource.Id.list);
            listView.SetItemSource<ItemHolder>(DataHandler.GetData());
            listView.PRAItemClick += listView_PRAItemClick;

            #endregion
        }

        void listView_PRAItemClick(object sender, PRAListItemClickEventArg<object> e)
        {
            Toast.MakeText(this, string.Format("Hey! you clicked {0}", ((ItemHolder)e.Item).Title), ToastLength.Short).Show();
        }

        #endregion

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
    }
}

