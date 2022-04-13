using System;

namespace Chemistry_Tools.UserSettings.WindowsLanguage;
public class ReactionStoichiometryWindowLanguage : IFeatureWindowLanguage
{
    public string Title { get; set; }
    public string TextBoxWatermark { get; set; }
    public string ComboBoxWatermark { get; set; }
    public string Examples { get; set; }
    public string InstructionsTitle { get; set; }
    public string ErrorMessageFormat { get; set; }
    public string SuccessMessageFormat { get; set; }
    public string CalculateButtonText { get; set; }
    public string SecondarySuccessMessageFormat { get; set; }
    public string ExperimentalMassTextBlockText { get; set; }
    public string SecondActionTitle { get; set; }
}
