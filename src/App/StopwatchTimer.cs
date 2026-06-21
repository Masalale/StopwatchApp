using System;

namespace Stopwatch.App;

public class StopwatchTimer
{
    public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;

    public bool IsRunning { get; private set; }

    public string Formatted =>
        $"{(int)Elapsed.TotalHours:00}:{Elapsed.Minutes:00}:{Elapsed.Seconds:00}";

    /// <summary>Starts the stopwatch from 00:00:00.</summary>
    public void Start()
    {
        Elapsed = TimeSpan.Zero;
        IsRunning = true;
    }

    /// <summary>Pauses the stopwatch, keeping the current time.</summary>
    public void Pause()
    {
        IsRunning = false;
    }

    /// <summary>Resumes counting after a pause.</summary>
    public void Resume()
    {
        IsRunning = true;
    }

    /// <summary>Resets the time back to 00:00:00.</summary>
    public void Reset()
    {
        Elapsed = TimeSpan.Zero;
    }

    /// <summary>Stops the stopwatch, keeping the last recorded time.</summary>
    public void Stop()
    {
        IsRunning = false;
    }

    /// <summary>Adds one second while running; called once per second by the UI.</summary>
    public void Tick()
    {
        if (IsRunning)
            Elapsed += TimeSpan.FromSeconds(1);
    }
}
