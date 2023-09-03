namespace MediaTrackerAuthenticationService.utils
{
    public class TokenReponse
    {
        public required string access_token { get; set; }
        public required string token_type { get; set; }
        public required int expires_in { get; set; }
        public required string refresh_token { get; set; }
        public required string scope { get; set; }
    }
}