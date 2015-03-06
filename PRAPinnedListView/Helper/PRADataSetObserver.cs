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
using Android.Database;

namespace PRAPinnedListView
{
    internal class PRADataSetObserver : DataSetObserver
    {
        private readonly Action _OnChanged;
        private readonly Action _OnIvalidated;
        public PRADataSetObserver(Action OnChanged, Action OnInvalidate)
        {
            _OnChanged = OnChanged;
            _OnIvalidated = OnInvalidate;
        }
        public override void OnChanged()
        {
            _OnChanged();
        }
        public override void OnInvalidated()
        {
            _OnIvalidated();
        }
    }
}