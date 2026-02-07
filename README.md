# URL Shortener 

A full-stack **URL Shortener** application built with **ASP.NET Core Web API** and a **React + TypeScript + Vite** frontend.

The project focuses on clean API design, proper data modeling with Entity Framework Core, and cloud-ready architecture.

---

# Overview

The application allows users to:

* Submit a long URL
* Receive a shortened URL
* Use the short URL to redirect to the original address

The system is split into two independent parts:

* **Backend** – REST API responsible for URL generation, persistence, and redirects
* **Frontend** – React UI for interacting with the API

---

# Tech Stack

## Backend

* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Swagger / OpenAPI

## Frontend

* React
* TypeScript
* Vite

## Tooling

* .NET SDK 8+
* Node.js 18+
* npm

---

# Project Structure

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

# Getting Started

## Prerequisites

Make sure you have installed:

* .NET SDK 8.0+
* Node.js 18+
* PostgreSQL

---

## Clone the Repository

```bash
git clone https://github.com/KaanMyumyun/UrlShortener.git
cd UrlShortener
```

---

# Backend – Run the API

```bash
cd URLShortner
dotnet restore
dotnet run
```

The API will be available at:

```
http://localhost:5245
```

Swagger UI:

```
http://localhost:5245/swagger
```

---

# Frontend – Run the UI

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

# API Endpoints

| Method | Endpoint                | Description                  |
| -----: | ----------------------- | ---------------------------- |
|   POST | /api/Url/CreateShortUrl | Create a shortened URL       |
|    GET | /api/Url/{code}         | Redirect to the original URL |

---

# Request & Response Examples

## Create Short URL

**Endpoint**

```
POST /api/Url/CreateShortUrl
```

**Request Body**

```json
{
  "url": "https://www.youtube.com/"
}
```

**Response – 200 OK**

```json
{
  "shortUrl": "http://localhost:5245/Q1g3Cx0"
}
```

---

## Redirect to Original URL

**Endpoint**

```
GET /api/Url/{code}
```

**Example**

```
GET /api/Url/Q1g3Cx0
```

**Behavior**

* Redirects the client to the original URL
* Returns an HTTP redirect response

---

# Error Handling

The API uses standard HTTP status codes:

* `200 OK` – Request successful
* `400 Bad Request` – Invalid input or malformed URL
* `404 Not Found` – Short code not found
* `500 Internal Server Error` – Unexpected error

**Example Error Response**

```json
{
  "error": "Invalid URL format"
}
```

---

# Database

The backend uses **Entity Framework Core** with **PostgreSQL** to store shortened URLs.

### Database Name

```
UrlShortenerDb
```

---

## PostgreSQL – Create Database

```sql
CREATE DATABASE "UrlShortenerDb";
```

---

## PostgreSQL – Create Table

```sql
CREATE TABLE "ShortenUrls" (
    "Id" UUID PRIMARY KEY,
    "LongUrl" VARCHAR(2048) NOT NULL,
    "Code" TEXT NOT NULL,
    "ShortUrl" TEXT NOT NULL,
    "CreatedOnUtc" TIMESTAMP NOT NULL
);
```

Recommended index for fast lookups:

```sql
CREATE UNIQUE INDEX idx_shortenurls_code
ON "ShortenUrls" ("Code");
```

---

# Entity Model

```csharp
public class ShortenUrl
{
    public Guid Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
}
```

---

# Security Considerations

* URL input is validated before processing
* Redirects only occur for stored URLs
* No sensitive data stored client-side
* HTTPS should be enforced in production
* Secrets will be handled via environment variables

---

# Cloud Hosting & Deployment (Planned)

The application is designed to be **cloud-ready**.

Planned improvements:

* Dockerized backend and frontend
* Docker Compose for local and cloud parity
* Cloud-hosted PostgreSQL instance
* Environment-based configuration
* Container deployment to a cloud provider

---

# Containerization (Planned)

* Dockerfile for ASP.NET Core API
* Dockerfile for React frontend
* Optional database container for development
* One-command startup using Docker Compose

---

# Scalability & Performance (Future Work)

* Indexing on short URL codes
* Caching frequently accessed URLs
* Rate limiting to prevent abuse
* Structured logging and monitoring

---

# Planned Cloud Hosting

The application is designed with cloud deployment in mind and will be hosted on a cloud platform in the future.

Planned hosting approach:

- Dockerized backend and frontend
- Cloud-hosted PostgreSQL database
- Environment-based configuration (Development / Production)
- Secure secrets management using environment variables
- HTTPS-enabled public access

---

# Testing Strategy (Planned)

* Unit tests for URL generation logic
* Integration tests for API endpoints
* Frontend component tests

---

# Known Limitations

* No authentication or authorization
* No analytics or click tracking
* No custom short codes yet

---

# Project Goals

* Practice full-stack development
* Learn cloud-ready application design
* Build a production-style URL shortener
* Apply clean backend architecture principles


