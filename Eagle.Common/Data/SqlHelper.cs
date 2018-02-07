using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Eagle.Common.Data
{
    public class SqlHelper
    {
        protected string strConnect = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private SqlConnection conn;
        public SqlCommand sqlCommand;

        public SqlHelper() { }

        public SqlConnection GetConnection
        {
            set
            {
                conn = value;
            }
            get
            {
                return this.conn;
            }
        }

        public void Connect()
        {
            try
            {
                conn = new SqlConnection(strConnect);

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                sqlCommand = conn.CreateCommand();
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return true;
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                return false;
            }
        }

        public DataTable GetDataTable(string strCommand)
        {
            DataTable dt = new DataTable();
            sqlCommand = new SqlCommand(strCommand);
            Connect();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.Fill(dt);
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                return null;
            }
            Disconnect();
            return dt;
        }

        public DataSet GetDataSet(string strCommand)
        {
            Connect();
            DataSet ds = new DataSet();
            sqlCommand = new SqlCommand(strCommand);            
            try
            {                
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.Fill(ds);
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                return null;
            }
            Disconnect();
            return ds;
        }

        public int ExecuteCommand(string strCommand)
        {
            int i = -1;
            try
            {
                Connect();
                sqlCommand = new SqlCommand(strCommand);               
                i = sqlCommand.ExecuteNonQuery();
                Disconnect();
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                return i;
            }
            return i;
        }

        public int ExecuteNonQuery(string queryName, CommandType commandType, string[] parameterName, object[] parameterValue)
        {
            int x = -1;
            try
            {
                Connect();
                sqlCommand = new SqlCommand(queryName);
                sqlCommand.CommandType = commandType;
                for (int i = 0; i < parameterName.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue(parameterName[i], parameterValue[i]);
                }     
                x = sqlCommand.ExecuteNonQuery();
                Disconnect();
            }
            catch (Exception exc)
            {
                x = -1;
                exc.Message.ToString();
            }
            
            return x;
        }

        public SqlCommand ExecNoneQueryWithOutput(string spName, params SqlParameter[] param)
        {
            Connect();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = spName;
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] != null)
                    sqlCommand.Parameters.Add(param[i]);
            }
            try
            {               
                sqlCommand.ExecuteNonQuery();              
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e.ToString());
            }
            Disconnect();
            return sqlCommand;
        }

        public SqlDataReader ExecuteReader(string strsql)
        {
            Connect();
            sqlCommand.CommandText = strsql;            
            SqlDataReader dr = sqlCommand.ExecuteReader();
            Disconnect();
            return dr;
        }

        public object ExecuteScalar(string strCommand)
        {
            Connect();
            sqlCommand.CommandText = strCommand;            
            object obj = sqlCommand.ExecuteScalar();
            Disconnect();
            return obj;
        }

        public string ExecScalar(string spName, params SqlParameter[] param)
        {
            Connect();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = spName;
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] != null)
                    sqlCommand.Parameters.Add(param[i]);
            }
            string strRe = "";
            try
            {               
                strRe = sqlCommand.ExecuteScalar().ToString();
                Disconnect();
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e.ToString());
            }            
            return strRe;
        }

        public int ExecuteStoredProcedure(string spName)
        {
            Connect();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = spName;
            // Return value as parameter
            SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;
            sqlCommand.Parameters.Add(returnValue);
            try
            {                
                sqlCommand.ExecuteNonQuery();                
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e.ToString());
            }
            Disconnect();
            return Convert.ToInt32(returnValue.Value);
        }

        public void ExecuteStoreProcedure(string spName, params SqlParameter[] param)
        {
            Connect();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = spName;
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] != null)
                    sqlCommand.Parameters.Add(param[i]);
            }
            try
            {                
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e.ToString());
            }
            Disconnect();
        }

        public int ExecuteStoreProcedure(string sp_Name, params object[] pars)
        {
            int x = -1;
            try
            {
                Connect();
                sqlCommand = new SqlCommand(sp_Name, this.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < pars.Length; i += 2)
                {
                    SqlParameter par = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                    sqlCommand.Parameters.Add(par);
                }
                x = sqlCommand.ExecuteNonQuery();
                Disconnect();
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                return x;
            }
            return x;
        }

        public int CheckPrimaryKey(string queryName, CommandType commandType, string[] parameterName, object[] parameterValue)
        {
            int isDuplicate = -1;
            try
            {
                Connect();
                sqlCommand = new SqlCommand(queryName, this.GetConnection);
                sqlCommand.CommandType = commandType;
                for (int i = 0; i < parameterName.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue(parameterName[i], parameterValue[i]);
                }
                isDuplicate = Convert.ToInt32(sqlCommand.ExecuteScalar());
                Disconnect();
            }
            catch (Exception ex)
            {
                isDuplicate = -1;
                ex.Message.ToString();
            }
            return isDuplicate;
        }

        public int GetMaxRequestLength()
        {
            // presume that the section is not defined in the web.config
            int defaultSize = 4096;

            System.Web.Configuration.HttpRuntimeSection section = System.Configuration.ConfigurationManager.GetSection("system.web/httpRuntime") as System.Web.Configuration.HttpRuntimeSection;
            if (section != null) defaultSize = section.MaxRequestLength;

            return defaultSize;
        }
    }
}
