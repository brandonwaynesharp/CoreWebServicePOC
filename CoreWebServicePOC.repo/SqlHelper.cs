using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace CoreWebServicePOC.repo
{
    public class SQLHelper
    {
        private static string VALUES_SQL_CONNECTION = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ValueDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string ValuesSqlConnection
        {
            get { return VALUES_SQL_CONNECTION; }
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlConnection conn, string cmdText, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);
            var val = cmd.ExecuteNonQueryAsync();
            cmd.Parameters.Clear();
            return await val;
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            using (conn)
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = await cmd.ExecuteNonQueryAsync();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);            
            var rdr = cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            return await rdr;
        }
        public static async Task<DataTable> ExecuteDataTableAsync(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("id");
            dt.Columns.Add("value");
            SqlDataReader dr = await ExecuteReaderAsync(conn, cmdType, cmdText, cmdParms);

            while (dr.Read())
            {
                dt.Rows.Add(dr[0], dr[1]);
            }
            return dt;
        }

        public static DataTable ExecuteDataTableSqlDA(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);
            da.Fill(dt);
            return dt;
        }

        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();
            return val;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;

            if (commandParameters != null)
            {
                AttachParameters(cmd, commandParameters);
            }
        }
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter p in commandParameters)
            {
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }
    }
}