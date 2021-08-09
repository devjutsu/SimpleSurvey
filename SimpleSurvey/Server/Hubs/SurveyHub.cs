using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SimpleSurvey.Shared;


namespace SimpleSurvey.Server.Hubs
{
    public interface ISurveyHub
    {
        Task SurveyAdded(SurveySummary survey);
        Task SurveyUpdated(Survey survey);
    }

    public class SurveyHub: Hub<ISurveyHub>
    {
        public async Task JoinSurveyGroup(Guid surveyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, surveyId.ToString());
        }

        public async Task LeaveSurveyGroup(Guid surveyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, surveyId.ToString());
        }
    }
}
