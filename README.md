# Proyecto Final Programación IV - SmartSell

![image](https://user-images.githubusercontent.com/58148764/140632035-c247d70a-5a3a-456a-8a46-a17b6d91b466.png)

**Integrantes:**
- Nestor Lozada
- Jorge Padilla
- Alain Ruales

Al clonar por primera vez el repositorio, es necesario compilar la solución para que se instalen de manera automática las dependencias NuGet.
  
Finalmente, debemos ejecutar el comando `Update-Database` en la consola de administrador de paquetes de NuGet para poder generar la base de datos correspondiente (en caso de no estar creada localmente).

**Observación:** En caso de que el proyecto no funcione al compilar y ejecutar, se debe limpiar y recompilar la solución para resolver errores vinculados a la resolución de rutas y paquetes NuGet.

## ¿Cómo restaurar la base de datos?

Para restaurar la base de datos, se deben seguir los siguientes pasos:

1. Ejecutar el comando `Update-Database`
2. Restaurar los datos de prueba de la base de datos: Con Microsoft SQL Management Studio (SSMS), ejecutar el script SQL `DataBackup.sql` ubicado en la raíz del proyecto, el cual solo contiene los datos de prueba de la base de datos

**Observación:** Los datos de prueba de la base de datos se respaldan por medio del SSMS.

## Ejecución de proyectos

Para ejecutar un proyecto en específico dentro de la solución, debemos modificar las configuraciones de la solución. Para ello, nos dirigimos al *Explorador de soluciones*, abrimos sus *Propiedades*, y en el apartado *Propiedades comunes* > *Proyecto de inicio* marcamos la opción *Selección actual*. Con ello, al trabajar con cualquiera de los proyectos existentes, se desplegará automáticamnete la opción para ejecutar la solución seleccionada.

En caso de que una de las soluciones presente problemas al compliar o nos arroje mensajes de omisión al compilar la solución, debemos dirigirnos a la opción *Compilar* > *Administrador de configuración*, y marcamos la opción *Compilación* para todos los proyectos, y la opción *Implementar* para los proyectos que tengan esta opción habilitada.
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
