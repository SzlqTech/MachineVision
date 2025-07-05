using FreeSql.DataAnnotations;
using System;

namespace MachineVision.Core.Entity
{
    public class BaseEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Code { get; set; }
    }
}
