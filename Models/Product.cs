using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caractecres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caractecres")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve conter entre 3 e 60 caractecres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria invalida")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}