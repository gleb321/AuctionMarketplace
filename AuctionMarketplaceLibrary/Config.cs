namespace AuctionMarketplaceLibrary {
     public static class Config {
         //Hosts
         // public const string BidServiceHost = "bid-service";
         // public const string AuctionServiceHost = "auction-service";
         // public const string AuthenticationServerHost = "auth-service";
         // public const string AuctionLiveServiceHost = "auction-live-service";
         // public const string AuctionServiceDataBaseHost = "postgresql-auction-service";
         // public const string AuthenticationServerDataBaseHost = "postgresql-auth-service";
         public const string BidServiceHost = "localhost";
         public const string AuctionServiceHost = "localhost";
         public const string AuthenticationServerHost = "localhost";
         public const string AuctionLiveServiceHost = "localhost";
         public const string AuctionServiceDataBaseHost = "localhost";
         public const string AuthenticationServerDataBaseHost = "localhost";
         //Signatures
         public const string AccessSignature = "AccessSecretSignature791";
         public const string RefreshSignature = "RefreshSecretSignature274";
         //Databases
         public const string AuctionServiceDataBaseName = "auction";
         public const string AuthenticationServerDataBaseName = "auth";
         //Database users info
         // public const string AuctionServiceDataBaseUser = "auction_service";
         // public const string AuctionServiceDataBasePassword = "auctionpsqlpass218";
         // public const string AuthenticationServerDataBaseUser = "auth_service";
         // public const string AuthenticationServerDataBasePassword = "authpsqlpass218";
         public const string AuctionServiceDataBaseUser = "glebevlakhov";
         public const string AuctionServiceDataBasePassword = "auctionpsqlpass218";
         public const string AuthenticationServerDataBaseUser = "glebevlakhov";
         public const string AuthenticationServerDataBasePassword = "authpsqlpass218";
         //Ports
         public const ushort BidServicePort = 7267;
         public const ushort AuctionServicePort = 7061;
         public const ushort AuctionLiveServicePort = 7019;
         public const ushort AuthenticationServerPort = 7137;
         // public const ushort AuctionServiceDataBasePort = 5432;
         // public const ushort AuthenticationServerDataBasePort = 5431;
         public const ushort AuctionServiceDataBasePort = 5432;
         public const ushort AuthenticationServerDataBasePort = 5432;
    }
}