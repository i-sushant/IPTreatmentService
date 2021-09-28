using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IPTreatmentMicroservice.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AilmentCategory
    {
        Orthopaedics,
        Urology
    }
}
