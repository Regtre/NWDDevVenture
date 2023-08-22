using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Newtonsoft.Json;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDFoundation.WebEdition.Models;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools.Sessions;

namespace NWDWebEditor.Controllers;

public abstract class NWDModelEditionAsyncController<T> : NWDRawController where T : NWDPlayerData, new()
{
    #region Constant

    protected const string KControllerName = "ControllerName";
    protected const string KResultToUse = "ResultToUse";
    protected const string KModelToUse = "ModelToUse";
    protected const string KJavascript = "Javascript";

    #endregion

    #region Static properties

    private NWDWebEditionModelDataManagerStandard<T> _Items = new NWDWebEditionModelDataManagerStandard<T>();
    private NWDWebEditionPagination? _Pagination;
    private List<string> JavascriptList = new List<string>();

    public void AddJavascript(string sScript)
    {
        JavascriptList.Add(sScript);
    }

    #endregion

    #region Instance properties

    private NWDWebEditionPagination Pagination(NWDWebEditionPagination? sPagination)
    {
        if (_Pagination == null)
        {
            NWDWebEditionPagination tPagination = new NWDWebEditionPagination()
            {
                JsonClipboard = false,
                DefaultValues = "",
                ItemPerPage = 5,
                PageCount = 1,
                ActivePage = 1,
                SortBy = "",
                ItemsPerPageOptions = new int[] { 5, 10, 20, 30 },
                SortByOptions = new string[] { "Reference" },
                Columns = new string[] { "Reference" },
                ColumnPrimary = "Reference",
                SortDirection = NWDWebEditionSortDirection.Ascending,
                Reference = "",
                ShowButton = true,
                ShowReference = false,
            };
            NWDWebClassDescriptionAttribute? tAttribute = typeof(T).GetCustomAttributes(typeof(NWDWebClassDescriptionAttribute), true).FirstOrDefault() as NWDWebClassDescriptionAttribute;
            if (tAttribute != null)
            {
                tPagination.ItemsPerPageOptions = tAttribute.Infos.ItemsPerPageOption;
                tPagination.JsonClipboard = tAttribute.Infos.JsonClipboard;
                tPagination.ShowButton = tAttribute.Infos.ShowButton;
                tPagination.ShowReference = tAttribute.Infos.ShowReference;
            }

            List<string> tSortByOptionsList = new List<string>();
            List<string> tColumnsList = new List<string>();
            foreach (PropertyInfo tProp in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                NWDWebPropertyDescriptionAttribute? tAttribut = tProp.GetCustomAttribute<NWDWebPropertyDescriptionAttribute>();
                if (tAttribut != null)
                {
                    if (tAttribut.Infos.UseAsSortBy)
                    {
                        tSortByOptionsList.Add(tProp.Name);
                    }

                    if (tAttribut.Infos.UseAsColumn)
                    {
                        if (tAttribut.Infos.IsPrimaryColumn)
                        {
                            tColumnsList.Insert(0, tProp.Name);
                            tPagination.ColumnPrimary = tProp.Name;
                        }
                        else
                        {
                            tColumnsList.Add(tProp.Name);
                        }
                    }
                }
            }

            if (tSortByOptionsList.Count > 0)
            {
                tPagination.SortByOptions = tSortByOptionsList.ToArray();
            }

            if (tColumnsList.Count > 0)
            {
                tPagination.Columns = tColumnsList.ToArray();
            }

            _Pagination = tPagination;
        }

        NWDWebEditionPagination? rReturn = null;
        if (sPagination != null)
        {
            rReturn = new NWDWebEditionPagination()
            {
                JsonClipboard = _Pagination.JsonClipboard,
                DefaultValues = sPagination.DefaultValues,
                ItemPerPage = sPagination.ItemPerPage,
                PageCount = sPagination.PageCount,
                ActivePage = sPagination.ActivePage,
                SortBy = sPagination.SortBy,
                ItemsPerPageOptions = _Pagination.ItemsPerPageOptions,
                SortByOptions = _Pagination.SortByOptions,
                Columns = _Pagination.Columns,
                ColumnPrimary = _Pagination.ColumnPrimary,
                SortDirection = sPagination.SortDirection,
                Reference = sPagination.Reference,
                ShowButton = _Pagination.ShowButton
            };
        }
        else
        {
            rReturn = new NWDWebEditionPagination()
            {
                JsonClipboard = _Pagination.JsonClipboard,
                DefaultValues = _Pagination.DefaultValues,
                ItemPerPage = _Pagination.ItemsPerPageOptions[0],
                PageCount = _Pagination.PageCount,
                ActivePage = 1,
                SortBy = _Pagination.SortByOptions[0],
                ItemsPerPageOptions = _Pagination.ItemsPerPageOptions,
                SortByOptions = _Pagination.SortByOptions,
                Columns = _Pagination.Columns,
                ColumnPrimary = _Pagination.ColumnPrimary,
                SortDirection = _Pagination.SortDirection,
                Reference = string.Empty,
                ShowButton = _Pagination.ShowButton
            };
        }

        return rReturn;
    }

