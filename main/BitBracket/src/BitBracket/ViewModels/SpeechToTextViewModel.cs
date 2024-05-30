using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BitBracket.ViewModels
{

    public class SpeechToTextViewModel
    {
        public List<SelectListItem> Languages { get; set; }
        public string SelectedLanguage { get; set; }

        public SpeechToTextViewModel()
        {
            Languages = new List<SelectListItem>
            {
                new SelectListItem("English", "en"),
                new SelectListItem("Spanish", "es"),
                new SelectListItem("French", "fr"),
            };
        }
    }
}
