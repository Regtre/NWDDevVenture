using System;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.WebEdition.Models
{
    [Serializable]
    public class NWDWebEditionPagination
    {
        public const int K_PageRange = 2;
        public bool JsonClipboard { set; get; } = false;
        public string DefaultValues { set; get; } = "";
        public int ItemPerPage { set; get; } = 5;
        public int PageCount { set; get; } = 1;
        public int ActivePage { set; get; } = 1;
        public string SortBy { set; get; } = "";
        public int[] ItemsPerPageOptions { set; get; } = new int[] {  5, 10, 20, 30 };
        public string[] SortByOptions { set; get; } = new string[] { "Title", "Description", "Reference" };
        public string[] Columns { set; get; } = new string[] {"Reference" };
        public string ColumnPrimary { set; get; } = "Reference";
        public NWDWebEditionSortDirection SortDirection { set; get; } = NWDWebEditionSortDirection.Ascending;
        public string Reference { set; get; } = "";

        public int PagePreview => Math.Max(ActivePage - 1, 1);
        public int PageFirst { get; set; } = 1;
        public int PageLast => PageCount; 
        public int PageNext =>Math.Min(ActivePage + 1, PageCount);
        public int PageMin =>Math.Max(ActivePage - K_PageRange, 1);
        public int PageMax =>Math.Min(ActivePage + 1, PageCount);

        public bool ShowReference { set; get; } = false;
        public bool ShowButton { set; get; } = false;
        public NWDWebEditionPagination NewItemPerPage(int sItemPerPage)
        {
            return new NWDWebEditionPagination()
            {
                ItemPerPage = sItemPerPage,
                ActivePage = this.ActivePage,
                SortBy = this.SortBy,
                Reference = this.Reference,
                SortDirection = this.SortDirection,
                ShowReference = this.ShowReference,
                ShowButton = this.ShowButton,
                DefaultValues = this.DefaultValues,
            };
        }
        public NWDWebEditionPagination NewActivePage(int sActivePage)
        {
            return new NWDWebEditionPagination()
            {
                ItemPerPage = this.ItemPerPage,
                ActivePage = sActivePage,
                SortBy = this.SortBy,
                Reference = this.Reference,
                SortDirection = this.SortDirection,
                ShowReference = this.ShowReference,
                ShowButton = this.ShowButton,
                DefaultValues = this.DefaultValues,
            };
        }
        public NWDWebEditionPagination NewSortBy(string sSortBy)
        {
            return new NWDWebEditionPagination()
            {
                ItemPerPage = this.ItemPerPage,
                ActivePage = this.ActivePage,
                SortBy = sSortBy,
                Reference = this.Reference,
                SortDirection = this.SortDirection,
                ShowReference = this.ShowReference,
                ShowButton = this.ShowButton,
                DefaultValues = this.DefaultValues,
            };
        }
        public NWDWebEditionPagination NewSortDirection(NWDWebEditionSortDirection sSortDirection)
        {
            return new NWDWebEditionPagination()
            {
                ItemPerPage = this.ItemPerPage,
                ActivePage = this.ActivePage,
                SortBy = this.SortBy,
                Reference = this.Reference,
                SortDirection = sSortDirection,
                ShowReference = this.ShowReference,
                ShowButton = this.ShowButton,
                DefaultValues = this.DefaultValues,
            };
        }
        public NWDWebEditionPagination NewReference(string sReference)
        {
            return new NWDWebEditionPagination()
            {
                ItemPerPage = this.ItemPerPage,
                ActivePage = this.ActivePage,
                SortBy = this.SortBy,
                Reference = sReference,
                SortDirection = this.SortDirection,
                ShowReference = this.ShowReference,
                ShowButton = this.ShowButton,
                DefaultValues = this.DefaultValues,
            };
        }
        
    }
}