using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice
{
    public class Practitioner : Person //Inherits from the Person class
    {
        #region Private Field Variables
        //A DataTable to more permanently hold data that we pull back form the database, through our DataAccessLayer class
        private DataTable _dtPractitioner;
        #endregion

        #region Public Property

        public int Practitioner_ID { get; set; }

        public IEnumerable<Appointment> Appointments { get; set; }

        //public Appointments Appointments { get; set; }
        public string RegistrationNumber { get; set; }
        public string PractnrTypeName_Ref { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        #endregion Public Property

        #region Constructors
        public Practitioner()
        {
        }

        public Practitioner(int practitioner_ID) : base()
        {
            SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Practitioner_ID", practitioner_ID) };

                this._dtPractitioner = myDAL.ExecuteStoredProc("usp_GetPractitioner", parameters);

                if (this._dtPractitioner != null && this._dtPractitioner.Rows.Count > 0)
                {
                    //We've got a practitioner...
                    //Now map the data returned from the database to the properties of this instance of the practitioner.
                    LoadPractitionerProperties(this._dtPractitioner.Rows[0]);
                }

            }
            catch (Exception)
            {

                throw;
            }
           
        }


        public Practitioner(DataRow practitionerRow)
        {
            this.Practitioner_ID = (int)practitionerRow["Practitioner_ID"];
            this.FirstName = practitionerRow["FirstName"].ToString();
            this.LastName = practitionerRow["LastName"].ToString();
            this.Street = practitionerRow["Street"].ToString();
            this.Suburb = practitionerRow["Suburb"].ToString();
            this.State = practitionerRow["State"].ToString();
            this.PostCode = practitionerRow["PostCode"].ToString();
            this.HomePhone = practitionerRow["HomePhone"].ToString();
            this.Mobile = practitionerRow["Mobile"].ToString();
            this.RegistrationNumber = practitionerRow["RegistrationNumber"].ToString();
            this.PractnrTypeName_Ref = practitionerRow["PractnrTypeName_Ref"].ToString();
            this.Monday = (bool)practitionerRow["Monday"];
            this.Tuesday = (bool)practitionerRow["Tuesday"];
            this.Wednesday = (bool)practitionerRow["Wednesday"];
            this.Thursday = (bool)practitionerRow["Thursday"];
            this.Friday = (bool)practitionerRow["Friday"];
            this.Saturday = (bool)practitionerRow["Saturday"];
            this.Sunday = (bool)practitionerRow["Sunday"];

            //Appointments appointments = new Appointments(this);
            //this.Appointments = appointments;
        }

        #endregion Constructors

        #region Private Methods
        private void LoadPractitionerProperties(DataRow practitionerRow)
        {
            this.Practitioner_ID = (int)practitionerRow["Practitioner_ID"];
            //GetPractitnrDetails(this.Practitioner_ID);

            this.FirstName = practitionerRow["FirstName"].ToString();
            this.LastName = practitionerRow["LastName"].ToString();
            this.Street = practitionerRow["Street"].ToString();
            this.Suburb = practitionerRow["Suburb"].ToString();
            this.State = practitionerRow["State"].ToString();
            this.PostCode = practitionerRow["PostCode"].ToString();
            this.HomePhone = practitionerRow["HomePhone"].ToString();
            this.Mobile = practitionerRow["Mobile"].ToString();
            this.RegistrationNumber = practitionerRow["RegistrationNumber"].ToString();
            this.PractnrTypeName_Ref = practitionerRow["PractnrTypeName_Ref"].ToString();
            this.Monday = (bool)practitionerRow["Monday"];
            this.Tuesday = (bool)practitionerRow["Tuesday"];
            this.Wednesday = (bool)practitionerRow["Wednesday"];
            this.Thursday = (bool)practitionerRow["Thursday"];
            this.Friday = (bool)practitionerRow["Friday"];
            this.Saturday = (bool)practitionerRow["Saturday"];
            this.Sunday = (bool)practitionerRow["Sunday"];

            //Appointments appointments = new Appointments(this);
            //this.Appointments = appointments;
        }

        private void GetPractitnrDetails(int Practitioner_ID)
        {
            SqlDataAccesslayer myDAL = new SqlDataAccesslayer();
            SqlParameter[] parameters = { new SqlParameter(@"Practitioner_ID", Practitioner_ID) };
            DataRow practitionerRow = myDAL.ExecuteStoredProc("usp_GetPractitioner", parameters).Rows[0];

            this.FirstName = practitionerRow["FirstName"].ToString();
            this.LastName = practitionerRow["LastName"].ToString();
            this.Street = practitionerRow["Street"].ToString();
            this.Suburb = practitionerRow["Suburb"].ToString();
            this.State = practitionerRow["State"].ToString();
            this.PostCode = practitionerRow["PostCode"].ToString();
            this.HomePhone = practitionerRow["HomePhone"].ToString();
            this.Mobile = practitionerRow["Mobile"].ToString();
            this.RegistrationNumber = practitionerRow["RegistrationNumber"].ToString();
            this.PractnrTypeName_Ref = practitionerRow["Practitioner_ID"].ToString();
            this.Monday = (bool)practitionerRow["Monday"];
            this.Tuesday = (bool)practitionerRow["Tuesday"];
            this.Wednesday = (bool)practitionerRow["Wednesday"];
            this.Thursday = (bool)practitionerRow["Thursday"];
            this.Friday = (bool)practitionerRow["Friday"];
            this.Saturday = (bool)practitionerRow["Saturday"];
            this.Sunday = (bool)practitionerRow["Sunday"];
        }

        #endregion Private Methods

        #region Public Method
        //public override DataTable GetPerson(int person_ID)
        //{

        //}

        public void LoadPractitionerProperties(string firstName, string lastName, string street, string suburb, string state, string postCode, string homePhone, string mobile, string registrationNumber, string practnrTypeName_Ref)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Street = street;
            this.Suburb = suburb;
            this.State = state;
            this.PostCode = postCode;
            this.HomePhone = homePhone;
            this.Mobile = mobile;
            this.RegistrationNumber = registrationNumber;
            this.PractnrTypeName_Ref = practnrTypeName_Ref;
        }

        public void SetAvaibleDay(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        {
            this.Monday = monday;
            this.Tuesday = tuesday;
            this.Wednesday = wednesday;
            this.Thursday = thursday;
            this.Friday = friday;
            this.Saturday = saturday;
            this.Sunday = sunday;
        }

        #region Insert()
        /// <summary>
        /// Insert the details of a New Practitioner into the Database
        /// </summary>
        /// <returns>int, return 1 when INSERT successed, 0 if failed</returns>
        public override int Insert()
        {
            try
            {
                SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
                SqlParameter[] parameters = {
                    new SqlParameter("@FirstName", this.FirstName),
                    new SqlParameter("@LastName", this.LastName),
                    new SqlParameter("@Street", this.Street),
                    new SqlParameter("@Suburb", this.Suburb),
                    new SqlParameter("@State", this.State),
                    new SqlParameter("@PostCode", this.PostCode),
                    new SqlParameter("@HomePhone", this.HomePhone),
                    new SqlParameter("@Mobile", this.Mobile),
                    new SqlParameter("@RegistrationNumber", this.RegistrationNumber),
                    new SqlParameter("@PractnrTypeName_Ref", this.PractnrTypeName_Ref),
                    new SqlParameter("@Monday", this.Monday),
                    new SqlParameter("@Tuesday", this.Tuesday),
                    new SqlParameter("@Wednesday", this.Wednesday),
                    new SqlParameter("@Thursday", this.Thursday),
                    new SqlParameter("@Friday", this.Friday),
                    new SqlParameter("@Saturday", this.Saturday),
                    new SqlParameter("@Sunday", this.Sunday)
                };

                int rowAffected = myDAL.ExecuteNonQuerySP("usp_InsertPractitioner", parameters);
                return rowAffected;
            }
            catch (Exception ex)
            {
                //throw new Exception("This Paractitioner's details could not be inserted into database", ex);
                throw new Exception("This practitioner was failed to insert", ex);
            }
        }
        #endregion

        #region Update()
        /// <summary>
        /// update practitioner details
        /// </summary>
        /// <returns>int: how many rows were affected</returns>
        public override int Update()
        {
            try
            {
                SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
                SqlParameter[] parameters = {
                    new SqlParameter("@Practitioner_ID", this.Practitioner_ID),
                    new SqlParameter("@FirstName", this.FirstName),
                    new SqlParameter("@LastName", this.LastName),
                    new SqlParameter("@Street", this.Street),
                    new SqlParameter("@Suburb", this.Suburb),
                    new SqlParameter("@State", this.State),
                    new SqlParameter("@PostCode", this.PostCode),
                    new SqlParameter("@HomePhone", this.HomePhone),
                    new SqlParameter("@Mobile", this.Mobile),
                    new SqlParameter("@RegistrationNumber", this.RegistrationNumber),
                    new SqlParameter("@PractnrTypeName_Ref", this.PractnrTypeName_Ref),
                    new SqlParameter("@Monday", this.Monday),
                    new SqlParameter("@Tuesday", this.Tuesday),
                    new SqlParameter("@Wednesday", this.Wednesday),
                    new SqlParameter("@Thursday", this.Thursday),
                    new SqlParameter("@Friday", this.Friday),
                    new SqlParameter("@Saturday", this.Saturday),
                    new SqlParameter("@Sunday", this.Sunday)
                };
                int rowAffected = myDAL.ExecuteNonQuerySP("usp_UpdatePractioner", parameters);

                return rowAffected;

            }
            catch (Exception ex)
            {
                throw new Exception("This Paractitioner's details could not be updated", ex);
            }
        }
        #endregion

        #region Delete()
        /// <summary>
        /// Delete this practitioner from the database
        /// </summary>
        /// <returns>Int: how many rows was deleted from database</returns>
        public override int Delete()
        {
            try
            {
                SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
                SqlParameter[] parameters = {
                    new SqlParameter("@Practitioner_ID", this.Practitioner_ID)
                };

                int rowAffected = myDAL.ExecuteNonQuerySP("usp_DeletePractitioner", parameters);

                return rowAffected;
            }
            catch (Exception ex)
            {

                throw new Exception("This practitioner was failed to deleted", ex);
            }
        }
        #endregion

        #region Load
        public void Load(int practitioner_id)
        {
            
        }
        #endregion

        #endregion Public Method
    }
}
