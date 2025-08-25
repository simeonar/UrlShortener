# Projektmanagement-Leitfaden – URL Shortener Dienst

Ziel dieses Dokuments ist es, als verbindliche Arbeitsgrundlage für die Entwicklung, Planung, Koordination und Ressourcensteuerung des Projekts „URL Shortener Dienst“ zu dienen. Es beschreibt Ziele, Rollen, Arbeitsweise, Qualitätsanforderungen sowie Prozesse für Planung, Umsetzung, Test, Release und Betrieb.

---

## 1. Ziele, Umfang, Nicht-Ziele

- Ziele
  - Bereitstellung eines stabilen URL-Shortener-Dienstes mit Kurzlink-Erstellung, Weiterleitung, Statistiken und QR-Codes.
  - Bereitstellung einer modernen Weboberfläche (Angular) für Nutzer und Admins.
  - Nachweis moderner .NET-Entwicklung, Testbarkeit, Observability und containerisiertem Deployment.
- Umfang (Scope)
  - Backend: ASP.NET Core API (Shorten, Redirect, Stats, QR).
  - Core/Infrastructure: Geschäftslogik, Services, Repositories.
  - Frontend: Angular SPA mit Dashboard und Verwaltungsfunktionen.
  - Tests: Unit-, Integrations- und perspektivisch E2E-/Lasttests.
- Nicht-Ziele (vorerst)
  - Social-Login, Webhooks, Bulk-API, Mobile-App, White-Labeling.

## 2. Stakeholder, Rollen und Verantwortlichkeiten (RACI)

- Product Owner (PO): Priorisierung, Vision, Abnahme (A/R).
- Projektleiter (PL): Planung, Roadmap, Ressourcensteuerung, Risiken, Reporting (A/R).
- Tech Lead (TL): Architektur, technische Leitplanken, Code-Qualität, Reviews (A/R).
- Backend Devs: API, Core, Tests (R).
- Frontend Devs: Angular SPA, UI/UX, Tests (R).
- QA/Testing: Teststrategie, Automatisierung, Qualitätssicherung (R/C).
- DevOps: CI/CD, Infrastruktur, Monitoring, Security-Standards (R/C).
- Stakeholder/Management: Budget, Meilensteine, Freigaben (C/I).

RACI: R = Responsible, A = Accountable, C = Consulted, I = Informed.

## 3. Architekturleitlinien

- Schichten: API (Präsentation/Endpoints), Core (Domäne/Services), Infrastructure (Datenzugriff/extern), Web (SPA).
- Saubere Trennung von Domäne und technischen Details; Dependency Injection überall.
- Konfiguration per appsettings und Umgebungsvariablen; keine Secrets im Code.
- Logging über Serilog; strukturierte Logs; Korrelation-ID pro Request.
- Validierung mit FluentValidation; Fehler als problem+json zurückgeben.
- QR-Generierung mit Caching; Shortcodes Base62, 6–8 Zeichen.

## 4. Arbeitsmodell & Zeremonien

- Vorgehen: Iterativ-inkrementell (Scrum/Kanban hybrid), 2‑wöchige Sprints.
- Events
  - Sprint Planning: Ziel(e), Umfang, Kapazität, Risiken.
  - Daily (15 Min): Blocker, Fortschritt, Nächstes.
  - Review/Demo: Inkrement zeigen, Feedback sammeln.
  - Retro: Lernen, Maßnahmen ableiten.
- Artefakte
  - Product Backlog (PO-own), Sprint Backlog, Burndown, Release-Plan.
  - Definition of Ready (DoR), Definition of Done (DoD) – siehe Anhang.

## 5. Backlog-Management & Priorisierung

- Ticket-Template
  - Titel, Business Value, Akzeptanzkriterien (Given/When/Then), Nicht-Ziele, Abhängigkeiten.
  - Tech Notes (Architektur/Design-Skizze), Risiken, Testnotizen.
- Priorisierung: MoSCoW (Must/Should/Could/Won’t), zusätzlich WSJF optional.
- Schätzung: Story Points (Fibonacci), Referenzstories beibehalten.

## 6. Branching-, Review- und Merge-Strategie

