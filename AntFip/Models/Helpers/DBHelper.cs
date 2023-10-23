﻿using System.Data;
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

            if(args != null)
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
            try
            {
                json = Convert.ToString(ConnectionReader[0]);
            }catch (Exception ex)
            {
            }
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
        public static string CallNonQueryTable(string procedureName, Dictionary<string, object> args, DataTable dataTable, string typeName)
        {
            Connect();                
                    
            SqlCommand CommandConnection = _connection.CreateCommand();
            CommandConnection.CommandType = CommandType.StoredProcedure;
            CommandConnection.CommandText = procedureName;

            if (args != null) {
                foreach (string arg in args.Keys)
                {
                    CommandConnection.Parameters.AddWithValue("@" + arg, args[arg]);
                }
            }            

            SqlParameter parameter = CommandConnection.Parameters.AddWithValue("p"+ typeName, dataTable);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = "dbo." + typeName;
            
            string result = Convert.ToString(CommandConnection.ExecuteNonQuery());
            Disconect();
            CommandConnection.Dispose();
            args.Clear();
            return result;
        }


    }
}
