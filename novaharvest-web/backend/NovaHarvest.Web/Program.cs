var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapStaticAssets();
app.MapGet("/", () => Page("Inicio",
    "<p>NovaHarvest conecta a los productores del Valle del Jerte con quienes pueden aprovechar sus excedentes.</p>" +
    "<p>Cada semana sobran frutas y verduras que no llegan a venderse.</p>" +
    "<p>Residencias, comedores sociales, restaurantes y particulares podrán consultar la disponibilidad.</p>"));

app.MapGet("/about", () => Page("Acerca de",
    "<p>Proyecto: NovaHarvest</p>" +
    "<p>Centro: IES Valle del Jerte</p>" +
    "<p>Autor: Ángel García Martínez</p>" +
    "<p>Versión: 0.1.0</p>"));

app.MapGet("/status", () => Page("Estado del servidor",
    $"<p>Fecha y hora del servidor: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>" +
    "<p>Página generada por ASP.NET Core (Kestrel)</p>"));

app.Run();

static IResult Page(string title, string content)
{
    var html = $"""
        <!DOCTYPE html>
        <html lang="es">
        <head>
            <meta charset="UTF-8">
            <title>NovaHarvest - {title}</title>
            <link rel="stylesheet" href="/css/site.css">
        </head>
        <body>
            <header>
                <img src="/img/logo.png" alt="Logotipo de NovaHarvest" height="40">
                <span>NovaHarvest</span>
                <nav>
                    <a href="/">Inicio</a>
                    <a href="/about">Acerca de</a>
                    <a href="/status">Estado</a>
                </nav>
            </header>
            <main>
                <h1>{title}</h1>
                {content}
            </main>
            <footer>NovaHarvest · IES Valle del Jerte · {DateTime.Now.Year}</footer>
        </body>
        </html>
        """;
    return Results.Content(html, "text/html; charset=utf-8");
}