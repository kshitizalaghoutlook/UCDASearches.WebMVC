using System.Collections.Concurrent;

namespace UCDASearches.WebMVC.Services
{
    public class InMemoryUserService : IUserService
    {
        private readonly ConcurrentDictionary<string, string> _users = new()
        {
            ["test@test.com"] = "123456"
        };

        public Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var valid = _users.TryGetValue(email, out var storedPassword) &&
                        storedPassword == password;
            return Task.FromResult(valid);
        }
    }
}
