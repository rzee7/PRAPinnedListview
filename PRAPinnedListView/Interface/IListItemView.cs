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
    public interface IListItemView<T>
    {
        /// <summary>
        /// Generate Item Header View.
        /// </summary>
        /// <param name="position">Current item's position.</param>
        /// <param name="convertView">>Current rendered view.</param>
        /// <param name="parent">View parent group.</param>
        /// <param name="headerItem">Header item.</param>
        /// <returns></returns>
        View GetItemHeaderView(int position, View convertView, ViewGroup parent, T headerItem);

        /// <summary>
        /// Generate Item View.
        /// </summary>
        /// <param name="position">Current item's position.</param>
        /// <param name="convertView">>Current rendered view.</param>
        /// <param name="parent">View parent group.</param>
        /// <param name="Item">Item.</param>
        /// <returns></returns>
        View GetItemView(int position, View convertView, ViewGroup parent, T item);
    }
}