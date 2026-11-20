namespace NovaHarvest.Web.Models;

public class SiteStats
{
    public DateTime ServerTime {get; set;}
    public int Visits {get; set;}
    public string ServerName {get; set; } = "";
}