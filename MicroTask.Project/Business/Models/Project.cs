using Chloe.Entity;

namespace Business
{

    [Table("projects")]
    public class Project
    {
        [Column("Id", IsPrimaryKey = true)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string OwnerId { get; set; }
        public string ManagerId { get; set; }
    }

}
