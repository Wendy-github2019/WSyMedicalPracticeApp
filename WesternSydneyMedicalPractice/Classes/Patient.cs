using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice
{
    public class Patient : Person
    {
        private DataTable _dtPatient;
        public int Patient_ID { get; set; }
        public string MedicalNumber { get; set; }

        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PatientNote { get; set; }

        #region Constructors
        public Patient()
        {
        }

        public Patient(int patient_Id) : base()
        {
            SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Patient_ID", patient_Id) };

                _dtPatient = myDAL.ExecuteStoredProc("usp_GetPatient", parameters);

                if (_dtPatient != null && _dtPatient.Rows.Count > 0)
                {
                    //We've got a practitioner...
                    //Now map the data returned from the database to the properties of this instance of the practitioner.
                    LoadPatientProperty(_dtPatient.Rows[0]);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Patient(DataRow patientRow)
        {
            LoadPatientProperty(patientRow);
            //Appointments appointments = new Appointments(this);
            //this.Appointments = appointments;
        }


        #endregion


        #region Private method      
        private void LoadPatientProperty(DataRow patientRow)
        {
            this.Patient_ID = (int)patientRow["Patient_ID"];
            this.Gender = patientRow["Gender"].ToString();
            this.DateOfBirth = (DateTime)patientRow["DateOfBirth"];
            this.FirstName = patientRow["FirstName"].ToString();
            this.LastName = patientRow["LastName"].ToString();
            this.Street = patientRow["Street"].ToString();
            this.Suburb = patientRow["Suburb"].ToString();
            this.State = patientRow["State"].ToString();
            this.PostCode = patientRow["PostCode"].ToString();
            this.HomePhone = patientRow["HomePhone"].ToString();
            this.Mobile = patientRow["Mobile"].ToString();
            this.MedicalNumber = patientRow["MedicareNumber"].ToString();
            this.PatientNote = patientRow["Notes"].ToString();

            //Appointments appointments = new Appointments(this);
            //this.Appointments = appointments;
        }

        #endregion Private method

        #region Public method

        #region patient Insert()
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
                    new SqlParameter("@Gender", this.Gender),
                    new SqlParameter("@DateOfBirth", this.DateOfBirth),
                    new SqlParameter("@FirstName", this.FirstName),
                    new SqlParameter("@LastName", this.LastName),
                    new SqlParameter("@Street", this.Street),
                    new SqlParameter("@Suburb", this.Suburb),
                    new SqlParameter("@State", this.State),
                    new SqlParameter("@PostCode", this.PostCode),
                    new SqlParameter("@HomePhone", this.HomePhone),
                    new SqlParameter("@Mobile", this.Mobile),
                    new SqlParameter("@MedicareNumber", this.MedicalNumber),
                    new SqlParameter("@Notes", this.PatientNote)
                };

                int rowAffected = myDAL.ExecuteNonQuerySP("usp_InsertPatient", parameters);
                return rowAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("This Patient was failed to insert", ex);
            }
        }
        #endregion

        #region patient Update()
        /// <summary>
        /// update patient details
        /// </summary>
        /// <returns>int: how many rows were affected</returns>
        public override int Update()
        {
            try
            {
                SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
                SqlParameter[] parameters = {
                    new SqlParameter("@Patient_ID", this.Patient_ID),
                    new SqlParameter("@Gender", this.Gender),
                    new SqlParameter("@DateOfBirth", this.DateOfBirth),
                    new SqlParameter("@FirstName", this.FirstName),
                    new SqlParameter("@LastName", this.LastName),
                    new SqlParameter("@Street", this.Street),
                    new SqlParameter("@Suburb", this.Suburb),
                    new SqlParameter("@State", this.State),
                    new SqlParameter("@PostCode", this.PostCode),
                    new SqlParameter("@HomePhone", this.HomePhone),
                    new SqlParameter("@Mobile", this.Mobile),
                    new SqlParameter("@MedicareNumber", this.MedicalNumber),
                    new SqlParameter("@Notes", this.PatientNote)
                };
                int rowAffected = myDAL.ExecuteNonQuerySP("usp_UpdatePatient", parameters);

                return rowAffected;

            }
            catch (Exception ex)
            {
                throw new Exception("This Patient's details could not be updated", ex);
            }
        }
        #endregion

        #region Patient Delete()
        /// <summary>
        /// Delete this patient from the database
        /// </summary>
        /// <returns>Int: how many rows was deleted from database</returns>
        public override int Delete()
        {
            try
            {
                SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
                SqlParameter[] parameters = {
                    new SqlParameter("@Patient_ID", this.Patient_ID)
                };

                int rowAffected = myDAL.ExecuteNonQuerySP("usp_DeletePatient", parameters);

                return rowAffected;
            }
            catch (Exception ex)
            {

                throw new Exception("This Patient was failed to deleted", ex);
            }
        }
        #endregion

        #endregion
    }
}
