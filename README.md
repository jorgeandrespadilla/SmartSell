# Proyecto Final Programación IV - SmartSell

![image](https://user-images.githubusercontent.com/58148764/140632035-c247d70a-5a3a-456a-8a46-a17b6d91b466.png)

**Integrantes:**
- Nestor Lozada
- Jorge Padilla
- Alain Ruales

Al clonar por primera vez el repositorio, es necesario compilar la solución para que se instalen de manera automática las dependencias NuGet.
  
Finalmente, debemos ejecutar el comando `Update-Database` en la consola de administrador de paquetes de NuGet para poder generar la base de datos correspondiente (en caso de no estar creada localmente).

**Observación:** En caso de que el proyecto no funcione al compilar y ejecutar, se debe limpiar y recompilar la solución para resolver errores vinculados a la resolución de rutas y paquetes NuGet.

## Estructura de la solución

La solución creada para nuestro proyecto final tiene las siguientes características:
- Aplicación web desarrollada en ASP.NET (ASP.NET MVC Framework 4.7)
- API REST
- Aplicación de escritorio desarrollada en UWP
- Biblioteca de clases para .NET Standard 2.0

## ¿Cómo restaurar la base de datos?

Para restaurar la base de datos, se deben seguir los siguientes pasos:

1. Eliminar cualquier instancia creada previamente de la base de datos (`SmartSell`).
2. Ejecutar el comando `Update-Database` en la consola del *Administrador de Paquetes*, asegurándose de seleccionar `ProyectoFinal.Web` como el proyecto predeterminado.
![image](https://user-images.githubusercontent.com/58148764/147685226-cb16e937-4fe2-4598-9615-a35f4bfceab0.png)
3. Restaurar los datos de prueba de la base de datos: Con Microsoft SQL Management Studio (SSMS), ejecutar el script SQL `DataBackup.sql` ubicado en la raíz del proyecto, el cual solo contiene los datos de prueba de la base de datos.

*El proyecto también incluye un script de respaldo que permite restaurar la estructura de la base de datos junto con los datos de prueba (archivo `Backup.sql`).*

**Observación:** Los datos de prueba de la base de datos se respaldan por medio del SSMS.

## Ejecución de proyectos

Para ejecutar varios proyectos a la vez, debemos modificar las configuraciones de la solución. Para ello, nos dirigimos al *Explorador de soluciones*, abrimos sus *Propiedades*, y en el apartado *Propiedades comunes* > *Proyecto de inicio* marcamos la opción *Proyectos de inicio múltiples*. Una vez marcada esta opción, cambiamos la acción a `Iniciar` para todos los proyectos que queremos iniciar en paralelo (ej. API del proyecto web y aplicación UWP). Con ello, al correr la solución se ejecutarán varios proyectos a la vez.
![image](https://user-images.githubusercontent.com/58148764/147686176-5cf4f9b0-3669-4fc1-b40c-e22eb775e653.png)

En caso de que una de las soluciones presente problemas al compilar o nos arroje mensajes de omisión al compilar la solución, debemos dirigirnos a la opción *Compilar* > *Administrador de configuración*, y marcamos la opción *Compilación* para todos los proyectos, y la opción *Implementar* para los proyectos que tengan esta opción habilitada.
![image](https://user-images.githubusercontent.com/58148764/142246833-9b991726-3535-4d7a-b989-7b689cdbf60b.png)

## Configuración de proyecto UWP

Para ejecutar el proyecto UWP, es necesario verificar que tenemos habilitado el protocolo *TCP/IP* para SQL Express en nuestro equipo, para permitir la conexión de la aplicación de escritorio con la base de datos. Para ello, nos dirigimos al panel de *Administración de equipos* y habilitamos el protocolo *TCP/IP* para SQL Express, tal y como se muestra en la imagen a continuación.
![image](https://user-images.githubusercontent.com/58148764/142556391-0747bede-8b83-4b48-bf08-e55073df1723.png)

Una vez habilitado el protocolo, debemos reiniciar y verificar que los servicios *SQL Server (SQLEXPRESS)* y *SQL Server Browser* se encuentren activados (Estado: *En ejecución*) y habilitados (Tipo de inicio: *Automático*).

![image](https://user-images.githubusercontent.com/58148764/142557227-82344092-ac6f-44f8-b7bc-e2edeb0fb9d6.png)

![image](https://user-images.githubusercontent.com/58148764/142557377-1dc89393-4562-418a-bc40-c303cf813a3c.png)

## Pruebas con la API

Para realizar pruebas con la API, se puede hacer uso del archivo `ApiTest.http` que se encuentra en el directorio raíz del proyecto, cuyas peticiones pueden ser ejecutadas con el uso de la extensión **HTTP Client** para Visual Studio Code.

## ¿Cómo arreglar errores?

Para arreglar errores, se deben seguir los siguientes pasos:

1. Guardar archivos
2. Aplicar cambios en el código (hot reload)
3. Recargar página
4. Borrar cache (<kbd>CTRL</kbd> + <kbd>F5</kbd>)
5. Compilar solución
6. Actualizar base de datos (`Update-Database`)
