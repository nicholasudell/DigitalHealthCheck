using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.Model.Risks
{
    /// <summary>
    /// Interface for sending SQL commands to a database
    /// </summary>
    public interface ISqlCommandSender
    {
        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        void SendNonQuery(string sql);

        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        void SendNonQuery(string sql, IDictionary<string, object> parameters);

        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        Task SendNonQueryAsync(string sql);

        /// <summary>
        /// Sends a SQL command with no expected result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        Task SendNonQueryAsync(string sql, IDictionary<string, object> parameters);

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        object SendScalar(string sql);

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        object SendScalar(string sql, IDictionary<string, object> parameters);

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        Task<object> SendScalarAsync(string sql);

        /// <summary>
        /// Sends a SQL command with an expected scalar result.
        /// </summary>
        /// <param name="sql">The SQL to send to the database.</param>
        /// <param name="parameters">The parameters to apply to the SQL command.</param>
        /// <returns>The scalar result of the SQL query.</returns>
        Task<object> SendScalarAsync(string sql, IDictionary<string, object> parameters);
    }
}