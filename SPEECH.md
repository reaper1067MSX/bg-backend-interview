# Speech: Backend Architecture & Challenges

## 🏛️ Composición y Arquitectura
El backend fue construido bajo los principios de **Clean Architecture**, asegurando que la lógica de negocio esté completamente desacoplada de los detalles de implementación (como la base de datos o el framework web).

### Capas del Sistema:
1.  **Domain:** El núcleo. Contiene entidades puras como `Product` y `Supplier`, y la interfaz `IUnitOfWork`. No tiene dependencias externas.
2.  **Application:** Orquesta los casos de uso. Implementamos el **Result Pattern** para manejar errores de negocio de forma declarativa, evitando excepciones costosas para el flujo normal.
3.  **Infrastructure:** Implementa la persistencia con **EF Core** y **PostgreSQL**. Incluye interceptores para auditoría automática (`CreatedBy`, `UpdatedAt`) extrayendo el ID del usuario directamente del token JWT.
4.  **WebAPI:** Usa **.NET 10 Minimal APIs** con el patrón **REPR (Request-Endpoint-Response)**. Cada endpoint tiene una responsabilidad única, lo que facilita el mantenimiento y escalabilidad.

## 🧠 Decisiones Clave y Por Qué
-   **Optimistic Concurrency (xmin):** Dado que es un sistema de inventario propenso a colisiones (dos personas actualizando el mismo stock), implementamos concurrencia optimista usando la columna de sistema `xmin` de Postgres. Esto garantiza integridad financiera sin bloquear la base de datos.
-   **JWT + Interceptores:** La seguridad no es solo validar el acceso, sino asegurar la trazabilidad. El uso de interceptores en EF Core garantiza que cada cambio en el inventario tenga un autor verificado, cumpliendo con estándares bancarios.
-   **Result Pattern:** En lugar de lanzar excepciones para errores comunes (ej: SKU duplicado), devolvemos objetos de resultado. Esto hace que la API sea predecible, rápida y fácil de consumir para el frontend.

## 챌 Challenge & Desafíos
-   **Sincronización de Colecciones:** Uno de los mayores desafíos fue el manejo de la relación N:M entre Productos y Proveedores. Lograr que una actualización de producto pudiera agregar, borrar o editar proveedores existentes de forma atómica y sin errores de identidad (IDs perdidos) requirió una refactorización profunda de la lógica de rastreo de EF Core.
-   **Migración a .NET 10:** Adoptar la versión más reciente permitió usar características avanzadas de C# 13 como *Params collections* y mejoras en el *Pattern Matching*, resultando en un código más limpio y moderno.

---
Este backend no es solo un CRUD; es un motor transaccional robusto diseñado para la consistencia y la auditoría.
