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

namespace PRAPinnedListView.Droid
{
    public class ItemHolder
    {
        public static int ITEM = 0;
        public static int SECTION = 1;

        public int type { get; set; } //public int type;
        public string Text { get; set; } //public string text;

        public int SectionPosition { get; set; } //public int sectionPosition;
        public int ListPosition { get; set; } //public int listPosition;

        public ItemHolder(int type, string text)
        {
            this.type = type;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}