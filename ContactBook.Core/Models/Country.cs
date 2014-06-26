using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Core.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(40)]
        [DisplayName("Country")]
        public string CountryName { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
