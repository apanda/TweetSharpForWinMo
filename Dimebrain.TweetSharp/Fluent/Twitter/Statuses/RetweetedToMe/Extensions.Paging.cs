#region License

// TweetSharp
// Copyright (c) 2010 Daniel Crenna and Jason Diller
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

namespace TweetSharp.Fluent
{
    public static partial class Extensions
    {
        public static ITwitterRetweetedToMe Since(this ITwitterRetweetedToMe instance, int id)
        {
            instance.Root.Parameters.SinceId = id;
            return instance;
        }

        public static ITwitterRetweetedToMe Since(this ITwitterRetweetedToMe instance, long id)
        {
            instance.Root.Parameters.SinceId = id;
            return instance;
        }

        public static ITwitterRetweetedToMe Before(this ITwitterRetweetedToMe instance, int id)
        {
            instance.Root.Parameters.MaxId = id;
            return instance;
        }

        public static ITwitterRetweetedToMe Before(this ITwitterRetweetedToMe instance, long id)
        {
            instance.Root.Parameters.MaxId = id;
            return instance;
        }

        public static ITwitterRetweetedToMe Skip(this ITwitterRetweetedToMe instance, int page)
        {
            instance.Root.Parameters.Page = page;
            return instance;
        }

        public static ITwitterRetweetedToMe Take(this ITwitterRetweetedToMe instance, int count)
        {
            instance.Root.Parameters.Count = count;
            return instance;
        }
    }
}