# Investment Assistant

Aplikacja do analizy danych rynkowych i rekomendacji inwestycyjnych.

## Cel projektu

Osobiste narzędzie analityczne integrujące dane z trzech źródeł w celu wsparcia
decyzji inwestycyjnych:

- **Alpaca Markets API** — ceny akcji i ETF-ów
- **FRED API** — wskaźniki makroekonomiczne
- **GDELT Project API** — dane geopolityczne

## Stos technologiczny

### Frontend

- Next.js + TypeScript
- TanStack Query (zarządzanie stanem serwerowym)
- Zustand (zarządzanie stanem klienta)

### Backend

- ASP.NET Core Web API + C#
- Entity Framework Core
- Background Services
- Uwierzytelnianie JWT

### Warstwa danych

- PostgreSQL (dane historyczne)
- Redis (cache)

## Uruchomienie środowiska deweloperskiego

Wymagania: Docker Desktop, .NET 10 SDK, Node.js 22+.

```bash
# Uruchomienie infrastruktury (PostgreSQL + Redis)
cd infrastructure
docker compose up -d

# Weryfikacja statusu
docker compose ps
```

## Status projektu

W budowie. Aktualny etap: budowa szkieletu backendu (Etap 1).
