using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWithoutDrivers.Models.Tables
{
    [Table(Name = "Users")]
    public class UserTable
    {
        [Column(Name = "Id", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "Username")]
        public string Username { get; set; }
        [Column(Name = "Forename")]
        public string Forename { get; set; }
        [Column(Name = "Surname")]
        public string Surname { get; set; }
        [Column(Name = "Active")]
        public bool Active { get; set; }
    }
}
