namespace AuctionMarketplaceLibrary {
     public static class Config {
         //Hosts
         public const string AuthServiceHost = "auth-service"; 
         public const string AuctionServiceHost = "auction-service";
         public const string UsersDataBaseHost = "postgresql-users-service";
         public const string AuctionLiveServiceHost = "auction-live-service";
         public const string AuctionsDataBaseHost = "postgresql-auctions-service";
         //Signatures
         public const string AccessSignature = "AccessSecretSignature791";
         public const string RefreshSignature = "RefreshSecretSignature274";
         //Databases
         public const string AuctionServiceDataBaseName = "auctions";
         public const string AuthenticationServerDataBaseName = "users";
         //Database users info
         public const string UsersDataBaseUser = "glebevlakhov";
         public const string UsersDataBasePassword = "psqlpass218";
         public const string AuctionsDataBaseUser = "glebevlakhov";
         public const string AuctionsDataBasePassword = "psqlpass218";
         //Ports
         public const ushort UsersDataBasePort = 5432;
         public const ushort AuctionsDataBasePort = 5431;
         public const ushort AuctionServicePort = 7061;
         public const ushort AuctionLiveServicePort = 7019;
         public const ushort AuthenticationServerPort = 7137;
    }
}