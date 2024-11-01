namespace SPSAPI.Utilities.JWTTokenGenerator
{
    public interface IJWTTokenGenerator
    {
        public string Generate(string email, string role);
    }
}
