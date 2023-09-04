namespace MediaTrackerAuthenticationService.utils
{
    public class RedirectAuth
    {
        private readonly IConfiguration _configuration;

        public RedirectAuth(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public string CreateUrl(string scope)
        {
            string oauthUrl = $"{_configuration["GoogleOauth:Endpoint:Auth"]}" +
                $"?client_id={_configuration["GoogleOauth:ClientId"]}" +
                $"&redirect_uri={_configuration["GoogleOauth:RedirectURI"]}" +
                $"&scope={scope}" +
                $"&response_type=code" +
                $"&access_type=offline" +
                $"&prompt=consent";

            return oauthUrl;
        }
        
    }
}