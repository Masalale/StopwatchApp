using Stopwatch.App;

namespace Stopwatch.Tests;

public class StopwatchTimerTests
{
    // Returns a fresh, already-running stopwatch.
    private static StopwatchTimer Running()
    {
        var sw = new StopwatchTimer();
        sw.Start();
        return sw;
    }

    [Fact]
    public void New_StartsAtZeroAndNotRunning()
    {
        var sw = new StopwatchTimer();
        Assert.Equal(TimeSpan.Zero, sw.Elapsed);
        Assert.False(sw.IsRunning);
        Assert.Equal("00:00:00", sw.Formatted);
    }

    [Fact]
    public void Start_ZeroesAndRuns()
    {
        var sw = new StopwatchTimer();
        sw.Start();
        sw.Tick();
        sw.Start(); // restarting clears the previous time
        Assert.True(sw.IsRunning);
        Assert.Equal(TimeSpan.Zero, sw.Elapsed);
    }

    [Fact]
    public void Tick_IncrementsWhileRunning()
    {
        var sw = Running();
        sw.Tick();
        sw.Tick();
        Assert.Equal(TimeSpan.FromSeconds(2), sw.Elapsed);
    }

    [Fact]
    public void Tick_DoesNothingWhenNotStarted()
    {
        var sw = new StopwatchTimer();
        sw.Tick();
        Assert.Equal(TimeSpan.Zero, sw.Elapsed);
    }

    [Fact]
    public void Pause_StopsIncrementingButKeepsTime()
    {
        var sw = Running();
        sw.Tick();
        sw.Tick();
        sw.Pause();
        sw.Tick(); // ignored while paused
        Assert.False(sw.IsRunning);
        Assert.Equal(TimeSpan.FromSeconds(2), sw.Elapsed);
    }

    [Fact]
    public void Resume_ContinuesFromPausedTime()
    {
        var sw = Running();
        sw.Tick();
        sw.Pause();
        sw.Resume();
        sw.Tick();
        Assert.True(sw.IsRunning);
        Assert.Equal(TimeSpan.FromSeconds(2), sw.Elapsed);
    }

    [Fact]
    public void Reset_SetsBackToZero()
    {
        var sw = Running();
        sw.Tick();
        sw.Tick();
        sw.Reset();
        Assert.Equal(TimeSpan.Zero, sw.Elapsed);
        Assert.Equal("00:00:00", sw.Formatted);
    }

    [Fact]
    public void Stop_KeepsLastTimeAndStopsRunning()
    {
        var sw = Running();
        sw.Tick();
        sw.Tick();
        sw.Tick();
        sw.Stop();
        sw.Tick(); // ignored once stopped
        Assert.False(sw.IsRunning);
        Assert.Equal(TimeSpan.FromSeconds(3), sw.Elapsed);
    }

    [Theory]
    [InlineData(0, "00:00:00")]
    [InlineData(5, "00:00:05")]
    [InlineData(65, "00:01:05")]
    [InlineData(3661, "01:01:01")]   // seconds -> minutes -> hours
    [InlineData(90000, "25:00:00")]  // total hours past 24h
    public void Formatted_RollsSecondsToMinutesToHours(int seconds, string expected)
    {
        var sw = new StopwatchTimer();
        sw.Start();
        for (int i = 0; i < seconds; i++)
            sw.Tick();
        Assert.Equal(expected, sw.Formatted);
    }
}
