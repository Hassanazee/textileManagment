using System.ComponentModel.DataAnnotations.Schema;

namespace textileManagment.Entities.Base
{
    public class Address :GeneralBase
    {
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        [Column("Address")]
        public string? Address1 { get; set; }

        // For parking and building instructions
        public string? Instructions { get; set; }
        public string? UnitNumber { get; set; }
    }
}
