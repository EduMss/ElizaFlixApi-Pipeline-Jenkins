namespace ElizaFlixAPI.DTO
{
    public class AddSerie
    {
        public string nome { get; set; }
        public string sigla { get; set; }
        public string[] dir_Filmes { get; set; }
        public string dir_Thumb_Wid { get; set; }
        public string dir_Thumb_Heid { get; set; }
        public int quantidade_eps { get; set; }

    }
}
