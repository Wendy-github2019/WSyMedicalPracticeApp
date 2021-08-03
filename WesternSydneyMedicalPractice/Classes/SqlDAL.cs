using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//Add the following References
using System.Data;  //Contains the 'disconnected' objects (classes)
using System.Data.SqlClient;    //contains connect objects(classes) specific to SQL Server
using System.Configuration; //Contains the classes that allow us to read items from the app.config file


namespace WesternSydneyMedicalPractice
{
    class SqlDataAccesslayer
    {
        #region Field Variables (private)
        private string _connString;
        private string _connStringName;
        #endregion Field Variables (private)

        #region Constructors
        //Add a constructors that reads the app.config file. gets the conncection string and assigns it the the field variable _connString. Use this to read tge app.config file;
        //ConfigurationManager.ConnectionString["cnnStrWSMP"].ConnectionString;

        public SqlDataAccesslayer(string cnnStrName)
        {
            this._connString = ConfigurationManager.ConnectionStrings[cnnStrName].ConnectionString;
        }

        public SqlDataAccesslayer()
        {
        }

        //public SqlDataAccesslayer()
        //{
        //    //Default constructor, does nothing           
        //}
        #endregion

        #region public Properties ConnStringName

        public string ConnStringName
        {
            get
            {
                return _connStringName;
            }
            set 
            {
                _connStringName = value;
                _connString = ConfigurationManager.ConnectionStrings[value].ConnectionString;
            }
        }
        #endregion

        #region Public Methods

        #region ExecuteStoredProc(SPName)
        /// <summary>
        /// Excute a stored Procedure (without parameters) and returns a DataTable result set;
        /// </summary>
        /// <param name="SPName">String: The name of Strored Procedure to be executed.</param>
        /// <returns>DataTable: DataTable containing the redults of the stored Procedure execution</returns>
        public DataTable ExecuteStoredProc(string SPName)
        {
            //create a connection object. Objects prefixed with 'Sql...' are connected (SQL server Specific ) objects.
            SqlConnection conn = new SqlConnection(_connString);

            //Create a command object, which we use to specify the name of the stored proc or sql
            //that we want to excute on the database
            SqlCommand cmd = new SqlCommand(SPName, conn);
            //CommandType is a text,  e.x. a string of SQL
            cmd.CommandType = CommandType.StoredProcedure;


            try
            {
                //Try and open the connection to the database
                cmd.Connection.Open();

                //actually tries to execute cmd connect to database
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);

                throw;
            }
        }
        #endregion

