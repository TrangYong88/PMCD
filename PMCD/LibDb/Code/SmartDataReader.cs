using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Lib.Database
{
	public sealed class SmartDataReader
	{
		public SmartDataReader()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		private DateTime defaultDate;
		public SmartDataReader(SqlDataReader reader)
		{
			if (DateTime.MinValue.Year < 1900)
			{
				this.defaultDate = DateTime.MinValue.AddYears(1900 - DateTime.MinValue.Year);
			}
			else
			{
				this.defaultDate = DateTime.MinValue;
			}
			this.reader = reader;
		}

		public int GetInt32(string column)
		{
			try
			{
				int data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (int)0 : Convert.ToInt32(reader[column]);
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
				long data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (long)0 : Convert.ToInt64(reader[column]);
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
				short data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (short)0 : Convert.ToInt16(reader[column]);
				return data;
			}
			catch
			{
				return (short)0;
			}
		}

		public byte GetByte(string column)
		{
			try
			{
				byte data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (byte)0 : Convert.ToByte(reader[column]);
				return data;
			}
			catch
			{
				return (byte)0;
			}
		}

		//---------------------------------------------
		public float GetFloat(string column)
		{
			float RetVal = 0;
			try
			{
				string data = (reader.IsDBNull(reader.GetOrdinal(column))) ? "0" : reader[column].ToString();
				float.TryParse(data, out RetVal);
			}
			catch
			{
				return 0;
			}
			return RetVal;
		}
		//---------------------------------------------
		public bool GetBoolean(string column)
		{
			try
			{
				bool data = (reader.IsDBNull(reader.GetOrdinal(column)))
																 ? false : (bool)reader[column];
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
				string data = (reader.IsDBNull(reader.GetOrdinal(column)))
															 ? "" : reader[column].ToString();
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
				DateTime data = (reader.IsDBNull(reader.GetOrdinal(column))) ? defaultDate : (DateTime)reader[column];
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
				decimal data = (reader.IsDBNull(reader.GetOrdinal(column)))
																? (decimal)0 : (decimal)reader[column];
				return data;
			}
			catch
			{
				return (decimal)0;
			}
		}

		public bool Read()
		{
			return this.reader.Read();
		}
		private SqlDataReader reader;
		public void disposeReader(SqlDataReader reader)
		{
			if (reader != null || !reader.IsClosed)
			{
				reader.Close();
				reader.Dispose();
			}
		}
		public void DisposeReader(SqlDataReader reader)
		{
			disposeReader(reader);
		}
		//--------------------
		public double GetDouble(string column)
		{
			double RetVal = 0;
			try
			{
				string data = (reader.IsDBNull(reader.GetOrdinal(column)))? "0" : reader[column].ToString();
				double.TryParse( data, out RetVal);
			}
			catch
			{
			}
			return RetVal;
		}
	}
}