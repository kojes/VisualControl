using System.Collections.Generic;
using VueExample.Providers;
using VueExample.Providers.Srv6.Interfaces;
using Xunit;

namespace VueExample.Tests.ServiceTests
{
    public class StageServiceTests
    {
        private IStageProvider _stageProvider;
        public StageServiceTests(IStageProvider stageProvider)
        {
            _stageProvider = stageProvider;
        }
        [Fact]
        public async void GetById_Stage_ShouldReturnStage()
        {
        //Given
            var stageId = 39;
        //When
            var stage = await _stageProvider.GetById(stageId);
        //Then
            Assert.NotNull(stage);
            Assert.Equal(stage.StageId, stageId);
        }
        [Fact]
        public async void GetAll_Stage_ShouldReturnList()
        {
            var stagesList = await _stageProvider.GetAll();
        
            Assert.NotEmpty(stagesList);             
        }
        [Fact]
        public async void GetStagesByProcessId_Stage_ShouldReturnList()
        {
        //Given
            var processId = 335;
        //When
            var stagesList = await _stageProvider.GetStagesByProcessId(processId);
        //Then
            Assert.NotEmpty(stagesList);
        }

        [Fact]
        public async void GetStagesByWaferId_Stage_ShouldReturnList()
        {
        //Given
            var waferId = "2386";
        //When
            var stagesList = await _stageProvider.GetStagesByWaferId(waferId);
        //Then
            Assert.NotEmpty(stagesList);
        }
    }
}