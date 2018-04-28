using Chloe.Entity;
using Common.DomainSeed;

namespace Domain
{

    [Table("projects")]
    public class Project:IAggregateRoot
    {
        [Column("Id", IsPrimaryKey = true)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string OwnerId { get; set; }
        public string ManagerId { get; set; }
    }

}
