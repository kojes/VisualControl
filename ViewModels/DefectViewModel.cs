using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.ViewModels
{
    public class DefectViewModel
    {
        public int DefectId { get; set; }
        public long DieId { get; set; }
        public string WaferId { get; set; }
        public int StageId { get; set; }
        public int DefectTypeId { get; set; }
        public int DangerLevelId { get; set; }
        public string Operator { get; set; }
        public DateTime Date { get; set; }
        public List<string> CommentsList { get; set; }
        public List<string> LoadedPhotosList { get; set; }

    }
}
