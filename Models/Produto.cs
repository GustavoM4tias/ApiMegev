using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace megev.Models
{
    public class Produto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Referencia { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]
        [StringLength(50)]
        public string Categoria { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public string Image { get; set; }

        // Chave estrangeira para o usuário
        [Required]
        public int UsuarioId { get; set; }

        // Navegação para o usuário
        public Usuario Usuario { get; set; }
    }
}
