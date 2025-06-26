namespace MP_Backend.Infrastructure.Identity
{
    public static class Roles
    {
        public const string Retailer = "Retailer";
        public const string Customer = "Customer";
        public const string AllUsers = Customer + "," + Retailer;
    }
}