        #region ExecuteStoredProc(SPName, Parameters)
        public DataTable ExecuteStoredProc(string SPName, SqlParameter[] storedProcedureParas)
        {
            //create a connection object. Objects prefixed with 'Sql...' are connected (SQL server Specific ) objects.
            SqlConnection conn = new SqlConnection(_connString);

            //Create a command object, which we use to specify the name of the stored proc or sql
            //that we want to excute on the database

            SqlCommand cmd = new SqlCommand(SPName, conn);
            //CommandType is a text,  e.x. a string of SQL
            cmd.CommandType = CommandType.StoredProcedure;

            //call the fillParameters method passing to it the command object and the array of parameter
            FillParameters(cmd, storedProcedureParas);

            try
            {
                //Try and open the connection to the database
                cmd.Connection.Open();

                //actually tries to execute cmd connect to database
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);

                throw;
            }
        }

        #endregion

        #region ExecuteNonQuerySP(SPName)
        /// <summary>
        /// Execute Non-Query Stored Procedure without any parameters
        /// </summary>
        /// <param name="SPName">String: a string of Sql</param>
        /// <returns>int: number of rows to be affected </returns>
        public int ExecuteNonQuerySP(string SPName)
        {
            //create a connection object. Objects prefixed with 'Sql...' are connected (SQL server Specific ) objects.
            SqlConnection conn = new SqlConnection(_connString);

            //Create a command object, which we use to specify the name of the stored proc or sql
            //that we want to excute on the database

            SqlCommand cmd = new SqlCommand(SPName, conn);
            //CommandType is a text,  e.x. a string of SQL
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                //Try and open the connection to the database
                cmd.Connection.Open();

                //actually tries to execute cmd connect to database
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);

                throw;
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion        

        #region ExecuteNonQuerySP(SPName, Parameters)
        /// <summary>
        /// Execute Non-Query Stored Procedure with parameters for INSERTs, UPDATEs, or DELETEs.
        /// </summary>
        /// <param name="SPName">String: a string of Sql</param>
        /// <param name="parameters">SqlParameter[]: An array of sqlParameters required by the stroed Procedure. </param>
        /// <returns>int: indicating the number of rows to be affected </returns>
        public int ExecuteNonQuerySP(string SPName, SqlParameter[] storedProcedureParas)
        {
            //create a connection object. Objects prefixed with 'Sql...' are connected (SQL server Specific ) objects.
            SqlConnection conn = new SqlConnection(_connString);

            //Create a command object, which we use to specify the name of the stored proc or sql
            //that we want to excute on the database

            SqlCommand cmd = new SqlCommand(SPName, conn);
            //CommandType is a text,  e.x. a string of SQL
            cmd.CommandType = CommandType.StoredProcedure;

            //call the fillParameters method passing to it the command object and the array of parameter
            FillParameters(cmd, storedProcedureParas);

            try
            {
                //Try and open the connection to the database
                cmd.Connection.Open();

                //actually tries to execute cmd connect to database
                //SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);

                throw;
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }

        #endregion        

        #region FillParameters Method

        //reference type, create a pointer, when object change, method reference to it also changed.
        private void FillParameters(SqlCommand cmd, SqlParameter[] parameters)
        {
            foreach (SqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
        }
        #endregion

        #region ExecuteSql(sql)
        /// <summary>
        ///Excute a string of Sql with dataTable result returned( e.g.Select)
        /// </summary>
        /// <param name="sql">String:</param>
        /// <returns>DataTable: dataTable result returned</returns>
        public DataTable ExecuteSql(string sql)
        {
            //open a connection to database
            SqlConnection conn = new SqlConnection(_connString);


            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open(); //open a conncection a cmd project use, being a little bit more precise than conn.open();

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);//close the connection when it finished, 


                DataTable dataTable = new DataTable(); //it's a generic datable, can be used in other project
                dataTable.Load(dataReader);

                return dataTable;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion

        #region ExecuteNonQuerySql(sql)
        /// <summary>
        ///Excute a non-query Sql, with no dataTable result returned( e.g. INSERTs, UPDATEs and DELETEs) usign an 'embeded' SQL query string
        /// </summary>
        /// <param name="sql">String: The Sql commmand to be executed</param>
        /// <returns>Int: dataTable result returned</returns>
        public int ExecuteNonQuerySql(string sql)
        {
            //open a connection to database
            SqlConnection conn = new SqlConnection(_connString);

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open(); //open a conncection a cmd project use, being a little bit more precise than conn.open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion

        #region ExecuteNonQuerySql(sql, parameters)
        /// <summary>
        /// Execute a nonquery sql(insert, update, delete)
        ///Excute a string of Sql with no dataTable result returned( e.g.Select)
        /// </summary>
        /// <param name="sql">String:</param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters</param>
        /// <returns>Int: the number of rows affected by the query</returns>
        public int ExecuteNonQuerySql(string sql, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connString);

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text; // type is text, is default

            FillParameters(cmd, parameters);

            try
            {
                cmd.Connection.Open(); //open a conncection a cmd project use, being a little bit more precise than conn.open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion

        #region ExecuteScalarSql(sql)
        /// <summary>
        ///Excute a string of Sql and returns the value at the first row/colunm intersection of the result set, and ignor the rest( e.g.Select)
        /// </summary>
        /// <param name="sql">String: The SAL command to be executed. </param>
        /// <returns>object: reture  a single value as type objects. </returns>
        public object ExecuteScalarSql(string sql)
        {
            //open a connection to database
            SqlConnection conn = new SqlConnection(_connString);


            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open(); //open a conncection a cmd project use, being a little bit more precise than conn.open();

                object retureValue = cmd.ExecuteScalar();

                return retureValue;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion

        #region ExecuteScalarSql(sql, parameter)
        /// <summary>
        ///Excute a string of Sql with parameters, and returns the value at the first row/colunm intersection of the result set, and ignor the rest( e.g.Select)
        /// </summary>
        /// <param name="sql">String: The SAL command to be executed. </param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters</param>
        /// <returns>object: reture  a single value as type objects. </returns>
        public object ExecuteScalarSql(string sql, SqlParameter[] parameters)
        {
            //open a connection to database
            SqlConnection conn = new SqlConnection(_connString);


            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open(); //open a conncection a cmd project use, being a little bit more precise than conn.open();

                FillParameters(cmd, parameters);

                object retureValue = cmd.ExecuteScalar();

                return retureValue;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion

        #region ExecuteScalarSP(SPName)
        /// <summary>
        ///Excute a stroed procedure and returns the value at the first row/colunm intersection of the result set, and ignor the rest( e.g.Select)
        /// </summary>
        /// <param name="SPName">String: The name of stroed procedure. </param>
        /// <returns>object: reture  a single value as type objects. </returns>
        public object ExecuteScalarSP(string SPName)
        {
            //open a connection to database
            SqlConnection conn = new SqlConnection(_connString);

            SqlCommand cmd = new SqlCommand(SPName, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Connection.Open(); //open a conncection a cmd project use, being a little bit more precise than conn.open();

                object retureValue = cmd.ExecuteScalar();

                return retureValue;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion

        #region ExecuteScalarSP(SPName, parameters)
        /// <summary>
        ///Excute a stroed procedure with parameters and returns the value at the first row/colunm intersection of the result set, and ignor the rest( e.g.Select)
        /// </summary>
        /// <param name="SPName">String: The name of stroed procedure. </param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters</param>
        /// <returns>object: reture a single value as type objects. </returns>
        public object ExecuteScalarSP(string SPName, SqlParameter[] parameters)
        {
            //open a connection to database
            SqlConnection conn = new SqlConnection(_connString);

            SqlCommand cmd = new SqlCommand(SPName, conn);

            FillParameters(cmd, parameters);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Connection.Open(); //open a conncection a cmd project use, being a little bit more precise than conn.open();

                object retureValue = cmd.ExecuteScalar();

                return retureValue;
            }
            catch (SqlException SqlEx)
            {
                throw new Exception(SqlEx.Message);
            }
            finally
            {
                //check if the connection stay open, if yes, then close it
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion

        #endregion
    }
}
