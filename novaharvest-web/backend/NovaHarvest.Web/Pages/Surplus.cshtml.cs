using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

public class SurPlusModel : PageModel
{
    private const decimal SurplusDiscount = 0.3m;
    private const int BoxKg = 5;
    private const int ShelfLifeDays = 7;
    private const int LowStockKg = 50;

    public surPlus Surplus { get; } = new()
    {
        Product = "Cereza Picota",
        Producer = "Huerta de los castaños",
        Village = "Cabezuela del Valle",
        HarvestDate = DateTime.Today.AddDays(-2),
        AvailableKg = 343,
        ReservedKg = 120,
        PricePerKg = 3.2m,
        Note = "Recoger antes de las 14:00"
    };
    public int RemainingKg { get; private set; }
    public int Boxes { get; private set; }
    public decimal LooseKg { get; private set; }
    public decimal DiscountedPrice { get; private set; }

    public decimal SurplusValue { get; private set; }

    public int RemainingDays { get; private set; }

    public string Status { get; private set; } = "";

    public bool IsUrgent { get; private set; }

    public bool IsSoldOut { get; private set; }

    public void OnGet()
    {
        RemainingKg = Surplus.AvailableKg - Surplus.ReservedKg;
        Boxes = RemainingKg / BoxKg;
        LooseKg = RemainingKg % BoxKg;
        DiscountedPrice = Surplus.PricePerKg * (1 - SurplusDiscount);
        SurplusValue = RemainingKg * DiscountedPrice;
        RemainingDays = (Surplus.HarvestDate.AddDays(ShelfLifeDays) - DateTime.Today).Days;
        IsSoldOut = RemainingKg <= 0;

        IsUrgent = RemainingDays <= 2 && RemainingKg > 0;

        if (IsSoldOut)
        {
            Status = "Agotado";
        }
        else if (RemainingKg < LowStockKg)
        {
            Status = "¡Últimos kilos!";
        }
        else
        {
            Status = "Disponible";
        }
    }
}