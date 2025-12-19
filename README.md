# FitnessX - AI-Powered Fitness Center Management System

## 📌 Project Description
FitnessX is a modern ASP.NET Core MVC web application designed for fitness centers. It allows members to book appointments with trainers, manage their schedules, and receive AI-generated workout plans based on their personal data and photos.

## 🚀 Features

### 👤 User/Member Features
* **User Registration & Login:** Secure Identity-based authentication.
* **Appointment Booking:** Book sessions with trainers based on real-time availability.
* **My Appointments:** View status of bookings (Pending, Confirmed, Cancelled).
* **AI Fitness Planner:** Integrated **GPT-4o** AI that generates custom workout plans from user stats (Age, Weight, Goal).

### 🛠 Admin Features
* **Dashboard:** View platform statistics and popular trainers.
* **Manage Trainers:** CRUD operations (Create, Edit, Delete) for gym trainers.
* **Manage Services:** Add or modify gym classes (Yoga, Pilates, etc.).
* **Appointment Manager:** Approve or Reject member booking requests.

### 💻 Technical Implementation
* **Framework:** ASP.NET Core 9.0 MVC
* **Database:** Entity Framework Core (Code First) with SQL Server.
* **Architecture:** Area-based separation (Admin vs. User).
* **Web API:** RESTful endpoint (`/api/TrainersApi`) exposing trainer data.
* **LINQ:** Complex queries used for statistical analysis on the dashboard.

## 🤖 AI Integration
This project uses **GitHub Models (OpenAI GPT-4o)** to provide intelligent fitness advice. Users can upload an image which is processed via the `HttpClient` to the inference API.

---
*Developed for Web Application Development Course.*
