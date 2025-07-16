# URL-Shortener Dienst

**Projekt:** URL Shortener Service (bit.ly Alternative)  
**Version:** 1.0  
**Repository:** [https://github.com/simeonar/UrlShortener](https://github.com/simeonar/UrlShortener)

---

## Projektübersicht

Ein umfassender URL-Shortener-Dienst, der Link-Kürzung, benutzerdefinierte Aliase, Klick-Analysen, QR-Code-Generierung und eine moderne Weboberfläche bietet. Das Projekt demonstriert moderne .NET-Backend-Entwicklung und Cloud-Deployment.

---


## Hinweis zur Autorisierung

> ⚠️ Hinweis: Die Benutzer-Authentifizierung ist aktuell stark vereinfacht implementiert, um die Entwicklung und das Testen zu beschleunigen. In zukünftigen Versionen wird ein vollständiges und sicheres Authentifizierungs- und Berechtigungssystem integriert.

---

## Schnellstart

### Backend (API) starten

```sh
dotnet run --urls=http://0.0.0.0:5212
```
Startet die ASP.NET Core API auf Port 5212, erreichbar unter http://localhost:5212 oder http://<Ihre_IP>:5212

### Frontend (Angular) starten

```sh
npm start -- --host 0.0.0.0
```
Startet die Angular SPA auf http://localhost:4200 (oder http://<Ihre_IP>:4200)

---

## Projektstruktur

- `UrlShortener.API/` — ASP.NET Core Web API (Backend)
- `UrlShortener.Core/` — Geschäftslogik, Modelle, Services
- `UrlShortener.Infrastructure/` — Infrastruktur-Services, Repositories
- `UrlShortener.Web/` — Angular SPA (Frontend)
- `tests/` — Unit- und Integrationstests
- `data/` — JSON-Dateien für Links und Benutzer
- `docker/` — Dockerfile und Container-Setup
- `docs/` — Dokumentation, Anleitungen
- `PROGRESS.md` — Fortschrittstracker

---

## Funktionsübersicht

- **Kernfunktionalität:** Kürzen von URLs mit Base62-Algorithmus (6-8 Zeichen)
- **Benutzerdefinierte Features:** Aliase, Ablaufdatum, Aktivierung/Deaktivierung
- **Analytics:** Klick-Tracking (Zeit, IP anonymisiert, User-Agent, Referrer, Land)
- **Statistiken:** Übersicht, Details, Diagramme mit Gruppierung nach Tag/Stunde
- **QR-Code-Generierung:** Mit Caching (In-Memory & Dateisystem)
- **Web-Oberfläche:** Moderne Angular SPA mit Dashboard und Linkverwaltung
- **Nutzerverwaltung:** Login/Registrierung, API-Key-Verwaltung
- **Admin-Panel:** Moderation, Systemstatistik, Link-Blockierung
- **Rate-Limiting:** IP- und nutzerbasierte Begrenzungen

---

## API-Endpunkte

### Kernfunktionen
- `POST /api/shorten` – Kurzlink erstellen (mit optionalem Alias/Ablaufdatum)
- `GET /api/urls/{shortCode}` – Link-Info abrufen
- `GET /{shortCode}` – Weiterleitung zum ursprünglichen Link

### Statistiken
- `GET /api/stats/{shortCode}` – Statistik-Übersicht
- `GET /api/stats/{shortCode}/clicks` – Detaillierte Klick-Daten
- `GET /api/stats/{shortCode}/chart` – Chart-Daten für Diagramme

### QR-Code
- `GET /api/qr/{shortCode}` – QR-Code als Bild (Größe/Format wählbar)

---

## Technologie-Stack

- **Backend:** ASP.NET Core 8, Entity Framework Core, Serilog, QRCoder
- **Frontend:** Angular, Bootstrap 5, Chart.js
- **Datenbank:** SQL Server/PostgreSQL
- **Caching:** Redis, In-Memory
- **Deployment:** Docker, Azure/AWS, GitHub Actions (CI/CD)
- **Weitere:** AutoMapper, FluentValidation, Swagger

---

## Datenmodell

```csharp
public class ShortenedUrl
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortCode { get; set; }
    public string? CustomAlias { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public int ClickCount { get; set; }
    public List<ClickStatistic> ClickStatistics { get; set; }
}

public class ClickStatistic
{
    public int Id { get; set; }
    public int ShortenedUrlId { get; set; }
    public DateTime ClickedAt { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public string? Referrer { get; set; }
    public string? Country { get; set; }
}
```

---

## Entwicklungsfortschritt

### ✅ Erledigt
- Grundarchitektur und Hauptfunktionen
- Linkkürzung mit Base62-Algorithmus
- API für Kürzung, Info, Redirect, Statistik, QR
- Klick-Tracking und Statistikaggregation
- Web-UI: Kürzung, Ergebnis, Dashboard, Verwaltung
- Nutzer-Login und Speicherung
- Rate Limiting für Gäste und Nutzer
- Admin-Panel mit Link-Blockierung und Systemstatistik

### 🔄 In Arbeit
- Dockerfile, docker-compose, Cloud-Deployment
- Monitoring, Logging, Metriken
- Erweiterte Nutzerbereich-Features
- API-Key-Verwaltung
- Testabdeckung (Unit-, Integrations-, Lasttests)
- Code Style, Pre-Commit Hooks, Badges

### ❌ Nicht umgesetzt
- Webhooks, Bulk-API
- Social-Media-Integration
- Mobile-App
- White-Label-Anpassungen

---

## Architektur & Entwicklungsphasen

### Phase 1-2: Grundarchitektur & Kernfunktionalität (3-5 Tage)
- ASP.NET Core Web API Setup
- Base62-Algorithmus für Kurzcode
- Validierung und Speicherung

### Phase 3: Analyse & Statistik (2-3 Tage)
- Klick-Tracking-Middleware
- Statistik-API mit Aggregation
- Traffic-Quellenanalyse

### Phase 4: QR-Code-Generierung (1-2 Tage)
- QR-Code-Service mit Caching
- Verschiedene Größen und Formate

### Phase 5: Web-Oberfläche (3-5 Tage)
- Angular SPA mit Bootstrap
- Dashboard und Linkverwaltung
- Chart.js für Statistiken

### Phase 6: Erweiterungen (2-4 Tage)
- Nutzerverwaltung
- Admin-Panel
- Rate-Limiting

### Phase 7: Testen (2-3 Tage)
- Unit-Tests für Business-Logik
- Integrationstests für API
- Performance-Tests

### Phase 8: Deployment (1-2 Tage)
- Docker-Container
- Cloud-Deployment
- Monitoring-Setup

**Geschätzte Gesamtdauer:** 2-3 Wochen

---

## Test-Status

- [ ] Unit-Tests: Geschäftslogik, Validierung, Code-Generierung
- [ ] Integrationstests: API, Datenbank, Redirects
- [ ] Lasttests: Performance, Antwortzeit

Tests sind teilweise vorbereitet, die Abdeckung wird kontinuierlich ausgebaut.

---

## Lizenz

MIT License