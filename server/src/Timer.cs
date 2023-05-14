using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Timer
{
    public DateTime start;
    public TimeSpan duration;

    public DateTime End => start.Add(duration);

    public TimeSpan Remaining => End - DateTime.Now;
    public bool IsComplete => Remaining <= TimeSpan.Zero;

    public Timer(TimeSpan duration)
    {
        this.duration = duration;
        start = DateTime.Now;
    }

    /// <summary>
    /// Gets the time remaining and formats it nicely
    /// </summary>
    /// <returns>The time remaining formatted as {hrs}h{mins}m{secs}s</returns>
    public string FormattedTimeRemaining()
    {
        string text = "";

        if (Remaining > TimeSpan.Zero)
        {
            if (Remaining.Hours > 0) text += $"{Remaining.Hours}h";
            if (Remaining.Minutes > 0) text += $"{Remaining.Minutes}m";
            if (Remaining.Seconds > 0) text += $"{Remaining.Seconds}s";
        }
        else text = "0s";

        return text;
    }
}