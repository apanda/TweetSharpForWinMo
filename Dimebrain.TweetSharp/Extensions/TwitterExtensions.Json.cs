using System.Collections.Generic;
using TweetSharp.Model;
using TweetSharp.Model.Twitter;
using TweetSharp.Model.Twitter.Converters;
using Newtonsoft.Json;

namespace TweetSharp.Extensions
{
    partial class TwitterExtensions
    {
        // todo move to core extension
        public static string ToJson(this IModel instance)
        {
            var json = JsonConvert.SerializeObject(instance, 
                new TwitterDateTimeConverter(),
                new TwitterWonkyBooleanConverter());

            return json;
        }

        // todo move to core extension
        public static string ToJson(this IEnumerable<IModel> collection)
        {
            var json = JsonConvert.SerializeObject(collection, 
                new TwitterDateTimeConverter(),
                new TwitterWonkyBooleanConverter());

            return json;
        }

#if !SILVERLIGHT
        public static string ToJson(this TwitterResult result)
        {
            return PreProcessXml(result.Response);
        }
#endif
    }
}
