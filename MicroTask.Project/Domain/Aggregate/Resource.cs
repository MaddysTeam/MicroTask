using Chloe.Entity;
using Common.DomainSeed;

namespace Domain
{

    [Table("Resource")]
    public class Resource:Entity
    {
        public string ProjectID { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }

}
