# Copilot Instructions — NationalPark

## Build & Run

This is a WinUI 3 desktop app packaged with MSIX. It requires a platform-specific build (no AnyCPU).

```powershell
# Build (must specify platform)
dotnet build NationalPark\NationalPark.csproj -p:Platform=x64

# Run from Visual Studio with F5 (x64 configuration), or:
dotnet run --project NationalPark\NationalPark.csproj -p:Platform=x64
```

After switching target frameworks or updating the Windows App SDK, always clean `bin/` and `obj/` folders before rebuilding to avoid stale XAML resource errors.

## Architecture

Single-project WinUI 3 app targeting `net10.0-windows10.0.22000.0` with Windows App SDK 1.7.

### MVVM Structure (no framework)

The app uses a hand-rolled MVVM pattern — no MVVM toolkit or DI container.

- **Models** (`Models/`): Plain C# classes (`NationalParkModel`, `VisitRecord`) with auto-properties and `string.Empty` defaults.
- **ViewModels** (`ViewModels/`): Implement `INotifyPropertyChanged` manually via `OnPropertyChanged([CallerMemberName])`. Use `ObservableCollection<T>` for list bindings.
- **Services** (`Services/`): Business logic and data access. `NationalParkService` uses a static lazy-init singleton pattern with `lock` for thread safety. Data is loaded from `Data/nationalparks.json` at runtime with a hardcoded fallback.
- **Controls** (`Controls/`): Custom `UserControl` subclasses that expose events (e.g., `BackRequested`, `VisitRequested`) to communicate with the parent window. Controls set `DataContext = this` and expose wrapper properties for binding.
- **Converters** (`Converters/`): All `IValueConverter` implementations live in `ValueConverters.cs` as separate classes in the same file.
- **Dialogs** (`Dialogs/`): `ContentDialog` subclasses with `INotifyPropertyChanged` for two-way binding.

### Navigation

Navigation between home and detail views is done via `Visibility` toggling on elements in `MainWindow.xaml` — there is no `Frame`/`Page` navigation.

### Data Binding Conventions

- **XAML `x:Bind`** is used for compile-time bindings to the code-behind (e.g., `{x:Bind ViewModel.Parks}`, `{x:Bind GetParkCountText(...)}`).
- **XAML `{Binding}`** is used for runtime DataContext-based bindings within `DataTemplate` and custom controls.
- Helper methods in code-behind return `Visibility`, `SolidColorBrush`, or `string` for use in `x:Bind` function bindings (e.g., `GetVisitStatusIcon(bool)`).

### Data Loading

Park data comes from `Data/nationalparks.json` (copied to output on build via `CopyToOutputDirectory=Always`). `NationalParkService.EnsureInitialized()` loads it synchronously with `System.Text.Json` using `PropertyNameCaseInsensitive = true`. Visit records are stored in-memory only (not persisted).

## Key Conventions

- **Nullable reference types** are enabled (`<Nullable>enable</Nullable>`).
- **`var` usage**: `.editorconfig` sets `csharp_style_var_elsewhere = true` — prefer `var` when the type is apparent.
- **Debug logging**: `System.Diagnostics.Debug.WriteLine` is used throughout for tracing initialization and errors (not a logging framework).
- **Error handling in App.xaml.cs**: `OnLaunched` catches exceptions and shows a fallback error window. `UnhandledException` handler marks errors as handled to prevent crashes.
- **WinUI 3 theming**: Uses `MicaBackdrop`, `ThemeShadow`, and theme resources (`CardBackgroundFillColorDefaultBrush`, `SystemAccentColor`, etc.).
