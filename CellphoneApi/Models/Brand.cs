using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CellphoneApi.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int BrandId { get; set; }

        [MaxLength(50, ErrorMessage ="La longitud debe ser de 50")]
        [Required] 
        public string Name { get; set; }
    }
}
