using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public abstract class UnitTestBase
    {
        protected ApplicationDbContext context;
        
        protected UnitTestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("IoT");
            context = new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}