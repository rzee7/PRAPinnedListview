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
        private bool hasHeaderAndFooter;
        private bool isFastScroll;
        private bool addPadding;
        private bool isShadowVisible = true;
        private int mDatasetUpdateCount;
        private PRAListView listView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
           // var parentView = FindViewById<LinearLayout>(Resource.Id.parent);
            listView = FindViewById<PRAListView>(Resource.Id.list);
            //.AddView(listView);
            if (bundle != null)
            {
                isFastScroll = bundle.GetBoolean("isFastScroll");
                addPadding = bundle.GetBoolean("addPadding");
                isShadowVisible = bundle.GetBoolean("isShadowVisible");
                hasHeaderAndFooter = bundle.GetBoolean("hasHeaderAndFooter");
            }
            initializeHeaderAndFooter();
            initializeAdapter();
            initializePadding();

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuItem, menu);
            menu.GetItem(0).SetChecked(isFastScroll);
            menu.GetItem(1).SetChecked(addPadding);
            menu.GetItem(2).SetChecked(isShadowVisible);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.action_fastscroll:
                    isFastScroll = !isFastScroll;
                    item.SetChecked(isFastScroll);
                    initializeAdapter();
                    break;
                case Resource.Id.action_addpadding:
                    addPadding = !addPadding;
                    item.SetChecked(addPadding);
                    initializePadding();
                    break;
                case Resource.Id.action_showShadow:
                    isShadowVisible = !isShadowVisible;
                    item.SetChecked(isShadowVisible);
                    ((PRAListView)listView).SetShadowVisible(isShadowVisible);
                    break;
                case Resource.Id.action_showHeaderAndFooter:
                    hasHeaderAndFooter = !hasHeaderAndFooter;
                    item.SetChecked(hasHeaderAndFooter);
                    initializeHeaderAndFooter();
                    break;
                case Resource.Id.action_updateDataset:
                    updateDataset();
                    break;
            }
            return true;
        }


        private void updateDataset()
        {
            mDatasetUpdateCount++;
            SimpleAdapter adapter = (SimpleAdapter)listView.Adapter;
            switch (mDatasetUpdateCount % 4)
            {
                case 0: adapter.generateDataset('A', 'B', true); break;
                case 1: adapter.generateDataset('C', 'M', true); break;
                case 2: adapter.generateDataset('P', 'Z', true); break;
                case 3: adapter.generateDataset('A', 'Z', true); break;
            }
            adapter.NotifyDataSetChanged();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean("isFastScroll", isFastScroll);
            outState.PutBoolean("addPadding", addPadding);
            outState.PutBoolean("isShadowVisible", isShadowVisible);
            outState.PutBoolean("hasHeaderAndFooter", hasHeaderAndFooter);
        }

        private void initializePadding()
        {
            float density = Resources.DisplayMetrics.Density;
            int padding = addPadding ? (int)(16 * density) : 0;
            listView.SetPadding(padding, padding, padding, padding);
        }

        private void initializeHeaderAndFooter()
        {
            listView.Adapter = null;
            if (hasHeaderAndFooter)
            {
                ListView list = listView;

                LayoutInflater inflater = LayoutInflater.From(this);
                TextView header1 = (TextView)inflater.Inflate(Android.Resource.Layout.SimpleListItem1, list, false);
                header1.Text = "First header";
                list.AddHeaderView(header1);

                TextView header2 = (TextView)inflater.Inflate(Android.Resource.Layout.SimpleListItem1, list, false);
                header2.Text = "Second header";
                list.AddHeaderView(header2);

                TextView footer = (TextView)inflater.Inflate(Android.Resource.Layout.SimpleListItem1, list, false);
                footer.Text = "Single footer";
                list.AddFooterView(footer);
            }
            initializeAdapter();
        }
        private void initializeAdapter()
        {
            listView.FastScrollEnabled = isFastScroll;
            if (isFastScroll)
            {
                if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Honeycomb)
                {
                    listView.FastScrollAlwaysVisible = true;
                }
                listView.Adapter = new FastScrollAdapter(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1);
            }
            else
            {
                listView.Adapter = new SimpleAdapter(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1);
            }
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
            generateDataset(1, 26, false);
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

                int sectionsNumber = to - from + 1; // 26 - 1 + 1 = 26
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

        public FastScrollAdapter(Activity context, int resource, int textViewResourceId)
            : base(context, resource, textViewResourceId)
        {
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

