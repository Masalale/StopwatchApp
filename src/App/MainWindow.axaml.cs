using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Stopwatch.App;

public partial class MainWindow : Window
{
    private StopwatchTimer _timer = new();
    private DispatcherTimer _ticker;

    /// <summary>Sets up the window and starts the one-second timer.</summary>
    public MainWindow()
    {
        InitializeComponent();

        _ticker = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _ticker.Tick += (_, _) =>
        {
            _timer.Tick();
            UpdateDisplay();
        };
        _ticker.Start();
    }

    /// <summary>Updates the on-screen time.</summary>
    private void UpdateDisplay()
    {
        TimeDisplay.Text = _timer.Formatted;
    }

    /// <summary>Start button: begins timing from 00:00:00.</summary>
    private void OnStart(object? sender, RoutedEventArgs e)
    {
        _timer.Start();
        UpdateDisplay();
    }

    /// <summary>Pause button: pauses and shows the current time.</summary>
    private void OnPause(object? sender, RoutedEventArgs e)
    {
        _timer.Pause();
        UpdateDisplay();
    }

    /// <summary>Resume button: continues from the paused time.</summary>
    private void OnResume(object? sender, RoutedEventArgs e)
    {
        _timer.Resume();
        UpdateDisplay();
    }

    /// <summary>Reset button: returns the time to 00:00:00.</summary>
    private void OnReset(object? sender, RoutedEventArgs e)
    {
        _timer.Reset();
        UpdateDisplay();
    }

    /// <summary>Stop button: stops and shows the last recorded time.</summary>
    private void OnStop(object? sender, RoutedEventArgs e)
    {
        _timer.Stop();
        UpdateDisplay();
    }
}
