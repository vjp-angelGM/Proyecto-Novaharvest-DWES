using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NovaHarvest.Web.Pages;
public class IndexModel : PageModel
{
    public string TodayText {get; private set; } = "";

    public void OnGet()
    {
        TodayText = DateTime.Today.ToLongDateString();
    }
}