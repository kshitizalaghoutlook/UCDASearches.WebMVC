namespace UCDASearches.WebMVC.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentialsAsync(string email, string password);
    }
}
