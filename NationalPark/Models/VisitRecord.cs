using System;

namespace NationalPark.Models
{
    public class VisitRecord
    {
        public int Id { get; set; }
        public int NationalParkId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Comments { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5 stars
    }
}