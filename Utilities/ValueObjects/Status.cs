using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ValueObjects
{

    public abstract class StatusBaseDictionary
    {
        public string Code { get; set; }
        protected string Description { get; set; }

        public StatusBaseDictionary()
        {

        }

        public StatusBaseDictionary(string code)
        {
            this.Code = code;
        }
    } 
    public class StatusDictionary : StatusBaseDictionary
    {

        public StatusDictionary(string code) : base(code)
        {

        }

        public StatusDictionary()
        {

        }

        public static StatusDictionary CreateByDefault(string code)
        {
            var status = new StatusDictionary(code);
            return status;
        }
        
    }
}
