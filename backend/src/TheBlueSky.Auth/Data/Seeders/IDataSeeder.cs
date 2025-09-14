namespace TheBlueSky.Auth.Data.Seeders
{
    public interface IDataSeeder
    {
        public int Priority { get; }

        Task SeedAsync(CancellationToken cancellationToken = default);

    }
}