    public bool IncludeInResults(Dictionary<string, string> sDefaultReference)
    {
        return true;
    }

    #endregion

    #region methods for cookie/session user preference edition

    private static readonly NWDSessionInt SessionItemsPerPage = new NWDSessionInt(typeof(T).Name + "ItemPerPage", typeof(T).Name + " item per page", "Number of items per page in controller edition",
        NWDSessionDefinitionGroup.Preference, 5);

    private static readonly NWDSessionString SessionSortBy = new NWDSessionString(typeof(T).Name + "ItemSortBy", typeof(T).Name + " item sorting", "Sort items controller edition", NWDSessionDefinitionGroup.Preference,
        "Reference");

    private static readonly NWDSessionEnum<NWDWebEditionSortDirection> SessionSortDirection = new NWDSessionEnum<NWDWebEditionSortDirection>(typeof(T).Name + "ItemSortDirection",
        typeof(T).Name + " item sorting direction", "Sort items controller edition",
        NWDSessionDefinitionGroup.Preference, NWDWebEditionSortDirection.Ascending);

    public NWDSessionInt? UserItemsPerPage()
    {
        return SessionItemsPerPage;
    }

    public NWDSessionString? UserSortBy()
    {
        return SessionSortBy;
    }

    public NWDSessionEnum<NWDWebEditionSortDirection>? UserSortDirection()
    {
        return SessionSortDirection;
    }

    #endregion

    #region Virtual methods for paths to views edition

    public virtual void Before(string? sReference)
    {
    }
    public virtual NWDSideBarBlock[]? GetSideBarBlock(T? sObject)
    {
        return null;
    }
    public virtual NWDSideBarAnnexe[]? GetSideBarAnnexe(T? sObject)
    {
        return null;
    }
    public virtual NWDNavBarMenu[]? GetNavBarMenu(T? sObject)
    {
        return null;
    }

    public virtual void After(string? sReference)
    {
    }

    public virtual string DirectoryView()
    {
        return "NWDModelEditionAsync";
    }

