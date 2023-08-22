using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEditor.IMGUI.Controls;

namespace NWDUnityEditor.Tools
{
    public class NWDCustomDropdown<T> : AdvancedDropdown
    {
        internal class NWDAdvancedDropdownItem : AdvancedDropdownItem
        {
            public T Item;

            public NWDAdvancedDropdownItem(string sName, T sItem) : base(sName)
            {
                Item = sItem;
            }
        }

        protected IEnumerable<T> Elements;
        protected string Name;
        protected Action<T> OnSelected;
        protected Func<T, string> NameFunction;

        public NWDCustomDropdown(AdvancedDropdownState sState, IEnumerable<T> sElements, string sName, Func<T, string> sNameFunction, Action<T> sOnSelected): base(sState)
        {
            Elements = sElements;
            Name = sName;
            NameFunction = sNameFunction;
            OnSelected = sOnSelected;
        }
        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new AdvancedDropdownItem(Name);

            foreach (T tElement in Elements)
            {
                root.AddChild(new NWDAdvancedDropdownItem(NameFunction?.Invoke(tElement), tElement));
            }

            return root;
        }
        protected override void ItemSelected(AdvancedDropdownItem sItem)
        {
            base.ItemSelected(sItem);

            NWDAdvancedDropdownItem tItem = sItem as NWDAdvancedDropdownItem;
            if (tItem != null)
            {
                OnSelected?.Invoke(tItem.Item);
            }
        }
    }
    public class NWDCustomDropdown<T, U> : NWDCustomDropdown<T>
    {
        Func<U, string> GroupNameFunction;
        Func<T, U> GetGroupFunction;

        public NWDCustomDropdown(AdvancedDropdownState sState, IEnumerable<T> sElements, string sName, Func<T, string> sNameFunction, Func<U, string> sGroupNameFunction, Func<T, U> sGetGroupFunction, Action<T> sOnSelected) : base(sState, sElements, sName, sNameFunction, sOnSelected)
        {
            GroupNameFunction = sGroupNameFunction;
            GetGroupFunction = sGetGroupFunction;
        }
        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new AdvancedDropdownItem(Name);

            IEnumerable<U> tGroups = Elements.Select(GetGroupFunction).Distinct();

            foreach (U tGroup in tGroups)
            {
                AdvancedDropdownItem tItem = new AdvancedDropdownItem(GroupNameFunction(tGroup));
                foreach (T tElement in Elements.Where(x => GetGroupFunction(x).Equals(tGroup)))
                {
                    tItem.AddChild(new NWDAdvancedDropdownItem(NameFunction?.Invoke(tElement), tElement));
                }

                root.AddChild(tItem);
            }

            return root;
        }
        protected override void ItemSelected(AdvancedDropdownItem sItem)
        {
            base.ItemSelected(sItem);

            NWDAdvancedDropdownItem tItem = sItem as NWDAdvancedDropdownItem;
            if (tItem != null)
            {
                OnSelected?.Invoke(tItem.Item);
            }
        }
    }
}
