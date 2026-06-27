using System;

namespace Stopwatch.App;

/// <summary>Represents the operational state of the stopwatch.</summary>
public enum TimerState
{
    /// <summary>Initial state or after a reset; elapsed time is zero.</summary>
    Idle,

    /// <summary>The stopwatch is actively counting up.</summary>
    Running,

    /// <summary>Counting is paused; elapsed time is preserved.</summary>
    Paused,

    /// <summary>Counting has been stopped; the final time is preserved.</summary>
    Stopped
}

/// <summary>
/// Manages stopwatch state and elapsed time.
/// The UI is responsible for calling <see cref="Tick"/> once per second.
/// </summary>
public class StopwatchTimer
{
    /// <summary>Gets the current operational state of the stopwatch.</summary>
    public TimerState State { get; private set; } = TimerState.Idle;

    /// <summary>Gets the total time accumulated since the last <see cref="Start"/>.</summary>
    public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;

    /// <summary>Gets whether the stopwatch is actively counting.</summary>
    public bool IsRunning => State == TimerState.Running;

    /// <summary>
    /// Gets the elapsed time formatted as <c>hh:mm:ss</c>.
    /// TotalHours is cast to int so hours can exceed 23 (e.g. 25:00:00).
    /// </summary>
    /// <value>A string in the format <c>00:00:00</c>, supporting hours past 24.</value>
    public string Formatted =>
        $"{(int)Elapsed.TotalHours:00}:{Elapsed.Minutes:00}:{Elapsed.Seconds:00}";

    /// <summary>Starts the stopwatch from 00:00:00.</summary>
    public void Start()
    {
        // Always reset to zero so a fresh start clears any previous time
        Elapsed = TimeSpan.Zero;
        State   = TimerState.Running;
    }

    /// <summary>
    /// Pauses the stopwatch, preserving the current elapsed time.
    /// No-op if not currently running.
    /// </summary>
    public void Pause()
    {
        if (State == TimerState.Running)
        {
            State = TimerState.Paused;
        }
        // else: already paused, stopped, or idle — nothing to do
    }

    /// <summary>
    /// Resumes counting from the paused time.
    /// No-op if not currently paused.
    /// </summary>
    public void Resume()
    {
        if (State == TimerState.Paused)
        {
            State = TimerState.Running;
        }
        // else: not paused — nothing to do
    }

    /// <summary>
    /// Resets elapsed time to 00:00:00 and returns to <see cref="TimerState.Idle"/>.
    /// </summary>
    public void Reset()
    {
        Elapsed = TimeSpan.Zero;
        State   = TimerState.Idle;  // also stops counting if the timer was running
    }

    /// <summary>
    /// Stops the stopwatch, preserving the last recorded time.
    /// No-op if idle or already stopped.
    /// </summary>
    public void Stop()
    {
        if (State == TimerState.Running || State == TimerState.Paused)
        {
            State = TimerState.Stopped;
        }
        // else: already idle or stopped — preserve the existing time unchanged
    }

    /// <summary>
    /// Advances the elapsed time by one second if the stopwatch is running.
    /// Called once per second by the UI dispatcher timer.
    /// Seconds automatically roll over into minutes, and minutes into hours.
    /// </summary>
    public void Tick()
    {
        if (IsRunning)
        {
            // Add one second; TimeSpan handles the rollover from seconds → minutes → hours
            Elapsed += TimeSpan.FromSeconds(1);
        }
    }
}
