﻿using System;
using Sunrise.Client.Domains.Enum;

namespace Sunrise.Client.Domains.Models
{ 
    public class Individual
    {
        public int TenantId { get; set; }

        public DateTime Birthday { get; set; }
        public GenderEnum? Gender { get; set; }
        public string QatarId { get; set; }
        public string Company { get; set; }
    }
}