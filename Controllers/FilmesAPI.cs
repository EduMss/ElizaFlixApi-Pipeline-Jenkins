using Microsoft.AspNetCore.Mvc;
using ElizaFlixAPI.DTO;
using ElizaFlixAPI.Interfaces;

namespace ElizaFlixAPI.Controllers
{
    [ApiController]
    public class FilmesAPI : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFilmeRepository _filmeRepository;

        public FilmesAPI(IConfiguration configuration, IFilmeRepository filmeRepository)
        {
            _configuration = configuration;
            _filmeRepository = filmeRepository ?? throw new ArgumentNullException(nameof(filmeRepository));
        }

        [HttpGet]
        [Route("siglaExiste/{sigla}")]
        public ActionResult<FindSiglaBD> siglaExiste(string sigla) {
            FindSiglaBD findSiglaBD = new FindSiglaBD { existe = false};
            if (_filmeRepository.FindSigla(sigla)) findSiglaBD.existe = true;
            return findSiglaBD;
        }

        [HttpPost]
        [Route("AddFilme")]
        public ActionResult AddFilme([FromBody] AddFilme addFilme)
        {
            if (_filmeRepository.FindSigla(addFilme.sigla)) return Conflict("A sigla utilizada já existe! Favor revisar se o filme já existe.");
            Filme filme = new Filme
            {
                nome = addFilme.nome,
                sigla = addFilme.sigla,
                dir_Filmes = addFilme.dir_Filmes,
                dir_Thumb_Wid = addFilme.dir_Thumb_Wid,
                dir_Thumb_Heid = addFilme.dir_Thumb_Heid,
                quantidade_eps = -1,
                tipo = Tipo.filme
            };

            _filmeRepository.Add(filme);

            return Ok("Filme Adicionado com sucesso!");
        }

