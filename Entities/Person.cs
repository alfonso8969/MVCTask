using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MVCTask.Entities {
    public class Person {
        public int PersonId {get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessage = "Error.Required")]
        [StringLength(50, ErrorMessage = "StringLength")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "RegularExpression")]
        [Display(Name = "Name")]
        public string Name {get; set;}
     
        [Required(AllowEmptyStrings = false, ErrorMessage = "Error.Required")]
        [StringLength(50, ErrorMessage = "StringLength")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "RegularExpression")]
        [Display(Name = "LastName")]
        public string LastName {get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessage = "Error.Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        public string Email { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber  {get; set;}

        public int Id {get; set;}
        public IdentityUser User {get; set;}

        public int AddressId {get; set;}
        public Address Address {get; set;}

    }
}
