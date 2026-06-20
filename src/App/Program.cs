using Avalonia;
using System;

namespace Stopwatch.App;

class Program
{
    /// <summary>Starts the Avalonia desktop application.</summary>
    [STAThread]
    public static void Main(string[] args) => 
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    /// <summary>Builds and configures the Avalonia application.</summary>
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();
}
