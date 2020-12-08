using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Add some using references!
using System.Collections;
using System.Data;              //Contains the disconnected classes
using System.Data.SqlClient;    //Contains the connected classes
//using System.Data.SqlTypes;
using System.Diagnostics;       //Contains classes for 'debuggin' Debug.Print
using System.Xml;
using System.Configuration;
using System.IO;
using System.Security.Principal;


namespace WpfBITProject.Data_Access_Layer
{

    //The "sealed" keyword makes this class "not inheritable" - it can not be used as the base class for any derived class.
    //This class inherits from System.ComponentModel.Component which provides functionality for object sharing between applications.

    /// <summary>
    /// Handles connection to the database and processing of queries and stored procedures.
    /// </summary>
    public sealed class DataAccessLayer : System.ComponentModel.Component
    {

        #region Private Member Variables (Fields/Properties)

        //The underscore convention is used to denote a class level private member variable (private property).
        //FIRST: ADO.NET objects (needed for data access) are defined
        private SqlConnection _sqlConnection;
        private SqlCommand _sqlCommand;
        private SqlDataAdapter _sqlDataAdapter;
        private SqlDataReader _sqlDataReader;
        private DataSet _dataSet;
        private DataTable _dataTable;
        private XmlReader _xmlReader;
        /*
         * Parameters are going to be handled internally and this array is going to hold them.
         * This will allow us to create parameters in the calling code without having the SqlCommand 
         * object initialised. We will handle the conversion to SqlParameters internally.
         * The _parameterList will hold the converted parameters and will be used by methods that call
         * stored procedures.
         */
        private ArrayList _parameterList = new ArrayList();


        //SECOND: Connection string variables are defined
        private string _connectionString;
        private string _server;
        private string _database;
        private string _username;
        private string _password;

        //THIRD: Naming, cleanup and exception variables are defined
        private string _moduleName;
        private const string _exceptionMessage = "Data application error. Detailed error information can be found in the application log.";
        private bool _disposedBoolean;

        #endregion

        #region Public Properties

        //This enumeration will be used to conver the "non-SQL Server" data types passed from the 
        // UI tier or the BO tier to this DAL.
        public enum GenericDataType
        {
            genString,
            genChar,
            genInteger,
            genBit,
            genDateTime,
            genDate,
            genTime,
            genDecimal,
            genMoney,
            genImage,
            genTableType
        }

        public string ConnectionString
        {
            get
            {
                //If the ConnectionString doesn't exist, catch the exception and return an empty string
                try
                {
                    return _sqlConnection.ConnectionString;
                }
                catch
                {
                    return "";
                }
            }
            set { _connectionString = value; }
        }

        public string Server
        {
            get
            {
                try
                {
                    return _server;
                }
                catch
                {
                    return "";
                }
            }

            set { _server = value; }
        }

        public string Database
        {
            get
            {
                try
                {
                    return _database;
                }
                catch
                {
                    return "";
                }
            }

            set { _database = value; }
        }

        public string Username
        {
            get
            {
                try
                {
                    return _username;
                }
                catch
                {
                    return "";
                }
            }

            set { _username = value; }
        }

        public string Password
        {
            get
            {
                try
                {
                    return _password;
                }
                catch
                {
                    return "";
                }
            }

            set { _password = value; }
        }


        #endregion

        #region Public Constructors

        /*
         * There are 3 constructors:
         *  1st: has no parameters, it just stores the module name.
         *  2nd: is used if we know the connection string & we can pass it to the constructor.
         *  3rd: accepts all the parts of the connection string and then builds it internally.
         */
        
        /// <summary>
        /// Create a data access layer object.
        /// </summary>
        public DataAccessLayer() : base()
        {
            _moduleName = this.GetType().ToString();
        }

