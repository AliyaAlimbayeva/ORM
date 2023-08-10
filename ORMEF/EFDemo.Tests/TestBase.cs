using EFDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace EFDemo.Tests
{
    public class TestBase : IDisposable
    {
        public readonly EFMod EFMod;
        public EFDemoContext DbContext;

        public TestBase()
        {
            DbContext = new EFDemoContext();
            
            DbContext.Database.Migrate();

            EFMod = new EFMod(DbContext);

            EFMod.ClearAllData();
        }

        public void Dispose()
        {
            EFMod.ClearAllData();
            DbContext.Dispose();
        }
    }
}