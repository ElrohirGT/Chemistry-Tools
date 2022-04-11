using System;

namespace Chemistry_Tools.UserSettings.WindowsLanguage;
public class MolCalculatorWindowLanguage : IFeatureWindowLanguage
{
    public string Title { get; set; }
    public string TextBoxWatermark { get; set; }
    public string ErrorMessageFormat { get; set; }
    public string SuccessMessageFormat { get; set; }
    public string InstructionsTitle { get; set; }
    public string Examples { get; set; }
}
