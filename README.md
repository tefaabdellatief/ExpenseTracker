# Expense Tracker - .NET MAUI Template

A production-ready cross-platform mobile application template built with .NET MAUI, featuring clean architecture, MVVM pattern, and modern UI design.

## 🚀 Overview

ExpenseTracker is a comprehensive mobile application template that demonstrates best practices in .NET MAUI development. It features clean architecture, MVVM pattern, local data persistence, and a modern UI with theming support. This template serves as an excellent starting point for building cross-platform mobile applications.

## ✨ Features

### Core Functionality
- **Expense Management**: Complete CRUD operations for expense tracking
- **Dashboard Analytics**: Interactive charts showing expense breakdowns by category
- **Expense Categories**: Predefined categories with customizable options
- **Data Export**: Export expense data to CSV format
- **Search & Filter**: Advanced search and filtering capabilities

### User Experience
- **Authentication System**: Demo login with session management
- **Theme System**: Dynamic light/dark theme switching
- **Responsive UI**: Optimized for Android and iOS platforms
- **Data Persistence**: Local SQLite with automatic data seeding

### Technical Features
- **Offline-First**: Full functionality without internet connection
- **Data Synchronization**: Background sync service (ready for backend integration)
- **Error Handling**: Comprehensive error handling for smooth UX
- **Performance Optimized**: Release mode optimizations and linker configuration

## 🛠️ Technology Stack

- **.NET 9.0** - Latest .NET framework
- **.NET MAUI** - Cross-platform UI framework
- **C# 12** - Latest language features with nullable reference types
- **SQLite** - Local database (sqlite-net-pcl)
- **CommunityToolkit.Mvvm** - MVVM helpers and commands
- **CommunityToolkit.Maui** - Additional MAUI controls and features
- **Syncfusion.Maui.Charts** - Data visualization charts

## 📋 Prerequisites

### Required Software
- **Visual Studio 2022** (17.8+) with **.NET MAUI** workload
  - OR **Visual Studio Code** with C# extension and MAUI extension
- **.NET 9.0 SDK** or later
- **MAUI Workload**: `dotnet workload install maui`

### Platform-Specific Requirements

#### Android Development
- **Android SDK** (API Level 21+)
- **Android Studio** (recommended for emulator management)
- **Java Development Kit** (JDK) 11 or later

#### iOS Development (macOS only)
- **Xcode** 14.3 or later
- **Apple Developer Account** (for device deployment)
- **macOS** 12.5 or later

### Development Tools (Recommended)
- **Android Emulator** or physical Android device
- **iOS Simulator** or physical iOS device
- **SQLite Browser** (for database inspection)

## 🚀 Installation & Setup

### 1. Install Prerequisites

#### Install .NET MAUI Workload
```bash
dotnet workload install maui
```

#### Verify Installation
```bash
dotnet --version
# Should show .NET 9.0 or later
```

### 2. Clone and Setup
```bash
git clone <repository-url>
cd ExpenseTracker
```

### 3. Restore Dependencies
```bash
# From the project root
dotnet restore ExpenseTracker.sln

# Or from src directory
cd src
dotnet restore
```

### 4. Configure Development Environment

#### Android Setup
1. Open Visual Studio
2. Set target device to Android Emulator or physical device
3. Ensure Android SDK is properly configured

#### iOS Setup (macOS only)
1. Open Visual Studio for Mac
2. Connect to Mac build host
3. Set target to iOS Simulator or physical device

### 5. Build and Run

#### Android
```bash
# From src directory
dotnet build -t:Run -f net9.0-android
```

#### iOS (macOS only)
```bash
# From src directory
dotnet build -t:Run -f net9.0-ios
```

#### Windows (if supported)
```bash
# From src directory
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

## 📁 Project Structure

```
ExpenseTracker/
├── src/
│   ├── ExpenseTracker.App/          # Main MAUI application
│   │   ├── Views/                   # XAML pages and controls
│   │   ├── ViewModels/              # MVVM ViewModels
│   │   ├── Services/                # Application services
│   │   ├── Resources/               # Images, fonts, assets
│   │   ├── Platforms/               # Platform-specific code
│   │   └── App.xaml.cs             # Application entry point
│   ├── Entities/                   # Domain entities and DTOs
│   │   ├── Models/                  # Data models
│   │   ├── Enums/                   # Application enums
│   │   └── DTOs/                    # Data transfer objects
│   ├── Infrastructure/              # Data access and utilities
│   │   ├── Services/               # Data and mock services
│   │   ├── Interfaces/             # Service contracts
│   │   └── Database/               # SQLite setup
│   └── ExpenseTracker.Tests/       # Test projects
├── ExpenseTracker.sln              # Solution file
└── README.md                       # This file
```

## 🔧 Configuration

### Application Configuration

#### App Configuration (MauiProgram.cs)
```csharp
// Services registration
builder.Services.AddSingleton<IExpenseService, MockExpenseService>();
builder.Services.AddSingleton<IAuthenticationService, DemoAuthenticationService>();
builder.Services.AddSingleton<IDataSyncService, DataSyncService>();

// Database configuration
builder.Services.AddSingleton<ISqliteConnection, SqliteConnectionService>();
```

#### Theme Configuration
Themes are configured in `AppTheme.cs`:
```csharp
public static class AppTheme
{
    public static void SetTheme(Theme theme)
    {
        // Theme switching logic
    }
}
```

### Database Configuration

#### SQLite Setup
The application uses SQLite for local data persistence:
- **Database Location**: Local app storage
- **Initialization**: Automatic on first launch
- **Seeding**: Sample data for demonstration

#### Connection String
```csharp
private const string DatabaseName = "ExpenseTracker.db";
```

### Platform-Specific Configuration

#### Android (Platforms/Android/)
- **Permissions**: Storage access permissions
- **Linker Configuration**: Custom linker settings
- **Splash Screen**: Android-specific splash screen

#### iOS (Platforms/iOS/)
- **Info.plist**: iOS app configuration
- **Entitlements**: App capabilities and permissions
- **Launch Screen**: iOS-specific launch screen

## 🧪 Testing

### Run Tests
```bash
# From src directory
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Structure
- **Unit Tests**: ViewModel and service layer tests
- **Integration Tests**: Database and service integration
- **UI Tests**: (Optional) UI automation tests

