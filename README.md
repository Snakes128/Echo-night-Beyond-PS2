# Echo-night-Beyond-PS2
Herramienta creada para el juego de PS2 Echo night beyond (posiblemente pueda usarse para otros juegos de From Sofwtare en PS2), la herramienta sirve para exportar 
los archivos con extensión .fmg a archivos .txt e importar/crear los binarios a partir de estos txt.

# Autor
Copyright (C) 2022 Snake128

# Uso
Exportar
------------------
Se puede usar directamente arrastrando el archivo .fmg para su extracción a un archivo txt o en su defecto llamar al comando:
  - .\EchoNightBeyondTool.exe "input file.fmg"
  - .\EchoNightBeyondTool.exe "input file.fmg" true (sirve para no hacer pausa cuando termina el trabajo)
  
Esto creará un archivo txt con los textos del archivo pasado.

Importar/Crear
------------------
Se puede usar directamente arrastrando el archivo .txt para su importación/creación a un archivo binario .fmg o en su defecto llamar al comando:
  - .\EchoNightBeyondTool.exe "input file.txt"
  - .\EchoNightBeyondTool.exe "input file.txt" true (sirve para no hacer pausa cuando termina el trabajo)
  
Esto creará un archivo fmg preparado para meter en el juego.

Importante
------------------
* Ahora se puede usar el flag = true para que el programa no haga la pausa cuando termina.

* Tienes que tener en el mismo sitio de el .exe las tablas que quieras usar para exportar e importar, en la página principal tienes unos ejemplos
usados en la traducción al español (export.tbl e import.tbl)

# Posibles errores
Recuerda tener como mínimo el archivo export.tbl, si no tienes un import.tbl se usará el export.tbl para lo mismo.

Source code and executable files are included, with GNU General Public License.

# Historia
versión 2022-09-20 13:00
------------------
  - se ha subido un fix para poder usar la herramienta sin pausar.
  
versión 2022-09-20 10:00
------------------
  - primera versión pública.


