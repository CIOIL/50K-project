using System;
using System.Collections.Generic;
using System.Text;

namespace TokenApi.Common.DTO
{
    public class TokenBase
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public string TZ { get; set; }
        public string EMail { get; set; }
    }
}
