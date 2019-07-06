using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Threading;

namespace ScheduleHelper
{
    public sealed class SQLiteBulkCopy
    {
        public event SQLiteRowsCopiedEventHandler RowsCopied;
        public int NotifyAfter { get; set; }
        public string DestinationTableName { get; set; }
        private string _connectionString;
        public int WriteToServer(DataTable dt)
        {
            string[] sql = dt.ToSQLStringArray(this.DestinationTableName);
            return batchExecuteSQL(sql);
        }
        private int batchExecuteSQL(string[] sql)
        {
            int buffer = 0;
            SQLiteConnection _connection = new SQLiteConnection(this._connectionString);
            _connection.Open();
            SQLiteTransaction trans = _connection.BeginTransaction();
            SQLiteCommand cmd = _connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.Transaction = trans;
            try
            {
                for (int i = 0; i < sql.Length; i++)
                {
                    cmd.CommandText = sql[i];
                    cmd.ExecuteNonQuery();
                    buffer++;
                    //触发事件
                    if (buffer == NotifyAfter)
                    {
                        if (RowsCopied != null)
                        {
                            RowsCopied(this, new SQLiteRowsCopiedEventArgs(buffer, sql.Length));
                        }

                        buffer = 0;
                    }
                }

                if (buffer != 0)
                {
                    if (RowsCopied != null)
                    {
                        RowsCopied(this, new SQLiteRowsCopiedEventArgs(buffer, sql.Length));
                    }
                    buffer = 0;
                }

                //提交事务,只有所有的数据都没有问题才提交事务.
                trans.Commit();
                //异步压缩,分析数据库,确保所得的分析是最佳的.
                ThreadPool.QueueUserWorkItem(new WaitCallback((object o) =>
                {
                    StaticSQLiteHelper.ExecuteNonQuery("VACUUM ANALYZE");
                }));


            }
            catch (SQLiteException)
            {
                trans.Rollback();
                return 0;
            }
            finally
            {
                trans.Dispose();
                cmd.Dispose();
            }

            return sql.Length;

        }
        public SQLiteBulkCopy(string connectionString)
        {
            _connectionString = connectionString;
        }

    }
    public sealed class SQLiteRowsCopiedEventArgs : EventArgs
    {
        private int _rowsCopied;
        private int _total;
        public SQLiteRowsCopiedEventArgs(int rowsCopied, int total)
        {
            _rowsCopied = rowsCopied;
            _total = total;
        }

        public bool Abort { get; set; }

        public int RowsCopied { get { return _rowsCopied; } }
        public int Total { get { return _total; } }

    }
    public delegate void SQLiteRowsCopiedEventHandler(object sender, SQLiteRowsCopiedEventArgs e);
}
