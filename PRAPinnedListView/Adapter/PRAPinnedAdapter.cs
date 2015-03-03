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
    class PRAPinnedAdapter<T> : BaseAdapter<T>, IPinnedSectionListAdapter, ISectionIndexer
    {
        #region Private Declarations

        private List<ItemHolder> dataItem;
        private Activity context;

        #endregion

        #region Adapter Constructor

        public PRAPinnedAdapter(Activity _context, List<ItemHolder> _dataItems)
        {
            context = _context;
            dataItem = _dataItems;
        }

        #endregion

        #region IPinned IsItemViewTypePinned Implementation

        public bool IsItemViewTypePinned(int viewType)
        {
            return viewType == 1; //Section view
        }

        #endregion

        #region ISectionIndexer Implementation

        public int GetPositionForSection(int sectionIndex)
        {
            int sectionCount=dataItem.Count(x => x.IsSection == true);
            if (sectionIndex >= sectionCount)
            {
                sectionIndex = sectionCount - 1;
            }
            return dataItem.Where(x => x.IsSection == true).ToList()[sectionIndex].ListPosition;
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
            return null; //throw new NotImplementedException();
        }

        #endregion

        #region View Type Count

        public override int ViewTypeCount
        {
            get
            {
                return 2;
            }
        }

        #endregion

        #region No Method
        public override T this[int position]
        {
            get { throw new NotImplementedException(); }
        }

        public override T this[int position]
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region Items Count

        public override int Count
        {
            get { return dataItem.Count; }
        }

        #endregion

        #region Item ID

        public override long GetItemId(int position)
        {
            return 0;
        }

        #endregion

        #region Get Item

        public ItemHolder GetItem(int position)
        {
            return dataItem[position]; //TODO : Need to be set generic
        }

        #endregion

        #region Get View Method

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ItemHolder item = GetItem(position);
            View view = convertView;
            if (view == null)
                view = null; //LayoutInflater.Inflate(Resource.Layout.ListItem, parent, false);

            switch (GetItemViewType(position))
            {
                case 0:
                    view = null;// context.LayoutInflater.Inflate(Resource.Layout.ItemView, parent, false); //Item view
                    view.FindViewById<ImageView>(0/*ImageView ID*/).SetBackgroundResource(0/*Image ID*/);
                    view.FindViewById<TextView>(0/*TextView ID*/).Text = string.Empty;
                    break;
                case 1:
                    view = null;// context.LayoutInflater.Inflate(Resource.Layout.SectionView, parent, false); //Header view
                    view.FindViewById<ImageView>(0/*ImageView ID*/).SetBackgroundResource(0/*Image ID*/);
                    view.FindViewById<TextView>(0/*TextView ID*/).Text = string.Empty;
                    break;
                default:
                    break;
            }
            return view;
        }

        #endregion

        #region Getview Item Type

        public override int GetItemViewType(int position)
        {
            return GetItem(position).IsSection ? 1 : 0;
        }

        #endregion
    }
}