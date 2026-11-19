# Cuestiones Proyecto UT3 NovaHarvest

## De *`.cshtml`* a *`HTML`* 

**Pregunta**: 
Explica qué le pasa a *`Surplus.cshtml`* desde que haces *`dotnet build`* hasta que el navegador recibe el `HTML`. Compila con *`dotnet build -p:EmitCompilerGeneratedFiles=true`*, busca la clase generada para *`Status.cshtml`* y copia un *`WriteLiteral`* y un *`Write`*. ¿De qué parte de tu ```.cshtml``` sale cada uno? ¿Por qué no hay ningún ```.cshtml``` dentro del contenedor?

## Tecnologías

**Pregunta**:
Escribe la línea `Precio de excedente: ...` de tu página como se escribiría en ***PHP*** y en ***ASP*** clásico, y compárala con la tuya en **Razor**

## Etiquetas de código. 

Copia de tu proyecto un ejemplo de cada: expresión implícita,
expresión explícita, bloque @{ }, @@ y comentario @* *@, junto con el HTML que genera
cada uno (código fuente de la página). ¿Por qué info@novaharvest.es no necesita @@ y
@novaharvest sí?
## Sintaxis. 

Haz estos dos cambios en Surplus.cshtml (y deshazlos después): escribe
@Model.SurplusPrize en lugar de @Model.SurplusPrice, y escribe
@Model.Surplus.PricePerKg * 0.7m en alguna línea. Copia lo que ocurre en cada caso
(el error de dotnet build o el HTML generado). ¿Por qué el primero no compila y el
segundo sí, aunque el resultado esté mal? ¿Cómo se corrige el segundo?
## Efectos en el documento.

Compara cómo se ve la nota del productor en pantalla y en el
código fuente de /surplus. ¿Qué ha hecho Razor y por qué? ¿Qué podría pasar si usaras
@Html.Raw y la nota la escribiera un usuario malintencionado? Copia el elemento de los
días y el botón en los escenarios A, C y D, y explica por qué a veces el atributo class o
disabled no aparece. 
6. Directivas. Haz una tabla con cada directiva o fichero especial de tu proyecto (@page,
@page "/ticket", @model, @using, @namespace, @inject, _ViewStart.cshtml y Layout
= null) y el comportamiento por defecto que cambia. Quita @namespace de
_ViewImports.cshtml: copia el error y explícalo. ¿Por qué /reservations/ticket da
404?
## Tipos y operadores. 

Haz una tabla con cada dato que calcula SurplusModel: su tipo de
C#, los operadores que usa y por qué ese tipo. ¿Por qué RemainingKg / BoxKg da cajas
completas? ¿Qué pasaría si los kilos fueran decimal? ¿Qué escribe Razor si muestras
directamente un bool? ¿Y un null? Si los kilos fueran decimal, ¿cuántas cajas y kilos sueltos saldrían con 223,5 kg?
## Cultura. 

Quita UseRequestLocalization("es-ES") (y vuelve a ponerlo al terminar).
Copia lo que muestran los precios de /surplus y la fecha de / con Docker y con dotnet
run. Explica las diferencias y por qué esa línea las elimina.
## Ámbito. 

Cambia temporalmente el contador de /status por una variable local en un
@{ } de la página y recarga varias veces: ¿qué ves y por qué? Con SiteStats, ¿qué ve
otro navegador, y qué pasa tras docker compose restart backend? ¿Por qué el layout
no puede leer una variable declarada en Status.cshtml, y cómo le llega entonces el título?
10. PageModel. ¿Qué has puesto en SurplusModel y qué en Surplus.cshtml, y por qué?
Cita un cálculo concreto. Si cargas /surplus tres veces, ¿cuántos objetos SurplusModel
se crean y cuánto vive cada uno? ¿Y el contador de SiteStats?