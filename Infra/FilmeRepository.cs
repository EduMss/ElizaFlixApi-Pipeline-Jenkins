using ElizaFlixAPI.Interfaces;
using ElizaFlixAPI.DTO;

namespace ElizaFlixAPI.Infra
{
    public class FilmeRepository : IFilmeRepository
    {

        private readonly ConnectionContext _context = new ConnectionContext();


        public void Add(Filme filme)
        {
            _context.filmes.Add(filme);
            _context.SaveChanges();
        }

        public List<Filme> GetAll()
        {
            return _context.filmes.ToList();
        }

        public List<Filme> GetFilmes()
        {
            List<Filme> filmes = _context.filmes.ToList();

            filmes = filmes.Where(filme => filme.tipo == Tipo.filme).ToList();

            return filmes;
        }


        public List<Filme> GetSeries()
        {
            List<Filme> filmes = _context.filmes.ToList();

            filmes = filmes.Where(filme => filme.tipo == Tipo.serie).ToList();

            return filmes;
        }

        public bool FindSigla(string sigla)
        {
            List<Filme> filmes = _context.filmes.ToList();
            filmes = filmes.Where(filme => filme.sigla == sigla).ToList();

            if(filmes.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool AtualizarDirSerie(UpdateDirSerie updateDirSerie)
        {
            List<Filme> filmes = _context.filmes.ToList();
            Filme filme = filmes.FirstOrDefault(filme => filme.sigla == updateDirSerie.sigla) ?? new Filme();

            if(filme.nome == null)
            {
                return false;
            }

            if(updateDirSerie.dir_Filmes.Length < 1)
            {
                return false;
            }

            filme.dir_Filmes = updateDirSerie.dir_Filmes;

            _context.filmes.Update(filme);
            _context.SaveChanges();

            return true;
        }
    }
}
