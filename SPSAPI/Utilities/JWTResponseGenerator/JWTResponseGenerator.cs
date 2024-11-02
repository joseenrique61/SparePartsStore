using SPSAPI.Utilities.JWTTokenGenerator;
using SPSModels.Models;

namespace SPSAPI.Utilities.JWTResponseGenerator
{
    public class JWTResponseGenerator : IJWTResponseGenerator
    {
        private readonly IJWTTokenGenerator _tokenGenerator;

        public JWTResponseGenerator(IJWTTokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public JWTResponse Generate(string email, string role, int clientId)
        {
            return new()
            {
                Token = _tokenGenerator.Generate(email, role),
                Email = email,
                Role = role,
                ClientId = clientId
            };
        }
    }
}
