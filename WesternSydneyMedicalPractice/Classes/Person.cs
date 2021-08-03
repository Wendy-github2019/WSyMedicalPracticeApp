using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Add the following reference
using System.Data;

namespace WesternSydneyMedicalPractice
{
    /// <summary>
    /// Base class Person: Not implementable. Must be inherited.
    /// </summary>
    public abstract class Person //'abstract' means that the class MUST be inherited from, it can't be instantiated.
    {
        private int _personID;

        #region Public Property
        public virtual int PersonID
        {
            get
            {
                return _personID;
            }
            set
            {
                _personID = value;
            }
        }

        public virtual string FirstName  //this is an auto-property, i.e. the field variable is hidden from us and consequetly not accessible from within the class. when it's { get; set;} only
        {
            get;
            set;
        }

        public virtual string LastName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Suburb { get; set; }
        public virtual string State { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string HomePhone { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// A virtual method is a method that can be refined in derived class. It has an implemetation in the base class as well as the derived class, base class is top of the hirachy
        /// the derived calss will override the base class's implementation
        /// </summary>
        /// <returns></returns>
        public virtual int Insert()
        {

            throw new NotImplementedException();  //we need to give an implemetation , or it will throw an error
        }

        public virtual DataTable Load()
        {
            throw new NotImplementedException();
        }

        public virtual int Update()
        {
            throw new NotImplementedException();
        }

        public virtual int Delete()
        {
            throw new NotImplementedException();
        }
        #endregion Public Methods

    }
}
