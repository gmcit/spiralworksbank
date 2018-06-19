using Microsoft.Extensions.Configuration;
using SpiralWorks.Data.Ado.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SpiralWorks.Data.Ado
{
    public class AdoDbContext : IDbContext

    {
        public SqlCommand Command { get; set; }
        public SqlDataAdapter Adapter { get; set; }
        public DataSet DataSet { get; set; }

        private SqlConnection Connection;

        public string ConnectionString { get; set; }
        private SqlDataReader _reader;

        public AdoDbContext(IConfiguration Configuration)
        {
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            Connection = new SqlConnection(ConnectionString);
            try
            {
                Command = Connection.CreateCommand();

                Connection.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Connection error.", e);
            }
        }

        public string CommandText { get => Command.CommandText; set => Command.CommandText = value; }

        public CommandType CommandType { get => Command.CommandType; set => Command.CommandType = value; }

        public void AddParameter(SqlParameter parameter)
        {
            Command.Parameters.Add(parameter);
        }

        public void AddParameterWithValue(string parameterName, object value)
        {
            Command.Parameters.AddWithValue(parameterName, value);
        }

        public void ClearParam()
        {
            Command.Parameters.Clear();
        }


        public DataSet ExecuteDataset()
        {
            try
            {
                DataSet = new DataSet();
                Adapter = new SqlDataAdapter(Command);
                SqlCommandBuilder builder = new SqlCommandBuilder(Adapter)
                {
                    ConflictOption = ConflictOption.CompareRowVersion
                };
                DataSet.BeginInit();
                Adapter.Fill(DataSet);
                DataSet.EndInit();
                return DataSet;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public int ExecuteNonQuery()
        {
            return Command.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader()
        {
            _reader = Command.ExecuteReader();
            return _reader;
        }

        public T ExecuteScalar<T>() where T : new()
        {
            T result;
            if (typeof(T).IsPrimitive || typeof(string) == typeof(T))
            {

                object value = Command.ExecuteScalar();
                result = value != DBNull.Value ? (T)Convert.ChangeType(value, typeof(T)) : typeof(T) == typeof(string) ?
                    (T)(object)string.Empty : default(T);
            }
            else
            {
                throw new InvalidCastException("Error cannot convert to a none string or primitive type.");
            }
            return result;
        }
        public List<T> ExecuteToEntity<T>() where T : new()
        {
            try
            {
                List<T> result = new List<T>();
                _reader = Command.ExecuteReader();
                result = _reader.ToEntityList<T>();
                _reader.Close();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }

        public void Dispose()
        {
            Connection?.Dispose();
            Command?.Dispose();
            _reader?.Dispose();
        }
    }
}
