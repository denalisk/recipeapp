using System.Data;
using System.Data.SqlClient;

namespace RecipeApp
{
  public class DB
  {
    public static SqlConnection Connection()
    {
      SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
      return conn;
    }

    public static void CloseSqlConnection(SqlConnection conn, SqlDataReader rdr = null)
    {
      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll(string tableName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      string command = string.Format("DELETE FROM {0};", tableName);
      SqlCommand cmd = new SqlCommand(command, conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
