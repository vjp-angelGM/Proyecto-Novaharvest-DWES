# Cuestiones Proyecto UT3 NovaHarvest

## De `.cshtml` a `HTML`

**Pregunta**:

Explica qué le pasa a `Surplus.cshtml` desde que haces `dotnet build` hasta que el navegador recibe el `HTML`. Compila con `dotnet build -p:EmitCompilerGeneratedFiles=true`, busca la clase generada para `Status.cshtml` y copia un `WriteLiteral` y un `Write`. ¿De qué parte de tu `.cshtml` sale cada uno? ¿Por qué no hay ningún `.cshtml` dentro del contenedor?

**Respuesta**:

Los `.cshtml`no se ejecutan directamente. Cuando hacemos `dotnet build`, Razor los transforma en clases `C#`, que luego se compilan dentro de `NovaHarvest.Web.dll`. 

Si se utiliza *`EmitCompilerGeneratedFiles=true`*, además se guardan esos archivos `C#` en la carpeta *`/obj/`* para que Razor convierta el `HTML` y las expresiones `@` en código `C#`.

```
            WriteLiteral("\r\n<h1>Estado del servidor</h1>\r\n<br />\r\n<p>Hora del servidor: ");
#nullable restore
#line (11,24)-(11,46) 6 "D:\2daw\Proyecto\Proyecto Clonado\Proyecto-Novaharvest-DWES\novaharvest-web\backend\NovaHarvest.Web\Pages\Status.cshtml"
Write(Model.Stats.ServerTime

#line default
#line hidden
#nullable disable
            );
```
El método *`Write()`* se genera cuando `Razor` encuentra código *`C#`* dentro de la página. En mi proyecto aparece, por ejemplo, con `@Model.Stats.ServerTime`, que **Razor** transforma en *`Write(Model.Stats.ServerTime)`*. Al ejecutarse la página, ese valor se evalua y se escribe en el `HTML` que recibe el navegador.

El método *`WriteLiteral()`* corresponde al `HTML` de la página. Por ejemplo, `<h1>Estado del servidor</h1>` de `Status.cshtml` se transforma en una llamada a `WriteLiteral()`, porque ese contenido es siempre el mismo y no necesita evaluarse.

## Tecnologías

**Pregunta**:

Escribe la línea `Precio de excedente: ...` de tu página como se escribiría en ***PHP*** y en ***ASP*** clásico, y compárala con la tuya en **Razor**.

### 1. Razor
```
<p>Precio del excedente: @Model.DiscountedPrice €</p>
```
### 2. PHP
```
<p>Precio del excedente: <?php echo $discountedPrice; ?> €</p>
```
### 3. ASP Clásico
```
<p>Precio del excedente: <%= discounteredPrice %> €</p>
```

## Etiquetas de código

**Pregunta**:

Copia de tu proyecto un ejemplo de cada: `expresión implícita`, `expresión explícita`, `bloque @{ }`, `@@` y `comentario @* *@`, junto con el `HTML` que genera cada uno (código fuente de la página). ¿Por qué `info@novaharvest.es` no necesita `@@` y `@novaharvest` sí?

### 1. <u>Expresión Implícita</u>
```
@Model.Stats.Visits
```
Esto `Razor`lo hace cuando evalúa directamente algo detrás de un `@`. En el proyecto lo utilizo en `@Model.Stats.Visits` para mostrar el número de visitas de la página de `Estado`. Si el **contador** tuviera el valor de `5`, el HTML generado contendría el valor de `5`.

HTML GENERADO 
```
5
```

### 2. <u>Expresión Explícita</u>
```
@(Model.RemainingKg + "Kg")
```
HTML GENERADO
```
223 Kg
```
Una `expresión explícita` se escribe entre `@()` para indicarle a `Razor`donde empieza y donde termina la <u>`expresión`</u>. Se utiliza cuando la expresión es <u>**más compleja**</u> o donde se combinan varios elementos. En este caso, se evalúa primero la operación que realiza `@Model.RemainingKg + "kg"` y después escribe el resultado en el HTML generado, que en este caso sería `223 kg`.



### 3. <u>Bloque @{ }</u>
```
@{
    ViewData["Title"] = "Estado"
}
```
HTML GENERADO
```
No genera HTML pero en la pestaña aparecería: Novaharvest - Estado
```

Un bloque `@{ }` permite ejecutar código `C#` dentro de la página `Razor`. Este bloque no genera `HTML` directamente. En el proyecto, lo utilizo para asignar el valor de `ViewData["Title"]`, que luego es utilizado por la plantilla `_Layout.cshtml` para construir el contenido de la etiqueta `<title>` de la página.

### 4. <u>@@</u>
```
@@novaharvest
```
HTML GENERADO 
```
@Novaharvest
```
`@@` se utiliza para escribir el carácter `@` de forma literal. Si escribiéramos únicamente `@novaharvest`, **Razor** intentaría interpretarlo como una expresión `C#`, pero si escribimos, `@@novaharvest`, **Razor** genera el texto `@novaharvest` en el HTML sin intentar evaluarlo.

