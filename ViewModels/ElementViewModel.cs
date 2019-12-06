namespace VueExample.ViewModels
{
    public class ElementViewModel
    {
        public int ElementId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string PhotoPath { get; set; }
        public string DocName { get; set; }
        public int TypeId { get; set; }
        public bool IsAvaliableToDelete { get; set; } = true;
    }
}