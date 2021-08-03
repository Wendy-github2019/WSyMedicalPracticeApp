using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WesternSydneyMedicalPractice
{
    public class Practitioners : List<Practitioner>
    {
        #region constructors

        public Practitioners()
        {
            //Get all Practitioners
            //DAL
            SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
            //Call the stroed Proc

            DataTable PractitionerTable = myDAL.ExecuteStoredProc("usp_GetAllPractitioners");

            foreach (DataRow practitionerRow in PractitionerTable.Rows)
            {
                Practitioner aPractitioner = new Practitioner(practitionerRow);
                this.Add(aPractitioner);
            }
        }

        #region Get Practitioners by day Available
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayAvailable"></param>
        public Practitioners(DayOfWeek dayAvailable)
        {
            //Get all Practitioners aviailebe in a day
            //DAL
            SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
            //Call the stroed Proc

            SqlParameter[] parameters = { new SqlParameter("@WeekDay", dayAvailable) }; 

            DataTable PractitionerTable = myDAL.ExecuteStoredProc("usp_GetPractByDayAvail", parameters);

            foreach (DataRow practitionerRow in PractitionerTable.Rows)
            {
                Practitioner aPractitioner = new Practitioner(practitionerRow);
                this.Add(aPractitioner);
            }
        }
        #endregion  Get Practitioners by day Available

        #endregion

        #region public method
        public void Refresh()
        {
            this.Clear();
            //Get all Practitioners
            //DAL
            SqlDataAccesslayer myDAL = new SqlDataAccesslayer("cnnStrWSMP");
            //Call the stroed Proc

            DataTable PractitionerTable = myDAL.ExecuteStoredProc("usp_GetAllPractitioners");

            foreach (DataRow practitionerRow in PractitionerTable.Rows)
            {
                Practitioner aPractitioner = new Practitioner(practitionerRow);
                this.Add(aPractitioner);
            }
        }
        #endregion  public method
    }
}
