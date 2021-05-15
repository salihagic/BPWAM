namespace BPWA.Web.Helpers.Middleware
{
    public class ActivityStatusAllowedRoute
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Location => $"{Controller}/{Action}";
    }
}
