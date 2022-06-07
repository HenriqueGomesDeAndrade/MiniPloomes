using MiniPloomes.Entities;

namespace MiniPloomes.Persistence
{
    public class MiniPloomesContext
    {
        private readonly IConfiguration _config;

        public MiniPloomesContext(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString()
        {
            var connectionString = _config.GetConnectionString("MiniPloomesCS");
            return connectionString;
        }
    }
}
