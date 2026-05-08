# Banco Guayaquil - Inventory Management API

Sistema de backend para el manejo de inventario multiproveedor, desarrollado con estándares de nivel bancario y arquitectura limpia.

## 🚀 Tecnologías y Stack

- **Framework:** .NET 10 (Minimal APIs)
- **Base de Datos:** PostgreSQL 16
- **ORM:** Entity Framework Core con Optimistic Concurrency (xmin)
- **Arquitectura:** Clean Architecture (Domain, Application, Infrastructure, WebAPI)
- **Patrones:** Repository, Unit of Work, Result Pattern, REPR (Request-Endpoint-Response)
- **Contenedores:** Docker & Docker Compose

## 🛠️ Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Un IDE (preferentemente JetBrains Rider o VS Code)

## 🏁 Inicialización Local

### 1. Levantar la Base de Datos
El proyecto incluye un archivo `docker-compose.yml` para levantar PostgreSQL rápidamente.
```bash
docker-compose up -d
```

### 2. Configurar el Connection String
Asegúrate de que el archivo `src/BankGuayaquil.Inventory.WebAPI/appsettings.json` tenga las credenciales correctas (por defecto configurado para el docker-compose).

### 3. Ejecutar Migraciones y Data Seeder
La base de datos se migra y se llena con datos iniciales (DataSeeding) automáticamente al iniciar la aplicación.

### 4. Ejecutar el Proyecto
```bash
dotnet run --project src/BankGuayaquil.Inventory.WebAPI
```
La API estará disponible en: `http://localhost:5091`

## 🔐 Autenticación
El sistema utiliza **JWT**. 
- **Usuario Admin:** `admin` / `password123`
- **Usuario Consulta:** `user` / `password123`

## 📂 Estructura del Proyecto
- **Domain:** Entidades, interfaces de repositorio y excepciones de dominio.
- **Application:** Lógica de negocio, servicios, DTOs y validaciones.
- **Infrastructure:** Implementación de persistencia (Postgres), interceptores de auditoría y configuraciones.
- **WebAPI:** Endpoints, middlewares y configuración de la aplicación.

---
© 2026 Banco Guayaquil Inventory Challenge
