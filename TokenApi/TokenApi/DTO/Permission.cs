using System;
using System.Collections.Generic;
using System.Text;

namespace TokenApi.Common.DTO
{
   
        public class Permission 
        {
            public int PermissionId { get; set; }

           
            public int UserId { get; set; }

        
            public int UnitId { get; set; }

            
            public int RoleId { get; set; }
        }
    
}
