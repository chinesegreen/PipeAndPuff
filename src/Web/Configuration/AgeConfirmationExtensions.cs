namespace Web.Configuration
{
    public static class AgeConfirmationExtensions
    {
        const string key = "IsAgeConfirmed";

        public static void ConfirmAge(HttpContext context)
        {
            context.Response.Cookies.Append(key, "confirmed");
        }

        public static bool IsAgeConfirmed(HttpContext context)
        {
            string value;
            bool result = context.Request.Cookies.TryGetValue(key, out value);

            if (result && value == "confirmed")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
