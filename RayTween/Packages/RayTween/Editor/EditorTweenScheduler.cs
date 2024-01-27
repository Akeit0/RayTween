namespace RayTween.Editor
{
    /// <summary>
    /// Schedulers available in Editor.
    /// </summary>
    public static class EditorTweenScheduler
    {
        /// <summary>
        /// Scheduler that updates tween at EditorApplication.update.
        /// </summary>
        public static readonly ITweenScheduler Update = new EditorUpdateTweenScheduler();
    }
}