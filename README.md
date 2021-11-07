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

## ¿Cómo arreglar errores?

Para arreglar errores, se deben seguir los siguientes pasos:

1. Guardar archivos
2. Aplicar cambios en el código (hot reload)
3. Recargar página
4. Borrar cache (<kbd>CTRL</kbd> + <kbd>F5</kbd>)
5. Compilar solución
6. Actualizar base de datos (`Update-Database`)
