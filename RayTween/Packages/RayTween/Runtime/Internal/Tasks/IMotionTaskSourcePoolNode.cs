namespace RayTween.Internal
{
    internal interface ITweenTaskSourcePoolNode<T> where T : class
    {
        ref T NextNode { get; }
    }
}