        /// <summary>
        /// Create a data access layer object giving a full connection string.
        /// </summary>
        /// <param name="connectionString">A connection string for the database</param>
        public DataAccessLayer(string connectionString) : base()
        {
            //Assign the connection string to our private member variable
            _connectionString = connectionString;
            _moduleName = this.GetType().ToString();
        }

        /// <summary>
        /// Create a data access layer object giving all database connection details.
        /// </summary>
        /// <param name="server">The database server name</param>
        /// <param name="database">The database name</param>
        /// <param name="username">The database username</param>
        /// <param name="password">The database password</param>
        public DataAccessLayer(string server, string database, string username, string password) : base()
        {
            //Concatenate database connection details together to form a connection string
            //Server=???;Database=???;User ID=???;Password=???;
            _connectionString = "Server=" + server + ";Database=" + database + ";User ID=" + username + ";Password=" + password + ";";
            _moduleName = this.GetType().ToString();
        }


        #endregion
        
        #region Data Access Methods

        //Run SQL data table
        /// <summary>
        /// Executes an SQL string (strSQL) and returns a DataTable object.
        /// Use for non-updatable, forward only, read only data access.
        /// </summary>
        /// <param name="strSql">string: The SQL Statement to execute</param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable RunSqlDataTable(string strSql)
        {
            //Validate the SQL string (longer than 10 characters...)
            ValidateSqlStatement(strSql);

            //All code past this point is included in the try...catch block to catch any exceptions that occur (including creating the connection)
            try
            {
                //Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                //Create connection
                _sqlConnection = new SqlConnection(_connectionString);

                //Create command using the SQL statement and the connection
                //The CommandType doesn't have to be set because the default is "Text"
                _sqlCommand = new SqlCommand(strSql, _sqlConnection);

                //Open the connection - need to explicitly open the connection for the DataReader (ExecuteReader)
                _sqlCommand.Connection.Open();

                //Execute the command
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                //Translate reader into a data table
                _dataTable = new DataTable();
                _dataTable.Load(_sqlDataReader, LoadOption.OverwriteChanges);

                //Return the data table
                return _dataTable;

            }
            catch (Exception ex)
            {
                //Log the exception through our private LogException method
                LogException(ex);

                //If an exception occurs, it is passed back to the calling code with our custom message and specific exception information
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                //Check if the connection is still open - close it
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }

        //Run SP data table
        public DataTable RunSPDataTable(string SPName)
        {
            //Validate the SQL string (longer than 10 characters...)
            ValidateSPStatement(SPName);

            //All code past this point is included in the try...catch block to catch any exceptions that occur (including creating the connection)
            try
            {
                //Setting objects to handle parameters
                Parameter myGenericParameter;

                SqlParameter mySqlParameter;

                //The 'usedEnumerator makes it easy to step through the ArrayList of parameters
                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                //Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                //Create connection
                _sqlConnection = new SqlConnection(_connectionString);

                //Define the command object and set the commandType to process a Stored Procedure
                _sqlCommand = new SqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a SqlParameter
                    mySqlParameter = ConvertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(mySqlParameter);
                }
                

                //Open the connection - need to explicitly open the connection for the DataReader (ExecuteReader)
                _sqlCommand.Connection.Open();

                //Execute the command
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                //Translate reader into a data table
                _dataTable = new DataTable();
                _dataTable.Load(_sqlDataReader, LoadOption.OverwriteChanges);

                //Return the data table
                return _dataTable;

            }
            catch (Exception ex)
            {
                //Log the exception through our private LogException method
                LogException(ex);

                //If an exception occurs, it is passed back to the calling code with our custom message and specific exception information
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                //Check if the connection is still open - close it
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }

        //Run SP scalar
        /// <summary>
        /// Executes a Stored Procedure (SPName) and returns a scalar
        /// (the value from the first column, first row intersection of the Stored Procedure result set)
        /// </summary>
        /// <param name="SPName">string: The name of the Stored Procedure to execute</param>
        /// <returns>object</returns>
        public object RunSPScalar(string SPName)
        {
            //Validate the SQL string (longer than 10 characters...)
            ValidateSPStatement(SPName);

            //All code past this point is included in the try...catch block to catch any exceptions that occur (including creating the connection)
            try
            {
                //Setting objects to handle parameters
                Parameter myGenericParameter;

                SqlParameter mySqlParameter;

                //The 'usedEnumerator makes it easy to step through the ArrayList of parameters
                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                //Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                //Create connection
                _sqlConnection = new SqlConnection(_connectionString);

                //Define the command object and set the commandType to process a Stored Procedure
                _sqlCommand = new SqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a SqlParameter
                    mySqlParameter = ConvertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(mySqlParameter);
                }


                //Open the connection - need to explicitly open the connection for the DataReader (ExecuteReader)
                _sqlCommand.Connection.Open();

                /*  ExecuteScalar returns the first column of the first row in the result set.  Additional columns or rows are ignored.
                 */
                object objResult = _sqlCommand.ExecuteScalar();
                return objResult;

            }
            catch (Exception ex)
            {
                //Log the exception through our private LogException method
                LogException(ex);

                //If an exception occurs, it is passed back to the calling code with our custom message and specific exception information
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                //Check if the connection is still open - close it
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }
        //Run SP data set
        /// <summary>
        /// Executes a Stored Procedure and returns a DataSet object.
        /// </summary>
        /// <param name="SPName">string: The name of the Stored Procedure to execute.</param>
        /// <returns>DataSet</returns>
        public DataSet RunSPDataSet(string SPName)
        {
            //Validate the SQL string (longer than 10 characters...)
            ValidateSPStatement(SPName);

            //All code past this point is included in the try...catch block to catch any exceptions that occur (including creating the connection)
            try
            {
                //Setting objects to handle parameters
                Parameter myGenericParameter;

                SqlParameter mySqlParameter;

                //The 'usedEnumerator makes it easy to step through the ArrayList of parameters
                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                //Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                //Create connection
                _sqlConnection = new SqlConnection(_connectionString);

                //Define the command object and set the commandType to process a Stored Procedure
                _sqlCommand = new SqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a SqlParameter
                    mySqlParameter = ConvertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(mySqlParameter);
                }

                //We're going to use a DataAdapter which we'll use to Fill the DataSet object.
                _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
                _dataSet = new DataSet();
                //The SqlDataAdapter will manage the Opening and Closing of the connection automatically.
                _sqlDataAdapter.Fill(_dataSet);

                return _dataSet;

            }
            catch (Exception ex)
            {
                //Log the exception through our private LogException method
                LogException(ex);

                //If an exception occurs, it is passed back to the calling code with our custom message and specific exception information
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                //Check if the connection is still open - close it
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }
        //Run SP XML reader (?)
        /*  No ADO.Net objects need to be declared from the calling code, since a string is returned.
         *  Since XML is returned the calling code needs to deal with XML.  The Stored Procedure MUST
         *  use the FOR XML clause so that only XML is returned to this DataAccessLayer
         */
         /// <summary>
         /// Executes a Stored Procedure and returns XML as a string.
         /// The Stored Procedure MUST use the 'FOR XML' clause and only return XML.
         /// </summary>
         /// <param name="SPName">string: The name of the Stored Procedure to execute</param>
         /// <returns>XML as string</returns>
        public string RunSPXMLReader(string SPName)
        {
            //Validate the SQL string (longer than 10 characters...)
            ValidateSPStatement(SPName);

            //All code past this point is included in the try...catch block to catch any exceptions that occur (including creating the connection)
            try
            {
                //Setting objects to handle parameters
                Parameter myGenericParameter;

                SqlParameter mySqlParameter;

                //The 'usedEnumerator makes it easy to step through the ArrayList of parameters
                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                //Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                //Create connection
                _sqlConnection = new SqlConnection(_connectionString);

                //Define the command object and set the commandType to process a Stored Procedure
                _sqlCommand = new SqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a SqlParameter
                    mySqlParameter = ConvertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(mySqlParameter);
                }


                string returnXMLString = string.Empty;

                _sqlCommand.Connection.Open();
                _xmlReader = _sqlCommand.ExecuteXmlReader();

                //Loop through the reader and build the XML string.
                while (_xmlReader.Read())
                {
                    returnXMLString += _xmlReader.ReadOuterXml();
                }

                return returnXMLString;

            }
            catch (Exception ex)
            {
                //Log the exception through our private LogException method
                LogException(ex);

                //If an exception occurs, it is passed back to the calling code with our custom message and specific exception information
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                //Check if the connection is still open - close it
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }


        //Run SP non-query
        /// <summary>
        /// Executes a stored procedure and returns the number of rows affected. Use for INSERTs, UPDATEs and DELETEs.
        /// If the stored procedure fails, an Exception will be thrown.
        /// </summary>
        /// <param name="SPName">The name of the stored procedure to execute.</param>
        /// <returns>The number of rows affected (int).</returns>
        public int RunSPExecNonQuery(string SPName)
        {
            //Validate the stored procedure name
            ValidateSPStatement(SPName);

            //All code past this point is included in the try...catch block to catch any exceptions that occur (including creating the connection)
            try
            {
                //Setting objects to handle parameters
                Parameter myGenericParameter;

                SqlParameter mySqlParameter;

                //The 'usedEnumerator makes it easy to step through the ArrayList of parameters
                IEnumerator usedEnumerator = _parameterList.GetEnumerator();

                //Check to see if this object has already been disposed
                if (_disposedBoolean)
                {
                    throw new ObjectDisposedException(_moduleName, "This object has already been disposed. You cannot reuse it.");
                }

                //Create connection
                _sqlConnection = new SqlConnection(_connectionString);

                //Define the command object and set the commandType to process a Stored Procedure
                _sqlCommand = new SqlCommand(SPName, _sqlConnection);
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //Loop through the parameter list and add each one to the command object (with the help of the enumerator)
                while (usedEnumerator.MoveNext())
                {
                    myGenericParameter = null;
                    myGenericParameter = (Parameter)usedEnumerator.Current;
                    //Convert the parameter to a SqlParameter
                    mySqlParameter = ConvertParameters(myGenericParameter);
                    _sqlCommand.Parameters.Add(mySqlParameter);
                }

                //Open the connection and execute the command. 
                //If the SP fails, an exception will be thrown, otherwise the number of rows affected will be returned.
                _sqlCommand.Connection.Open();
                return _sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Log the exception through our private LogException method
                LogException(ex);

                //If an exception occurs, it is passed back to the calling code with our custom message and specific exception information
                throw new Exception(_exceptionMessage, ex);
            }
            finally
            {
                //Check if the connection is still open - close it
                if (_sqlCommand.Connection.State == ConnectionState.Open)
                {
                    _sqlCommand.Connection.Close();
                }
            }
        }


        #endregion

        #region Parameter Public Methods and Support Functions

        #region Parameter Class

        /// <summary>
        /// This class is used to represent our 'generic' (non SqlServer) parameter
        /// </summary>
        private class Parameter
        {

            #region Private and Public Fields and Properties
            private string _parameterName;
            private object _parameterValue;  //I don't know what 'type' the value will be so I define it as an 'object'
            private SqlDbType _parameterDataType;
            private int _parameterSize;
            private ParameterDirection _parameterDirection;
            //------------added for passing User Defined Table Types to SQL Stored Procs.
            private string _parameterUserDefinedTypeName;


            public string ParameterUserDefinedTypeName
            {
                get
                {
                    return _parameterUserDefinedTypeName;
                }
                set
                {
                    _parameterUserDefinedTypeName = value;
                }
            }
            public string ParameterName
            {
                get
                {
                    return _parameterName;
                }
                set
                {
                    _parameterName = value;
                }
            }

            public object ParameterValue
            {
                get
                {
                    return _parameterValue;
                }
                set
                {
                    _parameterValue = value;
                }
            }

            public SqlDbType ParameterDataType
            {
                get
                {
                    return _parameterDataType;
                }
                set
                {
                    _parameterDataType = value;
                }
            }

            public int ParameterSize
            {
                get
                {
                    return _parameterSize;
                }
                set
                {
                    _parameterSize = value;
                }
            }

            public ParameterDirection Parameterdirection
            {
                get
                {
                    return _parameterDirection;
                }
                set
                {
                    _parameterDirection = value;
                }
            }

            #endregion Private and Public Fields and Properties

            #region Constructors

            public Parameter(string passedName)
            {
                _parameterName = passedName;
            }

            public Parameter(string passedName,
                             object passedValue)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
            }

            public Parameter(string passedName,
                             object passedValue,
                             SqlDbType passedSqlType)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
                _parameterDataType = passedSqlType;
            }
            //--------------added to support passing User Defined Table Types to SQL Stored Procs
            public Parameter(string passedName,
                             object passedValue,
                             SqlDbType passedSqlType,
                             string passedUserDefinedTypeName)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
                _parameterDataType = passedSqlType;
                _parameterUserDefinedTypeName = passedUserDefinedTypeName;
            }

