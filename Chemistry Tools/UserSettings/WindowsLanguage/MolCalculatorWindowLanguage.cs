using System;

namespace Chemistry_Tools.UserSettings.WindowsLanguage;
public class MolCalculatorWindowLanguage
{
    public string Title { get; set; }
    public string TextBoxWatermark { get; set; }
    public string MainActionButton { get; set; }
    public string ParseMoleculeErrorMessage { get; set; }
    public string SuccessMessageFormat { get; internal set; }
}
