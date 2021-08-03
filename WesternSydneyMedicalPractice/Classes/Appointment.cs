using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//add
using System.Data;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice
{
    public class Appointment
    {
        #region Public Property
        public int Practitioner_ID;
        public DateTime AppointmentDate;
        public TimeSpan AppointmentTime;
        public int Patient_ID;
        public string PractitionerFisrtName;
        public string PractitionerLastName;
        public string PractitionerType;
        #endregion

        #region constructors
        public Appointment(DateTime appointDate, TimeSpan appointTime, int practitioner_id, int patient_id)
        {
            this.AppointmentDate = appointDate;
            this.AppointmentTime = appointTime;
            this.Practitioner_ID = practitioner_id;
            this.Patient_ID = patient_id;
        }

        public Appointment(DataRow appointmentRow)
        {
            this.Patient_ID = int.Parse(appointmentRow["Patient_ID"].ToString());
            this.AppointmentDate = DateTime.Parse(appointmentRow["AppointmentDate"].ToString());
            this.AppointmentTime = TimeSpan.Parse(appointmentRow["AppointmentTime_Ref"].ToString()); 
            //this.Practitioner_ID = int.Parse(appointmentRow["Practitioner_Ref"].ToString());
            this.Practitioner_ID = int.Parse(appointmentRow["Patient_ID"].ToString());
            this.PractitionerType = appointmentRow["PractnrTypeName_Ref"].ToString();
            this.PractitionerFisrtName  = appointmentRow["FirstName"].ToString();
            this.PractitionerLastName = appointmentRow["LastName"].ToString();
        }
        #endregion constructors

        #region Public Method

        #region GetDetails
        /// <summary>
        /// Get appointment details by patient_ID
        /// </summary>
        /// <param name="patient_Id"></param>
        /// <returns></returns>
        public DataTable GetDetails()
        {
            SqlDataAccesslayer myDal = new SqlDataAccesslayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Patient_ID", this.Patient_ID) };
            DataTable returenDataTable = myDal.ExecuteStoredProc("[usp_GetAppointmentsByPatient_ID]", parameters);

            return returenDataTable;
        }
        #endregion

        #region GetDetails by patient_Id
        /// <summary>
        /// Get appointment details by patient_ID
        /// </summary>
        /// <param name="patient_Id"></param>
        /// <returns></returns>
        public DataTable GetDetails(int patient_Id)
        {
            SqlDataAccesslayer myDal = new SqlDataAccesslayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Patient_ID", patient_Id) };
            DataTable returenDataTable = myDal.ExecuteStoredProc("[usp_GetAppointmentsByPatient_ID]", parameters);

            return returenDataTable;
        }
        #endregion GetDetails by patient_Id)
        
        #region GetPractitionerAppointments
        /// <summary>
        /// Get appointment details by patient_ID
        /// </summary>
        /// <param name="practitioner_Id"></param>
        /// <returns></returns>
        public DataTable GetPractitionerAppointments(int practitioner_Id)
        {
            SqlDataAccesslayer myDal = new SqlDataAccesslayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Practitioner_ID", practitioner_Id) };
            DataTable returenDataTable = myDal.ExecuteStoredProc("[usp_GetAppointDetailByPractitionerId]", parameters);

            return returenDataTable;
        }
        #endregion

        #region MakeAppointment
        /// <summary>
        /// Get appointment details by patient_ID
        /// </summary>
        /// <param name="practitioner_Id"></param>
        /// <returns></returns>
        public int MakeAppointment()
        {
            SqlDataAccesslayer myDal = new SqlDataAccesslayer("cnnStrWSMP");
            SqlParameter[] parameters =
                {
                    new SqlParameter("@Practitioner_ID", this.Practitioner_ID),
                    new SqlParameter("@AppointmentDate", this.AppointmentDate),
                    new SqlParameter("@AppointmentTime", this.AppointmentTime),
                    new SqlParameter("@Patient_ID", this.Patient_ID)
                };

            parameters[1].SqlDbType = SqlDbType.DateTime;
            parameters[2].SqlDbType = SqlDbType.Time;

            int rowsAffected = myDal.ExecuteNonQuerySP("usp_CreateAppointment", parameters);

            return rowsAffected;
        }
        #endregion


        #region CancelAppointment
        /// <summary>
        /// cancel an appointment
        /// </summary>
        /// <returns></returns>
        public int CancelAppointment()
        {
            SqlDataAccesslayer myDal = new SqlDataAccesslayer("cnnStrWSMP");
            SqlParameter[] parameters =
                {
                    new SqlParameter("@Practitioner_ID", this.Practitioner_ID),
                    new SqlParameter("@AppointmentDate", this.AppointmentDate),
                    new SqlParameter("@AppointmentTime", this.AppointmentTime),
                    new SqlParameter("@Patient_ID", this.Patient_ID)
                };

            parameters[1].SqlDbType = SqlDbType.DateTime;
            parameters[2].SqlDbType = SqlDbType.Time;

            int rowsAffected = myDal.ExecuteNonQuerySP("usp_CancelAppointment", parameters);

            return rowsAffected;
        }
        #endregion



        #endregion Public Method
    }
}
