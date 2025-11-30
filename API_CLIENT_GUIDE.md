# Guía de Integración para Cliente Móvil - API Proyecto_Moviles_Backend

## 1. CONFIGURACIÓN DE LA URL BASE

### URLs de Ejecución

La API se ejecuta en los siguientes puertos según el perfil de ejecución:

| Perfil | Protocolo | Host | Puerto | URL Completa |
|--------|-----------|------|--------|--------------|
| **http** (por defecto) | HTTP | localhost | 5181 | `http://localhost:5181` |
| **https** | HTTPS | localhost | 7086 | `https://localhost:7086` |
| **IIS Express** | HTTP/HTTPS | localhost | 39703/44345 | Configurable en IIS |

**Recomendación para desarrollo móvil:** Usar `http://localhost:5181` para Testing local.
**Nota importante:** Para conectar desde un dispositivo móvil físico o emulador, reemplazar `localhost` con la IP local de la máquina desarrolladora.

### Ejemplo de URL Base para Cliente Móvil
```
http://<IP_DE_DESARROLLO>:5181/api
```

---

## 2. LISTA COMPLETA DE ENDPOINTS

### A. PROFILES (Perfiles de Usuarios)

#### GET - Obtener todos los perfiles
```
GET /api/profiles
```
- **Descripción:** Retorna la lista completa de todos los perfiles
- **Parámetros:** Ninguno
- **Respuesta (200 Ok):** Array de objetos Profile
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "full_name": "Juan Pérez",
    "position": "Desarrollador Senior",
    "department_id": 1,
    "role": "Developer",
    "is_available_for_change": true,
    "created_at": "2025-01-10T10:30:00Z",
    "updated_at": "2025-01-15T14:45:00Z",
    "profile_skills": null
  }
]
```

#### GET - Obtener perfil por ID
```
GET /api/profiles/{id:guid}
```
- **Descripción:** Retorna un perfil específico por su ID
- **Parámetros de ruta:**
  - `id` (Guid): ID único del perfil (ej: `550e8400-e29b-41d4-a716-446655440000`)
- **Respuesta (200 Ok):** Objeto Profile
- **Respuesta (404 Not Found):** Si el perfil no existe

#### POST - Crear nuevo perfil
```
POST /api/profiles
Content-Type: application/json
```
- **Descripción:** Crea un nuevo perfil
- **Parámetros del cuerpo:**
  ```json
  {
    "full_name": "María García",
    "position": "Gerente de Proyectos",
    "department_id": 2,
    "role": "Manager",
    "is_available_for_change": false
  }
  ```
- **Respuesta (201 Created):** Objeto Profile con ID generado
- **Headers de respuesta:** `Location: /api/profiles/{id}`

#### PUT - Actualizar perfil
```
PUT /api/profiles/{id:guid}
Content-Type: application/json
```
- **Descripción:** Actualiza un perfil existente
- **Parámetros de ruta:**
  - `id` (Guid): ID del perfil a actualizar
- **Parámetros del cuerpo:**
  ```json
  {
    "full_name": "María García López",
    "position": "Directora de Proyectos",
    "department_id": 2,
    "role": "Manager",
    "is_available_for_change": true
  }
  ```
- **Respuesta (200 Ok):** Objeto Profile actualizado
- **Respuesta (404 Not Found):** Si el perfil no existe

#### DELETE - Eliminar perfil
```
DELETE /api/profiles/{id:guid}
```
- **Descripción:** Elimina un perfil
- **Parámetros de ruta:**
  - `id` (Guid): ID del perfil a eliminar
- **Respuesta (204 No Content):** Éxito
- **Respuesta (404 Not Found):** Si el perfil no existe

---

### B. SKILLS (Habilidades/Competencias)

#### GET - Obtener todas las habilidades
```
GET /api/skills
```
- **Descripción:** Retorna la lista completa de habilidades disponibles
- **Parámetros:** Ninguno
- **Respuesta (200 Ok):**
```json
[
  {
    "id": 1,
    "name": "C#"
  },
  {
    "id": 2,
    "name": "ASP.NET Core"
  }
]
```

#### GET - Obtener habilidad por ID
```
GET /api/skills/{id:int}
```
- **Descripción:** Retorna una habilidad específica
- **Parámetros de ruta:**
  - `id` (int): ID de la habilidad
- **Respuesta (200 Ok):** Objeto Skill
- **Respuesta (404 Not Found):** Si la habilidad no existe

#### POST - Crear nueva habilidad
```
POST /api/skills
Content-Type: application/json
```
- **Descripción:** Crea una nueva habilidad
- **Parámetros del cuerpo:**
  ```json
  {
    "name": "Java"
  }
  ```
- **Respuesta (201 Created):** Objeto Skill con ID generado

#### PUT - Actualizar habilidad
```
PUT /api/skills/{id:int}
Content-Type: application/json
```
- **Descripción:** Actualiza una habilidad existente
- **Parámetros de ruta:**
  - `id` (int): ID de la habilidad
- **Parámetros del cuerpo:**
  ```json
  {
    "name": "Python"
  }
  ```
- **Respuesta (200 Ok):** Objeto Skill actualizado
- **Respuesta (404 Not Found):** Si la habilidad no existe

#### DELETE - Eliminar habilidad
```
DELETE /api/skills/{id:int}
```
- **Descripción:** Elimina una habilidad
- **Parámetros de ruta:**
  - `id` (int): ID de la habilidad
- **Respuesta (204 No Content):** Éxito

---

### C. VACANCIES (Vacantes/Posiciones)

#### GET - Obtener todas las vacantes
```
GET /api/vacancies
```
- **Descripción:** Retorna la lista completa de vacantes
- **Parámetros:** Ninguno
- **Respuesta (200 Ok):**
```json
[
  {
    "id": 1,
    "title": "Desarrollador Backend Senior",
    "description": "Se busca un desarrollador con experiencia en ASP.NET Core",
    "department_id": 1,
    "created_by": "550e8400-e29b-41d4-a716-446655440000",
    "status": "open",
    "created_at": "2025-01-10T10:30:00Z",
    "vacancy_skills": null
  }
]
```

#### GET - Obtener vacante por ID
```
GET /api/vacancies/{id:int}
```
- **Descripción:** Retorna una vacante específica
- **Parámetros de ruta:**
  - `id` (int): ID de la vacante
- **Respuesta (200 Ok):** Objeto Vacancy
- **Respuesta (404 Not Found):** Si la vacante no existe

#### POST - Crear nueva vacante
```
POST /api/vacancies
Content-Type: application/json
```
- **Descripción:** Crea una nueva vacante
- **Parámetros del cuerpo:**
  ```json
  {
    "title": "Analista de Datos",
    "description": "Profesional con experiencia en análisis de datos",
    "department_id": 3,
    "created_by": "550e8400-e29b-41d4-a716-446655440000",
    "status": "open"
  }
  ```
- **Respuesta (201 Created):** Objeto Vacancy con ID generado

#### PUT - Actualizar vacante
```
PUT /api/vacancies/{id:int}
Content-Type: application/json
```
- **Descripción:** Actualiza una vacante existente
- **Parámetros de ruta:**
  - `id` (int): ID de la vacante
- **Parámetros del cuerpo:**
  ```json
  {
    "title": "Analista Senior de Datos",
    "description": "Profesional senior con experiencia en análisis de datos",
    "department_id": 3,
    "created_by": "550e8400-e29b-41d4-a716-446655440000",
    "status": "closed"
  }
  ```
- **Respuesta (200 Ok):** Objeto Vacancy actualizado
- **Respuesta (404 Not Found):** Si la vacante no existe

#### DELETE - Eliminar vacante
```
DELETE /api/vacancies/{id:int}
```
- **Descripción:** Elimina una vacante
- **Parámetros de ruta:**
  - `id` (int): ID de la vacante
- **Respuesta (204 No Content):** Éxito

---

### D. DEPARTMENTS (Departamentos)

#### GET - Obtener todos los departamentos
```
GET /api/departments
```
- **Descripción:** Retorna la lista completa de departamentos
- **Parámetros:** Ninguno
- **Respuesta (200 Ok):**
```json
[
  {
    "id": 1,
    "name": "Tecnología",
    "created_at": "2025-01-01T08:00:00Z"
  },
  {
    "id": 2,
    "name": "Recursos Humanos",
    "created_at": "2025-01-01T08:00:00Z"
  }
]
```

#### GET - Obtener departamento por ID
```
GET /api/departments/{id:int}
```
- **Descripción:** Retorna un departamento específico
- **Parámetros de ruta:**
  - `id` (int): ID del departamento
- **Respuesta (200 Ok):** Objeto Department
- **Respuesta (404 Not Found):** Si el departamento no existe

#### POST - Crear nuevo departamento
```
POST /api/departments
Content-Type: application/json
```
- **Descripción:** Crea un nuevo departamento
- **Parámetros del cuerpo:**
  ```json
  {
    "name": "Finanzas"
  }
  ```
- **Respuesta (201 Created):** Objeto Department con ID generado

#### PUT - Actualizar departamento
```
PUT /api/departments/{id:int}
Content-Type: application/json
```
- **Descripción:** Actualiza un departamento existente
- **Parámetros de ruta:**
  - `id` (int): ID del departamento
- **Parámetros del cuerpo:**
  ```json
  {
    "name": "Finanzas y Contabilidad"
  }
  ```
- **Respuesta (200 Ok):** Objeto Department actualizado
- **Respuesta (404 Not Found):** Si el departamento no existe

#### DELETE - Eliminar departamento
```
DELETE /api/departments/{id:int}
```
- **Descripción:** Elimina un departamento
- **Parámetros de ruta:**
  - `id` (int): ID del departamento
- **Respuesta (204 No Content):** Éxito

---

### E. PROFILE SKILLS (Habilidades de Perfiles)

#### GET - Obtener habilidades de un perfil
```
GET /api/profileskills/by-profile/{profileId:guid}
```
- **Descripción:** Retorna todas las habilidades asociadas a un perfil específico
- **Parámetros de ruta:**
  - `profileId` (Guid): ID del perfil
- **Respuesta (200 Ok):**
```json
[
  {
    "profile_id": "550e8400-e29b-41d4-a716-446655440000",
    "skill_id": 1
  },
  {
    "profile_id": "550e8400-e29b-41d4-a716-446655440000",
    "skill_id": 2
  }
]
```

#### POST - Asignar habilidad a perfil
```
POST /api/profileskills
Content-Type: application/json
```
- **Descripción:** Agrega una habilidad a un perfil
- **Parámetros del cuerpo:**
  ```json
  {
    "profile_id": "550e8400-e29b-41d4-a716-446655440000",
    "skill_id": 1
  }
  ```
- **Respuesta (201 Created):** Objeto ProfileSkill creado

#### DELETE - Remover habilidad de perfil
```
DELETE /api/profileskills
Content-Type: application/json
```
- **Descripción:** Elimina una habilidad de un perfil
- **Parámetros del cuerpo:**
  ```json
  {
    "profile_id": "550e8400-e29b-41d4-a716-446655440000",
    "skill_id": 1
  }
  ```
- **Respuesta (204 No Content):** Éxito

---

### F. VACANCY SKILLS (Habilidades Requeridas en Vacantes)

#### GET - Obtener habilidades requeridas de una vacante
```
GET /api/vacancyskills/by-vacancy/{vacancyId:int}
```
- **Descripción:** Retorna todas las habilidades requeridas para una vacante
- **Parámetros de ruta:**
  - `vacancyId` (int): ID de la vacante
- **Respuesta (200 Ok):**
```json
[
  {
    "vacancy_id": 1,
    "skill_id": 1
  },
  {
    "vacancy_id": 1,
    "skill_id": 2
  }
]
```

#### POST - Asignar habilidad requerida a vacante
```
POST /api/vacancyskills
Content-Type: application/json
```
- **Descripción:** Agrega una habilidad requerida a una vacante
- **Parámetros del cuerpo:**
  ```json
  {
    "vacancy_id": 1,
    "skill_id": 1
  }
  ```
- **Respuesta (201 Created):** Objeto VacancySkill creado

#### DELETE - Remover habilidad requerida de vacante
```
DELETE /api/vacancyskills
Content-Type: application/json
```
- **Descripción:** Elimina una habilidad requerida de una vacante
- **Parámetros del cuerpo:**
  ```json
  {
    "vacancy_id": 1,
    "skill_id": 1
  }
  ```
- **Respuesta (204 No Content):** Éxito

---

## 3. MODELOS DE DATOS (DTOs)

### Profile
```csharp
{
  "id": "Guid",                    // UUID único (ej: 550e8400-e29b-41d4-a716-446655440000)
  "full_name": "string",           // Nombre completo
  "position": "string",            // Posición/Cargo
  "department_id": "int?",         // ID del departamento (nullable)
  "role": "string",                // Rol (Developer, Manager, etc)
  "is_available_for_change": "bool", // ¿Disponible para cambio?
  "created_at": "DateTimeOffset?", // Fecha de creación (ISO 8601)
  "updated_at": "DateTimeOffset?", // Fecha de última actualización (ISO 8601)
  "profile_skills": "ProfileSkill[]?" // Lista de habilidades asociadas
}
```

### Skill
```csharp
{
  "id": "int",            // ID único (auto-incremento)
  "name": "string"        // Nombre de la habilidad
}
```

### Vacancy
```csharp
{
  "id": "int",                    // ID único (auto-incremento)
  "title": "string",              // Título de la vacante
  "description": "string?",       // Descripción detallada
  "department_id": "int?",        // ID del departamento (nullable)
  "created_by": "Guid?",          // ID del profile que creó la vacante
  "status": "string?",            // Estado (open, closed, etc)
  "created_at": "DateTimeOffset?",// Fecha de creación (ISO 8601)
  "vacancy_skills": "VacancySkill[]?" // Habilidades requeridas
}
```

### Department
```csharp
{
  "id": "int",                    // ID único (auto-incremento)
  "name": "string",               // Nombre del departamento
  "created_at": "DateTimeOffset?" // Fecha de creación (ISO 8601)
}
```

### ProfileSkill (Relación Many-to-Many)
```csharp
{
  "profile_id": "Guid",           // ID del perfil
  "skill_id": "int"               // ID de la habilidad
}
```

### VacancySkill (Relación Many-to-Many)
```csharp
{
  "vacancy_id": "int",            // ID de la vacante
  "skill_id": "int"               // ID de la habilidad
}
```

---

## 4. ESQUEMA DE AUTENTICACIÓN

### ⚠️ ESTADO ACTUAL: SIN AUTENTICACIÓN

**La API actual NO implementa ningún mecanismo de autenticación o autorización.**

### Detalles:
- ✅ **CORS abierto:** `AllowAnyOrigin()`, `AllowAnyHeader()`, `AllowAnyMethod()`
- ✅ **Sin atributos `[Authorize]`:** Ningún endpoint requiere autenticación
- ✅ **Sin middleware de autenticación:** No hay middleware de JWT, OAuth, o Basic Auth registrado
- ✅ **Sin validación de roles:** No hay control de roles en endpoints

### Implicaciones de Seguridad:
⚠️ **CRÍTICO PARA PRODUCCIÓN:** Esta configuración es ÚNICAMENTE apropiada para desarrollo local. 

Para un entorno de producción, se recomienda implementar:
1. **JWT (JSON Web Tokens)** - Autenticación basada en tokens
2. **OAuth 2.0** - Integración con proveedores de identidad (Google, Microsoft, etc)
3. **API Key** - Para acceso de aplicaciones cliente
4. **Supabase Auth** - Utilizar el sistema de autenticación nativo de Supabase (ya disponible)

### Para Desarrollo Móvil Actual:
- Los endpoints son completamente públicos y accesibles sin credenciales
- El cliente móvil puede hacer peticiones directamente sin tokens de autenticación
- No necesitas incluir headers de autorización en las peticiones

---

## 5. GUÍA DE EJEMPLO PARA CLIENTE MÓVIL (Android/iOS)

### Obtener todos los perfiles
```
GET http://192.168.1.100:5181/api/profiles
```

### Obtener un perfil específico
```
GET http://192.168.1.100:5181/api/profiles/550e8400-e29b-41d4-a716-446655440000
```

### Crear un nuevo perfil
```
POST http://192.168.1.100:5181/api/profiles
Content-Type: application/json

