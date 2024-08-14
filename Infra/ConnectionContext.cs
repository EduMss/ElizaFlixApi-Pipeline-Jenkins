using Microsoft.EntityFrameworkCore;
using ElizaFlixAPI.DTO;
using Newtonsoft.Json;

namespace ElizaFlixAPI.Infra
{
    public class ConnectionContext : DbContext
    {
        Config config;

        public ConnectionContext() {
            string value = System.IO.File.ReadAllText("/config/config.json");
            config = JsonConvert.DeserializeObject<Config>(value) ?? new Config();
        }

        public DbSet<Filme> filmes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(
            $"Server={config.ip_banco};" +
            $"Port={config.porta_banco};" +
            $"Database={config.databasse};" +
            $"User Id={config.user};" +
            $"Password={config.password};"
            );
    }
}
