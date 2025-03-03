namespace SBM_System_Backend.Entity
{
    public class Service
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime Duration { get; set; }

        public bool IsApproved { get; set; } = false;
        public string? Status { get; internal set; }
    }
}

