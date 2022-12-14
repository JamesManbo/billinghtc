using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikForNet.Api
{
    internal class ApiDoneSentence : ApiSentence, ITikDoneSentence
    {
        public ApiDoneSentence(IEnumerable<string> words) 
            : base(words)
        {
        }

        public string GetResponseWord()
        {
            return GetWordValue(TikSpecialProperties.Ret);
        }

        public string GetResponseWordOrDefault(string defaultValue)
        {
            return GetWordValueOrDefault(TikSpecialProperties.Ret, defaultValue);
        }
    }
}
