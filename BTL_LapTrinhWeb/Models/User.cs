using System;

namespace BTL_LapTrinhWeb.Models
{
    public class User
    {
        public int UserID { get; set; }

        public int Id
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string QueQuan { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}