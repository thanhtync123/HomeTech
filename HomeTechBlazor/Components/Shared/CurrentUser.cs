namespace HomeTechBlazor.Components.Shared
{
    public class CurrentUser
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public bool isLoggedIn { get; set; } = false;   
    }
}
