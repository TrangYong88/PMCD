using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Lib.Database
{
    public sealed class SmartDataRow
    {
        public SmartDataRow()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private DateTime defaultDate;
        public SmartDataRow(DataRow row)
        {
            this.defaultDate = DateTime.MinValue;
            this.row = row;
        }

        public int GetInt32(string column)
        {
            try
            {
                int data = (int)row[column];
                return data;
            }
            catch
            {
                return (int)0;
            }
        }

        public long GetInt64(string column)
        {
            try
            {
                long data = (long)row[column];
                return data;
            }
            catch
            {
                return (long)0;
            }
        }

        public short GetInt16(string column)
        {
            try
            {
                short data = (short)row[column];
                return data;
            }
            catch
            {
                return (short)0;
            }
        }

        public float GetFloat(string column)
        {
            try
            {
                float data = float.Parse(row[column].ToString());
                return data;
            }
            catch
            {
                return 0;
            }
        }

        public bool GetBoolean(string column)
        {
            try
            {
                bool data = (bool)row[column];
                return data;
            }
            catch
            {
                return false;
            }
        }

        public string GetString(string column)
        {
            try
            {
                string data = row[column].ToString();
                return data;
            }
            catch
            {
                return "";
            }
        }

        public DateTime GetDateTime(string column)
        {
            try
            {
                DateTime data = (DateTime)row[column];
                return data;
            }
            catch
            {
                return defaultDate;
            }
        }

        public decimal GetDecimal(string column)
        {
            try
            {
                decimal data = (decimal)row[column];
                return data;
            }
            catch
            {
                return (decimal)0;
            }
        }

        private DataRow row;
    }
}