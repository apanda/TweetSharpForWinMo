using System;


namespace Dimebrain.TweetSharp.Fluent.Twitter.Configuration
{
    [Flags]
    public enum RetryPolicy
    {
        NoRetries = 0,
        RetryOnFailWhale = 1,
        RetryOnTwitterError = 2, 
        RetryOnTimeout = 4, 
        RetryOnFailWhaleOrTimeout = RetryOnFailWhale | RetryOnTimeout
    }
}
