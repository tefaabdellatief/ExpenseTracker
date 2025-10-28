# Expense Tracker - .NET MAUI

## Overview

ExpenseTracker is a cross-platform mobile application built with .NET MAUI that helps users track and manage their personal expenses. 
The app features a clean, modern interface with comprehensive expense management capabilities, data visualization through charts, 
and a robust theming system supporting both light and dark modes.

## Features Implemented

### Core Functionality
- **Expense Management**: Add, edit, delete, and view expenses with categories
- **Dashboard Analytics**: Visual charts showing expense percentages breakdowns by category
- **Expense Categories**: Predefined categories (Food, Transport, Shopping, Entertainment, Bills, Other)
- **Data Export**: Export expense data to CSV format
- **Search & Filter**: Search expenses and filter by categories, dates, category name and/or amount

### User Experience
- **Authentication System**: Demo login with user session management
- **Theme System**: Dynamic light/dark theme switching with system theme detection
- **Responsive UI**: Optimized for both Android and iOS platforms
- **Data Persistence**: Local SQLite database with automatic data seeding when alunched first timeg

### Technical Features
- **Offline-First**: Full functionality without internet connection //TODO: internet handling
- **Data Synchronization**: Background data sync service in case of future backend integration //TODO: itegrate with remote server 
- **Error Handling**: Comprehensive error handling lead to smooth user experience without crashes, //TODO: add logging service
- **Performance Optimized**: Release mode optimizations with android linker configuration

## Architecture

The application follows **Clean Architecture** principles with clear separation of concerns:

### MVVM Pattern Implementation
- **ViewModels**: Business logic and data binding
- **Views**: XAML-based UI with data binding and commanding
- **Models**: Data transfer objects and view-specific models for charts

### Service Layer Structure
```
├── Presentation Layer (MAUI App)
│   ├── Views (XAML Pages)
│   ├── ViewModels (Business Logic)
│   └── Services (UI Services)
├── Domain Layer (Entities)
│   ├── DTOs (Data Transfer Objects)
│   └── Enums (Application Enums)
└── Infrastructure Layer
    ├── Services (Data & Mock Services)
    ├── Interfaces (Contracts)
    └── SQLite Configuration
```

### Key Architectural Decisions
- **Dependency Injection**: Built-in .NET MAUI DI container
- **Repository Pattern**: Abstracted data access through interfaces
- **Command Pattern**: RelayCommand for user interactions
- **Observer Pattern**: ObservableCollection and property change notifications

## Technologies Used

- **.NET 9.0** - Latest .NET framework
- **.NET MAUI** - Cross-platform UI framework
- **SQLite** - Local database (sqlite-net-pcl)
- **CommunityToolkit.Mvvm** - MVVM helpers and commands
- **CommunityToolkit.Maui** - Additional MAUI controls and features
- **Syncfusion.Maui.Charts** - Data visualization charts
- **C# 12** - Latest language features with nullable reference types

## Setup Instructions

### 1. Prerequisites
- **Visual Studio 2022 17.8+** or **Visual Studio Code** with C# extension
- **.NET 9.0 SDK** or later
- **MAUI Workload** installed (`dotnet workload install maui`)
- **Android SDK** (for Android development)
- **Xcode** (for iOS development on macOS)

### 2. Clone Repository
```bash
git clone https://github.com/tefaabdellatief/ExpenseTracker
cd ExpenseTracker
```

### 3. Build and Run Steps
```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run on Android
dotnet build -t:Run -f net9.0-android

# Run on iOS (macOS only)
dotnet build -t:Run -f net9.0-ios
```

### 4. Platform-Specific Notes

#### Android
- **Minimum SDK**: Android 5.0 (API 21)
- **Target SDK**: Latest Android API - 35
- **Release Mode**: Optimized with custom linker configuration

#### iOS
- **Minimum Version**: iOS 12.2
- **Deployment**: Requires Apple Developer account for device deployment
- **Simulator**: Runs on iOS Simulator without restrictions

## Screenshots

https://drive.google.com/drive/folders/10V79IjbQN9xq2uduuxTff8GUMVhLxihM?usp=sharing 

## Mock Service Implementation

The application uses a **mock backend approach** for demonstration purposes:

### Data Layer
- **SQLite Database**: Local persistence with automatic initialization
- **Mock Services**: Simulated API responses for realistic data flow
- **Demo Authentication**: Simplified login system for user session management

### Mock Services Include
- **MockExpenseService**: Simulates expense CRUD operations
- **DemoAuthenticationService**: Handles user authentication flow
- **DataSyncService**: Background synchronization simulation //TODO: integrate with real backend

### Data Seeding
- Automatic database initialization on first run
- Pre-populated sample expenses for immediate testing
- Category-based expense distribution for realistic charts

## Trade-offs and Future Improvements

### Current Trade-offs
- **Mock Backend**: Using local SQLite instead of real API for simplicity
- **Limited Categories**: Fixed expense categories vs. user-defined categories
- **Basic Authentication**: Demo login vs. full OAuth/JWT implementation
- **Local Storage**: SQLite only vs. cloud synchronization

### Future Improvements with More Time

#### Backend Integration //TODO
- **REST API Integration**: Replace mock services with real backend
- **Cloud Synchronization**: Multi-device data sync via cloud services
- **Real Authentication**: OAuth 2.0, JWT tokens, and secure user management
- **Push Notifications**: Expense reminders and budget alerts


#### Technical Improvements
- **Offline Sync**: Robust offline-first architecture with conflict resolution
- **Performance**: Virtualization for large datasets, lazy loading
- **Accessibility**: Full screen reader support and accessibility compliance
- **Testing**: Comprehensive unit and integration test coverage
- **CI/CD**: Automated build, test, and deployment pipelines

#### User Experience
- **Onboarding**: Interactive tutorial for new users
- **Customization**: User-defined categories, themes, and preferences
- **Sharing**: Export reports, share expenses with family/teams
- **Widgets**: Home screen widgets for quick expense entry

### Architecture Scalability
- **Microservices**: Split backend into focused microservices
- **CQRS Pattern**: Separate read/write operations for better performance
- **Event Sourcing**: Audit trail and data history tracking
- **Caching Strategy**: Redis/memory caching for improved performance
