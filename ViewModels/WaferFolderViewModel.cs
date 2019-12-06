namespace VueExample.ViewModels
{
    public class WaferFolderViewModel
    {
        public string FolderName { get; set; }
        public bool Disabled { get; set; } = true;
        public WaferViewModel Wafer { get; set; }
    }
}