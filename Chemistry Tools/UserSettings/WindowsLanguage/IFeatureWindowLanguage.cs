namespace Chemistry_Tools.UserSettings.WindowsLanguage;

public interface IFeatureWindowLanguage
{
    string Examples { get; set; }
    string InstructionsTitle { get; set; }
    string ErrorMessageFormat { get; set; }
    string SuccessMessageFormat { get; set; }
    string TextBoxWatermark { get; set; }
    string Title { get; set; }
}