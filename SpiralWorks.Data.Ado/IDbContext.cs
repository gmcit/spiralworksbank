using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SpiralWorks.Data.Ado
{
    public interface IDbContext : IDisposable
    {
        string CommandText { get; set; }

        CommandType CommandType { get; set; }

        SqlCommand Command { get; set; }

        string ConnectionString { get; set; }

        void AddParameterWithValue(string parameterName, object value);

        void AddParameter(SqlParameter parameter);

        SqlDataReader ExecuteReader();

        int ExecuteNonQuery();

        T ExecuteScalar<T>() where T : new();

        DataSet ExecuteDataset();

        List<T> ExecuteToEntity<T>() where T : new();
        void ClearParam();
    }
}
