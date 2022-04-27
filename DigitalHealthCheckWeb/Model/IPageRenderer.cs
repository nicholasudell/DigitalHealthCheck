using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalHealthCheckWeb.Pages
{
    /// <summary>
    /// Renders a page into a HTML string.
    /// </summary>
    public interface IPageRenderer
    {
        /// <summary>
        /// Renders a page into a HTML string asynchronously.
        /// </summary>
        /// <param name="pageName">Name of the page to render. This is the route to load.</param>
        /// <param name="currentPage">The page model for the current page.</param>
        /// <returns>A string containing all of the HTML from the page.</returns>
        Task<string> RenderHtmlAsync(string pageName, PageModel currentPage);
    }
}