
# URL-Shortener Dienst

**Projekt:** URL Shortener Service (bit.ly Alternative)   
**Version:** 1.0  

---

## Projektübersicht

Ein umfassender URL-Shortener-Dienst, der Link-Kürzung, benutzerdefinierte Aliase, Klick-Analysen, QR-Code-Generierung und eine moderne Weboberfläche bietet. Das Projekt demonstriert moderne .NET-Backend-Entwicklung und Cloud-Deployment.

---

## Architektur & Phasen

Das Projekt ist in mehrere Phasen und Module gegliedert, um eine saubere, skalierbare Architektur zu gewährleisten:

### 1. Projektaufbau & Grundarchitektur
- ASP.NET Core Web API (.NET 8)
- Struktur: `src/` (API, Core, Infrastructure, Web), `tests/`, `docker/`
- Wichtige Abhängigkeiten: Entity Framework Core, AutoMapper, FluentValidation, Serilog, Swagger, QRCoder

### 2. Kernfunktionalität: Link-Kürzung
- Base62-Algorithmus für Kurzcode (6-8 Zeichen, eindeutig)
- API-Endpunkte:
  - `POST /api/shorten` – Kurzlink erstellen (mit optionalem Alias/Ablaufdatum)
  - `GET /api/urls/{shortCode}` – Link-Info abrufen
  - `GET /{shortCode}` – Weiterleitung
- Validierung: URL-Format, Alias-Verfügbarkeit, Längen-/Zeichenbeschränkungen

### 3. Analyse & Statistik
- Klick-Tracking (Middleware): Zeit, IP (anonymisiert), User-Agent, Referrer, Land
- Statistik-API:
  - `GET /api/stats/{shortCode}` – Übersicht
  - `GET /api/stats/{shortCode}/clicks` – Details
  - `GET /api/stats/{shortCode}/chart` – Chart-Daten
- Features: Gruppierung (Tag/Stunde), Top-Länder/Browsers, Traffic-Quellen

### 4. QR-Code-Generierung
- `GET /api/qr/{shortCode}` – QR-Code als Bild (Größe/Format wählbar)
- Caching: In-Memory & Dateisystem

### 5. Web-Oberfläche (Angular)
- Startseite mit Kürzungsformular
- Ergebnisseite mit QR-Code
- Statistik-Dashboard
- Linkverwaltung
- Technologien: Angular, Bootstrap, Chart.js

### 6. Erweiterungen (Optional)
- Nutzerverwaltung (Registrierung, Login, API-Keys)
- Admin-Panel (Moderation, Systemstatistik)
- API-Rate-Limiting (IP-basiert, Nutzer-basiert)

### 7. Testen
- Unit-Tests: Business-Logik, Validierung, Code-Generierung
- Integrationstests: API, DB, Redirects
- Performance-Tests: Last, Antwortzeit

### 8. Deployment & Monitoring
- Dockerfile, docker-compose
- Cloud-Deployment (Azure/AWS), Cloud-DB, CDN
- Monitoring: Application Insights, Logging, Metriken

---

## Datenmodell (Beispiel)

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

## Technologie-Stack

- **Backend:** ASP.NET Core 8, Entity Framework Core, Serilog, QRCoder
- **Frontend:** Angular, Bootstrap 5, Chart.js
- **Datenbank:** SQL Server/PostgreSQL
- **Caching:** Redis
- **Deployment:** Docker, Azure/AWS, GitHub Actions (CI/CD)

---

## API-Überblick (Auswahl)

- `POST /api/shorten` – Kurzlink erstellen
- `GET /api/urls/{shortCode}` – Link-Info
- `GET /{shortCode}` – Redirect
- `GET /api/stats/{shortCode}` – Statistik
- `GET /api/qr/{shortCode}` – QR-Code

---

## Zeitplan (Beispiel)

- Phase 1-2: 3-5 Tage (Kernfunktionalität)
- Phase 3: 2-3 Tage (Analytics)
- Phase 4: 1-2 Tage (QR)
- Phase 5: 3-5 Tage (Web)
- Phase 6: 2-4 Tage (Erweiterungen)
- Phase 7: 2-3 Tage (Tests)
- Phase 8: 1-2 Tage (Deployment)

**Gesamtdauer:** 2-3 Wochen für die vollständige Umsetzung

---

## Portfolio-Highlights

1. **README.md** mit Projektbeschreibung & API
2. **Swagger-Dokumentation** mit Beispielen
3. **Unit- & Integrationstests**
4. **Dockerfiles** für einfachen Start
5. **Live-Demo** mit Echtdaten

---

## Mögliche Erweiterungen

- Bulk-API für Massenkürzung
- Webhooks für Benachrichtigungen
- Social-Media-Integration
- White-Label-Lösung für Unternehmen
- Mobile-App-API

---

## Projektstart

1. Repository klonen
2. Backend & Frontend gemäß Anleitung aufsetzen
3. Docker-Container bauen & starten (optional)
4. Swagger-UI für API-Tests nutzen

---

## Lizenz

MIT License

---

*Dieses Projekt demonstriert die Fähigkeit, ein vollständiges, produktionsreifes Web- und Cloud-System zu entwerfen und umzusetzen. Die README wurde erweitert, um die gesamte Projektarchitektur und Planung zu zeigen.*
