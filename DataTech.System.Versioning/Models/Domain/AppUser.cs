using System;

namespace DataTech.System.Versioning.Models.Domain
{
    public class AppUser : BaseEntity<Guid>
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
