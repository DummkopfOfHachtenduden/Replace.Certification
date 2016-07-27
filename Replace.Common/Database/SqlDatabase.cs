using NLog;
using System;
using System.Data.SqlClient;

namespace Replace.Common.Database
{
    public class SqlDatabase
    {
        private static Logger Logger = LogManager.GetLogger(nameof(SqlDatabase));

        private SqlConnection _connection;

        public SqlDatabase()
        {
            _connection = new SqlConnection();
            _connection.StateChange += _connection_StateChange;
        }

        private void _connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            Logger.Info($"{_connection.Database}: {e.OriginalState} -> {e.CurrentState}.");

            //if (e.OriginalState == System.Data.ConnectionState.Open &&
            //    e.CurrentState != System.Data.ConnectionState.Broken)
            //    this.Reconnect();
        }

        public bool Open(string connectionString)
        {
            Logger.Info($"Connecting to database...");
            try
            {
                _connection.ConnectionString = connectionString;
                _connection.Open();
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, $"{Caller.GetMemberName()}->{nameof(SqlException)}:");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"{Caller.GetMemberName()}->{nameof(Exception)}:");
                return false;
            }

            return true;
        }

        public void Reconnect()
        {
            this.Close();
            this.Open(_connection.ConnectionString); //missing password now
        }

        public void Close()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"{Caller.GetMemberName()}->{nameof(Exception)}:");
            }
        }

        public bool Execute(string sql)
        {
            using (var command = new SqlCommand(sql, _connection))
            {
                try
                {
#if DEBUG
                    if (Logger.IsTraceEnabled)
                        Logger.Trace($"{Caller.GetMemberName()}: {sql}");
#endif

                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    Logger.Error(sqlEx, $"{Caller.GetMemberName()}->{nameof(SqlException)}:");
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"{Caller.GetMemberName()}->{nameof(Exception)}:");
                    return false;
                }
            }

            return true;
        }

        public bool Execute(string sql, SqlParameter[] paramaters)
        {
            using (var command = new SqlCommand(sql, _connection))
            {
                try
                {
#if DEBUG
                    if (Logger.IsTraceEnabled)
                        Logger.Trace($"{Caller.GetMemberName()}: {sql}");
#endif

                    command.Parameters.AddRange(paramaters);
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    Logger.Error(sqlEx, $"{Caller.GetMemberName()}->{nameof(SqlException)}:");
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"{Caller.GetMemberName()}->{nameof(Exception)}:");
                    return false;
                }
            }

            return true;
        }

        public bool Execute(string sql, out SqlDataReader reader)
        {
            reader = null;

            using (var command = new SqlCommand(sql, _connection))
            {
                try
                {
#if DEBUG
                    if (Logger.IsTraceEnabled)
                        Logger.Trace($"{Caller.GetMemberName()}: {sql}");
#endif
                    reader = command.ExecuteReader();
                }
                catch (SqlException sqlEx)
                {
                    Logger.Error(sqlEx, $"{Caller.GetMemberName()}->{nameof(SqlException)}:");
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"{Caller.GetMemberName()}->{nameof(Exception)}:");
                    return false;
                }
            }

            return true;
        }
    }
}