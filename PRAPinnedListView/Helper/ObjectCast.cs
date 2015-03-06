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
    public static class ObjectTypeHelper
    {
        public static Java.Lang.Object Cast(this System.Object obj)
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as Java.Lang.Object;
        }
        public static System.Object CastBack(this Java.Lang.Object obj)
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as System.Object;
        }
    }
}