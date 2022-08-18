namespace AuctionMarketplaceLibrary {
     public static class Config {
         //Hosts
         public const string AuthServiceHost = "auth-service"; 
         public const string DataBaseHost = "pgdatabase-service";
         public const string AuctionServiceHost = "auction-service";
         public const string AuctionLiveServiceHost = "auction_live-service";
         //Signatures
         public const string AccessSignature = "AccessSecretSignature791";
         public const string RefreshSignature = "RefreshSecretSignature274";
         //Databases
         public const string AuctionServiceDataBaseName = "auctions";
         public const string AuthenticationServerDataBaseName = "users";
         //Database users info
         public const string DataBaseUser = "glebevlakhov";
         public const string DataBasePassword = "psqlpass218";
         //Ports
         public const ushort DataBasePort = 5432;
         public const ushort AuctionServicePort = 7061;
         public const ushort AuctionLiveServicePort = 7019;
         public const ushort AuthenticationServerPort = 7137;
    }
}