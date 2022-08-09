namespace AuctionMarketplaceLibrary {
     public static class Config {
         public const string Host = "localhost"; 
         public const string AccessSignature = "AccessSecretSignature791";
         public const string RefreshSignature = "RefreshSecretSignature274";
         public const string DataBaseHost = "localhost";
         public const string AuthenticationServerDataBaseName = "users";
         public const string AuctionServiceDataBaseName = "auctions";
         public const string DataBaseUser = "glebevlakhov";
         public const string DataBasePassword = "psqlpass218";
         public const ushort DataBasePort = 5432;
         public const ushort AuctionServicePort = 7061;
         public const ushort AuctionLiveServicePort = 7019;
    }
}