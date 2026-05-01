# Hidden Villa — Booking System

A villa booking and reservations platform with payments and an admin back office.

## Stack

- **Backend:** .NET 6, ASP.NET Core MVC + Razor Pages
- **Data:** Entity Framework Core, SQL Server
- **Identity:** ASP.NET Core Identity (roles: Admin, Customer)
- **Payments:** Stripe Checkout
- **PDF:** Rotativa.AspNetCore for booking confirmations
- **Email:** SendGrid
- **Front-end:** Razor + Tailwind / Bootstrap, jQuery for room availability UX

## What it does

Customers browse villas (rooms) by date range, see availability and amenities, place a booking, and pay through Stripe. The system emails a PDF confirmation, lets the customer view and cancel future stays, and pushes a status timeline through the booking lifecycle (Pending → Approved → Checked-In → Completed).

An admin role manages amenities, villa numbers, room photos, and overrides booking status. The availability calculation runs server-side against a date-range query — careful indexing on `CheckInDate` keeps it snappy even at higher record counts.

## Running locally

```bash
# Database
dotnet ef database update --project HiddenVilla_DataAccess --startup-project HiddenVilla_Server

# Run
dotnet run --project HiddenVilla_Server
```

Configure Stripe and SendGrid keys via user secrets:

```bash
dotnet user-secrets set "Stripe:SecretKey" "sk_test_…"
dotnet user-secrets set "SendGrid:ApiKey" "SG.…"
```

## Status

Learning project based on the Bhrugen Patel "Hidden Villa" course. Core booking flow works end-to-end; some admin polish and reporting features are still on the to-do list.

---

Part of [Ipeleng's portfolio](https://github.com/ipelengmekgwe/portfolio).
