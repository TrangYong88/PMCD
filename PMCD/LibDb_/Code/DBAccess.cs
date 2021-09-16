using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Reflection;
using Lib.Utils;
namespace Lib.Database
{
	public class DBAccess
	{
		private string dbms;
		private string connectionString;
		//---------------------------------------------------------------------------------------------------
		public DBAccess(string connStr)
		{
			connectionString = connStr;
			dbms = "SQL Server";
		}
		//---------------------------------------------------------------------------------------------------
		public DBAccess(string Dbms, string connStr)
		{
			connectionString = connStr;
			dbms = (string.IsNullOrEmpty(Dbms)) ? "SQL Server" : Dbms;
		}
		//---------------------------------------------------------------------------------------------------
		public DBAccess(string server, string userName, string userPass, string dbName)
		{
			connectionString = BuildConstr(server, userName, userPass, dbName);
		}
		//---------------------------------------------------------------------------------------------------
		public bool checkConnection(string connStr)
		{
			bool retVal = false;
			try
			{
				SqlConnection mySqlConnection = new SqlConnection(connStr);
				mySqlConnection.Open();
				if (mySqlConnection.State == ConnectionState.Open)
				{
					mySqlConnection.Close();
					retVal = true;
				}
			}
			catch
			{
				retVal = false;
			}
			return retVal;
		}
		//---------------------------------------------------------------------------------------------------
		public static string BuildConstr(string server, string userName, string userPass, string dbName)
		{
			string retVal = "";
			if ((server.Length > 0) && (userName.Length > 0) && (userPass.Length > 0) && (dbName.Length > 0))
			{
				retVal = "server=" + server + ";uid=" + userName + ";pwd=" + userPass + ";database=" + dbName + ";";
			}
			return retVal;
		}
		//---------------------------------------------------------------------------------------------------
		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
			set
			{
				connectionString = value;
			}
		}
		//---------------------------------------------------------------------------------------------------
		public SqlConnection getConnection()
		{
			try
			{
				return new SqlConnection(connectionString);
			}
			catch (SqlException sqlEx)
			{
				throw new ApplicationException("Connecting failed " + sqlEx.Message);
			}
		}
		//---------------------------------------------------------------------------------------------------
		public OleDbConnection getOLEConnection()
		{
			try
			{
				return new OleDbConnection(connectionString);
			}
			catch (SqlException sqlEx)
			{
				throw new ApplicationException("Connecting to database failed: " + sqlEx.Message);
			}
		}
		//---------------------------------------------------------------------------------------------------
		public void closeConnection(SqlConnection con)
		{
			try
			{
				if (con != null || con.State == ConnectionState.Open)
				{
					con.Close();
					con.Dispose();
				}
			}
			catch (SqlException sqlEx)
			{
				throw sqlEx;
			}
		}
		//---------------------------------------------------------------------------------------------------        #region Management Methods
		public SqlConnection OpenConnection(string connectionString)
		{
			try
			{
				SqlConnection mySqlConnection = new SqlConnection(connectionString);
				mySqlConnection.Open();
				return mySqlConnection;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public void CloseConnection(SqlConnection mySqlConnection)
		{
			try
			{
				if (mySqlConnection.State == ConnectionState.Open)
				{
					mySqlConnection.Close();
					mySqlConnection.Dispose();
				}
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public DataSet getDataSet(string strSQL)
		{
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlDataAdapter myDataAdapter = new SqlDataAdapter(strSQL, myConn);
				DataSet myDataSet = new DataSet();
				myDataAdapter.Fill(myDataSet);
				myDataAdapter.Dispose();
				myBase.CloseConnection(myConn);
				return myDataSet;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public SqlDataReader getReader(SqlCommand cmd)
		{
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				cmd.Connection = myConn;
				SqlDataReader reader = cmd.ExecuteReader();
				//myBase.CloseConnection(myConn);
				return reader;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + cmd.CommandText));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public SqlDataReader getReader(SqlCommand cmd, SqlConnection con)
		{
			try
			{
				cmd.Connection = con;
				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				return reader;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + cmd.CommandText));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public DataTable getDataTable(string strSQL)
		{
			return getDataSet(strSQL).Tables[0];
		}
		//---------------------------------------------------------------------------------------------------
		public DataSet getDataSet(string strSQL, SqlParameter[] Parameters)
		{
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				myCommand.Parameters.Clear();
				foreach (SqlParameter par in Parameters)
				{
					myCommand.Parameters.Add(par);
				}
				SqlDataAdapter myDataAdapter = new SqlDataAdapter(myCommand);
				DataSet myDataSet = new DataSet();
				myDataAdapter.Fill(myDataSet);
				myBase.CloseConnection(myConn);
				return myDataSet;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public DataTable getDataTable(string strSQL, params SqlParameter[] Parameters)
		{
			return getDataSet(strSQL, Parameters).Tables[0];
		}
		//---------------------------------------------------------------------------------------------------
		public DataSet getDataSet(SqlCommand mCommand)
		{
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				mCommand.Connection = myConn;
				SqlDataAdapter myDataAdapter = new SqlDataAdapter(mCommand);
				DataSet myDataSet = new DataSet();
				myDataAdapter.Fill(myDataSet);
				myBase.CloseConnection(myConn);
				return myDataSet;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + mCommand.CommandText));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public DataTable getDataTable(SqlCommand mCommand)
		{
			return getDataSet(mCommand).Tables[0];
		}
		//---------------------------------------------------------------------------------------------------
		public void ExecuteSQL(string strSQL)
		{
			DBAccess myBase = new DBAccess(connectionString);
			SqlConnection myConn = myBase.OpenConnection(connectionString);
			SqlTransaction tran = myConn.BeginTransaction();
			try
			{
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				myCommand.Transaction = tran;
				myCommand.ExecuteNonQuery();
				tran.Commit();
				myCommand.Dispose();
			}
			catch (Exception myException)
			{
				tran.Rollback();
				throw (new Exception(myException.Message + " => " + strSQL));
			}
			finally
			{
				myBase.CloseConnection(myConn);
			}
		}
		//---------------------------------------------------------------------------------------------------
		public void ExecuteSQL(string strSQL, params SqlParameter[] Parameters)
		{
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				myCommand.Parameters.Clear();
				foreach (SqlParameter par in Parameters)
				{
					myCommand.Parameters.Add(par);
				}
				myCommand.ExecuteNonQuery();
				myCommand.Dispose();
				myBase.CloseConnection(myConn);
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public void ExecuteSQL(string strSQL, IList paraList)
		{
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				myCommand.Parameters.Clear();
				foreach (SqlParameter par in paraList)
				{
					myCommand.Parameters.Add(par);
				}
				myCommand.ExecuteNonQuery();

				myCommand.Dispose();
				myBase.CloseConnection(myConn);
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}

		}
		//---------------------------------------------------------------------------------------------------
		public void ExecuteSQL(SqlCommand mCommand)
		{
			DBAccess myBase = new DBAccess(connectionString);
			SqlConnection myConn = myBase.OpenConnection(connectionString);
			try
			{
				//SqlConnection myConn = myBase.OpenConnection(connectionString);
				mCommand.Connection = myConn;
				mCommand.ExecuteNonQuery();
				mCommand.Dispose();
				//myBase.CloseConnection(myConn);
			}
			catch (Exception myException)
			{
				mCommand.Dispose();
				throw (new Exception(myException.Message + " => " + mCommand.CommandText));
			}
			finally
			{
				myBase.CloseConnection(myConn);
			}
		}
		//---------------------------------------------------------------------------------------------------
		public void ExecuteSQL(string strSQL, SqlConnection mConn)
		{
			try
			{
				SqlCommand myCom = new SqlCommand(strSQL, mConn);
				myCom.ExecuteNonQuery();
				myCom.Dispose();
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public string ExecuteScalarString(string LogFilePath, string LogFileName, string strSQL)
		{
			string RetVal = "";
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				object m_object=myCommand.ExecuteScalar();
				myCommand.Dispose();
				myBase.CloseConnection(myConn);
				if (m_object != null)
				{
					RetVal = m_object.ToString();
				}				
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message + ": " + strSQL, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------------------
		public int ExecuteScalar(string strSQL)
		{
			int RetVal = 0;
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				//RetVal = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
				object m_object=myCommand.ExecuteScalar();
				if (m_object!=null)
				{
					RetVal = StringUtils.StrToInt32(m_object.ToString());
				}
				myCommand.Dispose();
				myBase.CloseConnection(myConn);
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------------------
		public int ExecuteScalar(string strSQL, params SqlParameter[] Parameters)
		{
			int temp = 0;
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				myCommand.Parameters.Clear();
				foreach (SqlParameter par in Parameters)
				{
					myCommand.Parameters.Add(par);
				}

				temp = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
				myCommand.Dispose();
				myBase.CloseConnection(myConn);
				return temp;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public int ExecuteScalar(string strSQL, IList paraList)
		{
			int temp = 0;
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				SqlCommand myCommand = new SqlCommand(strSQL, myConn);
				myCommand.Parameters.Clear();
				foreach (SqlParameter par in paraList)
				{
					myCommand.Parameters.Add(par);
				}
				temp = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
				myCommand.Dispose();
				myBase.CloseConnection(myConn);
				return temp;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public int ExecuteScalar(SqlCommand mCommand)
		{
			int temp = 0;
			DBAccess myBase = new DBAccess(connectionString);
			try
			{
				SqlConnection myConn = myBase.OpenConnection(connectionString);
				mCommand.Connection = myConn;
				temp = Convert.ToInt32(mCommand.ExecuteScalar().ToString());
				mCommand.Dispose();
				myBase.CloseConnection(myConn);
				return temp;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + mCommand.CommandText));
			}
		}
		//---------------------------------------------------------------------------------------------------
		public int ExecuteScalar(string strSQL, SqlConnection mConn)
		{
			int temp = 0;
			try
			{
				SqlCommand myCom = new SqlCommand(strSQL, mConn);
				temp = Convert.ToInt32(myCom.ExecuteScalar().ToString());
				myCom.Dispose();
				return temp;
			}
			catch (Exception myException)
			{
				throw (new Exception(myException.Message + " => " + strSQL));
			}

		}
		//---------------------------------------------------------------------------------------------------
		public DataTable SelectDBRows(string query)
		{
			try
			{
				SqlConnection sqlcon = new SqlConnection(connectionString);
				DataTable dt = new DataTable();
				SqlDataAdapter da = new SqlDataAdapter();
				sqlcon.Open();
				da.SelectCommand = new SqlCommand(query, sqlcon);
				da.Fill(dt);
				sqlcon.Close();
				da.Dispose();
				sqlcon.Dispose();
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//---------------------------------------------------------------------------------------------------
		public long GetNumber(string Query)
		{
			try
			{
				Object num;
				SqlConnection sqlcon = new SqlConnection(connectionString);
				SqlCommand Com = new SqlCommand(Query, sqlcon);
				sqlcon.Open();
				num = (long)Com.ExecuteScalar();
				sqlcon.Close();
				sqlcon.Dispose();
				Com.Dispose();
				return (long)num;
			}
			catch
			{
				return 0;
			}
		}
		//---------------------------------------------------------------------------------------------------
		public string GetString(string Query)
		{
			string RetVal = "";
			try
			{
				Object num;
				SqlConnection sqlcon = new SqlConnection(connectionString);
				SqlCommand Com = new SqlCommand(Query, sqlcon);
				sqlcon.Open();
				num = (string)Com.ExecuteScalar();
				sqlcon.Close();
				sqlcon.Dispose();
				Com.Dispose();
				RetVal = (string)num;
			}
			catch
			{

			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------------------
		public bool SqlExecuteInt64(string LogFilePath, string LogFileName, string IpAddress, int ActUserId, string Sql, ref long Id)
		{
			bool RetVal = false;
			try
			{
				if (!string.IsNullOrEmpty(Sql))
				{
					SqlCommand cmd = new SqlCommand("SqlExecute");
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(new SqlParameter("@IpAddress", IpAddress));
					cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
					cmd.Parameters.Add(new SqlParameter("@Sql", Sql));
					cmd.Parameters.Add("@Id", SqlDbType.BigInt).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@RetVal", SqlDbType.Int).Direction = ParameterDirection.Output;
					ExecuteSQL(cmd);
					if (cmd.Parameters["@RetVal"].Value != null)
					{
						RetVal = (Convert.ToInt32(cmd.Parameters["@RetVal"].Value.ToString().Trim()) > 0);
						if (RetVal)
						{
							Id = Convert.ToInt64((cmd.Parameters["@Id"].Value == null) ? "0" : cmd.Parameters["@Id"].Value.ToString().Trim());
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------------------
		public bool SqlExecute(string LogFilePath, string LogFileName, string IpAddress, int ActUserId, string Sql, ref int Id)
		{
			bool RetVal = false;
			try
			{
				if (!string.IsNullOrEmpty(Sql))
				{
					SqlCommand cmd = new SqlCommand("SqlExecute");
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(new SqlParameter("@IpAddress", IpAddress));
					cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
					cmd.Parameters.Add(new SqlParameter("@Sql", Sql));
					cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@RetVal", SqlDbType.Int).Direction = ParameterDirection.Output;
                    ExecuteSQL(cmd);
					if (cmd.Parameters["@RetVal"].Value != null)
					{
						RetVal = (Convert.ToInt32(cmd.Parameters["@RetVal"].Value.ToString().Trim()) > 0);
						if (RetVal)
						{
							Id = Convert.ToInt32((cmd.Parameters["@Id"].Value == null) ? "0" : cmd.Parameters["@Id"].Value.ToString().Trim());
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}

		//---------------------------------------------------------------------------------------------------
		public bool SqlExecute(string LogFilePath, string LogFileName, string IpAddress, int ActUserId, string Sql)
		{
			int Id = 0;
			return SqlExecute(LogFilePath, LogFileName, IpAddress, ActUserId, Sql, ref Id);
		}

		//----------------------------------------------------------------------------------------------------
		public int GetAggregate(string LogFilePath, string LogFileName, string TableName, string FunctionName, string Condition)
		{
			int RetVal = 0;
			string Sql = "SELECT " + FunctionName + " FROM " + TableName;
			try
			{
				if (!string.IsNullOrEmpty(Condition))
				{
					Sql += " WHERE (" + Condition + ")";
				}
				RetVal = ExecuteScalar(Sql);
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message + ": " + Sql, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
		//----------------------------------------------------------------------------------------------------
		public DateTime GetAggregateDateTime(string LogFilePath, string LogFileName, string TableName, string FunctionName, string Condition)
		{
			DateTime RetVal = System.DateTime.Now;
			string Sql = "SELECT " + FunctionName + " FROM " + TableName;
			try
			{
				if (!string.IsNullOrEmpty(Condition))
				{
					Sql += " WHERE (" + Condition + ")";
				}
				string Tmp= ExecuteScalarString(LogFilePath, LogFileName,Sql);
				if (string.IsNullOrEmpty(Tmp))
				{
					RetVal = System.DateTime.Now;
				}
				else
				{
					if (!DateTime.TryParse(Tmp, out RetVal))
					{
						RetVal = System.DateTime.Now;
					}
				}
			}
			catch (Exception ex)
			{
				LogFiles.WriteLog(ex.Message + ": " + Sql, LogFilePath + "\\" + SystemConstants.LogFilePath_Exception, LogFileName + "." + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
			}
			return RetVal;
		}
	}
}