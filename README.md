# Stopwatch Application

A desktop stopwatch app built on C# with Avalonia UI.

## Features

- **Timer Controls**: Start, pause, resume, reset, and stop.
- **Live Display**: Elapsed time shown as `hh:mm:ss`, counting seconds → minutes → hours.
- **Tested Logic**: Stopwatch logic built test-first with xUnit.

![App screenshot](src/App/Assets/screenshot.png)

## Quick Start

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run
```bash
dotnet run --project src/App
```

### Test
```bash
dotnet test tests/Tests
```

## Structure

- `src/App/StopwatchTimer.cs` : Stopwatch logic
- `src/App/MainWindow.axaml` : UI layout
- `src/App/MainWindow.axaml.cs` : Button handlers & timer
- `tests/Tests/StopwatchTimerTests.cs` : Unit tests
