namespace SBM_System_Backend.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public eRole Role { get; set; }= eRole.Customer;
        public DateTime BookingDate { get; set; }
        public bool IsAdmin { get; set; } = false;
    }

    public enum eRole
    {
        Admin = 5,
        Customer = 10
    }
}
