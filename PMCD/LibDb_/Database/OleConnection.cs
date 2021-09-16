using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
namespace Lib.Database
{
    public class OleConnection
    {
        private string connectionString;
        //--------------------------------------------------------------------------------
        public OleConnection(string connStr)
        {
            connectionString = connStr;
        }
        //--------------------------------------------------------------------------------
        public OleDbConnection getConnection()
        {
            try
            {
                return new OleDbConnection(connectionString);
            }
            catch (OleDbException sqlEx)
            {
                throw new ApplicationException(sqlEx.Message);
            }
        }
        //--------------------------------------------------------------------------------
        public OleDbConnection getOLEConnection()
        {
            try
            {
                return new OleDbConnection(connectionString);
            }
            catch (OleDbException sqlEx)
            {
                throw new ApplicationException(sqlEx.Message);
            }
        }
        //--------------------------------------------------------------------------------
        public void closeConnection(OleDbConnection con)
        {
            try
            {
                if (con != null || con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            catch (OleDbException sqlEx)
            {
                throw sqlEx;
            }
        }
        //--------------------------------------------------------------------------------
        #region Management Methods
        public OleDbConnection OpenConnection(string connectionString)
        {
            try
            {
                OleDbConnection mySqlConnection = new OleDbConnection(connectionString);
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception myException)
            {
                throw (new Exception(myException.Message));
            }
        }
        //--------------------------------------------------------------------------------
        public void CloseConnection(OleDbConnection mySqlConnection)
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
        //--------------------------------------------------------------------------------
        public DataSet getDataSet(string strSQL)
        {
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(strSQL, myConn);
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
        //--------------------------------------------------------------------------------
        public OleDbDataReader getReader(OleDbCommand cmd)
        {
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                cmd.Connection = myConn;
                OleDbDataReader reader = cmd.ExecuteReader();
                //myBase.CloseConnection(myConn);
                return reader;
            }
            catch (Exception myException)
            {
                throw (new Exception(myException.Message + " => " + cmd.CommandText));
            }
        }
        //--------------------------------------------------------------------------------
        public OleDbDataReader getReader(OleDbCommand cmd, OleDbConnection con)
        {
            try
            {
                con.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception myException)
            {
                throw (new Exception(myException.Message + " => " + cmd.CommandText));
            }
        }
        //--------------------------------------------------------------------------------
        public DataTable getDataTable(string strSQL)
        {
            return getDataSet(strSQL).Tables[0];
        }
        //--------------------------------------------------------------------------------
        public DataSet getDataSet(string strSQL, OleDbParameter[] Parameters)
        {
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                OleDbCommand myCommand = new OleDbCommand(strSQL, myConn);
                myCommand.Parameters.Clear();
                foreach (OleDbParameter par in Parameters)
                {
                    myCommand.Parameters.Add(par);
                }
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myCommand);
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
        //--------------------------------------------------------------------------------
        public DataTable getDataTable(string strSQL, params OleDbParameter[] Parameters)
        {
            return getDataSet(strSQL, Parameters).Tables[0];
        }
        //--------------------------------------------------------------------------------
        public DataSet getDataSet(OleDbCommand mCommand)
        {
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                mCommand.Connection = myConn;
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(mCommand);
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
        //--------------------------------------------------------------------------------
        public DataTable getDataTable(OleDbCommand mCommand)
        {
            return getDataSet(mCommand).Tables[0];
        }
        //--------------------------------------------------------------------------------
        public void ExecuteSQL(string strSQL)
        {
            OleConnection myBase = new OleConnection(connectionString);
            OleDbConnection myConn = myBase.OpenConnection(connectionString);
            OleDbTransaction tran = myConn.BeginTransaction();

            try
            {
                OleDbCommand myCommand = new OleDbCommand(strSQL, myConn);
                myCommand.Transaction = tran;
                myCommand.ExecuteNonQuery();
                tran.Commit();
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
        //--------------------------------------------------------------------------------
        public void ExecuteSQL(string strSQL, params OleDbParameter[] Parameters)
        {
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                OleDbCommand myCommand = new OleDbCommand(strSQL, myConn);
                myCommand.Parameters.Clear();
                foreach (OleDbParameter par in Parameters)
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
        //--------------------------------------------------------------------------------
        public void ExecuteSQL(string strSQL, IList paraList)
        {
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                OleDbCommand myCommand = new OleDbCommand(strSQL, myConn);
                myCommand.Parameters.Clear();
                foreach (OleDbParameter par in paraList)
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
        //--------------------------------------------------------------------------------
        public void ExecuteSQL(OleDbCommand mCommand)
        {
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                mCommand.Connection = myConn;
                mCommand.ExecuteNonQuery();

                mCommand.Dispose();
                myBase.CloseConnection(myConn);
            }
            catch (Exception myException)
            {
                throw (new Exception(myException.Message + " => " + mCommand.CommandText));
            }

        }
        //--------------------------------------------------------------------------------
        public void ExecuteSQL(string strSQL, OleDbConnection mConn)
        {
            try
            {
                OleDbCommand myCom = new OleDbCommand(strSQL, mConn);
                myCom.ExecuteNonQuery();
                myCom.Dispose();
            }
            catch (Exception myException)
            {
                throw (new Exception(myException.Message + " => " + strSQL));
            }

        }
        //--------------------------------------------------------------------------------
        public int ExecuteScalar(string strSQL)
        {
            int temp = 0;
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                OleDbCommand myCommand = new OleDbCommand(strSQL, myConn);
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
        //--------------------------------------------------------------------------------
        public int ExecuteScalar(string strSQL, params OleDbParameter[] Parameters)
        {
            int temp = 0;
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                OleDbCommand myCommand = new OleDbCommand(strSQL, myConn);
                myCommand.Parameters.Clear();
                foreach (OleDbParameter par in Parameters)
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
        //--------------------------------------------------------------------------------
        public int ExecuteScalar(string strSQL, IList paraList)
        {
            int temp = 0;
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
                OleDbCommand myCommand = new OleDbCommand(strSQL, myConn);
                myCommand.Parameters.Clear();
                foreach (OleDbParameter par in paraList)
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
        //--------------------------------------------------------------------------------
        public int ExecuteScalar(OleDbCommand mCommand)
        {
            int temp = 0;
            OleConnection myBase = new OleConnection(connectionString);
            try
            {
                OleDbConnection myConn = myBase.OpenConnection(connectionString);
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
        //--------------------------------------------------------------------------------
        public int ExecuteScalar(string strSQL, OleDbConnection mConn)
        {
            int temp = 0;
            try
            {
                OleDbCommand myCom = new OleDbCommand(strSQL, mConn);
                temp = Convert.ToInt32(myCom.ExecuteScalar().ToString());
                myCom.Dispose();
                return temp;
            }
            catch (Exception myException)
            {
                throw (new Exception(myException.Message + " => " + strSQL));
            }
        }
        #endregion
    }
}