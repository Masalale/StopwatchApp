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

        // _ticker fires once per second; each tick advances the timer by one second
        // and refreshes the display — this is the core timing loop of the application
        _ticker = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _ticker.Tick += (_, _) =>
        {
            _timer.Tick();
            UpdateDisplay();
        };

        // Set initial display without starting the ticker
        UpdateDisplay();
    }

    /// <summary>
    /// Refreshes the time label, state indicator, message area, and button opacity
    /// to reflect the current <see cref="StopwatchTimer.State"/>.
    /// </summary>
    private void UpdateDisplay()
    {
        // Update the large time readout
        TimeDisplay.Text = _timer.Formatted;

        // Choose a state label and foreground colour based on the current state
        (string label, IBrush brush) = _timer.State switch
        {
            TimerState.Running => ("[ RUNNING ]", (IBrush)Brushes.White),
            TimerState.Paused  => ("[ PAUSED  ]", new SolidColorBrush(Color.FromRgb(160, 160, 160))),
            TimerState.Stopped => ("[ STOPPED ]", new SolidColorBrush(Color.FromRgb(110, 110, 110))),
            _                  => ("[ IDLE    ]", new SolidColorBrush(Color.FromRgb(60,  60,  60)))
        };
        StateDisplay.Text       = label;
        StateDisplay.Foreground = brush;

        // Loop through each button and set its opacity:
        // active buttons (valid for the current state) are fully visible;
        // inactive buttons are dimmed to 20% to signal they cannot be used.
        var buttonStates = new (Button Btn, bool Active)[]
        {
            (BtnStart,  _timer.State is TimerState.Idle or TimerState.Stopped),
            (BtnPause,  _timer.State == TimerState.Running),
            (BtnResume, _timer.State == TimerState.Paused),
            (BtnReset,  true),
            (BtnStop,   _timer.State is TimerState.Running or TimerState.Paused)
        };

        foreach (var (btn, active) in buttonStates)
            btn.Opacity = active ? 1.0 : 0.2;
    }

    /// <summary>Displays a status message in the message area.</summary>
    /// <param name="text">The message to show.</param>
    private void ShowMessage(string text) => MessageDisplay.Text = text;

    /// <summary>
    /// Start button: begins timing from 00:00:00.
    /// Only valid when the stopwatch is idle or has been stopped.
    /// </summary>
    /// <param name="sender">The Start button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnStart(object? sender, RoutedEventArgs e)
    {
        if (_timer.State == TimerState.Idle || _timer.State == TimerState.Stopped)
        {
            // Start the engine and the one-second ticker
            _timer.Start();
            _ticker.Start();
            ShowMessage("> STOPWATCH STARTED");
            UpdateDisplay();
        }
        else
        {
            ShowMessage("> ALREADY RUNNING OR PAUSED");
        }
    }

    /// <summary>
    /// Pause button: freezes the timer and shows the time at which it was paused.
    /// Only valid when the stopwatch is running.
    /// </summary>
    /// <param name="sender">The Pause button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnPause(object? sender, RoutedEventArgs e)
    {
        if (_timer.State == TimerState.Running)
        {
            // Stop the ticker first so no extra second is added after pausing
            _timer.Pause();
            _ticker.Stop();
            UpdateDisplay();
            ShowMessage($"> PAUSED AT {_timer.Formatted}");
        }
        else
        {
            ShowMessage("> CANNOT PAUSE — STOPWATCH IS NOT RUNNING");
        }
    }

    /// <summary>
    /// Resume button: continues counting from the last paused time.
    /// Only valid when the stopwatch is paused.
    /// </summary>
    /// <param name="sender">The Resume button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnResume(object? sender, RoutedEventArgs e)
    {
        if (_timer.State == TimerState.Paused)
        {
            _timer.Resume();
            _ticker.Start();
            ShowMessage("> RESUMED");
            UpdateDisplay();
        }
        else
        {
            ShowMessage("> CANNOT RESUME — STOPWATCH IS NOT PAUSED");
        }
    }

    /// <summary>
    /// Reset button: returns the stopwatch to 00:00:00.
    /// Available from any state.
    /// </summary>
    /// <param name="sender">The Reset button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnReset(object? sender, RoutedEventArgs e)
    {
        // Reset is always allowed regardless of the current state
        _timer.Reset();
        _ticker.Stop();
        ShowMessage("> RESET TO 00:00:00");
        UpdateDisplay();
    }

    /// <summary>
    /// Stop button: halts the stopwatch and preserves the final recorded time.
    /// Only valid when the stopwatch is running or paused.
    /// </summary>
    /// <param name="sender">The Stop button that raised the event.</param>
    /// <param name="e">Event data (unused).</param>
    private void OnStop(object? sender, RoutedEventArgs e)
    {
        if (_timer.State == TimerState.Running || _timer.State == TimerState.Paused)
        {
            _timer.Stop();
            _ticker.Stop();
            UpdateDisplay();
            ShowMessage($"> STOPPED — FINAL TIME: {_timer.Formatted}");
        }
        else
        {
            ShowMessage("> STOPWATCH IS NOT ACTIVE");
        }
    }
}