        [HttpPost]
        [Route("AddSerie")]
        public ActionResult AddSerie([FromBody] AddSerie addSerie)
        {
            if (_filmeRepository.FindSigla(addSerie.sigla)) return Conflict("A sigla utilizada já existe! Favor revisar se a serie já existe.");

            Filme filme = new Filme
            {
                nome = addSerie.nome,
                sigla = addSerie.sigla,
                dir_Filmes = addSerie.dir_Filmes,
                dir_Thumb_Wid = addSerie.dir_Thumb_Wid,
                dir_Thumb_Heid = addSerie.dir_Thumb_Heid,
                quantidade_eps = addSerie.quantidade_eps,
                tipo = Tipo.serie
            };

            _filmeRepository.Add(filme);

            return Ok("Serie Adicionado com sucesso!");
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<Filmes> GetAll()
        {
            Filmes filmes = new Filmes { filmes = _filmeRepository.GetAll().ToArray() };
            return filmes;
        }

        [HttpGet]
        [Route("GetAllFilme")]
        public ActionResult<Filmes> GetFilmes()
        {
            Filmes filmes = new Filmes { filmes = _filmeRepository.GetFilmes().ToArray() };

            return filmes;
        }

        [HttpGet]
        [Route("GetAllSerie")]
        public ActionResult<Filmes> GetSerie()
        {
            Filmes filmes = new Filmes { filmes = _filmeRepository.GetSeries().ToArray() };

            return filmes;
        }

        [HttpGet]
        [Route("GetInfoFilme/{sigla}")]
        public ActionResult<Filme> GetInfoFilme(string sigla)
        {
            Filme filme = _filmeRepository.GetFilmes().ToList().FirstOrDefault(filme => filme.sigla == sigla) ?? new Filme();

            if (filme.nome == null)
            {
                Response.StatusCode = 404;
                return NotFound("Serie não encontrada!");
            }

            return filme;
        }

        [HttpGet]
        [Route("GetInfoSerie/{sigla}")]
        public ActionResult<Filme> GetInfoSerie(string sigla)
        {
            Filme filme = _filmeRepository.GetSeries().ToList().FirstOrDefault(filme => filme.sigla == sigla) ?? new Filme();

            if(filme.nome == null)
            {
                Response.StatusCode = 404;
                return NotFound("Serie não encontrada!");
            }

            return filme;
        }

        [HttpPut]
        [Route("AtualizarDirSerie/")]
        public ActionResult AtualizarDirSerie([FromBody] UpdateDirSerie updateDirSerie)
        {
            if (_filmeRepository.AtualizarDirSerie(updateDirSerie))
            {
                Response.StatusCode = 200;
                return Ok("Diretório atualizado!");
            }

            Response.StatusCode = 404;
            return NotFound("Serie não encontrada!");
        }

        [HttpGet]
        [Route("videos/{tipo}/{sigla}/{ep}")]
        public async Task<IActionResult> GetVideo(Tipo tipo, string sigla, int ep = 0)
        {
            Filmes filmes;
            string filePath;

            if (tipo == Tipo.filme)
            {
                filmes = new Filmes { filmes = _filmeRepository.GetFilmes().ToArray() };
                filePath = filmes.filmes.FirstOrDefault(item => item.sigla == sigla).dir_Filmes[0] ?? "";
                filePath = $"{_configuration["dir_filesVideos"]}{filePath}";
            } else
            {
                filmes = new Filmes { filmes = _filmeRepository.GetSeries().ToArray() };
                filePath = filmes.filmes.FirstOrDefault(item => item.sigla == sigla).dir_Filmes[ep] ?? "";
                filePath = $"{_configuration["dir_filesSeries"]}{filePath}";
            }

            if(_configuration["sistema_operacional"] != "Windows")
            {
                Console.WriteLine($"filePath antes da conversão: {filePath}");
                filePath = filePath.Replace(@"\", "/");
                filePath = filePath.Replace("\\", "/");
                Console.WriteLine($"filePath depois da conversão: {filePath}");
            }
            

            if(!System.IO.File.Exists(filePath))
            {
                Response.StatusCode = 404;
                return NotFound($"Video não encontrado!\nfilePath: {filePath}");
            }

            var fileInfo = new FileInfo(filePath);
            var fileSize = fileInfo.Length;
            var range = Request.Headers["Range"];

            if (!string.IsNullOrEmpty(range))
            {
                var parts = range.ToString().Replace("bytes=", "").Split("-");
                var start = int.TryParse(parts[0], out int parsedStart) ? parsedStart : 0;
                //var start = int.Parse(parts[0]);
                var end = parts.Length > 1 && !string.IsNullOrEmpty(parts[1]) && int.TryParse(parts[1], out int parsedEnd) ? parsedEnd : fileSize - 1;
                //var end = parts.Length > 1 && !string.IsNullOrEmpty(parts[1]) ? int.Parse(parts[1]) : fileSize - 1;


                var chunkSize = end - start + 1;
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Seek(start, SeekOrigin.Begin);

                var head = new Dictionary<string, string> {
                        { "Content-Range", $"bytes {start}-{end}/{fileSize}" },
                        { "Accept-Ranges", "bytes" },
                        { "Content-Length", chunkSize.ToString() },
                        { "Content-Type", "video/mp4" },
                        { "Accept-Language", "pt-BR,pt;q=0.9;en-US;q=0.8,en;q=0.7"}
                };
                Response.StatusCode = 206;
                foreach (var kvp in head)
                {
                    Response.Headers.Add(kvp.Key, kvp.Value);
                }

                // Leia os dados do FileStream para o buffer em partes menores e escreva-os na resposta
                int bytesRead;
                var buffer = new byte[8192]; //Tamanho do buffer de leitura (pode ajustar conforme necessário)
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await Response.Body.WriteAsync(buffer, 0, bytesRead);
                }

                return new EmptyResult();
            }
            else
            {
                var head = new Dictionary<string, string>
            {
                { "Content-Length", fileSize.ToString() },
                { "Content-Type", "video/mp4" },
                { "Accept-Language", "pt-BR"}
            };
                Response.StatusCode = 200;
                foreach (var kvp in head)
                {
                    Response.Headers.Add(kvp.Key, kvp.Value);
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "video/mp4");
            }
        }


        [HttpGet]
        [Route("image/{tipo}/{sigla}")]
        public IActionResult GetImage(Tipo tipo,string sigla)
        {
            Filmes filmes;
            if (tipo == Tipo.filme) {
                filmes = new Filmes { filmes = _filmeRepository.GetFilmes().ToArray() };
            } else {
                filmes = new Filmes { filmes = _filmeRepository.GetSeries().ToArray() }; 
            }
            
            Filme filme = filmes.filmes.FirstOrDefault(item => item.sigla == sigla);
            if(filme == null) {
                Response.StatusCode = 404;
                return NotFound("Imagem não encontrado!");
            }
            string filePath = filme.dir_Thumb_Heid;

            if (_configuration["sistema_operacional"] != "Windows")
            {
                filePath.Replace("\\", "/");

            }

            filePath = $"{_configuration["dir_filesThumb"]}{filePath}";

            if (!System.IO.File.Exists(filePath))
            {
                Response.StatusCode = 404;
                return NotFound($"Imagem não encontrado!\nfilePath: {filePath}");
            }

            var fileInfo = new FileInfo(filePath);
            var fileSize = fileInfo.Length;
            var range = Request.Headers["Range"];

            if (!string.IsNullOrEmpty(range))
            {
                var parts = range.ToString().Replace("bytes=", "").Split("-");
                var start = int.Parse(parts[0]);
                var end = parts.Length > 1 ? int.Parse(parts[1]) : fileSize - 1;

                var chunkSize = end - start + 1;
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Seek(start, SeekOrigin.Begin);

                var head = new Dictionary<string, string>
                {
                    { "Content-Range", $"bytes {start}-{end}/{fileSize}" },
                    { "Accept-Ranges", "bytes" },
                    { "Content-Length", chunkSize.ToString() },
                    { "Content-Type", "image/png" }
                };
                Response.StatusCode = 206;
                foreach (var kvp in head)
                {
                    Response.Headers.Add(kvp.Key, kvp.Value);
                }

                // Verifique se chunkSize é maior que o máximo valor de int
                if (chunkSize > int.MaxValue)
                {
                    // Faça algo se chunkSize for muito grande para ser um int
                    // Neste exemplo, vamos lançar uma exceção, mas você pode tomar outras ações adequadas
                    throw new ArgumentOutOfRangeException(nameof(chunkSize), "O tamanho do chunk é muito grande para ser tratado como um int.");
                }

                // Faça o casting de chunkSize para int
                var bufferSize = (int)chunkSize;
                var buffer = new byte[bufferSize];

                // Leia os dados do FileStream para o buffer
                fileStream.Read(buffer, 0, bufferSize);
                return File(buffer, "image/png");
            }
            else
            {
                var head = new Dictionary<string, string>
                {
                    { "Content-Length", fileSize.ToString() },
                    { "Content-Type", "image/png" }
                };
                Response.StatusCode = 200;
                foreach (var kvp in head)
                {
                    Response.Headers.Add(kvp.Key, kvp.Value);
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "image/png");
            }
        }
    }
}
