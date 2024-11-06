using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MVCTask.Entities {
    public class Person {
        public int PersonId {get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
        public string Name {get; set;}
     
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name must be less than 50 characters")]
        public string LastName {get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string PhoneNumber  {get; set;}

        public int Id {get; set;}
        public IdentityUser User {get; set;}

        public int AddressId {get; set;}
        public Address Address {get; set;}

    }
}
