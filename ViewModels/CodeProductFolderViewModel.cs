namespace VueExample.ViewModels
{
    public class CodeProductFolderViewModel
    {
        public string FolderName { get; set; }
        public bool Warning { get; set; } = true;
        public CodeProductViewModel CodeProduct { get; set; }
    }
}