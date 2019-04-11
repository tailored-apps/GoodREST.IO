namespace GoodREST.Middleware.Interface
{
    public interface IAuthService
    {
        string AuthUrl { get; }
        T AuthUser<T>(string login, string pass, string salt) where T : class, new();
        string PassGen(string password, string salt);
        T CheckAccess<T>(string xauth) where T : class, new();
    }
}
