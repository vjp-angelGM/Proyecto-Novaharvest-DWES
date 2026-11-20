using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

public class StatusModel : PageModel
{
    private static int _visits;
    public SiteStats Stats { get; private set; } = new();

    public void OnGet()
    {
        _visits++;

        Stats = new SiteStats
        {
          ServerTime = DateTime.Now,
          Visits = _visits,
          ServerName = Environment.MachineName

        };
    }
}