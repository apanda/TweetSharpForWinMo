using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TweetSharp.Core.Web
{
    public class OAuthCredentials : ICredentials
    {
        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            return null;
        }
    }
}
