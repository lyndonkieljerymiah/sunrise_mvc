﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Events.Abstract
{
    public interface IHandler<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}
