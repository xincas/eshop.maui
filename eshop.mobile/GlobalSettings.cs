namespace Eshop.Mobile;

public static class GlobalSettings
{
    //public const string ServerUrl = "https://inarofuel.beget.app";
    //public const string ApiUrl = $"{ServerUrl}/api";


    public const string ServerUrl = "http://192.168.0.109:1337";
    public const string ApiUrl = $"{ServerUrl}/api";

    //public const string ServerUrl = "http://10.0.2.2:1337";
    //public const string ApiUrl = $"{ServerUrl}/api";

    public const string DatabaseFilename = "eshop.db3";

    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}