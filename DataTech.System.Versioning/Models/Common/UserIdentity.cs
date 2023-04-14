using System;
using System.Collections.Generic;
using System.Text;

namespace DataTech.System.Versioning.Models.Common
{
    public class UserIdentity
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Culture { get; set; }
    }
}
