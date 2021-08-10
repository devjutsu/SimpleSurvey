using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSurvey.Shared
{
    public record Survey : IExpirable
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Title { get; init; }
        public DateTime ExpiresAt { get; init; }
        public List<string> Options { get; init; }
        public List<SurveyAnswer> Answers { get; init; }

        public SurveySummary ToSummary() => new SurveySummary
        {
            Id = this.Id,
            Title = this.Title,
            Options = this.Options,
            ExpiresAt = this.ExpiresAt,
        };
    }

    public record SurveyAnswer
    {
        public Guid Id { get; init; }
        public Guid SurveyId { get; init; }
        public string Option {  get; init; }
    }
}
