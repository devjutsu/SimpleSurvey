using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSurvey.Shared
{
    public class SurveyHttpClient
    {
        private readonly HttpClient _http;
        public SurveyHttpClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<SurveySummary[]> GetSurveys()
            => await _http.GetFromJsonAsync<SurveySummary[]>("api/survey");

        public async Task<Survey> GetSurvey(Guid id)
            => await _http.GetFromJsonAsync<Survey>($"api/survey/{id}");

        public async Task<HttpResponseMessage> Add(AddSurveyModel model)
            => await _http.PutAsJsonAsync<AddSurveyModel>("api/survey", model);

        public async Task<HttpResponseMessage> Answer(Guid id, SurveyAnswer answer)
            => await _http.PostAsJsonAsync<SurveyAnswer>($"api/survey/{id}/answer", answer);


    }
}
