using System;

namespace BTL_LapTrinhWeb.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Author { get; set; }
        public bool CanEdit { get; set; }
    }
}