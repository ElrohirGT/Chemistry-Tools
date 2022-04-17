namespace Chemistry_Tools.UserSettings.WindowsLanguage;

public interface IFeatureWindowLanguage
{
    string ErrorMessageFormat { get; set; }
    string SuccessMessageFormat { get; set; }
    string TextBoxWatermark { get; set; }
    string Title { get; set; }
}