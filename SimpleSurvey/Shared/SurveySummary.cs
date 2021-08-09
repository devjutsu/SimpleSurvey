﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSurvey.Shared
{
    public record SurveySummary
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public DateTime ExpiresAt { get; init; }
        public List<string> Options { get; init; }
    }
}