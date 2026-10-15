namespace Spendly.Data;

public static class Constants
{

    // The name of the database file on the phone

    public const string DatabaseFilename = "Spendly.db3";

    // Open the file to read and write, and create it on the first run
    public const SQLite.SQLiteOpenFlags Flags =
    SQLite.SQLiteOpenFlags.ReadWrite |
    SQLite.SQLiteOpenFlags.Create |
    SQLite.SQLiteOpenFlags.SharedCache;

    // The app's private folder + the file name = the full path
    public static string DatabasePath =>

    Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}