- Trunk-based mit Kurzlebigen Feature-Branches: feature/*, fix/*, chore/*.
- Pull Requests
  - Mind. 1 Reviewer (vorzugsweise TL) + grüne CI (Build + Tests + Lint).
  - Small PRs (< 400 LOC diff), klare Beschreibung, Checkliste ausfüllen.
- Konventionen
  - Conventional Commits (feat:, fix:, refactor:, test:, docs:, chore: …).
  - SemVer für Releases (Major.Minor.Patch).

## 7. Qualitätsrichtlinien (Engineering)

- .NET 8, Nullable Reference Types on, Analyzers als Warning → Fix vor Merge.
- Architekturrichtlinie: keine Infrastructure-Abhängigkeit in Core.
- Logging: kein PII im Log; User-Agent/Referrer ggf. gehasht/anonymisiert.
- Performance-Budgets
  - Redirect P95 < 50 ms (Serverzeit, ohne Netz);
  - Shorten P95 < 150 ms;
  - QR-Generierung mit Cache-Hit < 80 ms.
- Sicherheit: OWASP ASVS L1; Rate Limiting aktiv; Secrets via Umgebungsvariablen/KeyVault.

## 8. Umgebungen & Konfiguration

- Local Dev: dotnet run (API), npm start (SPA); In-Memory/Datei-Backends möglich.
- Test/Staging: identisches Setup in Containern, echte DB/Cache.
- Prod: Container-Orchestrierung (z. B. Azure/AWS), Logs/Metadaten zentral.
- Konfigurationshierarchie: Default → Environment → Secret Store.

## 9. CI/CD-Pipeline (Richtlinie)

1) Build & Restore (API, Core, Infrastructure, Web)
2) Lint/Analyse (C# Analyzers, ESLint)
3) Tests: Unit → Integration (Mock-Services wenn nötig)
4) SCA/Dependency-Check (z. B. GitHub Dependabot)
5) Package/Docker Image Build
6) Deployment nach Test/Staging; Smoke-Tests; manuelles Gate für Prod

## 10. Teststrategie & Abdeckung

- Unit-Tests (Core/Services, Repositories mit Mocks) – Ziel: ≥ 70% in Core.
- Integrations-Tests (API-Endpunkte, Redirect, QR, Stats)
- E2E/Contract‑Tests optional (Postman/Playwright) – Happy + Fehlerpfade.
- Lasttests (k6/JMeter) für Redirect/Shorten-Pfade, Basisprofile dokumentieren.

## 11. Observability, Betrieb & Support

- Logging: Serilog mit strukturierter Ausgabe; Request-ID, User/Session-IDs.
- Metriken: Requests/s, Latenzen (P50/P95), Fehlerquoten, Cache-Hitrate, Queue-Länge.
- Dashboards & Alerts: 24/7-fähige Alarme für Fehlerquote/Latenzen > Schwellwert.
- Incident Management: Triage (P1–P3), On‑Call Rotation, Post‑Mortems mit Maßnahmen.

## 12. Security & Compliance

- Geheimnisse nie im Repo; sichere Bereitstellung (Vault/KeyStore).
- Eingabewerte strikt validieren; keine offenen Redirects ohne Kontrolle.
- Datenschutz: IPs anonymisieren, Retention für Logs/Statistiken definieren.
- Abhängigkeiten aktuell halten; CVEs zeitnah schließen.

## 13. Release- und Versionsmanagement

- Versionierung: SemVer; CHANGELOG pro Release.
- Release-Kandidaten über Staging validieren; Abnahme durch PO/PL.
- Rollback-Strategie: Blue/Green oder Rolling mit schnellem Zurücksetzen.

## 14. Risiko-Management (Top-Risiken)

- Engpässe bei Ressourcen/Kapazität → Priorisierung, Scope-Schnitt, Fokus.
- Technische Schulden → kontinuierliche Refactoring-Slots (max 20% Sprint-Kapazität).
- Sicherheitslücken in Libs → Routine-Updates, SCA in CI.
- Unklare Anforderungen → DoR strikt, frühe PO‑Einbindung, Spike‑Stories.

## 15. Ressourcen- & Kapazitätsplanung

- Kapazität je Sprint in Personentagen dokumentieren; Velocity tracken.
- RACI pro Meilenstein aktualisieren.
- Budget-/Zeit-Tracking via Tickets und Releases.

## 16. Roadmap & Meilensteine (Beispiel)

- M1: Kernfunktionen Shorten/Redirect, Stats-API Basis, QR-Service – „Beta“.
- M2: Web-UI vollständig, Auth (Basis), Rate Limiting – „RC1“.
- M3: Observability/Performance/Lasttests, Security-Hardening – „1.0“.
- M4: CI/CD vollautomatisch, Cloud-Deployment – „1.1“.

## 17. Definition of Ready (DoR)

- Klarer Business Value, Abnahmekriterien, Design/Tech-Notizen vorhanden.
- Abhängigkeiten benannt; Risiken grob bewertet; Testnotizen definiert.
- Schätzung und Priorität festgelegt.

## 18. Definition of Done (DoD)

- Implementiert, gelintet, getestet (Unit/Integration), Dokumentation aktualisiert.
- Security/Privacy geprüft, Logs angemessen, Konfiguration mit Env‑Variablen.
- CI grün, Review erfolgt, Merge in main; Feature-Flags dokumentiert (falls genutzt).

## 19. Beitrags‑/PR‑Leitfaden (Kurz)

- Branch: feature/<kurz-beschreibung>
- PR: Beschreibung, Screenshots (UI), Risiko, Testhinweise, Checkliste.
- Reviewer zuweisen; nach Freigabe Squash‑Merge.

## 20. Lokale Entwicklung (Referenz)

- Backend (API)
  - Start: `dotnet run --urls=http://0.0.0.0:5212`
- Frontend (Angular)
  - Start: `npm start -- --host 0.0.0.0`

---

Ansprechperson: Projektleiter (PL) – Koordination, Freigaben, Eskalation.

---

## 21. Governance & Steuerung

- Steuerkreis (monatlich/bei Bedarf): PO, PL, TL, DevOps, QA. Entscheidungen zu Scope, Budget, Risiken.
- Entscheidungswege: Technische Entscheidungen beim TL, produktseitige beim PO, projektübergreifend beim PL.
- Eskalation: PL → Steering → Management.

## 22. Termin- und Meilensteinplan

- Sprint-Kalender: 2 Wochen, Start jeweils Montag 10:00, Ende Freitag 15:00 (Review/Retro).
- Zieltermine je Meilenstein (M1–M4) werden im Release-Plan geführt (siehe Templates/Release-Plan).
- Gantt-/Roadmap-Übersicht in Projektboard gepflegt; Abweichungen > 10% werden gemeldet (Weekly Report).

## 23. KPIs, SLAs/SLOs & OKRs

- Produkt-KPIs: Redirect-Latenz (P95), Verfügbarkeit (SLA 99,5%), Fehlerrate (< 0,5%).
- Prozess-KPIs: Deployment-Frequenz, MTTR, Lead/Cycle Time, Testabdeckung Core ≥ 70%.
- OKRs pro Quartal definieren und im Steering überwachen.

## 24. Reporting & Controlling

- Wöchentlicher Statusbericht (Green/Yellow/Red), Fortschritt ggü. Plan, Risiken, Blocker.
- Burndown/Burnup pro Sprint; Velocity-Tracking.
- Budget- und Kapazitätsreport (monatlich) durch PL.

## 25. Kommunikationsplan

- Dailies (Dev-Team), Weekly (PL ↔ Stakeholder), Sprint Review/Retro, Steering monatlich.
- Kanäle: Issue-Tracker (Tickets), Chat (kurz), E-Mail (Entscheidungen), Wiki/Docs (Dauerhaftes).
- Protokollpflicht für Entscheidungen (Decision Log).

## 26. Change Management

- Änderungen an Scope/Terminen via Change Request; Bewertung: Aufwand, Risiko, Impact.
- Freigabe durch PO/PL; Versionierung der Roadmap.

## 27. Budget & Ressourcen

- Ressourcenplan pro Sprint (Kapazität in PT); Auslastung tracken.
- Budgetverbrauch (geschätzt/ist) monatlich; Kosten für Infrastruktur/Tools separat.

## 28. Releasekalender & Abnahme

- Geplante Releases im Kalender (Monat, Sprint‑Ende, Hotfix-Slots).
- Abnahme durch PO/PL gegen Akzeptanzkriterien; Go/No‑Go-Checkliste.

## 29. Entscheidungshistorie

- Alle wesentlichen Architektur‑/Produktentscheidungen im Decision Log erfassen (Datum, Kontext, Optionen, Entscheidung, Folgen).

## 30. Templates & Artefakte

- Siehe Ordner docs/templates für Status‑Report, Risk-/RAID‑Log, Change Request, Decision Log, Release‑Plan, Kommunikationsplan, KPI/SLO‑Matrix, Sprint‑Kalender, Ressourcenplan.
