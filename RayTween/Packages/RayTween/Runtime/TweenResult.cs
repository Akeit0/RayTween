using System;

namespace RayTween
{

    public struct TweenResult
    {
        public TweenResultType ResultType;
        public Exception Error;
        public bool IsCompleted => ResultType == TweenResultType.Completed;

        public static TweenResult Completed => new TweenResult() { ResultType = TweenResultType.Completed };
        public static TweenResult Canceled => new TweenResult() { ResultType = TweenResultType.Canceled };
    }
    public enum TweenResultType
    {
        Completed,
        Canceled,
        CancelWithError
    }
}