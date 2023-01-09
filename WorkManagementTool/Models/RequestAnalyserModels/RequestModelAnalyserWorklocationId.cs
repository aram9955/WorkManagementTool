﻿using Microsoft.Build.Framework;
using System.ComponentModel;

namespace WorkManagementTool.Models
{
    public class RequestModelAnalyserWorklocationId
    {
        public List<int> WorkLocationId { get; set; } = new();

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public bool Archived { get; set; }
        [Required]
        public bool IsTrash { get; set; }
    }
}