using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DigitalHealthCheckWeb.Pages
{
    /// <summary>
    /// Renders a page into a HTML string.
    /// </summary>
    /// <seealso cref="DigitalHealthCheckWeb.Pages.IPageRenderer" />
    public class PageRenderer : IPageRenderer
    {
        private readonly IRazorPageActivator razorPageActivator;
        private readonly IRazorViewEngine razorViewEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRenderer"/> class.
        /// </summary>
        /// <param name="razorViewEngine">The razor view engine.</param>
        /// <param name="razorPageActivator">The razor page activator.</param>
        /// <exception cref="ArgumentNullException">
        /// razorViewEngine
        /// or
        /// razorPageActivator
        /// </exception>
        public PageRenderer(IRazorViewEngine razorViewEngine, IRazorPageActivator razorPageActivator)
        {
            this.razorViewEngine = razorViewEngine ?? throw new ArgumentNullException(nameof(razorViewEngine));
            this.razorPageActivator = razorPageActivator ?? throw new ArgumentNullException(nameof(razorPageActivator));
        }

        /// <summary>
        /// Renders a page into a HTML string asynchronously.
        /// </summary>
        /// <param name="pageName">Name of the page to render. This is the route to load.</param>
        /// <param name="currentPage">The page model for the current page.</param>
        /// <returns>
        /// A string containing all of the HTML from the page.
        /// </returns>
        /// <exception cref="ArgumentException">'{nameof(pageName)}' cannot be null or empty. - pageName</exception>
        /// <exception cref="ArgumentNullException">
        /// currentPage
        /// or
        /// The page {pageName} cannot be found.
        /// </exception>
        public async Task<string> RenderHtmlAsync(string pageName, PageModel currentPage)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                throw new ArgumentException($"'{nameof(pageName)}' cannot be null or empty.", nameof(pageName));
            }

            if (currentPage is null)
            {
                throw new ArgumentNullException(nameof(currentPage));
            }

            var actionContext = new ActionContext(
                currentPage.HttpContext,
                currentPage.RouteData,
                currentPage.PageContext.ActionDescriptor
            );

            using var writer = new StringWriter();

            var result = razorViewEngine.FindPage(actionContext, pageName);

            if (result.Page == null)
            {
                throw new ArgumentNullException($"The page {pageName} cannot be found.");
            }

            var page = result.Page;

            var view = new RazorView(razorViewEngine,
                razorPageActivator,
                new List<IRazorPage>(),
                page,
                HtmlEncoder.Default,
                new DiagnosticListener("ViewRenderService"));

            var viewContext = new ViewContext(
                actionContext,
                view,
                currentPage.ViewData,
                currentPage.TempData,
                writer,
                new HtmlHelperOptions()
            );

            var pageNormal = ((Page)result.Page);

            pageNormal.PageContext = currentPage.PageContext;

            pageNormal.ViewContext = viewContext;

            razorPageActivator.Activate(pageNormal, viewContext);

            await page.ExecuteAsync();

            return writer.ToString();
        }
    }
}