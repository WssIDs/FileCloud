namespace FileCloud.Shared.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool IsLocked { get; set; }

        public IList<string> Roles { get; set; }
    }
}