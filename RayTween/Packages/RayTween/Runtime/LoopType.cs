namespace RayTween
{
    /// <summary>
    /// Specifies the behavior when repeating the animation with `WithLoops`.
    /// </summary>
    public enum LoopType : byte
    {
        /// <summary>
        /// Repeat from beginning.
        /// </summary>
        Restart,
        /// <summary>
        /// Cycles back and forth between the end and start values (e.g. InSine[0->1] -> OutSine[1->0]).
        /// </summary>
        Yoyo,
        /// <summary>
        /// Increase the value each time the repeats.
        /// </summary>
        Incremental,
        /// <summary>
        /// Cycles back and forth between the end and start values  (e.g. InSine[0->1] -> InSine[1->0]).
        /// </summary>
        Flip
    }
}