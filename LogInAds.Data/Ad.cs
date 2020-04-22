using System;

namespace LogInAds.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
