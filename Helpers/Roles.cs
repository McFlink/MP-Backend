namespace MP_Backend.Helpers
{
    public static class Roles
    {
        public const string Retailer = "Retailer";
        public const string Customer = "Customer";
        public const string AllUsers = Customer + "," + Retailer;
    }
}
