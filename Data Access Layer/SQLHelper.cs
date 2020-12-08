using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WpfBITProject.Data_Access_Layer
{
    public class SQLHelper
    {
        private string _conn;

        public SQLHelper(string conn)
        {
            _conn = ConfigurationManager.ConnectionStrings[conn].ConnectionString;
        }

        public Object ExecuteSQLScalar(string sql, SqlParameter[] parameters = null, bool storedProcedure = false)
        {
            //create empty return value
            Object returnValue = null;
            //create connection object
            SqlConnection dbConnection = new SqlConnection(_conn);
            //create command object
            //get the SQL statement to execute from the sql parameter
            //passed as input into this method
            SqlCommand dbCommand = new SqlCommand(sql, dbConnection);
            //check if we want to execute a stored procedure
            if (storedProcedure == true)
            {
                dbCommand.CommandType = CommandType.StoredProcedure;
            }
            //if parameters is not null add them to the command object
            if (parameters != null)
            {
                AddParameters(dbCommand, parameters);
            }
            try
            {
                //open database connection
                dbConnection.Open();
                //execute SQL and return data in object
                returnValue = dbCommand.ExecuteScalar();

                dbConnection.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        public DataTable ExecuteSQL(string sql, SqlParameter[] sqlParameters = null, bool storedProcedure = false)
        {
            DataTable dataTable = new DataTable();
            SqlConnection dbConnection = new SqlConnection(_conn);
            SqlCommand dbCommand = new SqlCommand(sql, dbConnection);
            

            if (sqlParameters != null)
            {
                AddParameters(dbCommand, sqlParameters);
            }
            if (storedProcedure == true)
            {
                dbCommand.CommandType = CommandType.StoredProcedure;
            }

            try
            {
                dbConnection.Open();
                SqlDataReader drResults = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                dataTable.Load(drResults);
                return dataTable;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddParameters(SqlCommand objCommand, SqlParameter[] parameters)
        {
            for(int i=0; i < parameters.Length; i++)
            {
                objCommand.Parameters.Add(parameters[i]);
            }
        }

        //you call this method when your query/stored proc are modifying the database
        //meaning you are writing query statements with insert, update or delete command
        //then you call ExecuteNonQuery() method of the sqlCommand
        //and that is why this method
        public int ExecuteNonQuery(string sql, SqlParameter[] sqlParameters = null,
            bool storedProcedure = false)
        {
            int returnValue = -1;
            SqlConnection dbConnection = new SqlConnection(_conn);
            SqlCommand dbCommand = new SqlCommand(sql, dbConnection);//default sqlcommand type is query
            if (sqlParameters != null)
            {
                AddParameters(dbCommand, sqlParameters);
            }
            if (storedProcedure == true)
            {
                dbCommand.CommandType = CommandType.StoredProcedure;
            }
            try
            {
                dbConnection.Open();
                returnValue = dbCommand.ExecuteNonQuery();
                return returnValue;

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}
