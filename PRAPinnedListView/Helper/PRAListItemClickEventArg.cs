using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PRAPinnedListView
{
    public class PRAListItemClickEventArg<T> : EventArgs
    {
        public PRAListItemClickEventArg(long id, View view, AdapterView parent, T item)
        {
            Id = id;
            this.View = view;
            Parent = parent;
            Item = item;
        }

        public long Id { get; set; }
        public View View { get; set; }
        public AdapterView Parent { get; set; }

        public T Item { get; set; }
    }
}