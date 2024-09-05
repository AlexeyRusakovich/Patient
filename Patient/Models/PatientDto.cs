﻿using Patient.Api.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Patient.Api.Models
{
    public class PatientDto
    {
        public PatientName? Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender? Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }
    }
}
