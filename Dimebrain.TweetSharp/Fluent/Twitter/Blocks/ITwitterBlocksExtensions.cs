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

using TweetSharp.Core.Attributes;
using TweetSharp.Model;

namespace TweetSharp.Fluent
{
    public static class ITwitterBlocksExtensions
    {
        [RequiresAuthentication]
        public static ITwitterBlocksCreate Block(this ITwitterBlocks instance, int id)
        {
            instance.Root.Parameters.Action = "create";
            instance.Root.Parameters.Id = id;
            return new TwitterBlocksCreate(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksCreate Block(this ITwitterBlocks instance, long id)
        {
            instance.Root.Parameters.Action = "create";
            instance.Root.Parameters.Id = id;
            return new TwitterBlocksCreate(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksCreate Block(this ITwitterBlocks instance, string screenName)
        {
            instance.Root.Parameters.Action = "create";
            instance.Root.Parameters.ScreenName = screenName;
            return new TwitterBlocksCreate(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksCreate Block(this ITwitterBlocks instance, TwitterUser user)
        {
            instance.Root.Parameters.Action = "create";
            instance.Root.Parameters.ScreenName = user.ScreenName;
            return new TwitterBlocksCreate(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksDestroy Unblock(this ITwitterBlocks instance, int id)
        {
            instance.Root.Parameters.Action = "destroy";
            instance.Root.Parameters.Id = id;
            return new TwitterBlocksDestroy(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksDestroy Unblock(this ITwitterBlocks instance, long id)
        {
            instance.Root.Parameters.Action = "destroy";
            instance.Root.Parameters.Id = id;
            return new TwitterBlocksDestroy(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksDestroy Unblock(this ITwitterBlocks instance, string screenName)
        {
            instance.Root.Parameters.Action = "destroy";
            instance.Root.Parameters.ScreenName = screenName;
            return new TwitterBlocksDestroy(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksDestroy Unblock(this ITwitterBlocks instance, TwitterUser user)
        {
            instance.Root.Parameters.Action = "destroy";
            instance.Root.Parameters.ScreenName = user.ScreenName;
            return new TwitterBlocksDestroy(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksDestroy Exists(this ITwitterBlocks instance, long id)
        {
            instance.Root.Parameters.Action = "exists";
            instance.Root.Parameters.Id = id;
            return new TwitterBlocksDestroy(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksDestroy Exists(this ITwitterBlocks instance, int id)
        {
            instance.Root.Parameters.Action = "exists";
            instance.Root.Parameters.Id = id;
            return new TwitterBlocksDestroy(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksExists Exists(this ITwitterBlocks instance, string screenName)
        {
            instance.Root.Parameters.Action = "exists";
            instance.Root.Parameters.ScreenName = screenName;
            return new TwitterBlocksExists(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksExists Exists(this ITwitterBlocks instance, TwitterUser user)
        {
            instance.Root.Parameters.Action = "exists";
            instance.Root.Parameters.ScreenName = user.ScreenName;
            return new TwitterBlocksExists(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksList ListUsers(this ITwitterBlocks instance)
        {
            instance.Root.Parameters.Action = "blocking";
            return new TwitterBlocksList(instance.Root);
        }

        [RequiresAuthentication]
        public static ITwitterBlocksListIds ListIds(this ITwitterBlocks instance)
        {
            instance.Root.Parameters.Action = "blocking/ids";
            return new TwitterBlocksListIds(instance.Root);
        }
    }
}