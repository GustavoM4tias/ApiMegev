namespace megev.Models
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Referencia { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public decimal? Preco { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public UsuarioDto Usuario { get; set; }
    }

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
    }
}
