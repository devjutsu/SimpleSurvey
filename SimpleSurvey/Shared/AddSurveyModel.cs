using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSurvey.Shared
{
    public class AddSurveyModel : IValidatableObject
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set;  }
        [Required]
        public int? Minutes { get; set; }
        public List<OptionsCreateModel> Options { get; set; } = new List<OptionsCreateModel>();
        public void RemoveOption(OptionsCreateModel option) => this.Options.Remove(option);
        public void AddOption() => this.Options.Add(new OptionsCreateModel());

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(this.Options.Count < 2)
            {
                yield return new ValidationResult("A survey required at least 2 options");
            }
        }
    }

    public class OptionsCreateModel
    {
        public string OptionValue { get; set; }
    }
}
