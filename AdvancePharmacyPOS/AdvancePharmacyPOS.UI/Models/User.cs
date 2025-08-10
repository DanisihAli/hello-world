namespace AdvancePharmacyPOS.Models
{
    public enum UserRole
    {
        Admin = 1,
        Cashier = 2
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
    }
}