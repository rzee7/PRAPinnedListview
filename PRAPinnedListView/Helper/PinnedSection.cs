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
    public class PinnedSection
    {
        public View ViewHolder { get; set; }
        public int Position { get; set; }

        //TODO : Make generic
        public long ID { get; set; }

    }
}