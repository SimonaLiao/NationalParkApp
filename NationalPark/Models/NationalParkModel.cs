using System;
using System.Collections.Generic;

namespace NationalPark.Models
{
    public class NationalParkModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime EstablishedDate { get; set; }
        public int AreaInAcres { get; set; }
        public bool IsVisited { get; set; }
        public List<VisitRecord> VisitRecords { get; set; } = new List<VisitRecord>();
        public string Region { get; set; } = string.Empty;
        public DateTime VisitedDate { get; set; } // empty when not visited
    }
}