using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WesternSydneyMedicalPractice
{
    public class Patients:List<Patient>
    {
        public int Patient_ID;

        public Patients()
        {
            //Get all Practitioners
            //DAL
            SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
            //Call the stroed Proc

            DataTable PatientsTable = myDAL.ExecuteStoredProc("usp_GetAllPatients");

            foreach (DataRow patientRow in PatientsTable.Rows)
            {
                Patient aPatient = new Patient(patientRow);
                this.Add(aPatient);
            }
        }
    }
}
