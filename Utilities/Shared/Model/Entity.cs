using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Shared.Model
{
    public abstract class Entity<TType>
    {
        public Entity(TType id)
        {
            Id = id;
        }

        public TType Id { get; protected set; }

        public DateTime DateStamp { get; set; }

    }
}
