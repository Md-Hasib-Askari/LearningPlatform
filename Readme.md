# E-Learning Platform with Assessment System

## Minimal Design Document (MVP)

---

## 1. Overview

This document describes the design of an **E-Learning Management System (LMS)** that supports online courses, YouTube-based video lessons, assessments, progress tracking, and certifications.

The MVP focuses on **core learning workflows** while outsourcing video hosting to YouTube for scalability and cost efficiency.

**Primary Goals**

* Demonstrate clean N-tier architecture
* Implement logic beyond basic CRUD
* Showcase assessment, analytics, and gamification
* Meet academic and production-quality standards

---

## 2. Scope

### Included in MVP

* Course, module, and lesson management
* YouTube video-based lessons
* Quiz and assessment system with auto-grading
* Assignment submission and grading
* Progress tracking and learning analytics
* Gamification (points, achievements, leaderboard)
* Certificate generation and verification
* Forums and notifications

### Out of Scope (Future)

* Live classes
* Payments
* Mobile applications
* AI-based proctoring
* Advanced ML recommendations

---

## 3. Architecture Overview

### High-Level Architecture

```
Client (Web)
   ↓
API Layer (ASP.NET Core)
   ↓
Business Logic Layer
   ↓
Data Access Layer
   ↓
SQL Server + External Services
```

### Architecture Style

* N-Tier / Clean Architecture
* Repository + Unit of Work
* Stateless REST API
* Event-driven notifications
* Background job processing

---

## 4. Technology Stack

### Backend

* ASP.NET Core 8 Web API
* Entity Framework Core 8
* SQL Server
* JWT Authentication
* Redis (caching, tokens)
* SignalR (real-time updates)
* Hangfire (background jobs)

### External Services

* YouTube (video hosting)
* SMTP / SendGrid (email)
* Blob Storage (files, certificates)
* QuestPDF (certificate generation)

---

## 5. Layered Design

### Presentation Layer (API)

Responsibilities:

* HTTP endpoints
* Authentication and authorization
* Validation and exception handling
* API versioning and documentation

Key Components:

* Controllers (Auth, Courses, Quizzes, Assignments, Progress, Gamification)
* SignalR hubs (notifications, forums)
* Middleware (logging, errors, rate limiting)

---

### Business Logic Layer (Services)

Responsibilities:

* Business rules
* Assessment grading
* Progress calculations
* Gamification logic
* Certificate generation
* Notifications

Key Services:

* CourseService
* QuizService
* AssignmentService
* ProgressService
* GamificationService
* CertificateService
* RecommendationService

---

### Data Access Layer

Responsibilities:

* Database interactions
* Transactions
* Query optimization

Patterns Used:

* Repository pattern
* Unit of Work
* EF Core configurations and migrations

---

### Domain Layer

Responsibilities:

* Entity definitions
* Enums and value objects
* Core domain rules

Key Entities:

* User, Course, Module, Lesson
* Quiz, Question, QuizAttempt
* Assignment, Submission
* Enrollment, Progress
* Achievement, Certificate, Notification

---

## 6. Database Design

### Key Characteristics

* Fully normalized schema
* Strong referential integrity
* Indexing on high-read tables
* Soft deletes where required
* Audit logging

### Core Data Groups

* Users and Roles
* Courses, Modules, Lessons
* Enrollments and Progress
* Quizzes and Attempts
* Assignments and Submissions
* Gamification and Achievements
* Certificates and Verification
* Notifications and Audit Logs

---

## 7. Authentication & Authorization

### Authentication

* JWT access tokens (short-lived)
* Refresh tokens stored in Redis
* BCrypt password hashing

### Authorization

* Role-based access control:

  * Admin
  * Instructor
  * Student
* Resource-based rules:

  * Only enrolled students access course content
  * Instructors manage only their courses
  * Students access only their submissions and certificates

---

## 8. Key Features (Beyond CRUD)

### 1. Assessment Engine

* Multiple question types
* Auto-grading with partial credit
* Time limits and attempt limits
* Randomized questions
* Detailed result analytics

### 2. Progress Tracking & Analytics

* Lesson completion tracking
* Time-spent analytics
* Learning streaks
* Performance trends
* Leaderboards

### 3. Gamification

* Points for learning actions
* Achievements and badges
* Global and course-specific leaderboards
* Anti-abuse rules and rate limiting

### 4. Assignment Workflow

* File uploads
* Late submission penalties
* Instructor grading
* Feedback and resubmissions
* Automated reminders

### 5. Certificate System

* Auto-generated certificates
* PDF generation
* Unique certificate numbers
* Public verification endpoint

### 6. Recommendation Engine

* Category-based suggestions
* Collaborative filtering
* Skill-gap analysis
* Trending courses
* Cached scoring system

---

## 9. YouTube Integration

* Lessons store YouTube video IDs and URLs
* Videos embedded via iframe
* Optional YouTube API usage for metadata
* Progress tracked based on watch completion

---

## 10. API Design

### Characteristics

* RESTful endpoints
* JSON request/response
* Proper HTTP status codes
* Versioned APIs
* Swagger documentation

### Major API Groups

* Authentication
* Users
* Courses, Modules, Lessons
* Quizzes and Attempts
* Assignments and Submissions
* Forums
* Progress and Analytics
* Gamification
* Certificates
* Notifications

---

## 11. Non-Functional Requirements

* Stateless API design
* Horizontal scalability
* Caching for heavy reads
* Background processing for long tasks
* Secure token handling
* Audit logging for critical actions

---

## 12. Academic Alignment

* Full REST compliance
* Proper HTTP verb usage
* Clear N-tier separation
* Complex business logic
* Well-designed relational database
* Production-level patterns
* Exceeds “CRUD-only” expectations

---

## 13. Implementation Plan (High Level)

1. Foundation: Auth, DB, core entities
2. Learning Flow: Courses, lessons, YouTube integration
3. Assessments: Quizzes and assignments
4. Engagement: Progress, gamification, forums
5. Automation: Notifications, certificates
6. Testing, optimization, documentation
