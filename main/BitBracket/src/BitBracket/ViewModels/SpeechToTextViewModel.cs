//ViewModels/SpeechToTextViewModel.cs

using System.Collections.Generic;

namespace BitBracket.ViewModels
{
    public class SpeechToTextViewModel
    {
        public List<Language> SupportedLanguages { get; set; } = new List<Language>();

        // Constructor
        public SpeechToTextViewModel()
        {
            // Populate with supported languages. For demonstration, English and Spanish are added.
            // You should populate this list based on the languages supported by Whisper API or your implementation.
            SupportedLanguages.Add(new Language { Code = "en-US", Name = "English" });
            SupportedLanguages.Add(new Language { Code = "es-ES", Name = "Spanish" });
            // Add more languages as needed
        }
    }

    public class Language
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
