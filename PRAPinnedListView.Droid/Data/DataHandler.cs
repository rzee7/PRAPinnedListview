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
    public class DataHandler
    {
        private static List<ItemHolder> _dataItems = new List<ItemHolder>();
        public static List<ItemHolder> GetData(int sectionsNumber = 16)
        {
            int sectionPosition = 0, listPosition = 0;
            for (int i = 0; i < sectionsNumber; i++)
            {
                var section = new ItemHolder();
                section.SectionPosition = sectionPosition;
                section.ListPosition = listPosition++;
                section.IsSection = true;
                section.Title = string.Format("Header {0}", i);
                _dataItems.Add(section);

                //Item Handling
                int itemsNumber = (int)Math.Abs((Math.Cos(2f * Math.PI / 3f * sectionsNumber / (i + 1f)) * 25f));
                for (int j = 0; j < itemsNumber; j++)
                {
                    ItemHolder item = new ItemHolder();
                    item.SectionPosition = sectionPosition;
                    item.ListPosition = listPosition++;
                    item.Title = string.Format("Item {0}", j);
                    _dataItems.Add(item);
                }
                sectionPosition++;
            }
            return _dataItems;
        }
    }
    public class ItemHolder : IHeaderModel
    {
        public bool IsSection { get; set; }

        public string Title { get; set; }

        public int SectionPosition { get; set; }

        public int ListPosition { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Title, IsSection ? "Header" : "Item");
        }
    }
}