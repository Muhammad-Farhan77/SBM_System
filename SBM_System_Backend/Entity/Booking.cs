namespace SBM_System_Backend.Entity
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public bool IsRequest { get; set; } = false;

        //navigation properties
        public User? User { get; set; }
        public Service? Service { get; set; }
    }
    public enum BookingStatus
    {
        Pending = 5,
        Approved = 10,
        Rejected = 15
    }
}

