# Cuestiones Proyecto UT2 NovaHarvest

## 1. Cliente y servidor

**Pregunta:**: 
Indica qué se ejecuta en el cliente y qué en el servidor cuando abres *`/status`*. Cuando NovaHarvest permita reservar producto, la regla "no se pueden reservar más kilos de los disponibles", ¿dónde debe comprobarse obligatoriamente? ¿Por qué?


**Respuesta**:
Cuando abrimos la página *`/status`*, el navegador actúa como cliente y pide la página al servidor. El navegador, recibe ese HTML, lo renderiza junto con los estilos CSS definidos. En el servidor, se ejecuta ASP.NET, mediante Kestrel, que procesa la petición y ejecuta el código de C# de Program.cs.

Este código obtiene la fecha y hora del sistema y genera el contenido de la página. Más adelante, cuando NovaHarvest permita reservar productos, la comprobación de los kilos disponibles deberá hacerse obligatoriamente en el servidor, así evitaremos que un usuario pueda modificar los datos desde el navegador o reservar más cantidad de la que realmente existe.

## 2. Estático o dinámico

**Pregunta**: 
¿Cuál de tus 3 páginas podría ser un HTML estático y cuál no? Justifícalo. Pon dos ejemplos de páginas que NovaHarvest necesitará y que no podrían ser estáticas.

**Respuesta**: 
De las tres páginas de NovaHarvest, la página de inicio *(`/`)* y la página sobre el proyecto *(`/about`)* podrían ser HTML estáticos, porque muestran información que no cambia. Sin embargo, la página de estado del servidor *(`/status`)* debe ser dinámica, ya que muestra la fecha y hora actual del servidor. Para generar esta información se ejecuta código C# en ASP.NET Core. En el futuro, NovaHarvest necesitará páginas dinámicas como el perfil de cada usuario y el apartado de reservas de productos. Estas páginas necesitarán consultar y mostrar información actualizada de los usuarios y de los productos disponibles.

## 3. Ejecución en el servidor

**Pregunta**:
Explica paso a paso cómo una petición a *`http://localhost:8080/status`* acaba ejecutando tu código C#. ¿Qué cambiaría si NovaHarvest funcionara con CGI? ¿Y con un módulo embebido en el servidor web?

**Respuesta**:
Cuando escribimos *`http://localhost:8080/status`* en el navegador, enviamos una petición al puerto *`8080`* de mi equipo. Al estar utilizando *`Docker`*, la petición llega primero al contenedor, donde se encuentra *`Nginx`*, éste, actúa como un *`reverse proxy`*. reenviando la petición al *`backend`*, utilizando el puerto *`8080`*. Allí ASP.NET junto con Kestrel, lo que hace es, recibir la petición y busca la ruta *`/status`*, que está definida en *`Program.cs`*. 

Ejecuta el código correspondiente en *`C#`*, generándose una página HTML con la fecha y hora del servidor. Si se usase *`CGI`*, el servidor , tiene que ejecutar un servicio externo para procesar dicha petición y al tener un módulo embebido, éste, se integra dentro del mismo servidor.

## 4. Reverse proxy 

**Pregunta**:
Haz *`curl -I`* (o mira la pestaña Red del navegador) a *`/status`* con Docker y con *`dotnet run`*. ¿Qué cabecera *`Server`* aparece en cada caso y por qué? Para el backend *(`docker compose stop backend`)* y pide *`/status`* y *`/css/site.css`*: ¿qué código HTTP recibes y por qué? ¿Por qué *`backend`* no publica ningún puerto?

**Respuesta**:
Al ejecutar NovaHarvest con *`Docker`*, la cabecera *`Server`* nos muestra *`Nginx`*, porque es el servidor que recibe las peticiones del navegador y actúa como *`Reverse Proxy`*. Al ejecutarlo con *`dotnet run`*, la cabecera muestra diréctamente *`Kestrel`*, porque la petición llega directamente al servidor *`ASP.NET`*. 

Cuando paramos o detenemos el contenedor con *`docker compose stop backend`*, el servidor *`Nginx`* devuelve un código de error *`502`*, porque no puede comunicarse con el backend.

Esto nos pasa tanto con *`/status`* como con *`/css/sites.css`*, ya que *`Nginx`* reenvia todas las peticiones al backend y este no publica ningún puerto, porque <u>*`solo necesita`*</u> comunicarse con *`Nginx`* dentro de la red *`Docker`*

## 5. NGINX

**Pregunta**: 
¿Por qué Nginx no sirve `site.css` ni `logo.png` por su cuenta? Cita dos funciones que Nginx podría aportar si NovaHarvest se publicara en Internet.


**Respuesta**
En el proyecto, *`Nginx`* no sirve directamente los estilos ni el logo porque está configurado como proxy inverso. Su función es recibir las peticiones del navegador y reenviarlas al backend, donde *`ASP.NET`* se encarga de responder y servir los archivos estáticos.

