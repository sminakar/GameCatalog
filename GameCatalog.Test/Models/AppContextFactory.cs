using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.Test
{
    public class AppContextFactory : IDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GameCatalog;Integrated Security=True");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
