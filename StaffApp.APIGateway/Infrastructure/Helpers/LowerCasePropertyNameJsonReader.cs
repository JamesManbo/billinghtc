using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.Helpers
{
    public class LowerCasePropertyNameJsonReader : JsonTextReader
    {
        public LowerCasePropertyNameJsonReader(TextReader textReader)
        : base(textReader)
        {
        }
        public override object Value
        {
            get
            {
                if (TokenType == JsonToken.PropertyName )
                {
                    string ke = ((string)base.Value);
                    return char.ToLower(ke[0]) + ke.Substring(1);
                }
                    
                return base.Value;
            }
        }
    }
}
