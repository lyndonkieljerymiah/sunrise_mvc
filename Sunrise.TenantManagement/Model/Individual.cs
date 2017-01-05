using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.TenantManagement.Model
{
    
    public class Individual
    {
        public Individual(DateTime birthday, GenderEnum gender, string qatarId, string company) : this()
        {

            this.Birthday = birthday;
            this.Gender = gender;
            this.QatarId = qatarId;
            this.Company = company;
        }


        public Individual()
        {
            this.Birthday = DateTime.Today;
        }
        public string TenantId { get; private set; }

        public DateTime Birthday { get; set; }
        public GenderEnum? Gender { get; set; }
        public string QatarId { get; set; }
        public string Company { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
