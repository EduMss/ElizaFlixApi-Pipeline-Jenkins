using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElizaFlixAPI.DTO
{
    [Table("filmes")]
    public class Filme
    {
        public string nome { get; set; }
        [Key]
        public string sigla { get; set; }
        public string[] dir_Filmes { get; set; }
        public string dir_Thumb_Wid { get; set; }
        public string dir_Thumb_Heid { get; set; }
        public int quantidade_eps { get; set; }
        public Tipo tipo { get; set; } //e filme ou serie

    }
}
