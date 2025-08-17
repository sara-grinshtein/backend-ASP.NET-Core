# 🚗 ProjectYedidim – Volunteer Assistance Backend (ASP.NET Core)

ASP.NET Core Web API backend for managing volunteer dispatch to roadside assistance events: request creation, candidate filtering, assignment, and tracking.

---

## 📑 Table of Contents
- ⚙️ [Architecture & Technologies](#architecture--technologies)
- ⚡ [Installation & Run](#installation--run)
- 🛠 [Configuration (appsettings)](#configuration-appsettings)
- 🗄 [EF Core Migrations](#ef-core-migrations)
- 🌐 [API & Endpoints](#api--endpoints)
- 📬 [Request Examples](#request-examples)
- 🧮 [Dispatch Algorithm](#dispatch-algorithm)
- 📦 [Main Models](#main-models)
- 🔐 [Security](#security)
- 🧪 [Testing](#testing)
- 🚀 [Deployment](#deployment)
- 📜 [License](#license)

---

## 🛠 Architecture & Technologies
- **Framework:** ASP.NET Core Web API (.NET 7)
- **ORM:** Entity Framework Core
- **Authentication/Authorization:** 🔑 JWT (Bearer)
- **Object Mapping:** 🔄 AutoMapper
- **Layers:**
  - 📂 `Repository` – EF entities and repositories  
  - 🧩 `Service` – business logic & dispatch algorithm  
  - 🌐 `Controllers` – API layer  
- **Projects:**
  - 🚀 `ProjectYedidim` – main API  
  - 📂 `Repository`  
  - 🧩 `Service`  
  - 🧪 `ProjectYedidim.Test` – unit tests  
  - 📦 `Common` – DTOs

---

## ⚡ Installation & Run
Requirements: .NET 7 SDK, SQL Server (LocalDB works), EF Core tools.

~~~bash
git clone https://github.com/sara-grinshtein/backend-ASP.NET-Core.git
cd backend-ASP.NET-Core/ProjectYedidim

# Database creation & run (Dev)
dotnet ef database update
dotnet run

# Default launch settings:
# 🌍 HTTP: http://localhost:5171
# 📖 Swagger: http://localhost:5171/swagger
~~~

---

## ⚙️ Configuration (appsettings)

Create `appsettings.Development.json` (ignored by Git):

~~~json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\ProjectModels;Database=ProjectYedidim;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "REPLACE_WITH_STRONG_SECRET",
    "Issuer": "http://localhost:5171",
    "Audience": "http://localhost:5171"
  },
  "EmailSettings": {
    "From": "noreply@example.com",
    "SmtpHost": "smtp.example.com",
    "SmtpPort": "587",
    "Username": "smtp-user",
    "Password": "smtp-pass"
  },
  "GoogleMaps": {
    "ApiKey": "REPLACE_OR_REMOVE_IF_NOT_USED"
  }
}
~~~

⚠️ Never commit real secrets. Use **User Secrets** (`dotnet user-secrets`) in dev or **Secrets Manager** in production.

---

## 🗄 EF Core Migrations
~~~bash
dotnet ef migrations add Init
dotnet ef database update
~~~

---

## 📡 API & Endpoints

| 📂 Controller                     | 🌐 Base Route                  | 🔧 Main Actions                                                                 |
|----------------------------------|--------------------------------|---------------------------------------------------------------------------------|
| `LoginController`                | `/api/Login`                   | 🔑 `POST / (login)`, `POST /login (JWT)`                                        |
| `VolunteerController`            | `/api/Volunteer`               | 👤 `GET /`, `GET /{id}`, `POST /`, `PUT /{id}`, `DELETE /{id}`                 |
| `HelpedController`               | `/api/Helped`                  | 🙋 `GET /`, `GET /{id}`, `PUT /{id}`, `DELETE /{id}`                           |
| `MessageController`              | `/api/Message`                 | 📨 `GET /`, `GET /{id}`, `POST / (new request)`, `PUT /{id}`, `DELETE /{id}`   |
| `ResponseController`             | `/api/Response`                | 💬 `GET /`, `GET /{id}`, `GET /public`                                         |
| `My_areas_of_knowledge_Controller` | `/api/My_areas_of_knowledge` | 📚 `GET /`, `GET /{id}`, `POST /`, `PUT /{id}`, `DELETE /{id}`, `GET /volunteer/{volunteerId}` |

For the complete, up-to-date list check **Swagger**.

---

## 📬 Request Examples

### 1️⃣ Login (JWT)
~~~bash
curl -X POST http://localhost:5171/api/Login/login \
 -H "Content-Type: application/json" \
 -d '{"email":"volunteer@example.com","password":"Secret123"}'
~~~

### 2️⃣ Open Assistance Request (Message)
~~~bash
curl -X POST http://localhost:5171/api/Message \
 -H "Authorization: Bearer <JWT>" \
 -H "Content-Type: application/json" \
 -d '{
   "helped_id": 10,
   "location": "Highway 1, Harel Interchange",
   "latitude": 31.8,
   "longitude": 35.2,
   "need": "tire_change",
   "priority": "high"
 }'
~~~

### 3️⃣ Update Volunteer Info
~~~bash
curl -X PUT http://localhost:5171/api/Volunteer/123 \
 -H "Authorization: Bearer <JWT>" \
 -H "Content-Type: application/json" \
 -d '{
   "volunteer_id":123,
   "email":"vol123@example.com",
   "tel":"050-0000000",
   "areas_of_knowledge":[{"ID_knowledge":1,"KnowledgeCategory":"tire_change"}]
 }'
~~~

---

## 🧮 Dispatch Algorithm
Steps:  
- 📥 **DataFetcher** → 🕵️ **CandidateScreening** → 🔀 **DinicAlgorithm** → 🎯 **Assignment**  
- Filters by availability, distance, area of knowledge  
- Uses `IDistanceService` to calculate distances  
- 🌍 Integrated with **Google Maps API** to compute accurate geographic distances  
- Triggered automatically when a new request (`POST /api/Message`) is created  

---

## 📦 Main Models
- 👤 `VolunteerDto` – volunteer details + knowledge areas  
- 🙋 `HelpedDto` – assisted user details + location  
- 📨 `MessageDto` – assistance request  
- 💬 `ResponseDto` – volunteer/system response  
- 📚 `My_areas_of_knowledge_Dto` – skill categories  

---

## 🔐 Security
- 🔑 JWT Bearer authentication  
- 👥 RBAC (Admin / Volunteer / Dispatcher)  
- 🔒 Sensitive data encrypted, secrets excluded from Git  
- 🛡 Recommended: Rate limiting, CORS with allowed origins  

---

## 🧪 Testing
`ProjectYedidim.Test` includes unit tests for the dispatch algorithm.

~~~bash
dotnet test
~~~

---

## 🚀 Deployment

### 📦 Build
~~~bash
dotnet publish ProjectYedidim -c Release -o out
~~~

### ▶️ Run
~~~bash
dotnet out/ProjectYedidim.dll
~~~

### 🐳 Basic Dockerfile
~~~dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY ./out .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet","ProjectYedidim.dll"]
~~~

---

## 📜 License