{
  "full_name": "Carlos López",
  "position": "Desarrollador Frontend",
  "department_id": 1,
  "role": "Developer",
  "is_available_for_change": true
}
```

### Actualizar un perfil
```
PUT http://192.168.1.100:5181/api/profiles/550e8400-e29b-41d4-a716-446655440000
Content-Type: application/json

{
  "full_name": "Carlos López García",
  "position": "Desarrollador Frontend Senior",
  "department_id": 1,
  "role": "Senior Developer",
  "is_available_for_change": false
}
```

### Eliminar un perfil
```
DELETE http://192.168.1.100:5181/api/profiles/550e8400-e29b-41d4-a716-446655440000
```

---

## 6. NOTAS IMPORTANTES

### Tipos de Datos JSON
- **ISO 8601 para fechas:** `2025-01-15T14:45:00Z` o `2025-01-15T14:45:00+00:00`
- **Null para valores opcionales:** Propiedades con `?` pueden ser `null`
- **Case-sensitive en JSON:** Las propiedades JSON están en `snake_case` (ej: `full_name`, `department_id`)

### Códigos HTTP de Respuesta
| Código | Significado |
|--------|------------|
| 200 | OK - Operación exitosa |
| 201 | Created - Recurso creado exitosamente |
| 204 | No Content - Operación exitosa sin contenido en respuesta |
| 404 | Not Found - Recurso no encontrado |
| 400 | Bad Request - Parámetros inválidos |
| 500 | Internal Server Error - Error del servidor |

### Headers de Solicitud Recomendados
```
Content-Type: application/json
Accept: application/json
```

### Headers de Respuesta
```
Content-Type: application/json
```

---

## 7. CONFIGURACIÓN RECOMENDADA PARA CLIENTE MÓVIL

### Variables de Entorno/Configuración

**Para Development (máquina local):**
```
API_BASE_URL=http://localhost:5181/api
```

**Para Testing en dispositivo (reemplazar con IP local):**
```
API_BASE_URL=http://192.168.1.100:5181/api
```

**Para Production (cuando esté deployada):**
```
API_BASE_URL=https://api.tudominio.com/api
```

---

**Documento generado:** 25 de Noviembre de 2025
**Versión de API:** 1.0 (ASP.NET Core 8.0)
**Base de datos:** Supabase PostgreSQL
