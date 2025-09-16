namespace TheBlueSky.Flights.Data.Seeders
{
    public class DatabaseSeeder
    {
        private readonly IEnumerable<IDataSeeder> _dataSeeders;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(IEnumerable<IDataSeeder> dataSeeders, ILogger<DatabaseSeeder> logger)
        {
            _dataSeeders = dataSeeders;
            _logger = logger;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting data seeding...");

            var seedersByPriority = _dataSeeders.OrderBy(s => s.Priority);

            foreach (var seeder in seedersByPriority)
            {
                _logger.LogInformation("Running seeder: {SeederName}", seeder.GetType().Name);
                await seeder.SeedAsync(cancellationToken);
            }

            _logger.LogInformation("Data seeding completed.");

        }

    }
}
