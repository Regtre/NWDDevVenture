using System;
using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDFoundation.WebEdition.Models
{
    /// <summary>
    /// Use for the web edition form request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class NWDWebEditionRequest<T> where T : NWDBasicModel
    {
        /// <summary>
        /// Item instance use in web form
        /// </summary>
        public T? Item { set; get; }
        /// <summary>
        /// List of items use in list
        /// </summary>
        public List<T>? ItemsList { set; get; }
        /// <summary>
        /// Controller name to use for async methods
        /// </summary>
        public string ControllerName { set; get; } = "Unknown";
        /// <summary>
        /// Pagination information to layout the list page
        /// </summary>
        public NWDWebEditionPagination Pagination { set; get; } = new NWDWebEditionPagination();
    }
}