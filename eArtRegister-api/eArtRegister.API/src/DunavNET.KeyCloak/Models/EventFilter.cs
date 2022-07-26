using System;
using System.Collections.Generic;

namespace KeyCloak.Models
{
    public class EventFilter
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public List<string> EventTypes { get; set; }

        public string UserId { get; set; }
    }
}
