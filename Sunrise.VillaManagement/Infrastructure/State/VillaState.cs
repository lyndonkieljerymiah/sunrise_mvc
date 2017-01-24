using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.VillaManagement.Infrastructure.State
{
    public class VillaState : IVillaState
    {
        private Action OnStatusChange { get; }

        public VillaState(Action onStatusChange)
        {
            this.OnStatusChange = onStatusChange;
        }

        public VillaState()
        {

        }

        public string Occupied()
        {
            TriggerOnStatusChange();
            return "vsna";
        }

        private void TriggerOnStatusChange()
        {
            if(this.OnStatusChange != null)
                this.OnStatusChange();
        }

        public string Reserved()
        {
            TriggerOnStatusChange();
            return "vsres";
        }

        public string Vacant()
        {
            TriggerOnStatusChange();
            return "vsav";
        }
    }
}
