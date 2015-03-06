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
    internal class PRAPinnedAdapter<T> : BaseAdapter<T>, IPinnedSectionListAdapter, ISectionIndexer where T : IHeaderModel
    {
        #region Private Declarations

        private T[] PRAdataItem;

        #endregion

        #region Adapter Constructor

        public PRAPinnedAdapter(IEnumerable<T> _dataItems)
        {
            PRAdataItem = _dataItems.ToArray();
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
            int sectionCount = PRAdataItem.Count(x => x.IsSection == true);
            if (sectionIndex >= sectionCount)
            {
                sectionIndex = sectionCount - 1;
            }
            return PRAdataItem.Where(x => x.IsSection == true).ToList()[sectionIndex].ListPosition;
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

        #region Get Item Method
        public override T this[int position]
        {
            get { return GetItem(position); }
        }

        #endregion

        #region Items Count

        public override int Count
        {
            get { return PRAdataItem.Length; }
        }

        #endregion

        #region Item ID

        public override long GetItemId(int position)
        {
            return 0;
        }

        #endregion

        #region Get Item

        public new T GetItem(int position)
        {
            return PRAdataItem[position]; 
        }

        #endregion

        #region Get View Method

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = null;
            switch (GetItemViewType(position))
            {
                case 0:
                    view = PRAService.Resolve<IListItemView<T>>().GetItemView(position, convertView, parent, PRAdataItem[position]);
                    break;
                case 1:
                    view = PRAService.Resolve<IListItemView<T>>().GetItemHeaderView(position, convertView, parent, PRAdataItem[position]);
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