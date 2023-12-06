using System.Data;
using System.Data.SqlClient;

namespace IT_Arg_API.Models.Helpers
{
    public class DBHelper
    {

        private static string _connectionString;
        private static SqlConnection? _connection;

        public static string ConnectionString { set => _connectionString = value; }
        public static void Connect()
        {

            _connection = new SqlConnection(_connectionString);
            _connection.Open();

        }

        public static void Disconect()
        {
            try
            {
                _connection.Close();
            }
            catch
            {
                _connection.Close();
            }

        }

        public static string callProcedureReader(string procedureName, Dictionary<string, object> args = null)
        {
            string? json = "";

            Connect();

            SqlCommand CommandConnection = _connection.CreateCommand();
            CommandConnection.CommandType = CommandType.StoredProcedure;
            CommandConnection.CommandText = procedureName;

            if (args != null)
            {
                foreach (string arg in args.Keys)
                {
                    if (arg != null)
                    {
                        CommandConnection.Parameters.AddWithValue("@" + arg, args[arg]);
                    }
                }
                args.Clear();
            }

            SqlDataReader ConnectionReader = CommandConnection.ExecuteReader();
            ConnectionReader.Read();

            json = Convert.ToString(ConnectionReader[0]);

            Disconect();
            ConnectionReader.DisposeAsync();
            CommandConnection.Dispose();
            return json;
        }

        public static string CallNonQuery(string procedureName, Dictionary<string, object> args)
        {
            Connect();

            SqlCommand CommandConnection = _connection.CreateCommand();
            CommandConnection.CommandType = CommandType.StoredProcedure;

            CommandConnection.CommandText = procedureName;
            foreach (string arg in args.Keys)
            {
                CommandConnection.Parameters.AddWithValue("@" + arg, args[arg]);
            }
            string result = Convert.ToString(CommandConnection.ExecuteNonQuery());
            Disconect();
            CommandConnection.Dispose();
            args.Clear();
            return result;
        }
        public static int CallNonQueryTable(string procedureName, Dictionary<string, object> args, DataTable dataTable, string typeName)
        {
            if (string.IsNullOrEmpty(procedureName) || dataTable == null || string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("Invalid argument(s) provided.");
            }
            Connect();
            int affectedRows = 0;

            using (SqlCommand CommandConnection = _connection.CreateCommand())
            {
                CommandConnection.CommandType = CommandType.StoredProcedure;
                CommandConnection.CommandText = procedureName;

                if (args != null)
                {
                    foreach (KeyValuePair<string, object> arg in args)
                    {
                        CommandConnection.Parameters.AddWithValue("@" + arg.Key, arg.Value);
                    }
                }

                SqlParameter parameter = CommandConnection.Parameters.AddWithValue("p" + typeName, dataTable);
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = "dbo." + typeName;

                affectedRows = CommandConnection.ExecuteNonQuery();
            }

            Disconect();
            args.Clear();
            return affectedRows;
        }




        public static List<Dictionary<string, object>> callProcedureDataTableReader(string procedureName, string typeName, DataTable dataTable, Dictionary<string, object> args = null)
        {
            if (string.IsNullOrEmpty(procedureName) || dataTable == null || string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("Invalid argument(s) provided.");
            }

            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>(dataTable.Rows.Count);

            try
            {
                Connect();

                using var CommandConnection = _connection.CreateCommand();
                CommandConnection.CommandType = CommandType.StoredProcedure;
                CommandConnection.CommandText = procedureName;

                foreach (KeyValuePair<string, object> arg in args ?? Enumerable.Empty<KeyValuePair<string, object>>())
                {
                    CommandConnection.Parameters.AddWithValue("@" + arg.Key, arg.Value);
                }

                SqlParameter parameter = CommandConnection.Parameters.AddWithValue("p" + typeName, dataTable);
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = "dbo." + typeName;

                using var reader = CommandConnection.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>(reader.FieldCount);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    results.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while executing the stored procedure.", ex);
            }
            finally
            {
                Disconect();
            }

            return results;
        }




    }
}
