using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    [Table("tasks")]
    public class Task
    {
        [Column("Id", IsPrimaryKey = true)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Owner { get; set; }
    }

}
