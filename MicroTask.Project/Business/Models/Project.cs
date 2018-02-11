using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public class Project
    {
       public Guid Id { get; set; }
       public string Name { get; set; }
       public string Code { get; set; }
       public string InnerCode { get; set; }
    }

}
