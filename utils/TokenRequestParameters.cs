namespace MediaTrackerAuthenticationService.utils
{
    public class TokenRequestParameters
    {
        private string _code { get; set; }
        public string tokenEndpoint { get; set; }

        private readonly IConfiguration _configuration;

        public TokenRequestParameters(IConfiguration configuration, string code)
        {
            _configuration = configuration;
            _code = code;
            tokenEndpoint = _configuration["GoogleOauth:Endpoint:Token"];
        }

        public Dictionary<string, string> parametersToDictionary()
        {
            var parameters = new Dictionary<string, string>
            {
                {"client_id", _configuration["GoogleOauth:ClientId"]},
                {"client_secret", _configuration["GoogleOauth:ClientSecret"]},
                {"code", _code},
                {"redirect_uri", _configuration["GoogleOauth:RedirectURI"]},
                {"grant_type", "authorization_code"}
            };

            return parameters;
        }
    }
}