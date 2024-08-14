using ElizaFlixAPI.DTO;

namespace ElizaFlixAPI.Interfaces
{
    public interface IFilmeRepository
    {
        void Add(Filme filme);

        List<Filme> GetAll();

        List<Filme> GetFilmes();

        List<Filme> GetSeries();

        bool FindSigla(string sigla); //Se achar um sigla no banco, ela retorna verdadeiro

        bool AtualizarDirSerie(UpdateDirSerie updateDirSerie);
    }
}
