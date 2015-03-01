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
    [Activity(Label = "PRAPinnedListView.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var list = FindViewById<PRAListView>(Resource.Id.list);

        }
    }
    public class SimpleAdapter : ArrayAdapter<ItemHolder>, IPinnedSectionListAdapter
    {
        Activity context;

        //Color
        private static int[] COLORS = new int[] { Resource.Color.green_light, Resource.Color.orange_light, Resource.Color.blue_light, Resource.Color.red_light };

        public SimpleAdapter(Activity _context, int _item, int resID)
            : base(_context, _item, resID)
        {
            context = _context;
        }

        public bool IsItemViewTypePinned(int viewType)
        {
            return viewType == ItemHolder.SECTION;    
        }

        public override int GetItemViewType(int position)
        {
            return GetItem(position).type;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TextView view = (TextView)base.GetView(position, convertView, parent);
            view.SetTextColor(Color.DarkGray);
            view.Tag = "" + position;
            ItemHolder item = GetItem(position);
            if (item.type == ItemHolder.SECTION)
            {
                //view.setOnClickListener(PinnedSectionListActivity.this);
                view.SetBackgroundColor(parent.Resources.GetColor(COLORS[item.SectionPosition % COLORS.Length]));
            }
            return view;
        }


        public override int ViewTypeCount
        {
            get
            {
                return 2;
            }
        }

        #region Data Handler

        public void generateDataset(int from, int to, bool clear)
        {

            if (clear) Clear();

            int sectionsNumber = to - from + 1;
            prepareSections(sectionsNumber);

            int sectionPosition = 0, listPosition = 0;
            for (int i = 0; i < sectionsNumber; i++)
            {
                ItemHolder section = new ItemHolder(ItemHolder.SECTION, string.Format("A", i));
                section.SectionPosition = sectionPosition;
                section.ListPosition = listPosition++;
                onSectionAdded(section, sectionPosition);
                Add(section);

                int itemsNumber = (int)Math.Abs((Math.Cos(2f * Math.PI / 3f * sectionsNumber / (i + 1f)) * 25f));
                for (int j = 0; j < itemsNumber; j++)
                {
                    ItemHolder item = new ItemHolder(ItemHolder.ITEM, section.Text.ToUpper(new CultureInfo("en-US")) + " - " + j);
                    item.SectionPosition = sectionPosition;
                    item.ListPosition = listPosition++;
                    Add(item);
                }

                sectionPosition++;
            }
        }

        #endregion

        protected virtual void prepareSections(int sectionsNumber) { }
        protected virtual void onSectionAdded(ItemHolder section, int sectionPosition) { }
    }

    public class FastScrollAdapter : SimpleAdapter, ISectionIndexer
    {

        #region Constructor

        private ItemHolder[] sections;

        public FastScrollAdapter(Activity context, int resource, int textViewResourceId) :base(context, resource, textViewResourceId){
        }

        protected override void prepareSections(int sectionsNumber)
        {
            sections = new ItemHolder[sectionsNumber];
        }

        protected override void onSectionAdded(ItemHolder section, int sectionPosition)
        {
            sections[sectionPosition] = section;
        }

        #endregion

        public int GetPositionForSection(int sectionIndex)
        {
            if (sectionIndex >= sections.Length)
            {
                sectionIndex = sections.Length - 1;
            }
            return sections[sectionIndex].ListPosition;
        }

        public int GetSectionForPosition(int position)
        {
            if (position >= Count)
            {
                position = Count - 1;
            }
            return GetItem(position).SectionPosition;
        }

        public Java.Lang.Object[] GetSections()
        {
            return null; //sections.Cast<Java.Lang.Object>();
        }
        public ItemHolder[] FetchSection()
        {
            return sections;
        }
    }
    public static class ObjectTypeHelper
    {
        public static T[] Cast<T>(this Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T[];
        }
    }
}

