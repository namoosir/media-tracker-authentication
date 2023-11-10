namespace MediaTrackerAuthenticationService.utils
{
    public class TokenResponse
    {
        public required string access_token { get; set; }
        public required string token_type { get; set; }
        public required int expires_in { get; set; }
        public  string? refresh_token { get; set; }
        public required string scope { get; set; }
        public string? id_token { get; set; }
    }
}