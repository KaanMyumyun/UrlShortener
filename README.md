# URL Shortener

[![Live Demo](https://img.shields.io/badge/Live_Demo-Click_Here-success?style=for-the-badge&logo=cloudflare)](https://urlshortener-az5.pages.dev/)

A full-stack **URL Shortener** built with **ASP.NET Core Web API** and a **React + TypeScript + Vite** frontend.

---

##  Live Demo

| Layer | Service | Link |
| ----- | ------- | ---- |
| Frontend | Cloudflare Pages | [urlshortener-az5.pages.dev](https://urlshortener-az5.pages.dev/) |
| Backend API | Render | [urlshortener-p7ao.onrender.com](https://urlshortener-p7ao.onrender.com) |
| Database | Neon (Serverless PostgreSQL) | Persistent, always on |

>  The backend runs on a free Render instance. If inactive, it may take **30–60 seconds to spin up** on first request.

---

## Deployment

### Free Cloud Hosting (Primary)

* **Frontend:** Hosted on [Cloudflare Pages](https://pages.cloudflare.com/)
* **Backend API:** Hosted on [Render](https://render.com/)
* **Database:** Serverless PostgreSQL hosted on [Neon](https://neon.tech/)

### Self-Hosted (Azure VM)

* **Frontend:** React build served via Nginx container
* **Backend API:** ASP.NET Core container on port 8080
* **Database:** Serverless PostgreSQL hosted on [Neon](https://neon.tech/) *(Shared with the free tier — data persists across deployments)*
* **Web Server:** Nginx reverse proxy with SSL/HTTPS via Let's Encrypt
* **Instance:** Azure VM with Docker and Docker Compose
* **CI/CD:** GitHub Actions — auto-deploys on merge to main

---

## DevOps & Infrastructure

### CI/CD Pipeline (GitHub Actions)
- Branch protection on `main` — all changes require a passing pipeline before merge
- On merge to `main`: Docker images built and pushed to Docker Hub
- Images tagged with both `latest` and commit SHA for easy rollback
- Automated deployment to Azure VM via SSH on successful build

### Tech Stack
- **CI/CD:** GitHub Actions
- **Containerization:** Docker, Docker Compose
- **Web Server:** Nginx + Let's Encrypt (SSL)
- **Cloud:** Azure VM
- **Registry:** Docker Hub
- **Database:** Neon (Serverless PostgreSQL)

---

## Overview

The application allows users to:

* Submit a long URL
* Receive a shortened URL
* Use the short URL to redirect to the original address

The system is split into two independent parts:

* **Backend** – REST API responsible for URL generation, persistence, and redirects
* **Frontend** – React UI for interacting with the API

---

## Tech Stack

### Backend
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Swagger / OpenAPI

### Frontend
* React
* TypeScript
* Vite

### Tooling
* .NET SDK 8+
* Node.js 18+
* npm

---

## Project Structure

```
UrlShortener
├── URLShortner/          # ASP.NET Core Web API
│   ├── Controllers
│   ├── Models
│   ├── Services
│   └── Program.cs
│
├── front-end/            # React + TypeScript + Vite
│   ├── src
│   ├── public
│   └── vite.config.ts
│
└── URLShortner.sln
```

---

## Getting Started

### Prerequisites
* .NET SDK 8.0+
* Node.js 18+
* PostgreSQL

### Clone the Repository

```bash
git clone https://github.com/KaanMyumyun/UrlShortener.git
cd UrlShortener
```

---

## Backend – Run the API

```bash
cd URLShortner
dotnet restore
dotnet run
```

API available at:
```
http://localhost:5245
```

Swagger UI:
```
http://localhost:5245/swagger
```

---

## Frontend – Run the UI

```bash
cd front-end
npm install
npm run dev
```

Frontend runs at:
```
http://localhost:5173
```

---

## Run with Docker

```bash
docker compose up --build
```

---

## API Endpoints

| Method | Endpoint                | Description                  |
| -----: | ----------------------- | ---------------------------- |
|   POST | /api/Url/CreateShortUrl | Create a shortened URL       |
|    GET | /api/Url/{code}         | Redirect to the original URL |

---

## Request & Response Examples

### Create Short URL

```
POST /api/Url/CreateShortUrl
```

Request body:
```json
{
  "url": "https://www.youtube.com/"
}
```

Response:
```json
{
  "shortUrl": "https://urlshortener-p7ao.onrender.com/Q1g3Cx0"
}
```

### Redirect to Original URL

```
GET /api/Url/{code}
```

Redirects the client to the original URL.

---

## Error Handling

* `200 OK` – Request successful
* `400 Bad Request` – Invalid input or malformed URL
* `404 Not Found` – Short code not found
* `500 Internal Server Error` – Unexpected error

---

## Database

Entity Framework Core with PostgreSQL (Neon serverless).

```sql
CREATE TABLE "ShortenUrls" (
    "Id" UUID PRIMARY KEY,
    "LongUrl" VARCHAR(2048) NOT NULL,
    "Code" TEXT NOT NULL,
    "ShortUrl" TEXT NOT NULL,
    "CreatedOnUtc" TIMESTAMP NOT NULL
);

CREATE UNIQUE INDEX idx_shortenurls_code ON "ShortenUrls" ("Code");
```

---

## Security

* URL input validated before processing
* Redirects only occur for stored URLs
* No sensitive data stored client-side
* HTTPS enforced in production
* Secrets managed via environment variables

---

## Project Goals

* ✅ Full-stack URL shortener with clean API design
* ✅ Deployed to Cloudflare Pages + Render + Neon
* ✅ Containerized with Docker and Docker Compose
* ✅ CI/CD pipeline with GitHub Actions
* ✅ Self-hosted on Azure VM with Nginx and SSL

## Roadmap

* Click tracking and analytics
* Custom short codes
* Rate limiting
* Unit and integration tests