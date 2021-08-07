namespace BoxingStore.Test.Mocks
{
    using System;
    using BoxingStore.Data;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseMock
    {
        public static BoxingStoreDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<BoxingStoreDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new BoxingStoreDbContext(dbContextOptions);
            }
        }
    }
}