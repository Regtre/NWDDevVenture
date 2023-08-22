using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NWDWebRuntime.Models;

namespace NWDWebRuntime.Tools
{
    /// <summary>
    /// Create render HTML for specific view outside the website context
    /// </summary>
    public class NWDViewRenderer
    {
        private IRazorViewEngine _MyViewEngine;
        private ITempDataProvider _MyTempDataProvider;
        private IServiceProvider _MyServiceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sViewEngine"></param>
        /// <param name="sTempDataProvider"></param>
        /// <param name="sServiceProvider"></param>
        public NWDViewRenderer(
            IRazorViewEngine sViewEngine,
            ITempDataProvider sTempDataProvider,
            IServiceProvider sServiceProvider)
        {
            _MyViewEngine = sViewEngine;
            _MyTempDataProvider = sTempDataProvider;
            _MyServiceProvider = sServiceProvider;
        }
        
        private IView FindView(ActionContext sActionContext, string sViewName)
        {
            var tGetViewResult = _MyViewEngine.GetView(executingFilePath: null, viewPath: sViewName, isMainPage: false);
            if (tGetViewResult.Success)
            {
                return tGetViewResult.View;
            }

            var tFindViewResult = _MyViewEngine.FindView(sActionContext, sViewName, isMainPage: false);
            if (tFindViewResult.Success)
            {
                return tFindViewResult.View;
            }

            var tSearchedLocations = tGetViewResult.SearchedLocations.Concat(tFindViewResult.SearchedLocations);
            var tErrorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{sViewName}'. The following locations were searched:" }.Concat(tSearchedLocations));

            throw new InvalidOperationException(tErrorMessage);
        }

        /// <summary>
        /// Render the model with the path's view
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="sHttpContext"></param>
        /// <param name="sPath"></param>
        /// <param name="sModel"></param>
        /// <returns></returns>
        public string RenderWithModel<TModel>(HttpContext sHttpContext, string sPath, TModel sModel)
        {
            var tActionContext = GetActionContext(sHttpContext);
            var tView = FindView(tActionContext, sPath);
            using (var tOutput = new StringWriter())
            {
                var tViewContext = new ViewContext(
                    tActionContext,
                    tView,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = sModel
                    },
                    new TempDataDictionary(
                        tActionContext.HttpContext,
                        _MyTempDataProvider),
                    tOutput,
                    new HtmlHelperOptions());
                tView.RenderAsync(tViewContext).GetAwaiter().GetResult();
                return tOutput.ToString();
            }
        }
        public string Render(HttpContext sHttpContext, string sPath, ViewDataDictionary sViewData)
        {
            var tActionContext = GetActionContext(sHttpContext);
            var tView = FindView(tActionContext, sPath);

            using (var tOutput = new StringWriter())
            {
                var tViewContext = new ViewContext(
                    tActionContext,
                    tView,
                    sViewData,
                    new TempDataDictionary(
                        tActionContext.HttpContext,
                        _MyTempDataProvider),
                    tOutput,
                    new HtmlHelperOptions());

                tView.RenderAsync(tViewContext).GetAwaiter().GetResult();

                return tOutput.ToString();
            }
        }
        private ActionContext GetActionContext(HttpContext sHttpContext)
        {
            sHttpContext.RequestServices = _MyServiceProvider;
            return new ActionContext(sHttpContext, new RouteData(), new ActionDescriptor());
        }
    }
}