using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIsMovies.Utils
{
    public class KeysHandler
    {
        private const string OurIssuer = "our-app";
        private const string KeySection = "JwtAuth";
        private const string KeySection_Issuer = "Issuer";
        private const string KeySection_Value = "Value";

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration) => GetKey(configuration, OurIssuer);    

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(KeySection).GetChildren().FirstOrDefault(key => key[KeySection_Issuer] == issuer);   
            if(signingKey != null && signingKey[KeySection_Value] is string secretKey) 
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            }

        }

        public static IEnumerable<SecurityKey> GetAllKeys(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(KeySection).GetChildren();

            foreach (var signingKey in signingKeys)
            {
                if (signingKey[KeySection_Value] is string secretKey)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
                }
            }
        }
    }
}
