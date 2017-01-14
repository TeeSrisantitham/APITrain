using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckList.API.Repository
{
    public class RepositoryBase : IDisposable
    {
        protected SqlConnection connection;
        public RepositoryBase(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                try
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
                catch { }

                disposedValue = true;
            }
        }

        ~RepositoryBase()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}