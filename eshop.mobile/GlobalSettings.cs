namespace Eshop.Mobile;

public static class GlobalSettings
{
    public const string ServerUrl = "SERVER_URL";
    public const string DaDataApiKey = "API_KEY";
    public const string ApiUrl = $"{ServerUrl}/api";


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