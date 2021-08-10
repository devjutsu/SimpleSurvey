using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleSurvey.Shared;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using SimpleSurvey.Server.Hubs;

namespace SimpleSurvey.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly IHubContext<SurveyHub, ISurveyHub> _hub;
        private static ConcurrentBag<Survey> surveys = new ConcurrentBag<Survey>
        {
            new Survey
            {
                Id = Guid.NewGuid(),
                Title = "Are you satisfied with Blazor?",
                ExpiresAt = DateTime.Now.AddHours(1),
                Options = new List<string> {"Very much!", "Yes", "Not sure", "No", "It's a crap!"},
                Answers = new List<SurveyAnswer>
                {
                    new SurveyAnswer {Option = "Very much!"},
                    new SurveyAnswer {Option = "Very much!"},
                    new SurveyAnswer {Option = "Very much!"},
                    new SurveyAnswer {Option = "Yes"},
                }
            },
            new Survey
            {
                Id = Guid.NewGuid(),
                Title = "Are you happy with SignalR?",
                ExpiresAt = DateTime.Now.AddMinutes(5),
                Options = new List<string> {"Very much!", "Yes", "Not sure", "No", "It's a crap!"},
                Answers = new List<SurveyAnswer>
                {
                    new SurveyAnswer {Option = "Very much!"},
                    new SurveyAnswer {Option = "Not sure!"},
                    new SurveyAnswer {Option = "Yes"},
                }
            },
            new Survey
            {
                Id = Guid.NewGuid(),
                Title = "How's your mood today?",
                ExpiresAt = DateTime.Now.AddMinutes(1),
                Options = new List<string> {"Good :)", "Ok :|", "Bad :("},
                Answers = new List<SurveyAnswer>
                {
                    new SurveyAnswer {Option = "Good :)"},
                    new SurveyAnswer {Option = "Ok :|"},
                    new SurveyAnswer {Option = "Bad :("},
                }
            }
        };

        public SurveyController(IHubContext<SurveyHub, ISurveyHub> hub)
        {
            _hub = hub;
        }

        [HttpGet()]
        public IEnumerable<SurveySummary> GetSurveys()
        {
            return surveys.Select(s => s.ToSummary());
        }

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var survey = surveys.SingleOrDefault(s => s.Id == id);
            if (survey != null)
                return new JsonResult(survey);
            else
                return NotFound();
        }

        [HttpPut()]
        public async Task<Survey> Add([FromBody]AddSurveyModel model)
        {
            var survey = new Survey
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                ExpiresAt = DateTime.Now.AddMinutes(model.Minutes.Value),
                Options = model.Options.Select(o => o.OptionValue).ToList(),
                Answers = new List<SurveyAnswer>()
            };
            surveys.Add(survey);
            await _hub.Clients.All.SurveyAdded(survey.ToSummary());

            return survey;
        }

        [HttpPost("{surveyId}/answer")]
        public async Task<ActionResult> Answer(Guid surveyId, [FromBody] SurveyAnswer answer)
        {
            var survey = surveys.SingleOrDefault(t => t.Id == surveyId);
            if (survey == null) return NotFound();
            else if((survey as IExpirable).IsExpired) return StatusCode(400, "Has expired");


            survey.Answers.Add(new SurveyAnswer
            {
                SurveyId = surveyId,
                Option = answer.Option
            });
            await _hub.Clients.Group(surveyId.ToString()).SurveyUpdated(survey);

            return new JsonResult(survey);
        }
    }
}
