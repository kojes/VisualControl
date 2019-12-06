using System.Collections.Generic;

namespace VueExample.ViewModels
{
    public class DieTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DieTypeUpdatingViewModel
    {
        public string Name { get; set; }
        public List<int> CodeProductIdsList { get; set; }
        public List<ElementViewModel> ElementsList { get; set; }
    }


}