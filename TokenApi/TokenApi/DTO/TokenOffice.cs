using System;
using System.Collections.Generic;
using System.Text;

namespace TokenApi.Common.DTO
{
    
        public class TokenOffice : TokenBase
        {
            public string ParentUnitName { get; set; }
            public int ParentUnitId { get; set; }
            public List<Permission> Permissions { get; set; }
        }
   
}
