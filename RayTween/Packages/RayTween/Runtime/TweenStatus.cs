namespace RayTween
{
    /// <summary>
    /// Tween status.
    /// </summary>
    public enum TweenStatus
    {
        None = 0,
        Idle=1,
        Scheduled = 2,
        Delayed = 3,
        Playing = 4,
        Completed = 5,
        Canceled = 6,
    }
}