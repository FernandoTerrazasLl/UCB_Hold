# ProjectQ

# Proyecto de Reservas y Gestion de Articulos de Mecatronica

## 📷 Modelo Entidad–Relación

![Image](/Images/bd.png)

---

## 1. Tablas, Procedimientos Almacenados, Triggers y Vistas

### Tablas

- `usuarios`, `prestamos`, `detalles_prestamos`, `categorias`, `carreras`, `empresas_mantenimiento`, `mantenimientos`, `detalles_mantenimientos`, `grupos_equipos`, `equipos`, `gaveteros`, `muebles`, `accesorios`, `componentes`.
- Todas incluyen columna `estado_eliminado BOOLEAN DEFAULT FALSE` para borrado lógico.

### Triggers

- **En `equipos`:**
  - **AFTER INSERT/UPDATE/DELETE** sobre relación a `grupos_equipos` → Recalcula `cantidad_equipos` en `grupos_equipos`.
- **En `gaveteros`:**
  - **AFTER INSERT/UPDATE/DELETE** sobre relación a `muebles` → Recalcula `numero_gaveteros` en `muebles`.

### Vistas

- **`vw_equipos_necesitan_mantenimiento`**
- **`vw_ubicaciones_grupos_equipos`**

## 2. Índices Bien Diseñados y Queries Reescritas

**usuarios**  
Los índices sobre correo electrónico y estado de eliminación aceleran las búsquedas de usuario activo por su email, clave en operaciones de login y validación. Además, el índice sobre nombre y estado garantiza respuestas rápidas en listados y filtros de usuarios sin cargar filas dadas de baja.

**prestamos**  
El índice compuesto que abarca fechas de préstamo y devolución, junto con el carnet de usuario y el indicador de eliminación lógica, optimiza las consultas de rangos temporales y el filtrado por cliente. Esto es vital para reportes de retrasos y cadencias de préstamo sin escanear toda la tabla.

**mantenimientos**  
Los índices que combinan fecha de inicio, fecha final y empresa de mantenimiento, más el flag de borrado, permiten segmentar rápidamente históricos de servicio por compañía y período. De esta forma, los informes de cumplimiento SLA y auditorías temporales se resuelven con mínimas lecturas de disco.

**grupos_equipos**  
El índice sobre categoría, nombre, modelo, marca y estado lógico mejora la localización de familias de equipos. Con él, las búsquedas de inventario por atributos combinados (por ejemplo, “todos los impresoras HP activas”) devuelven resultados de forma mucho más eficaz.

**gaveteros**  
Al indexar nombre de gavetero, mueble asociado y estado de eliminación, las operaciones de asignación y consulta de espacio de almacenamiento responden instantáneamente. Esto evita bloqueos costosos cuando múltiples procesos monitorean la distribución de gavetas.

**equipos**  
El índice que contempla grupo de equipo, código único IMT y estado lógico es fundamental para cualquier unión o filtro de equipos activos. Permite acceder directamente a un equipo en particular o a todos los de un grupo sin escanear la totalidad de la tabla.

**empresas_mantenimiento**  
Con un índice en nombre y estado se acelera la búsqueda de proveedores activos al generar ordenes de trabajo o seleccionar empresas para cotizaciones, garantizando que sólo se consideren entidades vigentes.

**detalles_prestamos**  
El índice por identificador de préstamo y estado de eliminación optimiza la obtención de ítems de un préstamo específico, esencial para calcular multas, verificar activos prestados y generar informes de devoluciones.

**detalles_mantenimientos**  
Indexar por mantenimiento y flag de borrado permite reconstruir con rapidez el histórico detallado de intervenciones de un equipo, lo que simplifica diagnósticos y análisis de fallos.

**componentes**  
El índice sobre nombre de componente, equipo asociado y estado lógico facilita verificaciones de stock, compatibilidad y asignación de repuestos sin recorrer filas “muertas” o registros obsoletos.

**categorias**  
Un índice en nombre y estado de eliminación reduce drásticamente el costo de validar unicidad en inserciones y de listar las categorías activas para menús o filtros en la interfaz de usuario.

**carreras**  
Indexar la columna nombre junto al estado lógico agiliza las consultas para poblar dropdowns o validar inscripciones en la carrera correspondiente, evitando demoras en formularios de alta de usuario.

**accesorios**  
El índice que agrupa nombre, equipo y estado de eliminación acelera la asociación y revisión de accesorios disponibles para cada equipo, fundamental para operaciones de complemento y preparación de solicitudes de mantenimiento.

### Análisis de Plan de Ejecución

Consulta pesada sin indices

![Image](https://github.com/user-attachments/assets/90820cbc-6f9d-4186-8b0d-4777f01e61f9)

Consulta pesada con indices

![Image](https://github.com/user-attachments/assets/102805fd-879f-4817-845c-df516831b876)

## 3. Transacciones Funcionales y Niveles de Aislamiento

En todos los procedures tenemos principios ACID con atomicidad y transacciones

- **Nivel de aislamiento**: `SERIALIZABLE`
  - **Justificación**: Garantiza ausencia de lecturas no repetibles y lecturas fantasmas.

### cosas a instalar

npm install signature_pad --save

npm install jspdf html2canvas

### Miembros

- Josue Galo Balbontin Ugarteche
- Alejandro Ramirez Vallejos
- Fernando Terrazas Llanos
