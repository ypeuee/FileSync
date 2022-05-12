
namespace FIleSyncData
{
    public class SqlitedContext  
    {
        public static System.Data.SQLite.SQLiteConnection NewConnection()
        {
            return new System.Data.SQLite.SQLiteConnection("Data Source=Cloud.ClientApp.db3;Version = 3");
        }
  
    }
}
