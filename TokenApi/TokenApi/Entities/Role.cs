using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenApi.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UnitId { get; set; }
        public int RoleId { get; set; }
    }
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Unit 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int? UnitTypeId { get; set; }
        public string Code { get; set; }

    }
}
