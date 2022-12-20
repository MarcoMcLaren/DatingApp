using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AppUser
    {
        [Key] //uses using System.ComponentModel.DataAnnotations;
        public int Id { get; set; } //a convention used by EF
        public string UserName { get; set; }
    }
}