namespace eArtRegister.API.Domain.Enums
{
    public static class NFTStatus
    {
        public static string Minted { get => "MINTED"; }
        public static string OnSale { get => "ON_SALE"; }
        public static string NotOnSale { get => "NOT_ON_SALE"; }
        public static string Pending { get => "PENDING"; }
        public static string Sold { get => "SOLD"; }
    }
}
