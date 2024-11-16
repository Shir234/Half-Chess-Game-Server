using SERVER.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERVER.Models
{
    public class TblPlayers
    {

        [Required]
        [Range(1, 1000, ErrorMessage ="Id must be between 1 to 1000")]
        [UniqueID]
        public int Id { get; set; }
        
        [Required]
        [StringLength(21, MinimumLength = 2, ErrorMessage = "Name must be atleast 2 characters")]
        public string Name { get; set; }
        
        [Required]
        //[RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must be exactly 10 digits and start with 0")]
        public int Phone { get; set; }

        [Required]
        [StringLength(21)]
        public string Country { get; set; }

        /// NOT SURE - NAVIGATION PROPERTY  
        public ICollection<TblGames> Games { get; set; }
    }
}
