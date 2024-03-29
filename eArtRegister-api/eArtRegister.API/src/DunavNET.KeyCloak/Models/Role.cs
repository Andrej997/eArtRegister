﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace KeyCloak.Models
{
    public class Role
    {
        public Role() { }

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("composite")]
        public bool? Composite { get; set; }
        [JsonProperty("composites")]
        public RoleComposite Composites { get; set; }
        [JsonProperty("clientRole")]
        public bool? ClientRole { get; set; }
        [JsonProperty("containerId")]
        public string ContainerId { get; set; }
        [JsonProperty("attributes")]
        public IDictionary<string, object> Attributes { get; set; }
    }
}
