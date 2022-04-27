using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DigitalHealthCheckWeb.Model.Risks
{
    /// <summary>
    /// Sends SQL commands and handles scalar results. Allows mocking of database connections.
    /// </summary>
    /// <seealso cref="ISqlCommandSender"/>
    /// <seealso cref="IDisposable"/>
    public class SqlCommandSender : ISqlCommandSender, IDisposable
    {
        private readonly Lazy<SqlConnection> connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandSender"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        public SqlCommandSender(string connectionString) =>
            connection = new Lazy<SqlConnection>(() =>
            {
                var con = new SqlConnection(connectionString);
                con.Open();

                return con;
            });

        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        public void SendNonQuery(string sql) => SendNonQuery(sql, new Dictionary<string, object>());

        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        public void SendNonQuery(string sql, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(sql));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var cmd = CreateCommand(sql, parameters);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        public object SendScalar(string sql) => SendScalar(sql, new Dictionary<string, object>());

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        public object SendScalar(string sql, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(sql));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var cmd = CreateCommand(sql, parameters);

            var result = cmd.ExecuteScalar();

            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        /// <exception cref="ArgumentException">
        /// Argument cannot be null or the empty string. - sql
        /// </exception>
        /// <exception cref="ArgumentNullException">parameters</exception>
        public async Task<object> SendScalarAsync(string sql, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(sql));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var cmd = CreateCommand(sql, parameters);

            var result = await cmd.ExecuteScalarAsync();

            return result == DBNull.Value ? null : result;
        }

        private SqlCommand CreateCommand(string sql, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(sql));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var cmd = new SqlCommand(sql, connection.Value);

            foreach (var parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            return cmd;
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        public async Task SendNonQueryAsync(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(sql));
            }

            await SendNonQueryAsync(sql, new Dictionary<string, object>());
        }

        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        public async Task SendNonQueryAsync(string sql, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(sql));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var cmd = CreateCommand(sql, parameters);

            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        public async Task<object> SendScalarAsync(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(sql));
            }

            return await SendScalarAsync(sql, new Dictionary<string, object>());
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (connection.IsValueCreated)
                {
                    connection.Value.Close();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SqlCommandSender"/> class.
        /// </summary>
        ~SqlCommandSender()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion IDisposable Support
    }
}