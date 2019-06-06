using VueExample.Contexts;
using System.Linq;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6
{
    public class StatParameterService
    {
        public StatParameterForStage GetByStatParameterIdAndStageId(int statisticParameterId, int? stageId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var statParameter = srv6Context.StatParametersForStage.FirstOrDefault(x => x.StageId == stageId && x.StatisticParameterId == statisticParameterId);
                return statParameter;
            }
        }
    }
}