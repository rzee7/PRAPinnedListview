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
using Java.Lang;

namespace PRAPinnedListView
{
    internal class RunnableHolder : Java.Lang.Object, IRunnable
    {
        private readonly Action _Run;
        public RunnableHolder(Action run)
        {
            _Run = run;
        }

        public void Run()
        {
            _Run();
        }
    }
}