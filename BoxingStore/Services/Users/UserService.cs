namespace BoxingStore.Services.User
{
    using BoxingStore.Data;

    public class UserService : IUserService
    {
        private readonly BoxingStoreDbContext data;

        public UserService(BoxingStoreDbContext data)
        {
            this.data = data;
        }

        public string FindByFullName(string userId) => this.data.Users.Find(userId).FullName;

        public string FindByEmail(string userId) => this.data.Users.Find(userId).Email; 
    }
}
