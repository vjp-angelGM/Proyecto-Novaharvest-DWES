using Microsoft.AspNetCore.Mvc.RazorPages;
namespace NovaHarvest.Web.Pages.Reservations;

public class TicketModel : PageModel
{
    public string ReservationCode {get; private set; } = "";
    public string Product{get; private set; } = "";
    public int Kg {get; private set; }
    public string PickupPoint{get; private set;} = "";
    public DateTime GeneratedAt{get; private set;}

    public void OnGet()
    {
        ReservationCode = "RES-00001";
        Product = "Cereza Picota";
        Kg = 20;
        PickupPoint = "Cooperativa Valle del Jerte";
        GeneratedAt = DateTime.Now;
    }
}