using System;

namespace DataTech.System.Versioning.Models.Domain
{
    public class BaseEntity<T> : IEntity
    {
        public T Id { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public bool Deleted { get; set; }
    }
}
