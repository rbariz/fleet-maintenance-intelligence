# 1. Fleet Maintenance Intelligence — Architecture

This document describes the architecture and design decisions behind the Fleet Maintenance Intelligence system.

---

# 2. Overview

Fleet Maintenance Intelligence is a system designed to:

- monitor vehicle condition
- manage maintenance plans
- detect anomalies
- generate alerts
- provide operational visibility

The system focuses on **predictive and rule-based maintenance** rather than simple CRUD.

---

# 3. High-Level Architecture

The project follows a **Clean Architecture / Modular design**:

- Domain → business rules
- Application → use cases
- Infrastructure → technical implementation
- API → delivery layer


Client (Web Dashboard)
↓
ASP.NET Core API
↓
Application Layer (Use Cases)
↓
Domain Layer (Entities + Rules)
↓
Infrastructure (In-Memory / future DB)


---

# 4. Domain Model

## 4.1 Core Entities

### FleetVehicle
Represents a tracked vehicle.

- identity (VIN, registration)
- current mileage
- operational status

---

### MaintenancePlan
Defines when maintenance should occur.

Types:
- Distance-based (e.g., every 10,000 km)
- Time-based (e.g., every 90 days)
- Hybrid

---

### MaintenanceRecord
Represents actual maintenance operations performed.

- preventive
- corrective
- inspection

---

### VehicleTelemetrySnapshot
Represents real-time or periodic vehicle data:

- mileage
- fuel level
- battery voltage
- engine temperature

---

### MaintenanceAlert
Represents a detected issue:

- maintenance overdue
- abnormal telemetry
- system anomalies

---

# 5. Core Use Cases

## 5.1 EvaluateMaintenanceAlerts

This is the **core intelligence of the system**.

It:

1. loads vehicle
2. loads maintenance plans
3. loads latest telemetry
4. evaluates rules
5. generates alerts

---

### Rules implemented

#### Maintenance rules

- overdue (distance/time exceeded)
- due soon (threshold approaching)

#### Telemetry rules

- high engine temperature
- low battery voltage
- low fuel level

---

## 5.2 Alert Lifecycle

Alerts follow a workflow:

Open → Acknowledged → Resolved


Operations:
- Acknowledge → operator confirms awareness
- Resolve → issue handled

---

## 5.3 Idempotency (Critical Design)

The system avoids duplicate alerts using:

- VehicleId
- Title
- Status (Open/Acknowledged)

This ensures:
- no alert spam
- stable system behavior
- production readiness

---

## 5.4 Vehicle Timeline

The system exposes a unified timeline combining:

- maintenance records
- telemetry snapshots
- alerts

This provides a **complete operational history** of the vehicle.

---

## 5.5 Vehicle Summary

A synthetic view including:

- vehicle identity
- maintenance plans
- latest telemetry
- open alerts

Used for dashboards and operator decisions.

---

# 6. Infrastructure (Current)

The system currently uses:

- In-Memory repositories
- Simple UnitOfWork
- SystemClock abstraction

This allows:
- fast prototyping
- easy testing
- demonstration without DB

---

# 7. API Design

Main endpoints:


POST /api/maintenancealerts/evaluate/{vehicleId}
GET /api/maintenancealerts
POST /api/maintenancealerts/{id}/acknowledge
POST /api/maintenancealerts/{id}/resolve

GET /api/vehicles/{id}/timeline
GET /api/vehicles/{id}/summary

GET /api/fleet-dashboard


---

# 8. Web Dashboard

A minimal web UI provides:

- alert monitoring
- vehicle summary
- maintenance visibility
- operator actions (acknowledge/resolve)

The dashboard is intentionally simple but demonstrates:

- real-time interaction
- operational workflows
- system behavior

---

# 9. Key Design Decisions

## 9.1 Clean separation of concerns

- Domain is pure business logic
- Application orchestrates use cases
- Infrastructure is replaceable

---

## 9.2 Domain-first approach

Business rules are implemented inside:

- entities
- use cases

NOT inside controllers.

---

## 9.3 Idempotent alert generation

Avoids duplication and ensures stability.

---

## 9.4 Extendable architecture

The system is ready for:

- database integration (PostgreSQL)
- real-time updates (SignalR)
- predictive analytics
- AI-based anomaly detection

---

# 10. Future Improvements

Planned extensions:

- persistence layer (EF Core + PostgreSQL)
- background jobs (Hangfire)
- predictive maintenance (ML models)
- real-time telemetry ingestion
- multi-vehicle fleet management
- alert prioritization & scoring

---

# 11. Conclusion

This project demonstrates:

- real-world domain modeling
- rule-based intelligence
- operational workflows
- clean architecture implementation

It reflects how production systems are designed beyond CRUD applications.
