# URL-Shortener Dienst

**Projekt:** URL Shortener Service (bit.ly Alternative)
**Version:** 1.0  

---

## Projektübersicht

Dieses Projekt ist ein umfassender URL-Shortener-Dienst, der das Kürzen von Links, benutzerdefinierte Aliase, Klick-Analysen, QR-Code-Generierung und eine moderne Weboberfläche bietet. Ziel ist es, moderne .NET-Backend-Entwicklung und Cloud-Deployment zu demonstrieren. Die Architektur ist skalierbar und modular aufgebaut, um Best Practices der Softwareentwicklung zu zeigen.

### Hauptfunktionen

- **Link-Kürzung:** Lange URLs werden in kurze, leicht teilbare Links umgewandelt.
- **Benutzerdefinierte Aliase:** Nutzer können eigene Aliase für ihre Links wählen, sofern diese verfügbar sind.
- **Ablaufdatum:** Optionales Ablaufdatum für jeden Link.
- **Klick-Analyse:** Erfassung von Klicks, inkl. Zeitstempel, User-Agent, IP-Adresse (anonymisiert), Referrer und Land.
- **Statistiken:** Übersichtliche Statistiken zu Klicks, Herkunftsländern, Browsern und Traffic-Quellen.
- **QR-Code-Generierung:** Automatische Erstellung und Caching von QR-Codes für jeden Kurzlink.
- **Web-Interface:** Intuitive Angular-Oberfläche mit Dashboard, Linkverwaltung und Statistiken.
- **API:** Moderne REST-API mit Swagger-Dokumentation.
- **Sicherheit:** Validierung von URLs und Aliases, optionale Nutzerverwaltung und API-Keys.
- **Cloud- und Docker-Deployment:** Bereit für Containerisierung und Cloud-Betrieb.

---

## Technologiestack

- **Backend:** ASP.NET Core 8, Entity Framework Core, Serilog, QRCoder
- **Frontend:** Angular, Bootstrap 5, Chart.js
- **Datenbank:** SQL Server/PostgreSQL
- **Caching:** Redis
- **Deployment:** Docker, Azure/AWS

---

## API-Funktionen (Auswahl)

- `POST /api/shorten` – Erstellen eines Kurzlinks (mit optionalem Alias und Ablaufdatum)
- `GET /api/urls/{shortCode}` – Informationen zu einem Kurzlink abrufen
- `GET /{shortCode}` – Weiterleitung zum Original-Link
- `GET /api/stats/{shortCode}` – Statistiken zum Kurzlink
- `GET /api/qr/{shortCode}` – QR-Code als Bild abrufen

---

## Architektur & Besonderheiten

Das Projekt ist in mehrere Module unterteilt:
- **Core:** Enthält die Geschäftslogik und Datenmodelle (z.B. `ShortenedUrl`, `ClickStatistic`).
- **API:** Stellt die REST-Schnittstellen bereit und implementiert Middleware für Tracking und Validierung.
- **Infrastructure:** Datenzugriff, Caching und externe Dienste.
- **Web:** Angular-Frontend für Nutzerinteraktion und Visualisierung.

Die Lösung demonstriert:
- Saubere Trennung von Verantwortlichkeiten (Clean Architecture)
- Erweiterbarkeit (z.B. Nutzerverwaltung, Admin-Panel, Rate Limiting)
- Moderne Entwicklungspraktiken (Unit- und Integrationstests, CI/CD, Docker)

---

## Beispiel für Datenmodell

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

## Projektstart

1. Repository klonen
2. Backend und Frontend gemäß Anleitung im Projekt aufsetzen
3. Docker-Container bauen und starten (optional)
4. Swagger-UI für API-Tests nutzen

---

## Portfolio-Highlights

- **Vollständige API-Dokumentation (Swagger)**
- **Unit- und Integrationstests**
- **Docker- und Cloud-Deployment**
- **Moderne Weboberfläche**
- **Skalierbare Architektur**

---

## Lizenz

MIT License

---

*Dieses Projekt demonstriert die Fähigkeit, ein vollständiges, produktionsreifes Web- und Cloud-System zu entwerfen und umzusetzen.*