            public Parameter(string passedName,
                             object passedValue,
                             SqlDbType passedSqlType,
                             int passedSize)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
                _parameterDataType = passedSqlType;
                _parameterSize = passedSize;
            }

            public Parameter(string passedName,
                             object passedValue,
                             SqlDbType passedSqlType,
                             int passedSize,
                             ParameterDirection passedDirection)
            {
                _parameterName = passedName;
                _parameterValue = passedValue;
                _parameterDataType = passedSqlType;
                _parameterSize = passedSize;
                _parameterDirection = passedDirection;
            }
            #endregion Constructors

            

        }
        #endregion Parameter Class


        #endregion

        #region addParameter Public Methods
        /*  The minimum requirement for SQL Server
         *  Stored Procedures is the Parameter name, value and Data type
         */
         /// <summary>
         /// Allows non SqlDbType parameters to be passed to the DataAccessLayer which
         /// will convert the parameters internally to SQL Server specific data type
         /// System.Data.SqlDbType for use in System.Data.SqlClient.SqlParameter
         /// </summary>
         /// <param name="parameterName">System.String The name of the Parameter</param>
         /// <param name="value">System.object The value of the Parameter</param>
         /// <param name="genType">DataAccessLayer.GenericDataType The generic data type of the Parameter</param>
        public void AddParameter(string parameterName,
                                 object value,
                                 GenericDataType genType)
        {
            SqlDbType targetSqlDataType = default(SqlDbType);
            Parameter buildParameter = null;

            //Call to converToSqlDbType that does the conversion
            targetSqlDataType = ConvertToSqlDbType(genType);
            buildParameter = new Parameter(parameterName, value, targetSqlDataType);
            _parameterList.Add(buildParameter);
        }

        /// <summary>
        /// Allows non SqlDbType parameters to be passed to the DataAccessLayer which
        /// will convert the parameters internally to SQL Server specific data type
        /// System.Data.SqlDbType for use in System.Data.SqlClient.SqlParameter
        /// </summary>
        /// <param name="parameterName">System.String The name of the Parameter</param>
        /// <param name="value">System.object The value of the Parameter</param>
        /// <param name="genType">DataAccessLayer.GenericDataType The generic data type of the Parameter</param>
        /// <param name="UserDefinedTableTypeName">System.String The Type Name of the SQL Server User-Defined Table Type.</param>
        public void AddParameter(string parameterName,
                                 object value,
                                 GenericDataType genType,
                                 string UserDefinedTableTypeName)
        {
            SqlDbType targetSqlDataType = default(SqlDbType);
            Parameter buildParameter = null;

            //Call to converToSqlDbType that does the conversion
            targetSqlDataType = ConvertToSqlDbType(genType);
            buildParameter = new Parameter(parameterName, value, targetSqlDataType, UserDefinedTableTypeName);
            _parameterList.Add(buildParameter);
        }

        /// <summary>
        /// Allows non SqlDbType parameters to be passed to the DataAccessLayer which
        /// will convert the parameters internally to SQL Server specific data type
        /// System.Data.SqlDbType for use in System.Data.SqlClient.SqlParameter
        /// </summary>
        /// <param name="parameterName">System.String The name of the Parameter</param>
        /// <param name="value">System.object The value of the Parameter</param>
        /// <param name="genType">DataAccessLayer.GenericDataType The generic data type of the Parameter</param>
        /// <param name="size">System.Int32 This size of the Parameter</param>
        public void AddParameter(string parameterName,
                                 object value,
                                 GenericDataType genType,
                                 int size)
        {
            SqlDbType targetSqlDataType = default(SqlDbType);
            Parameter buildParameter = null;

            //Call to converToSqlDbType that does the conversion
            targetSqlDataType = ConvertToSqlDbType(genType);
            buildParameter = new Parameter(parameterName, value, targetSqlDataType, size);
            _parameterList.Add(buildParameter);
        }

        /// <summary>
        /// Allows non SqlDbType parameters to be passed to the DataAccessLayer which
        /// will convert the parameters internally to SQL Server specific data type
        /// System.Data.SqlDbType for use in System.Data.SqlClient.SqlParameter
        /// </summary>
        /// <param name="parameterName">System.String The name of the Parameter</param>
        /// <param name="value">System.object The value of the Parameter</param>
        /// <param name="genType">DataAccessLayer.GenericDataType The generic data type of the Parameter</param>
        /// <param name="size">System.Int32 This size of the Parameter</param>
        /// <param name="direction">System.Data.ParameterDirection  The direction of the Parameter (Input/Output)</param>
        public void AddParameter(string parameterName,
                                 object value,
                                 GenericDataType genType,
                                 int size,
                                 ParameterDirection direction)
        {
            SqlDbType targetSqlDataType = default(SqlDbType);
            Parameter buildParameter = null;

            //Call to converToSqlDbType that does the conversion
            targetSqlDataType = ConvertToSqlDbType(genType);
            buildParameter = new Parameter(parameterName, value, targetSqlDataType, size, direction);
            _parameterList.Add(buildParameter);
        }

        #endregion addParameter Public Methods

        #region Parameter Conversion

        private SqlParameter ConvertParameters(Parameter passedParameter)
        {
            SqlParameter convertedSqlParam = new SqlParameter();
            convertedSqlParam.ParameterName = passedParameter.ParameterName;
            convertedSqlParam.Value = passedParameter.ParameterValue;
            convertedSqlParam.SqlDbType = passedParameter.ParameterDataType;
            convertedSqlParam.Size = passedParameter.ParameterSize;
            convertedSqlParam.Direction = passedParameter.Parameterdirection;
            // ------------added for table valued types
            convertedSqlParam.TypeName = passedParameter.ParameterUserDefinedTypeName;

            return convertedSqlParam;
        }

        private SqlDbType ConvertToSqlDbType(GenericDataType typeToConvert)
        {
            SqlDbType convertedType;
            switch (typeToConvert)
            {
                case GenericDataType.genString:
                    convertedType = SqlDbType.NVarChar;
                    break;
                case GenericDataType.genChar:
                    convertedType = SqlDbType.NChar;
                    break;
                case GenericDataType.genInteger:
                    convertedType = SqlDbType.Int;
                    break;
                case GenericDataType.genBit:
                    convertedType = SqlDbType.Bit;
                    break;
                case GenericDataType.genDateTime:
                    convertedType = SqlDbType.DateTime;
                    break;
                case GenericDataType.genDate:
                    convertedType = SqlDbType.Date;
                    break;
                case GenericDataType.genTime:
                    convertedType = SqlDbType.Time;
                    break;
                case GenericDataType.genDecimal:
                    convertedType = SqlDbType.Decimal;
                    break;
                case GenericDataType.genMoney:
                    convertedType = SqlDbType.Money;
                    break;
                case GenericDataType.genImage:
                    convertedType = SqlDbType.Image;
                    break;
                case GenericDataType.genTableType:
                    convertedType = SqlDbType.Structured;
                    break;
                default:
                    convertedType = SqlDbType.NVarChar;
                    break;
            }

            return convertedType;
        }

        #endregion Parameter Conversion

        #region Validation
        /*  These validations are really just stubs (place holders for where we would do more meaningful validation).
         */
        private void ValidateSqlStatement(string strSql)
        {
            //SQL statement must be at least 10 characters long (SELECT * FROM x)
            if (strSql.Length < 10)
            {
                throw new Exception(_exceptionMessage + " The SQL statement must be provided and must be at least 10 characters long");
            }
        }

        private void ValidateSPStatement(string SPName)
        {
            //Stored Proc name should be greater than 1 character
            if(SPName.Length < 1)
            {
                throw new Exception(_exceptionMessage + " The Name of the Stored Procedure should be at least 1 character long");
            }
        }

        #endregion

        #region Exception Logging

        private void LogException(Exception ex)
        {
            //This is the message that will be passed to the log
            string eventLogMessage;

            try
            {
                /*
                 * Create the message to be logged:
                 *  .Source - gets the name of the application or object that causes the error
                 *  .Message - gets a message that describes the current exception
                 *  .StackTrace - gets a string representation of the frames on the call stack at the time the exception was thrown
                 *  .TargetSite - gets the method that throws the current exception
                 */
                eventLogMessage = "An error occurred in the following module: " + _moduleName + Environment.NewLine +
                                  "The source (application/object) was: " + ex.Source + Environment.NewLine +
                                  "The message was: " + ex.Message + Environment.NewLine +
                                  "Stack trace: " + ex.StackTrace + Environment.NewLine +
                                  "The target site (method) was: " + ex.TargetSite.ToString();

                //Define EventLog as an Application log entry (as opposed to Security log, System log, etc)
                EventLog myEventLog = new EventLog("Application");

                //Set the source of the EventLog
                myEventLog.Source = _moduleName;

                //Write the entry to Application log
                myEventLog.WriteEntry(eventLogMessage, EventLogEntryType.Error);
                    
            }
            catch (Exception eventLogException)
            {
                //If logging the event fails (e.g. the user account is not a member of the debugger group), we pass on the error
                throw new Exception("EventLog Error: " + eventLogException.Message, eventLogException);
            }
        }

        #endregion

        #region Overlridden dispose method

        protected override void Dispose(bool disposing)
        {

            //Check if Dispose has already been called, if not we can proceed
            //if (!_disposedBoolean)
            if (_disposedBoolean == false)
            {
                try
                {
                    //Dispose of and free up any resources used by the connection (if not already done)
                    _sqlConnection.Dispose();
                }
                catch (Exception ex)
                {
                    //Log the exception
                    LogException(ex);
                }
                finally
                {
                    //Call the base class's (Component) Dispose method
                    base.Dispose(disposing);

                    //Suppress the Garbage Collector (GC) having to take 2 passes
                    GC.SuppressFinalize(this);

                    //Set the disposed boolean flag
                    //This flag lets us check between disposal and garbage collection so we don't resurrect the object falesly
                    _disposedBoolean = true;
                }
            }

            
        }

        #endregion

    }
}