### 5. <u>Comentario @* *@</u>
```
@* Esto solo sirve como comentario para la persona que tiene que modificar la web *@
```
HTML GENERADO 
```
No genera HTML sino que es un comentario que queda incrustado en la web para el desarrollador
``` 
Los comentarios en `Razor` son para dejar anotaciones dentro de una página `.cshtml` y a diferencia de los comentarios en `HTML (<! --  -->)`, `Razor` los elimina durante la compilación, por lo que no llegan al navegador ni aparecen en el código fuente de la página.

## Sintaxis

**Pregunta**:

Haz estos dos cambios en `Surplus.cshtml` (y deshazlos después): escribe `@Model.SurplusPrize` en lugar de `@Model.SurplusPrice`, y escribe `@Model.Surplus.PricePerKg * 0.7m` en alguna línea. Copia lo que ocurre en cada caso (el error de `dotnet build` o el `HTML` generado). ¿Por qué el primero no compila y el segundo sí, aunque el resultado esté mal? ¿Cómo se corrige el segundo?

**Respuesta**:

En el primero, lo que he modificado, es en el `Surplus.cshtml` el apartado donde hago el `@Model.DiscountedPrice`, lo cambio a `@Model.DiscountedPrize` y lo que me devuelve es un error el cual dice lo siguiente:
```
error CS1061: "SurPlusModel" no contiene una definición para "DiscountedPrize" ni un método de extensión accesible "DiscountedPrize" que acepte un primer argumento del tipo "SurPlusModel" (¿falta alguna directiva using o una referencia de ensamblado?)
```
Este primer paso no compila porque quiere hacer referencia a una propiedad que no está dentro de `SurPlusModel` y al compilar, se genera código `C#` y el compilador devuelve ese error.

En el segundo caso, en `Surplus.cshtml` donde está el primer 
```
<p>Precio por Kg: @Model.Surplus.PricePerKg</p>
```
le añado una segunda linea justo debajo en la que le pongo exactamente lo mismo pero le añado el precio `0.7m`  
```
<p> Precio por Kg 2: @Model.Surplus.PricePerKg * 0.7m</p>
```
y al ejecutar de nuevo el proyecto (que si compila), lo que me devuelve en la web es lo siguiente:

```
<p>Precio por Kg 2: 3.2 * 0.7<p>
```
Esto pasa porque `Razor` solo interpreta hasta que acaba `@Model.Surplus.PricePerKg` y el `0.7m` lo interpreta como texto normal, por lo cual cuando ejecuto el código solo muestra la operación, pero no el resultado. Para corregir esto, lo que debemos hacer es una `expresión explícita`, para que pueda hacer el cálculo y mostrarnos el resultado, asi que quedaría de la siguiente forma:

```
<p>Precio por Kg 2: @(Model.Surplus.PricePerKg * 0.7m)</p>
```
Y al compilar de nuevo el proyecto, ya si nos muestra el resultado de la operación: 
```
<p>Precio por Kg 2: 2,24 €</p>
```

## Efectos en el documento

**Pregunta**:

Compara cómo se ve la nota del productor en pantalla y en el código fuente de `/surplus`. ¿Qué ha hecho `Razor` y por qué? ¿Qué podría pasar si usaras `@Html.Raw` y la nota la escribiera un usuario malintencionado? Copia el elemento de los días y el botón en los escenarios A, C y D, y explica por qué a veces el atributo `class` o `disabled` no aparece.

**Respuesta**:

Cuando muestro la página de excedente, la nota del productor en pantalla se muestra con las etiquetas `<b> </b>` y no aparece en negrita. 

Ejemplo
```
<p>Nota del productor: <b>Recoger antes de las 14:00</b></p>
```
Si muestro el código fuente de la página lo que se ve es: 
```
<p>Nota del productor: &lt;b&gt;Recoger antes de las 14:00&lt;/b&gt;</p>
```
Lo que hace `Razor` es detectar que `@Model.Surplus.Note` viene de un usuario y por seguridad convierte `<b> </b>` en `&lt;b&gt &lt;/b&gt`, lo que está haciendo es `HTML Encoding`.

**Nota**: El encoding en el `HTML` se hace con el fin de garantizar la integridad de la información, garantiza también la compatibilidad entre sistemas y el estándar que utiliza es el <b>`UTF-8`</b>, que soporta cualquier carácter e idioma.

#### ¿Porqué lo hace?
Lo hace para evitar que cualquier usuario malintencionado pueda inyectar HTML o Javascript malicioso dentro del código, y si `Razor` no protegiese el código, el navegador, podría ejecutar el script malicioso.

#### ¿Qué pasaría si usamos @Html.Raw()?
Al hacer esto, `Razor` no protege el contenido e interpretaría el navegador directamente el contenido del `HTML`, que también mostraría en este caso la etiqueta en negrita, pero también a cualquier usuario malicioso, podría ejecutar código, ya que está desactivando la protección automática de `Razor`, pudiendo éste, hacer una redirección a otra página con `window.location`, lo que supone un riesgo de seguridad.
#### Caso A: 
Como no realizamos ningún cambio la página muestra en los Kg disponibles 223. Lo que hace la web es mostrar el resultado tal cual.

