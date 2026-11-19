# Cuestiones Proyecto UT2 NovaHarvest

## 1. Cliente y servidor

**Pregunta**: 
Indica qué se ejecuta en el cliente y qué en el servidor cuando abres *`/status`*. Cuando NovaHarvest permita reservar producto, la regla "no se pueden reservar más kilos de los disponibles", ¿dónde debe comprobarse obligatoriamente? ¿Por qué?


**Respuesta**:
Cuando abrimos la página *`/status`*, el navegador actúa como cliente y pide la página al servidor. El navegador, recibe ese `HTML`, lo renderiza junto con los estilos CSS definidos. En el servidor, se ejecuta *`ASP.NET`*, mediante Kestrel, que procesa la petición y ejecuta el código de `C#` de Program.cs.

Este código obtiene la fecha y hora del sistema y genera el contenido de la página. Más adelante, cuando NovaHarvest permita reservar productos, la comprobación de los kilos disponibles deberá hacerse obligatoriamente en el servidor, así evitaremos que un usuario pueda modificar los datos desde el navegador o reservar más cantidad de la que realmente existe.

## 2. Estático o dinámico

**Pregunta**: 
¿Cuál de tus 3 páginas podría ser un `HTML` estático y cuál no? Justifícalo. Pon dos ejemplos de páginas que `NovaHarvest` necesitará y que no podrían ser estáticas.

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

## 5. Nginx

**Pregunta**: 
¿Por qué Nginx no sirve `site.css` ni `logo.png` por su cuenta? Cita dos funciones que `Nginx` podría aportar si `NovaHarvest` se publicara en Internet.

**Respuesta**:
En el proyecto, *`Nginx`* no sirve directamente los estilos ni el logo porque está configurado como proxy inverso. Su función es recibir las peticiones del navegador y reenviarlas al backend, donde *`ASP.NET`* se encarga de responder y servir los archivos estáticos.

Si `NovaHarvest` se publicase en Internet, *`Nginx`* podría aportar más seguridad al ocultar el acceso directo al backend, pudiendo también gestionar las conexiones **HTTPS** mediante certificados **SSL** y distribuir las peticiones entre varios servidores para mejorar el rendimiento.

## 6. Tecnologías

**Pregunta**:
Si la cooperativa pidiera hacer el backend con otra de las tecnologías *`server-side`* vistas en clase, ¿qué ficheros de tu repositorio cambiarían y cuáles se quedarían igual?

**Respuesta**:
Si la cooperativa quisiera cambiar `ASP.NET` por otra tecnología de servidor, como `PHP` o `Java`, tendría que modificar los archivos del *`backend`*, también tendría que cambiar el *`Program.cs`*, el *`.csproj`* y el *`Dockerfile`* del backend, porque dependen directamente del *`.NET`*.

También habría que adaptar la forma de generar las páginas y procesar las peticiones, pudiendo mantener la estructura general del proyecto, el archivo *`compose.yaml`* y la configuración de *`NGINX`*, siempre que se pueda adaptar la dirección y el puerto del nuevo backend y teniendo como objetivo conservar la comunicación entre *`NGINX`* y el servidor, aunque cambiase la tecnología utilizada.

## 7. Marcado y Código

**Pregunta**: 
Copia de tu `Program.cs` el código de una de las páginas y señala dos problemas concretos de generar HTML a base de cadenas. ¿Qué parte se repite en las tres páginas?

**Respuesta**:
En mi *`Program.cs`*, las páginas se generan utilizando `app.MapGet()` y `Results.Content()`. La ruta `/status` usa este sistema para generar el `HTML` y mostrar la fecha y hora del servidor.

```csharp
app.MapGet("/status"), () => Results.Content($"""
    <!-- Aquí se encuentra el contenido HTML de la página -->
    """, "text/html"));
```
## 8. Docker
**Pregunta** :
¿Cuántas imágenes y cuántos contenedores tienes con el entorno levantado? Explica la diferencia usando tu proyecto. ¿Por qué la imagen final del backend no incluye el SDK? ¿Qué pasaría sin el `.dockerignore`?

**Respuesta**:
En el entorno de `NovaHarvest` hemos levantado 2 contenedores: uno para `NGINX` y otro para el *`backend`* de `ASP.NET`. También utilizamos dos imágenes, una para el *`nginx`* y otra para el *`backend`*. Una `imagen` es una <u>`plantilla que sirve para crear contenedores`</u>, mientras que un `contenedor` es una <u>`instancia que se ejecuta`</u>. La imagen final del backend no incluye el `SDK`, porque solo necesita ejecutar la aplicación, no compilarla. La `compilación` se realiza en una etapa anterior del `Dockerfile`.

Sin el archivo `.dockerignore`, *`Docker`* podría copiar archivos innecesarios como las carpetas `/bin` y `/obj`, haciendo más pesada la construcción de la imagen.

## 9. Herramientas
**Pregunta**: Haz una tabla con las herramientas que has usado en el proyecto y el problema que resuelve cada una en él.

**Respuesta**
| Herramienta | Uso en NovaHarvest |
|---|---|
| Visual Studio Code | Editar los archivos del proyecto y programar. |
| ASP.NET Core | Crear el backend y gestionar las peticiones. |
| C# | Programar la lógica del servidor y generar las páginas. |
| .NET SDK | Compilar y ejecutar el proyecto durante el desarrollo. |
| Docker | Crear y ejecutar los contenedores de la aplicación. |
| Docker Compose | Levantar y gestionar los servicios del proyecto. |
| Nginx | Recibir las peticiones y enviarlas al backend. |
| Kestrel | Ejecutar la aplicación ASP.NET Core y responder a las peticiones. |
| PowerShell | Ejecutar comandos y gestionar el proyecto desde Windows. |
| Navegador web | Comprobar que las páginas funcionan correctamente. |
| curl | Realizar peticiones HTTP y comprobar las respuestas del servidor. |

## 10. Observación

**Pregunta**: Compara la hora de `/status` con `dotnet run` y con Docker. ¿Coinciden? Explica por qué.

**Respuesta**: 
Al ejecutar `NovaHarvest` con `dotnet run`, la página `/status` muestra la fecha y hora del equipo donde se está ejecutando la aplicación. 

Al utilizar `Docker`, la hora que aparece corresponde al reloj del contenedor backend. En principio, las horas deberían coincidir si ambos entornos utilizan la misma zona horaria. Si aparecen diferencias, puede deberse a que `Docker` utiliza una configuración de zona horaria diferente. Esto ocurre porque los contenedores tienen su propio entorno de ejecución y pueden utilizar una zona horaria distinta a la del sistema anfitrión.