### Test Categories
- **ViewModel Tests**: Business logic validation
- **Service Tests**: Data access and business rules
- **Model Tests**: Entity validation and behavior

## 📱 Platform Support

### Android
- **Minimum SDK**: Android 5.0 (API 21)
- **Target SDK**: Android API 35
- **Architecture**: ARM64, ARM32, x64
- **Features**: Material Design theming, navigation

### iOS
- **Minimum Version**: iOS 12.2
- **Target Version**: Latest iOS
- **Architecture**: ARM64
- **Features**: iOS design guidelines, navigation

### Windows (Optional)
- **Minimum Version**: Windows 10 (19041)
- **Architecture**: x64, x86
- **Features**: Windows design guidelines

## 🎨 UI and Theming

### Theme System
- **Light Theme**: Clean, bright interface
- **Dark Theme**: Dark interface for low-light environments
- **System Theme**: Automatically follows system preference

### Customization
- **Colors**: Defined in resource dictionaries
- **Fonts**: Custom fonts in Resources/Fonts
- **Icons**: SVG icons for scalability
- **Styles**: Reusable XAML styles

### Responsive Design
- **Adaptive Layout**: Works on different screen sizes
- **Orientation Support**: Portrait and landscape modes
- **Accessibility**: Screen reader support

## 📊 Data Management

### Local Database
- **SQLite**: Local data persistence
- **ORM**: sqlite-net-pcl for data access
- **Migrations**: Database schema management
- **Seeding**: Initial data population

### Data Synchronization
- **Background Sync**: Ready for cloud integration
- **Offline Support**: Full offline functionality
- **Conflict Resolution**: Prepared for sync conflicts

### Data Export
- **CSV Export**: Export data to CSV format
- **Data Privacy**: Local data only (no cloud transmission)

## 🚀 Deployment

### Development Deployment
```bash
# Debug build
dotnet build -c Debug

# Run on specific platform
dotnet build -t:Run -f net9.0-android
```

### Production Deployment
```bash
# Release build
dotnet build -c Release

# Publish for Android
dotnet publish -c Release -f net9.0-android

# Publish for iOS
dotnet publish -c Release -f net9.0-ios
```

### Store Deployment

#### Google Play Store
1. Create signed APK or AAB
2. Set up Google Play Console account
3. Upload and configure store listing
4. Submit for review

#### Apple App Store
1. Create iOS Archive in Xcode
2. Set up App Store Connect account
3. Upload and configure store listing
4. Submit for review

## 🔒 Security Considerations

### Data Security
- **Local Encryption**: SQLite database encryption (optional)
- **Secure Storage**: Sensitive data in secure storage
- **Data Privacy**: No data transmitted to external servers

### Application Security
- **Authentication**: Demo authentication system
- **Authorization**: Role-based access (ready for implementation)
- **Input Validation**: User input sanitization

## 📈 Performance Optimization

### Application Performance
- **Async Operations**: Non-blocking UI operations
- **Memory Management**: Proper disposal and cleanup
- **Image Optimization**: Efficient image loading and caching

### Database Performance
- **Indexing**: Proper database indexes
- **Query Optimization**: Efficient database queries
- **Connection Pooling**: SQLite connection management

### UI Performance
- **Virtualization**: List virtualization for large datasets
- **Lazy Loading**: On-demand data loading
- **UI Threading**: Proper thread management

## 🐛 Troubleshooting

### Common Issues

#### Build Issues
- **MAUI Workload**: Ensure MAUI workload is installed
- **Android SDK**: Verify Android SDK configuration
- **Dependencies**: Clean and restore NuGet packages

#### Runtime Issues
- **Database**: Check SQLite database permissions
- **Navigation**: Verify page registration in MauiProgram
- **Theming**: Ensure theme resources are properly loaded

#### Platform-Specific Issues
- **Android**: Check emulator/device configuration
- **iOS**: Verify Xcode and provisioning profiles
- **Windows**: Ensure Windows SDK is installed

### Debug Mode
Enable detailed logging:
```csharp
#if DEBUG
builder.Logging.AddDebug();
#endif
```

## 📚 API Documentation

### Service Interfaces
```csharp
public interface IExpenseService
{
    Task<IEnumerable<Expense>> GetExpensesAsync();
    Task<Expense> AddExpenseAsync(Expense expense);
    Task<bool> UpdateExpenseAsync(Expense expense);
    Task<bool> DeleteExpenseAsync(int expenseId);
}
```

### ViewModel Base Classes
```csharp
public abstract class BaseViewModel : ObservableObject
{
    // Common ViewModel functionality
}
```

## 🤝 Contributing

When contributing to this template:

1. Follow MVVM pattern and clean architecture
2. Add appropriate tests for new features
3. Update documentation for any changes
4. Ensure cross-platform compatibility
5. Follow .NET MAUI coding conventions

## 📄 License

This template is provided as-is for development purposes. Please review the license file for usage rights.

## 🆘 Support

For issues and questions:
- Check existing documentation
- Review troubleshooting section
- Verify all prerequisites are installed
- Check platform-specific requirements

---

**Built with .NET MAUI and modern mobile development practices** 📱
