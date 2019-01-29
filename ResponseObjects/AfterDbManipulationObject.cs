using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace VueExample.ResponseObjects
{
    public class AfterDbManipulationObject<T> where T : class 
    {
        public T TObject { get; private set; }
        private List<Error> ErrorsList { get; }
        public bool HasErrors { get; private set; }
        public string ManipulationType { get; }

        public AfterDbManipulationObject(string manipulationType = "ADD")
        {
            this.HasErrors = false;
            this.ManipulationType = manipulationType;
            this.ErrorsList = new List<Error>();
        }

        public void AddError(Error error)
        {
            this.ErrorsList.Add(error);
            if (!HasErrors)
            {
                this.HasErrors = true;
            }

        }

        public void SetObject(T TObject)
        {
            this.TObject = TObject;
        }

        public List<Error> GetErrors()
        {
            return this.ErrorsList;
        }
    }
}
