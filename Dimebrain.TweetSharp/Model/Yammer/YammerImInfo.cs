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

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TweetSharp.Model
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class YammerImInfo : PropertyChangedBase, IYammerModel
    {
        private string _provider;
        private string _userName;

        /// <summary>
        /// User's name or ID in the IM system
        /// </summary>
        [JsonProperty("username")]
#if !Smartphone
        [DataMember]
#endif
        public virtual string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value)
                {
                    return;
                }
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        /// <summary>
        /// the IM network
        /// </summary>
        [JsonProperty("provider")]
#if !Smartphone
        [DataMember]
#endif
        public virtual string Provider
        {
            get { return _provider; }
            set
            {
                if (_provider == value)
                {
                    return;
                }
                _provider = value;
                OnPropertyChanged("Provider");
            }
        }
    }
}