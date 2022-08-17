namespace eArtRegister.API.Domain.Enums
{
    public static class EventAction
    {
        public static string USER_CREATED { get => "USER_CREATED"; }
        public static string USER_CREATED_FAIL { get => "USER_CREATED_FAIL"; }
        public static string USER_DEPOSIT_CREATED { get => "USER_DEPOSIT_CREATED"; }
        public static string USER_DEPOSIT_CREATED_FAIL { get => "USER_DEPOSIT_CREATED_FAIL"; }
        public static string USER_ADDED_DEPOSIT { get => "USER_ADDED_DEPOSIT"; }
        public static string USER_ADDED_DEPOSIT_FAIL { get => "USER_ADDED_DEPOSIT_FAIL"; }
        public static string BUNDLE_CREATED { get => "BUNDLE_CREATED"; }
        public static string BUNDLE_CREATED_FAIL { get => "BUNDLE_CREATED_FAIL"; }
        public static string NFT_MINTED { get => "NFT_MINTED"; }
        public static string NFT_MINTED_FAIL { get => "NFT_MINTED_FAIL"; }
        public static string WITHDRAW_FROM_DEPOSIT { get => "WITHDRAW_FROM_DEPOSIT"; }
        public static string WITHDRAW_FROM_DEPOSIT_FAIL { get => "WITHDRAW_FROM_DEPOSIT_FAIL"; }
        public static string PURCHASE_CONTRACT_CREATED { get => "PURCHASE_CONTRACT_CREATED"; }
        public static string PURCHASE_CONTRACT_CREATED_FAIL { get => "PURCHASE_CONTRACT_CREATED_FAIL"; }
        public static string PURCHASE_CONTRACT_APPROVED { get => "PURCHASE_CONTRACT_APPROVED"; }
        public static string PURCHASE_CONTRACT_APPROVED_FAIL { get => "PURCHASE_CONTRACT_APPROVED_FAIL"; }
        public static string NFT_SET_ON_SALE { get => "NFT_SET_ON_SALE"; }
        public static string NFT_SET_ON_SALE_FAIL { get => "NFT_SET_ON_SALE_FAIL"; }
        public static string NFT_SOLD { get => "NFT_SOLD"; }
        public static string NFT_SOLD_FAIL { get => "NFT_SOLD_FAIL"; }
        public static string FUNDS_ADDED_FOR_NFT_SHOPPING { get => "FUNDS_ADDED_FOR_NFT_SHOPPING"; }
        public static string FUNDS_ADDED_FOR_NFT_SHOPPING_FAIL { get => "FUNDS_ADDED_FOR_NFT_SHOPPING_FAIL"; }
        public static string WITHDRAW_FROM_SOLD_NFT { get => "WITHDRAW_FROM_SOLD_NFT"; }
        public static string WITHDRAW_FROM_SOLD_NFT_FAIL { get => "WITHDRAW_FROM_SOLD_NFT_FAIL"; }
        public static string NFT_SALE_CANCELED { get => "NFT_SALE_CANCELED"; }
        public static string NFT_SALE_CANCELED_FAIL { get => "NFT_SALE_CANCELED_FAIL"; }
    }
}
