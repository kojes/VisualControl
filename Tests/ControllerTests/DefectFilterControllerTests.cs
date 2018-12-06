using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Providers;
using VueExample.ViewModels;
using Xunit;

namespace VueExample.Tests.ControllerTests
{
    public class DefectFilterControllerTests 
    {
        DefectProvider _defectProvider = new DefectProvider();
        DangerLevelProvider _dangerLevelProvider = new DangerLevelProvider();

        [Fact]
        public void GetDefectFilter_CreateFilter_ShouldReturnDefectFilter()
        {
            const string waferId = "096_1";
            var defectList = _defectProvider.GetByWaferId(waferId);
            var defectfilter = new DefectFilterViewModel();

            defectfilter.AvbDangerLevelList = _dangerLevelProvider.GetAll().FindAll(d =>
                defectList.Select(x => x.DangerLevelId).Distinct().Contains(d.DangerLevelId));

            Assert.Equal(3, defectfilter.AvbDangerLevelList.Count);

        }
    }
}
