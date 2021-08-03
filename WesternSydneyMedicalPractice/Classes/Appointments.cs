using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice
{
    /// <summary>
    /// Create a Collection (List) of Appointments
    /// </summary>
    public class Appointments : List<Appointment>
    {
        public Appointments()
        { 
        }
        public Appointments(Patient patient)
        {
            SqlDataAccesslayer myDal = new SqlDataAccesslayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Patient_ID", patient.Patient_ID) };
            DataTable appointmentTable = myDal.ExecuteStoredProc("[usp_GetAppointDetailByPatientId]", parameters);

            foreach (DataRow appointmentRow in appointmentTable.Rows)
            {
                //create a new instance of an Appointment for each row
                Appointment appointment = new Appointment(appointmentRow);

                //Add the appointment to this class's internal List
                this.Add(appointment);
            }

        }

        public Appointments(Practitioner practitioner)
        {
            SqlDataAccesslayer myDal = new SqlDataAccesslayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Patient_ID", practitioner.Practitioner_ID) };
            DataTable appointmentTable = myDal.ExecuteStoredProc("[usp_GetAppointDetailByPractitionerId]", parameters);

            foreach (DataRow appointmentRow in appointmentTable.Rows)
            {
                //create a new instance of an Appointment for each row
                Appointment appointment = new Appointment(appointmentRow);

                //Add the appointment to this class's internal List
                this.Add(appointment);
            }

        }


    }
}