Código para comprobar la operación:
```
AvailableKg = 343,
ReservedKg = 120,
```
Con lo que nos da lo que nos muestra la página por defecto: 
```
Kg Disponibles: 223Kg 
```
Y a la hora de visualizar la página nos muestra los siguientes datos: 
```
44 Cajas de 5kg y 3Kg sueltos
```
#### Caso B
En este caso lo que tenemos que modificar en el código del proyecto es `ReservedKg` y lo tenemos que poner como `ReservedKg = 300`.

Código para comprobar la operación:
```
AvailableKg = 343,
ReservedKg = 300,
```

A la hora de visualizar la página de nuevo con estos datos actualizados, nos muestra:
```
Kg Disponibles = 43
8 Cajas de 5kg y 3kg sueltos.
Valor del excedente = 96.32 
Estado = "¡Últimos Kilos!"
```
#### Caso C: 
Hacemos la misma operación que en el caso anterior pero esta vez el valor de `ReservedKg` es la totalidad de los Kg Disponibles = 343.

Código para comprobara la operación:
```
AvailableKg = 343,
ReservedKg = 343,
```
Si visualizamos de nuevo el contenido de la web, lo que nos devuelve es :
```
Kg Disponibles = 0
0 Cajas de 5kg y 0kg sueltos.
Valor del excedente = 00.00 
Estado = "Agotado"
```
Al haber agotado el producto, el botón de "Reservar" queda deshabilitado, indicandole al usuario que no puede realizar la reserva.

#### Caso D
En este caso lo que haremos será modificar el valor de días de consumo o **HarvestDate**, y lo que le pondremos en este caso en el código será: 
```
HarvestDate = DateTime.Today.AddDays(-6),
```
Esto indica al usuario que tiene 1 día de consumo preferente antes de que finalice la fecha de consumo preferente, debiendo aparecerle en la web como: 
```
Días para consumirlo = 1
```
#### Caso E
En este caso, lo que haremos será sustituir la nota que nos ha dejado el productor de **Recoger antes de las 14:00** por *`null`*. En el proyecto lo que deberemos modificar es en nuestra clase `Surplus`, el apartado donde está la nota, quedando de la siguiente forma:
```
Note = null
```
Al visualizar la web de nuevo, lo que nos debe de mostrar es 
```
Nota : Sin Observaciones
```
## Directivas 

**Pregunta**:

Haz una tabla con cada directiva o fichero especial de tu proyecto (`@page`, `@page "/ticket"`, `@model`, `@using`, `@namespace`, `@inject`, `_ViewStart.cshtml` y `Layout = null`) y el comportamiento por defecto que cambia. Quita `@namespace` de `_ViewImports.cshtml`: copia el error y explícalo. ¿Por qué `/reservations/ticket` da 404?

| Directiva | ¿Que cambia con respecto al comportamiento por defecto? |
|----------------------------|----------------------------------------|
|`@page`|Convierte el archivo .cshtml en una Razor Page, permitiendo responder a peticiones HTTP mediante una URL y la ruta se establece automáticamente según donde esté ubicado dentro de la carpeta *`pages`*|
|`@page "/ticket"` |Lo que hace es hacer un alias para que vaya directamente a ticket sin tener que usar /reservations/ticket|
|`@model` ||
|`@using` ||
|`@namespace` ||
|`@inject` ||
|`_ViewStart.cshtml` ||
|`Layout = null` ||

## Tipos y operadores

**Pregunta**:

Haz una tabla con cada dato que calcula `SurplusModel`: su tipo de C#, los operadores que usa y por qué ese tipo. ¿Por qué `RemainingKg / BoxKg` da cajas completas? ¿Qué pasaría si los kilos fueran `decimal`? ¿Qué escribe Razor si muestras directamente un `bool`? ¿Y un `null`? Si los kilos fueran `decimal`, ¿cuántas cajas y kilos sueltos saldrían con 223,5 kg?

## Cultura

**Pregunta**:

Quita `UseRequestLocalization("es-ES")` (y vuelve a ponerlo al terminar). Copia lo que muestran los precios de `/surplus` y la fecha de `/` con Docker y con `dotnet run`. Explica las diferencias y por qué esa línea las elimina.

## Ámbito

**Pregunta**:

Cambia temporalmente el contador de `/status` por una variable local en un `@{ }` de la página y recarga varias veces: ¿qué ves y por qué? Con `SiteStats`, ¿qué ve otro navegador, y qué pasa tras `docker compose restart backend`? ¿Por qué el layout no puede leer una variable declarada en `Status.cshtml`, y cómo le llega entonces el título?

## PageModel

**Pregunta**:

¿Qué has puesto en `SurplusModel` y qué en `Surplus.cshtml`, y por qué? Cita un cálculo concreto. Si cargas `/surplus` tres veces, ¿cuántos objetos `SurplusModel` se crean y cuánto vive cada uno? ¿Y el contador de `SiteStats`?