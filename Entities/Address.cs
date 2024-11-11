using System.ComponentModel.DataAnnotations;

namespace MVCTask.Entities {
    public class Address {

        public int AddressId { get; set; }
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }
        [Required]
        [Display(Name = "StreetNumber")]
        public int StreetNumber { get; set; }
        [Display(Name = "Plant")]
        public int Plant { get; set; }
        [Display(Name = "ApNumber")]
        [Required]
        public int ApartmentNumber { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Region")]
        public string Region { get; set; }  
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "ACommunity")]
        public string AutonomousCommunity { get; set; }
        [Required]
        [Display(Name = "PostalCode")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "InvalidPostalCode")]
        [StringLength(5, MinimumLength = 5)]
        public int PostalCode { get; set; }
    }
}