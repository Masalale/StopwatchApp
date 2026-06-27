using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace Stopwatch.App;

/// <summary>
/// Main application window for the Stopwatch.
/// Delegates all timing logic to <see cref="StopwatchTimer"/>; this class
/// manages the dispatcher ticker and routes button clicks to the timer.
/// </summary>
public partial class MainWindow : Window
{
    private readonly StopwatchTimer _timer = new();
    private readonly DispatcherTimer _ticker;

    /// <summary>Initialises the window and configures the one-second dispatcher ticker.</summary>
    public MainWindow()
    {
        InitializeComponent();

        _ticker = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _ticker.Tick += (_, _) =>
        {
            _timer.Tick();
            UpdateDisplay();
        };

        UpdateDisplay();
    }

    /// <summary>
    /// Refreshes the time label, state indicator, message area, and button opacity
    /// to reflect the current <see cref="StopwatchTimer.State"/>.
    /// </summary>
    private void UpdateDisplay()
    {
        TimeDisplay.Text = _timer.Formatted;

        (string label, IBrush brush) = _timer.State switch
        {
            TimerState.Running => ("[ RUNNING ]", (IBrush)Brushes.White),
            TimerState.Paused  => ("[ PAUSED  ]", new SolidColorBrush(Color.FromRgb(160, 160, 160))),
            TimerState.Stopped => ("[ STOPPED ]", new SolidColorBrush(Color.FromRgb(110, 110, 110))),
            _                  => ("[ IDLE    ]", new SolidColorBrush(Color.FromRgb(60,  60,  60)))
        };
        StateDisplay.Text       = label;
        StateDisplay.Foreground = brush;

        BtnStart.Opacity  = _timer.State is TimerState.Idle or TimerState.Stopped ? 1.0 : 0.2;
        BtnPause.Opacity  = _timer.State == TimerState.Running ? 1.0 : 0.2;
        BtnResume.Opacity = _timer.State == TimerState.Paused  ? 1.0 : 0.2;
        BtnReset.Opacity  = 1.0;
        BtnStop.Opacity   = _timer.State is TimerState.Running or TimerState.Paused ? 1.0 : 0.2;
    }

    /// <summary>
    /// Displays a retro-style status message in the message area.
    /// </summary>
    /// <param name="text">The message to show, prefixed with <c>&gt;</c> by convention.</param>
    private void ShowMessage(string text) => MessageDisplay.Text = text;

    /// <summary>Start button: begins timing from 00:00:00.</summary>
    /// <param name="sender">The Start button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnStart(object? sender, RoutedEventArgs e)
    {
        if (_timer.State == TimerState.Running || _timer.State == TimerState.Paused)
            return;

        _timer.Start();
        ShowMessage("> STOPWATCH STARTED");
        _ticker.Start();
        UpdateDisplay();
    }

    /// <summary>Pause button: pauses and displays the current time.</summary>
    /// <param name="sender">The Pause button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnPause(object? sender, RoutedEventArgs e)
    {
        if (_timer.State != TimerState.Running)
            return;

        _timer.Pause();
        _ticker.Stop();
        UpdateDisplay();
        ShowMessage($"> PAUSED AT {_timer.Formatted}");
    }

    /// <summary>Resume button: continues from the paused time.</summary>
    /// <param name="sender">The Resume button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnResume(object? sender, RoutedEventArgs e)
    {
        if (_timer.State != TimerState.Paused)
            return;

        _timer.Resume();
        ShowMessage("> RESUMED");
        _ticker.Start();
        UpdateDisplay();
    }

    /// <summary>Reset button: returns the time to 00:00:00.</summary>
    /// <param name="sender">The Reset button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnReset(object? sender, RoutedEventArgs e)
    {
        _timer.Reset();
        _ticker.Stop();
        ShowMessage("> RESET TO 00:00:00");
        UpdateDisplay();
    }

    /// <summary>Stop button: stops and shows the final recorded time.</summary>
    /// <param name="sender">The Stop button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnStop(object? sender, RoutedEventArgs e)
    {
        if (_timer.State == TimerState.Idle || _timer.State == TimerState.Stopped)
            return;

        _timer.Stop();
        _ticker.Stop();
        UpdateDisplay();
        ShowMessage($"> STOPPED — FINAL TIME: {_timer.Formatted}");
    }
}
