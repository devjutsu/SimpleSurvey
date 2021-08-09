using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSurvey.Shared
{
    public class AddSurveyModel
    {
        public string Title { get; set;  }
        public int? Minutes { get; set; }
        public List<OptionsCreateModel> Options { get; set; } = new List<OptionsCreateModel>();
        public void RemoveOption(OptionsCreateModel option) => this.Options.Remove(option);
        public void AddOption() => this.Options.Add(new OptionsCreateModel());
    }

    public class OptionsCreateModel
    {
        public string OptionValue { get; set; }
    }
}
