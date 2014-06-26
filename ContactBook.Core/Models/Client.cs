using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactBook.Core.Models
{
    public class Client
    {
        [Key]
        public int id { set; get; }

        [Required(ErrorMessage = "*")]
        [MaxLength(30)]
        [DisplayName("Last Name")]
        public string LastName { set; get; }

        [Required(ErrorMessage = "*")]
        [MaxLength(30)]
        [DisplayName("First Name")]
        public string FirstName { set; get; }

        [Required(ErrorMessage = "*")]
        [MaxLength(40)]
        [DisplayName("Address")]
        public string Address { set; get; }

        [MaxLength(30)]
        [DisplayName("Contact #")]
        public string ContactNo { set; get; }

        [Required(ErrorMessage = "*")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { set; get; }

        [DisplayName("Country")]
        public int CountryId { get; set; }

        [DisplayName("Remarks")]
        [StringLength(300)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