    public virtual string PathToIndexView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(Index) + ".cshtml";
    }

    public virtual string PathToListAllView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(ListAll) + ".cshtml";
    }

    public virtual string PathToViewAllView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(ViewAll) + ".cshtml";
    }

    public virtual string PathToPreviewView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(Preview) + ".cshtml";
    }

    public virtual string PathToEditView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(Edit) + ".cshtml";
    }

    public virtual string PathToModifyView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(Modify) + ".cshtml";
    }

    public virtual string PathToTrashView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(Trash) + ".cshtml";
    }

    public virtual string PathToShowView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(Show) + ".cshtml";
    }


    public virtual string PathToModifyInDivView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(ModifyInDiv) + ".cshtml";
    }

    public virtual string PathToShowInDivView()
    {
        return "/Views/" + DirectoryView() + "/" + nameof(ShowInDiv) + ".cshtml";
    }

    #endregion

    #region Virtual instances methods for Javascript post operation

    public virtual string IndexJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    public virtual string ListAllJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    public virtual string ViewAllJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    public virtual string PreviewJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    public virtual string EditJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    public virtual string ModifyJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    public virtual string TrashJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    public virtual string ShowJavascriptPostOperation(T? sObject)
    {
        return "";
    }

    #endregion

    #region Virtual instances methods

    protected virtual Func<U, bool> Filter<U>(Dictionary<string, string> sDictionary) where U : NWDPlayerData
    {
        return (sX => sX.GetType() == typeof(U));
    }

    public virtual void PrepareBeforeEdit(T? sObject, Dictionary<string, string>? sValues, HttpContext sHttpContext)
    {
        NWDLogger.TraceAttention(nameof(PrepareBeforeEdit));
    }

    public virtual void NewAddon(T sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        NWDLogger.TraceAttention(nameof(NewAddon));
    }

    public virtual void UpdateAddon(T sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        NWDLogger.TraceAttention(nameof(UpdateAddon));
    }

    public virtual void DeleteAddon(T sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        NWDLogger.TraceAttention(nameof(DeleteAddon));
    }

    #endregion

    #region Instance methods for async editions

    // public virtual void UseDictionnaryValues(T sObject, Dictionary<string, string> sValues)
    // {
    //     foreach (KeyValuePair<string, string> tKeyValue in sValues)
    //     {
    //         PropertyInfo tFieldInfo = typeof(T).GetProperty(tKeyValue.Key, BindingFlags.Public | BindingFlags.Instance);
    //         if (tFieldInfo != null)
    //         {
    //             Type tTypeOfThis = tFieldInfo.PropertyType;
    //             string tValue = tKeyValue.Value;
    //             if (tTypeOfThis.IsEnum)
    //             {
    //                 if (string.IsNullOrEmpty(tValue) == false)
    //                 {
    //                     tFieldInfo.SetValue(sObject, Enum.Parse(tFieldInfo.PropertyType, tValue, true), null);
    //                 }
    //             }
    //             else
    //             {
    //                 if (tTypeOfThis == typeof(Int16))
    //                 {
    //                     tFieldInfo.SetValue(sObject, Int16.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int32))
    //                 {
    //                     tFieldInfo.SetValue(sObject, Int32.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(long) || tTypeOfThis == typeof(Int64))
    //                 {
    //                     tFieldInfo.SetValue(sObject, Int64.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(UInt16))
    //                 {
    //                     tFieldInfo.SetValue(sObject, UInt16.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(uint) || tTypeOfThis == typeof(UInt32))
    //                 {
    //                     tFieldInfo.SetValue(sObject, UInt32.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(ulong) || tTypeOfThis == typeof(UInt64))
    //                 {
    //                     tFieldInfo.SetValue(sObject, UInt64.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(float))
    //                 {
    //                     tFieldInfo.SetValue(sObject, float.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Double))
    //                 {
    //                     tFieldInfo.SetValue(sObject, double.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(bool))
    //                 {
    //                     tFieldInfo.SetValue(sObject, bool.Parse(tFieldInfo.Name));
    //                 }
    //                 else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
    //                 {
    //                     tFieldInfo.SetValue(sObject, tValue);
    //                 }
    //                 // else if (IsGenericReference(tTypeOfThis))
    //                 // {
    //                 //     string tValue = tRdr.GetString(tFieldInfo.Name);
    //                 //     if (string.IsNullOrEmpty(tValue))
    //                 //     {
    //                 //         tValue = string.Empty;
    //                 //     }
    //                 //     Object tValueObject = tFieldInfo.GetValue(tObject);
    //                 //     ulong tValueToULong = 0;
    //                 //     ulong.TryParse(tValue, out tValueToULong);
    //                 //     tValueObject.GetType().GetProperty(nameof(NWDReference<NWDBasicModel>.Reference))
    //                 //         ?.SetValue(tValueObject, tValueToULong);
    //                 //     tFieldInfo.SetValue(tObject, tValueObject);
    //                 // }
    //                 // else if (IsGenericReferencesArray(tTypeOfThis))
    //                 // {
    //                 //     string tValue = tRdr.GetString(tFieldInfo.Name);
    //                 //     if (string.IsNullOrEmpty(tValue))
    //                 //     {
    //                 //         tValue = string.Empty;
    //                 //     }
    //                 //     Object tValueObject = tFieldInfo.GetValue(tObject);
    //                 //     tValueObject.GetType().GetProperty(nameof(NWDReferencesArray<NWDBasicModel>.References))
    //                 //         ?.SetValue(tValueObject, tValue);
    //                 // }
    //             }
    //         }
    //     }
    // }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult Index()
    {
        NWDLogger.TraceAttention(nameof(Index));
        PageInformation.AddTinymce();
        Before(null);
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(null)
        };

        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNewResult);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(IndexJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(null);
        PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(null), GetSideBarAnnexe(null),HttpContext);
        PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(null),HttpContext);
        PageInformation.SetNavFooter(null,HttpContext);
        AddFrequentlyAskedQuestions();
        return View(PathToIndexView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    [HttpGet]
    public IActionResult ViewAll(string? sDictionaryToSelect)
    {
        NWDLogger.TraceAttention(nameof(ViewAll));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(null)
        };
        tNewResult.Pagination.DefaultValues = sDictionaryToSelect ?? "";
        if (tNewResult.Pagination.SortByOptions.Length > 0)
        {
            tNewResult.Pagination.SortBy = tNewResult.Pagination.SortByOptions[0];
        }

        if (tNewResult.Pagination.ItemsPerPageOptions.Length > 0)
        {
            tNewResult.Pagination.ItemPerPage = tNewResult.Pagination.ItemsPerPageOptions[0];
        }

        NWDSessionInt? tUserItemsPerPage = UserItemsPerPage();
        NWDSessionString? tUserSortBy = UserSortBy();
        NWDSessionEnum<NWDWebEditionSortDirection>? tUserSortDirection = UserSortDirection();

        if (tUserItemsPerPage != null)
        {
            if (tUserItemsPerPage.Exists(HttpContext))
            {
                int tIndex = Math.Max(0, Array.IndexOf(tNewResult.Pagination.ItemsPerPageOptions, tUserItemsPerPage.GetValue(HttpContext)));
                tNewResult.Pagination.ItemPerPage = tNewResult.Pagination.ItemsPerPageOptions[tIndex];
            }
        }

        if (tUserSortBy != null)
        {
            if (tUserSortBy.Exists(HttpContext))
            {
                int tIndex = Math.Max(0, Array.IndexOf(tNewResult.Pagination.SortByOptions, tUserSortBy.GetValue(HttpContext)));
                tNewResult.Pagination.SortBy = tNewResult.Pagination.SortByOptions[tIndex];
            }
        }

        if (tUserSortDirection != null)
        {
            if (tUserSortDirection.Exists(HttpContext))
            {
                tNewResult.Pagination.SortDirection = tUserSortDirection.GetValue(HttpContext);
            }
        }

        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(tNewResult.Pagination.DefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(tNewResult.Pagination.DefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        Before(null);
        tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
        tNewResult.ItemsList = _Items.GetItemAtActualPage<T>(HttpContext, tNewResult.Pagination, Filter<T>(tDictionaryToSelect));
        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNewResult);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(ViewAllJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(null);
        return View(PathToViewAllView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult ViewPage(NWDWebEditionPagination sRequest)
    {
        NWDLogger.TraceAttention(nameof(ViewPage));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(sRequest)
        };
        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(sRequest.DefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sRequest.DefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
        tNewResult.Pagination.ActivePage = Math.Min(tNewResult.Pagination.PageCount, sRequest.ActivePage);
        tNewResult.ItemsList = _Items.GetItemAtActualPage<T>(HttpContext, tNewResult.Pagination, Filter<T>(tDictionaryToSelect));
        NWDSessionInt? tUserItemsPerPage = UserItemsPerPage();
        NWDSessionString? tUserSortBy = UserSortBy();
        NWDSessionEnum<NWDWebEditionSortDirection>? tUserSortDirection = UserSortDirection();

        if (tUserItemsPerPage != null)
        {
            tUserItemsPerPage.SetValue(HttpContext, tNewResult.Pagination.ItemPerPage);
        }

        if (tUserSortBy != null)
        {
            tUserSortBy.SetValue(HttpContext, tNewResult.Pagination.SortBy);
        }

        if (tUserSortDirection != null)
        {
            tUserSortDirection.SetValue(HttpContext, tNewResult.Pagination.SortDirection);
        }

        Before(sRequest.Reference);
        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNewResult);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(ViewAllJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(sRequest.Reference);
        return View(PathToViewAllView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    [HttpGet]
    public IActionResult ListAll(string? sDictionaryToSelect)
    {
        NWDLogger.TraceAttention(nameof(ListAll));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(null)
        };
        tNewResult.Pagination.DefaultValues = sDictionaryToSelect ?? "";
        if (tNewResult.Pagination.SortByOptions.Length > 0)
        {
            tNewResult.Pagination.SortBy = tNewResult.Pagination.SortByOptions[0];
        }

        if (tNewResult.Pagination.ItemsPerPageOptions.Length > 0)
        {
            tNewResult.Pagination.ItemPerPage = tNewResult.Pagination.ItemsPerPageOptions[0];
        }

        NWDSessionInt? tUserItemsPerPage = UserItemsPerPage();
        NWDSessionString? tUserSortBy = UserSortBy();
        NWDSessionEnum<NWDWebEditionSortDirection>? tUserSortDirection = UserSortDirection();

        if (tUserItemsPerPage != null)
        {
            if (tUserItemsPerPage.Exists(HttpContext))
            {
                int tIndex = Math.Max(0, Array.IndexOf(tNewResult.Pagination.ItemsPerPageOptions, tUserItemsPerPage.GetValue(HttpContext)));
                tNewResult.Pagination.ItemPerPage = tNewResult.Pagination.ItemsPerPageOptions[tIndex];
            }
        }

        if (tUserSortBy != null)
        {
            if (tUserSortBy.Exists(HttpContext))
            {
                int tIndex = Math.Max(0, Array.IndexOf(tNewResult.Pagination.SortByOptions, tUserSortBy.GetValue(HttpContext)));
                tNewResult.Pagination.SortBy = tNewResult.Pagination.SortByOptions[tIndex];
            }
        }

        if (tUserSortDirection != null)
        {
            if (tUserSortDirection.Exists(HttpContext))
            {
                tNewResult.Pagination.SortDirection = tUserSortDirection.GetValue(HttpContext);
            }
        }

        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(tNewResult.Pagination.DefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(tNewResult.Pagination.DefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
        tNewResult.ItemsList = _Items.GetItemAtActualPage<T>(HttpContext, tNewResult.Pagination, Filter<T>(tDictionaryToSelect));
        if (tNewResult.Pagination.ActivePage == 0)
        {
            tNewResult.Pagination.ActivePage = 1;
        }

        Before(null);
        ViewData.Add(KControllerName, GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNewResult);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(ListAllJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(null);
        return View(PathToListAllView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult ListPage(NWDWebEditionPagination sRequest)
    {
        NWDLogger.TraceAttention(nameof(ListPage));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(sRequest)
        };
        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(sRequest.DefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sRequest.DefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
        tNewResult.Pagination.ActivePage = Math.Min(tNewResult.Pagination.PageCount, sRequest.ActivePage);
        tNewResult.ItemsList = _Items.GetItemAtActualPage<T>(HttpContext, tNewResult.Pagination, Filter<T>(tDictionaryToSelect));

        NWDSessionInt? tUserItemsPerPage = UserItemsPerPage();
        NWDSessionString? tUserSortBy = UserSortBy();
        NWDSessionEnum<NWDWebEditionSortDirection>? tUserSortDirection = UserSortDirection();

        if (tUserItemsPerPage != null)
        {
            tUserItemsPerPage.SetValue(HttpContext, tNewResult.Pagination.ItemPerPage);
        }

        if (tUserSortBy != null)
        {
            tUserSortBy.SetValue(HttpContext, tNewResult.Pagination.SortBy);
        }

        if (tUserSortDirection != null)
        {
            tUserSortDirection.SetValue(HttpContext, tNewResult.Pagination.SortDirection);
        }

        if (tNewResult.Pagination.ActivePage == 0)
        {
            tNewResult.Pagination.ActivePage = 1;
        }

        Before(null);
        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNewResult);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(ListAllJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(null);
        return View(PathToListAllView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult Add(NWDWebEditionPagination sRequest)
    {
        NWDLogger.TraceAttention(nameof(Add));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(sRequest)
        };
        T tNew = new T();

        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(sRequest.DefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sRequest.DefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        Before(null);
        NewAddon(tNew, tDictionaryToSelect, HttpContext);
        PrepareBeforeEdit(tNew, tDictionaryToSelect, HttpContext);
        _Items.Add(HttpContext, tNew);
        tNewResult.Pagination = sRequest;
        tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
        tNewResult.Pagination.ActivePage = _Items.GetPageOfItem(HttpContext, tNew, sRequest, Filter<T>(tDictionaryToSelect));
        tNewResult.ItemsList = _Items.GetItemAtActualPage<T>(HttpContext, tNewResult.Pagination, Filter<T>(tDictionaryToSelect));
        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNewResult);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(ListAllJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(tNew.Reference.ToString());
        return View(PathToListAllView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult NewEdit(NWDWebEditionPagination sRequest)
    {
        NWDLogger.TraceAttention(nameof(NewEdit));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(sRequest),
        };
        T tNew = new T();
        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(sRequest.DefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sRequest.DefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        Before(null);
        NewAddon(tNew, tDictionaryToSelect, HttpContext);
        PrepareBeforeEdit(tNew, tDictionaryToSelect, HttpContext);
        tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
        tNewResult.Pagination.ActivePage = _Items.GetPageOfItem(HttpContext, tNew, sRequest, Filter<T>(tDictionaryToSelect));
        tNewResult.Item = tNew;
        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNewResult);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(EditJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(tNew.Reference.ToString());
        return View(PathToEditView());
    }

    [HttpPost]
    [NWDAuthorizeByAuthentication(true, false)]
    [ValidateAntiForgeryToken]
    public IActionResult Update(NWDWebEditionRequest<T> sRequest)
    {
        NWDLogger.TraceAttention(nameof(Update));
        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(sRequest.Pagination.DefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sRequest.Pagination.DefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        Before(sRequest.Item.Reference.ToString());
        if (sRequest.Item != null)
        {
            UpdateAddon(sRequest.Item, tDictionaryToSelect, HttpContext);
            _Items.Update<T>(HttpContext, sRequest.Item);
            NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
            {
                ControllerName = this.GetType().Name.Replace("Controller", ""),
                Pagination = Pagination(sRequest.Pagination)
            };
            tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
            tNewResult.ItemsList = _Items.GetItemAtActualPage<T>(HttpContext, tNewResult.Pagination, Filter<T>(tDictionaryToSelect));
            tNewResult.Item = sRequest.Item;
            ViewData.Add(KResultToUse, tNewResult);
        }

        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        AddJavascript(ListAllJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(sRequest.Item.Reference.ToString());
        return View(PathToListAllView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult Delete(NWDWebEditionPagination sRequest)
    {
        NWDLogger.TraceAttention(nameof(Delete));
        T? tItem = _Items.GetReal<T>(HttpContext, ulong.Parse(sRequest.Reference));
        if (tItem != null)
        {
            Dictionary<string, string>? tDictionaryToSelect = null;
            if (string.IsNullOrEmpty(sRequest.DefaultValues) == false)
            {
                tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sRequest.DefaultValues);
            }

            if (tDictionaryToSelect == null)
            {
                tDictionaryToSelect = new Dictionary<string, string>();
            }

            DeleteAddon(tItem, tDictionaryToSelect, HttpContext);

            _Items.Delete(HttpContext, ulong.Parse(sRequest.Reference));
            NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
            {
                ControllerName = GetType().Name.Replace("Controller", ""),
                Pagination = Pagination(sRequest)
            };

            tNewResult.Pagination.PageCount = _Items.PagesCount(HttpContext, tNewResult.Pagination.ItemPerPage, Filter<T>(tDictionaryToSelect));
            tNewResult.ItemsList = _Items.GetItemAtActualPage<T>(HttpContext, tNewResult.Pagination, Filter<T>(tDictionaryToSelect));
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tNewResult);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        }

        Before(sRequest.Reference);
        AddJavascript(ListAllJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(sRequest.Reference);
        return View(PathToListAllView());
    }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult Edit(NWDWebEditionPagination sRequest)
    {
        NWDLogger.TraceAttention(nameof(Edit));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(sRequest),
            Item = _Items.GetReal<T>(HttpContext, ulong.Parse(sRequest.Reference))
        };
        if (tNewResult.Item != null)
        {
            Before(sRequest.Reference);
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            PrepareBeforeEdit(tNewResult.Item, null, HttpContext);
            ViewData.Add(KResultToUse, tNewResult);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            AddJavascript(EditJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sRequest.Reference);
            return View(PathToEditView());
        }
        else
        {
            return View("/Views/Shared/_ErrorAsync.cshtml");
        }
    }

    [NWDAuthorizeByAuthentication(true, false)]
    public IActionResult Preview(NWDWebEditionPagination sRequest)
    {
        NWDLogger.TraceAttention(nameof(Preview));
        NWDWebEditionRequest<T> tNewResult = new NWDWebEditionRequest<T>
        {
            ControllerName = this.GetType().Name.Replace("Controller", ""),
            Pagination = Pagination(sRequest),
            Item = _Items.GetReal<T>(HttpContext, ulong.Parse(sRequest.Reference))
        };
        if (tNewResult.Item != null)
        {
            Before(sRequest.Reference);
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tNewResult);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            AddJavascript(PreviewJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sRequest.Reference);
            return View(PathToPreviewView());
        }
        else
        {
            return View("/Views/Shared/_ErrorAsync.cshtml");
        }
    }

    #endregion

    #region Instance methods for page edition

    [HttpGet]
    [NWDAuthorizeByAuthentication(true)]
    public IActionResult Show(string sReference)
    {
        NWDLogger.TraceAttention(nameof(Show));
        T? tItem = _Items.GetReal<T>(HttpContext, ulong.Parse(sReference));
        if (tItem != null)
        {
            Before(sReference);
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tItem);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            ViewData.Add(typeof(T).Name, tItem);
            AddJavascript(ShowJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sReference);
            PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
            PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
            PageInformation.SetNavFooter(null,HttpContext);
            AddFrequentlyAskedQuestions();
            return View(PathToShowView());
        }
        else
        {
            return View("/Views/Shared/_Error.cshtml");
        }
    }

    [NWDAuthorizeByAuthentication(true)]
    public IActionResult New(string? sDefaultValues)
    {
        NWDLogger.TraceAttention(nameof(New));
        PageInformation.AddTinymce();
        T tNew = new T();
        Dictionary<string, string>? tDictionaryToSelect = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(sDefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sDefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        Before(null);
        NewAddon(tNew, tDictionaryToSelect, HttpContext);
        PrepareBeforeEdit(tNew, tDictionaryToSelect, HttpContext);
        ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
        ViewData.Add(KResultToUse, tNew);
        ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
        ViewData.Add(typeof(T).Name, tNew);
        AddJavascript(ModifyJavascriptPostOperation(null));
        ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
        After(tNew.Reference.ToString());
        PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tNew), GetSideBarAnnexe(tNew),HttpContext);
        PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tNew),HttpContext);
        PageInformation.SetNavFooter(null,HttpContext);
        AddFrequentlyAskedQuestions();
        return View(PathToModifyView());
    }

    [HttpGet]
    [NWDAuthorizeByAuthentication(true)]
    public IActionResult Modify(string sReference, string? sDefaultValues)
    {
        NWDLogger.TraceAttention(nameof(Modify));
        PageInformation.AddTinymce();
        T? tItem = _Items.GetReal<T>(HttpContext, ulong.Parse(sReference));
        if (tItem != null)
        {
            Before(sReference);
            PrepareBeforeEdit(tItem, null, HttpContext);
            UpdateAddon(tItem, new Dictionary<string, string>(), HttpContext);
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tItem);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            ViewData.Add(typeof(T).Name, tItem);
            AddJavascript(ModifyJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sReference);
            PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
            PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
            PageInformation.SetNavFooter(null,HttpContext);
            AddFrequentlyAskedQuestions();
            return View(PathToModifyView());
        }
        else
        {
            return View("/Views/Shared/_Error.cshtml");
        }
    }

    [HttpPost]
    [NWDAuthorizeByAuthentication(true)]
    [ValidateAntiForgeryToken]
    public IActionResult Save(T sRequest, string? sDefaultValues)
    {
        NWDLogger.TraceAttention(nameof(Save));
        NWDWebEditionPagination tPagination = new NWDWebEditionPagination();
        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(sDefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sDefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        UpdateAddon(sRequest, tDictionaryToSelect, HttpContext);
        T? tItem = _Items.Update<T>(HttpContext, sRequest);
        if (tItem != null)
        {
            Before(sRequest.Reference.ToString());
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tItem);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            AddJavascript(ModifyJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sRequest.Reference.ToString());
            PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
            PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
            PageInformation.SetNavFooter(null,HttpContext);
            AddFrequentlyAskedQuestions();
            return View(PathToShowView());
        }
        else
        {
            return View("/Views/Shared/_Error.cshtml");
        }
    }

    [NWDAuthorizeByAuthentication(true)]
    public IActionResult Trash(string sReference, string? sDefaultValues)
    {
        NWDLogger.TraceAttention(nameof(Trash));
        T? tItem = _Items.GetReal<T>(HttpContext, ulong.Parse(sReference));

        Dictionary<string, string>? tDictionaryToSelect = null;
        if (string.IsNullOrEmpty(sDefaultValues) == false)
        {
            tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sDefaultValues);
        }

        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }

        if (tItem != null)
        {
            Before(sReference);
            DeleteAddon(tItem, tDictionaryToSelect, HttpContext);
            _Items.Delete(HttpContext, tItem.Reference);
            ViewData.Add(KControllerName, GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tItem);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            AddJavascript(TrashJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sReference);
            PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
            PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
            PageInformation.SetNavFooter(null,HttpContext);
            AddFrequentlyAskedQuestions();
            return View(PathToTrashView());
        }
        else
        {
            return View("/Views/Shared/_Error.cshtml");
        }
    }

    #endregion

    #region Instance methods for page edition

    [HttpGet]
    [NWDAuthorizeByAuthentication(true)]
    public IActionResult ShowInDiv(string sReference)
    {
        NWDLogger.TraceAttention(nameof(Show));
        T? tItem = _Items.GetReal<T>(HttpContext, ulong.Parse(sReference));
        if (tItem != null)
        {
            Before(sReference);
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tItem);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            ViewData.Add(typeof(T).Name, tItem);
            AddJavascript(ShowJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sReference);
            return View(PathToShowInDivView());
        }
        else
        {
            return View("/Views/Shared/_Error.cshtml");
        }
    }

    [HttpGet]
    [NWDAuthorizeByAuthentication(true)]
    public IActionResult ModifyInDiv(string sReference, string? sDefaultValues)
    {
        NWDLogger.TraceAttention(nameof(ModifyInDiv));
        NWDLogger.TraceAttention("try to "+nameof(ModifyInDiv)+" reference : " + sReference);
        PageInformation.AddTinymce();
        T? tItem = _Items.GetReal<T>(HttpContext, ulong.Parse(sReference));
        if (tItem == null)
        {
            tItem = new T();
            Dictionary<string, string>? tDictionaryToSelect = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(sDefaultValues) == false)
            {
                tDictionaryToSelect = JsonConvert.DeserializeObject<Dictionary<string, string>>(sDefaultValues);
            }
            if (tDictionaryToSelect == null)
            {
                tDictionaryToSelect = new Dictionary<string, string>();
            }
            NewAddon(tItem, tDictionaryToSelect, HttpContext);
        }

        if (tItem != null)
        {
            Before(sReference);
            PrepareBeforeEdit(tItem, null, HttpContext);
            UpdateAddon(tItem, new Dictionary<string, string>(), HttpContext);
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tItem);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            ViewData.Add(typeof(T).Name, tItem);
            AddJavascript(ModifyJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sReference);
            return View(PathToModifyInDivView());
        }
        else
        {
            return View("/Views/Shared/_Error.cshtml");
        }
    }

    [HttpPost]
    [NWDAuthorizeByAuthentication(true)]
    [ValidateAntiForgeryToken]
    public IActionResult SaveInDiv(T sRequest)
    {
        NWDLogger.TraceAttention(nameof(SaveInDiv));
        NWDLogger.TraceAttention("try to "+nameof(SaveInDiv)+" reference" + sRequest.Reference.ToString());
        Dictionary<string, string>? tDictionaryToSelect = null;
        if (tDictionaryToSelect == null)
        {
            tDictionaryToSelect = new Dictionary<string, string>();
        }
        UpdateAddon(sRequest, tDictionaryToSelect, HttpContext);
        T? tItem = _Items.Update<T>(HttpContext, sRequest);
        if (tItem != null)
        {
            Before(sRequest.Reference.ToString());
            ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
            ViewData.Add(KResultToUse, tItem);
            ViewData.Add(KModelToUse, typeof(T).AssemblyQualifiedName);
            AddJavascript(ModifyJavascriptPostOperation(null));
            ViewData.Add(KJavascript, string.Join(string.Empty, JavascriptList));
            After(sRequest.Reference.ToString());
            return View(PathToShowInDivView());
        }
        else
        {
            return View("/Views/Shared/_Error.cshtml");
        }
    }

    #endregion
}