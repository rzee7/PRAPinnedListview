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
    public interface IHeaderModel
    {
        bool IsSection { get; set; }
        int ListPosition { get; set; }
        int SectionPosition { get; set; }
    }
}