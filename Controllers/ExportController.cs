using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models.SRV6.Export;
using VueExample.StatisticsCore.Abstract;
using VueExample.ViewModels;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using OfficeOpenXml.Style;
using AutoMapper;
using Newtonsoft.Json.Linq;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]

    public class ExportController : Controller
    {
        private readonly IExportProvider _exportProvider;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        public ExportController(IExportProvider exportProvider, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _exportProvider = exportProvider;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        // [ProducesResponseType(typeof(GraphicViewModel), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route(("get/kurb"))]
        public IActionResult GetKurbatovByMeasurementId([FromQuery] int measurementRecordingId, [FromQuery] string statNames)
        {
            // string n = "S21<sub>(2.5GHz)</sub>(коэффициент передачи)$S<sub>22(5GHz)</sub>";
            var result = _exportProvider.Export(measurementRecordingId, statNames, "$");
            return Ok(result);
        }

        [HttpPost]
        [Route("create-kurb")]
        public IActionResult CreateKurb([FromBody] JObject kurbatovXLSBodyJObject)
        {
            var kurbatovXLSBody = kurbatovXLSBodyJObject.ToObject<KurbatovXLSBodyModel>();
            var xlsList = kurbatovXLSBody.kurbatovXLSViewModelList.Select(x => _mapper.Map<KurbatovXLS>(x)).ToList();             
            foreach (var xls in xlsList)
            {
                _exportProvider.PopulateKurbatovXLSByValues(xls);
            }

            var XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileDownloadName = $"{Guid.NewGuid().ToString()}.xlsx";
            var reportsFolder = "reports";

            using (var package = createExcelPackage(xlsList, kurbatovXLSBody.WaferId, kurbatovXLSBody.MSLNumber, kurbatovXLSBody.Date))
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));
            }

            return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }

        [HttpGet]
        [Route(("pattern/vp"))]
        public IActionResult GetPatternVP()
        {
            var xlsList = new List<KurbatovXLSViewModel>();
            var row6 = new KurbatovXLSViewModel();
            row6.ElementName = "TC1";
            row6.OperationNumber = "570.00.00";
            row6.StageName = "Снятие рабочей пластины с пластины-носителя";
            row6.parameters.Add(new KurbatovParameter{ParameterName = "Cmicap", Lower = 0.1E-12, Upper = 0.6E-12, DividerId = 1, RussianParameterName="Емкость элемента, Ф", ParameterNameStat="C при U=0.06В"});

            var row7 = new KurbatovXLSViewModel();
            row7.ElementName = "TC5";
            row7.OperationNumber = "570.00.00";
            row7.StageName = "Снятие рабочей пластины с пластины-носителя";
            row7.parameters.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.2, Upper = 2.3, Divider = 1000/75, DividerId = 3, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="r<sub>DS(on)</sub> (сопротивление открытого канала при Uси = 0.02В)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.9, Upper = -1.25, DividerId = 1, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "gmax", Lower = 0.3, Divider = 0.075, DividerId = 3, RussianParameterName="Максимум крутизны, См/мм", ParameterNameStat="g<sub>max</sub> (максимум крутизны)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Igss(-3V)", Lower = -3E-5, Divider = 0.075, DividerId = 3, RussianParameterName="Ток утечки затвора, А/мм", ParameterNameStat="I<sub>GSS(-3V)</sub> (ток утечки затвора при Uзи=-3В)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.3, Upper = 0.6, Divider = 0.075, DividerId = 3, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Ubr", Lower = 10.0, DividerId = 1, RussianParameterName="Напряжение пробоя сток-исток, В", ParameterNameStat="U<sub>BR</sub> (напряжение пробоя сток-исток)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "S21(5GHz)", Lower = 7.8, DividerId = 1, RussianParameterName="Коэффициент передачи, дБ", ParameterNameStat="S21<sub>(5GHz)</sub>(коэффициент передачи)"});
            
            var row8 = new KurbatovXLSViewModel();
            row8.ElementName = "TC6";
            row8.OperationNumber = "570.00.00";
            row8.StageName = "Снятие рабочей пластины с пластины-носителя";
            row8.parameters.Add(new KurbatovParameter{ParameterName = "S21(on)", Lower = -1.5, DividerId = 1, RussianParameterName="Коэффициент передачи в открытом состоянии на частоте 20 ГГц, дБ", ParameterNameStat="S<sub>21ON(20GHz)</sub>"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "S21(off)", Upper = -15, DividerId = 1, RussianParameterName="Коэффициент передачи в закрытом состоянии на частоте 1 ГГц, дБ", ParameterNameStat="S<sub>21OFF(1GHz)</sub>"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.2, Upper = 3.0, Divider=0.6, DividerId = 10, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="R<sub>ds(on)</sub> (сопротивление открытого канала)"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.3, Upper = 0.6, Divider=0.6, DividerId = 10, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.9, Upper = -1.25, DividerId = 1, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)"});
            
            
            var row9 = new KurbatovXLSViewModel();
            row9.ElementName = "TC8";
            row9.OperationNumber = "570.00.00";
            row9.StageName = "Снятие рабочей пластины с пластины-носителя";
            row9.parameters.Add(new KurbatovParameter{ParameterName = "Risol", Lower = 0.1, DividerId = 1, RussianParameterName="Сопротивление межприборной изоляции, ГОм", ParameterNameStat="Risol"});
            
            var row10 = new KurbatovXLSViewModel();
            row10.ElementName = "TC10";
            row10.OperationNumber = "570.00.00";
            row10.StageName = "Снятие рабочей пластины с пластины-носителя";
            row10.parameters.Add(new KurbatovParameter{ParameterName = "Rgate", Upper = 30, DividerId = 1, RussianParameterName="Сопротивление затвора, Ом", ParameterNameStat="Rgate"});

            var row11 = new KurbatovXLSViewModel();
            row11.ElementName = "TC12";
            row11.OperationNumber = "570.00.00";
            row11.StageName = "Снятие рабочей пластины с пластины-носителя";
            row11.parameters.Add(new KurbatovParameter{ParameterName = "Rc", Upper = 0.4, DividerId = 1, RussianParameterName="Контактное сопротивление омических контактов, Ом*мм", ParameterNameStat="Rc"});
            row11.parameters.Add(new KurbatovParameter{ParameterName = "Rs", Lower = 135, Upper = 185, DividerId = 1, RussianParameterName="Слоевое cопротивление, Ом/кв", ParameterNameStat="Rs"});
                        
            var row12 = new KurbatovXLSViewModel();
            row12.ElementName = "TC13";
            row12.OperationNumber = "570.00.00";
            row12.StageName = "Снятие рабочей пластины с пластины-носителя";
            row12.parameters.Add(new KurbatovParameter{ParameterName = "Rtfr_1", Lower = 40, Upper = 60, DividerId = 1, RussianParameterName="Удельное поверхностное сопротивление тонкопленочного резистора, Ом/кв", ParameterNameStat="TFR1_1sq"});

            var row13 = new KurbatovXLSViewModel();
            row13.ElementName = "TC14";
            row13.OperationNumber = "570.00.00";
            row13.StageName = "Снятие рабочей пластины с пластины-носителя";
            row13.parameters.Add(new KurbatovParameter{ParameterName = "Rtfr_2", Lower = 480, Upper = 720, DividerId = 1, RussianParameterName="Удельное поверхностное сопротивление тонкопленочного резистора, Ом/кв", ParameterNameStat="TFR2_1sq"});

            var row15 = new KurbatovXLSViewModel();
            row15.ElementName = "TC20";
            row15.OperationNumber = "570.00.00";
            row15.StageName = "Снятие рабочей пластины с пластины-носителя";
            row15.parameters.Add(new KurbatovParameter{ParameterName = "Rhole", Upper = 3, DividerId = 1, RussianParameterName="Сопротивление металлизированного отверстия, Ом", ParameterNameStat="Rhole"});

            var row16 = new KurbatovXLSViewModel();
            row16.ElementName = "TC21";
            row16.OperationNumber = "570.00.00";
            row16.StageName = "Снятие рабочей пластины с пластины-носителя";
            row16.parameters.Add(new KurbatovParameter{ParameterName = "Rind", Upper = 4, DividerId = 1, RussianParameterName="Сопротивление катушки индуктивности, Ом", ParameterNameStat="Rind"});
            
            var row17 = new KurbatovXLSViewModel();
            row17.ElementName = "TC23";
            row17.OperationNumber = "570.00.00";
            row17.StageName = "Снятие рабочей пластины с пластины-носителя";
            row17.parameters.Add(new KurbatovParameter{ParameterName = "Сmim", Lower = 270, Upper = 350, DividerId = 1, RussianParameterName="Удельная емкость МДМ-конденсатора, пФ/мм2", ParameterNameStat="C<sub>MIM</sub> при U=0.06B (удельная ёмкость МДМ-конденсатора)"});
            row17.parameters.Add(new KurbatovParameter{ParameterName = "Ubrc", Lower = 20, DividerId = 1, RussianParameterName="Пробивное напряжение МДМ-конденсатора, В", ParameterNameStat="U<sub>BRC</sub> (пробивное напряжение МДМ-конденсатора)"});
          
           
            xlsList.Add(row6);
            xlsList.Add(row7);
            xlsList.Add(row8);
            xlsList.Add(row9);
            xlsList.Add(row10);
            xlsList.Add(row11);
            xlsList.Add(row12);
            xlsList.Add(row13);
            xlsList.Add(row15);
            xlsList.Add(row16);
            xlsList.Add(row17);

            return Ok(xlsList);
        }

        [HttpGet]
        [Route(("pattern/kurb"))]
        public IActionResult GetPatternKurb()
        {
            var xlsList = new List<KurbatovXLSViewModel>();
            var row1 = new KurbatovXLSViewModel();
            row1.ElementName = "TC16";            
            row1.OperationNumber = "180.50.00";
            row1.IsAddedToCommonWorksheet = false;
            row1.StageName = "Формирование разводки и нижней обкладки";
            row1.parameters.Add(new KurbatovParameter{ParameterName = "Ro-m1", Upper = 17, DividerId = 1, RussianParameterName="Сопротивление цепочки, Ом", ParameterNameStat="Rchain_OK_RAZV"});
            
            var row2 = new KurbatovXLSViewModel();
            row2.ElementName = "TC17";
            row2.OperationNumber = "180.55.00";
            row2.IsAddedToCommonWorksheet = false;
            row2.StageName = "Формирование разводки и нижней обкладки";
            row2.parameters.Add(new KurbatovParameter{ParameterName = "Rg-m1", Upper = 12, DividerId = 1, RussianParameterName="Сопротивление цепочки, Ом", ParameterNameStat="Rchain_GATE_RAZV"});
            
            var row3 = new KurbatovXLSViewModel();
            row3.ElementName = "TC18";
            row3.OperationNumber = "295.65.00";
            row3.IsAddedToCommonWorksheet = false;
            row3.StageName = "Формирование верхней обкладки, мостов, катушек, гальваники";
            row3.parameters.Add(new KurbatovParameter{ParameterName = "Rm1-m2", Upper = 7, DividerId = 1, RussianParameterName="Сопротивление цепочки, Ом", ParameterNameStat="Rchain_RAZV_GALV"});
           
            var row4 = new KurbatovXLSViewModel();
            row4.ElementName = "TC8";
            row4.OperationNumber = "317.10.00";
            row4.IsAddedToCommonWorksheet = false;
            row4.StageName = "Формирование защиты ВСВ";
            row4.parameters.Add(new KurbatovParameter{ParameterName = "Risol", Lower = 0.15, DividerId = 1, RussianParameterName="Сопротивление межприборной изоляции, ГОм", ParameterNameStat="Risol"});
            
            var row5 = new KurbatovXLSViewModel();
            row5.ElementName = "TC5";
            row5.OperationNumber = "317.25.00";
            row5.IsAddedToCommonWorksheet = false;
            row5.StageName = "Формирование защиты ВСВ";
            row5.parameters.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.25, Upper = 2.1, DividerId = 3, Divider = 1000/75, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="r<sub>DS(on)</sub> (сопротивление открытого канала при Uси = 0.02В)"});
            row5.parameters.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.8, Upper = -1.3, DividerId = 1, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)"});
            row5.parameters.Add(new KurbatovParameter{ParameterName = "gmax", Lower = 0.32, Divider = 0.075, DividerId = 3, RussianParameterName="Максимум крутизны, См/мм", ParameterNameStat="g<sub>max</sub> (максимум крутизны)"});
            row5.parameters.Add(new KurbatovParameter{ParameterName = "Igss(-3V)", Lower = -2E-5, Divider = 0.075, DividerId = 3, RussianParameterName="Ток утечки затвора, А/мм", ParameterNameStat="I<sub>GSS(-3V)</sub> (ток утечки затвора при Uзи=-3В)"});
            row5.parameters.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.32, Upper = 0.56, Divider = 0.075, DividerId = 3, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)"});
            row5.parameters.Add(new KurbatovParameter{ParameterName = "Ubr", Lower = 11.0, DividerId = 1, RussianParameterName="Напряжение пробоя сток-исток, В", ParameterNameStat="U<sub>BR</sub> (напряжение пробоя сток-исток)"});
            row5.parameters.Add(new KurbatovParameter{ParameterName = "S21(5GHz)", Lower = 8.0, DividerId = 1, RussianParameterName="Коэффициент передачи, дБ", ParameterNameStat="S21<sub>(5GHz)</sub>(коэффициент передачи)"});
         
            var row6 = new KurbatovXLSViewModel();
            row6.ElementName = "TC1";
            row6.OperationNumber = "560.00.00";
            row6.StageName = "Снятие рабочей пластины с пластины-носителя";
            row6.parameters.Add(new KurbatovParameter{ParameterName = "Cmicap", Lower = 0.1E-12, Upper = 0.6E-12, DividerId = 1, RussianParameterName="Емкость элемента, Ф", ParameterNameStat="C при U=0.06В"});

            var row7 = new KurbatovXLSViewModel();
            row7.ElementName = "TC5";
            row7.OperationNumber = "560.00.00";
            row7.StageName = "Снятие рабочей пластины с пластины-носителя";
            row7.parameters.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.25, Upper = 2.1, Divider = 1000/75, DividerId = 3, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="r<sub>DS(on)</sub> (сопротивление открытого канала при Uси = 0.02В)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.8, Upper = -1.3, DividerId = 1, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "gmax", Lower = 0.32, Divider = 0.075, DividerId = 3, RussianParameterName="Максимум крутизны, См/мм", ParameterNameStat="g<sub>max</sub> (максимум крутизны)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Igss(-3V)", Lower = -2E-5, Divider = 0.075, DividerId = 3, RussianParameterName="Ток утечки затвора, А/мм", ParameterNameStat="I<sub>GSS(-3V)</sub> (ток утечки затвора при Uзи=-3В)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.32, Upper = 0.56, Divider = 0.075, DividerId = 3, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "Ubr", Lower = 11.0, DividerId = 1, RussianParameterName="Напряжение пробоя сток-исток, В", ParameterNameStat="U<sub>BR</sub> (напряжение пробоя сток-исток)"});
            row7.parameters.Add(new KurbatovParameter{ParameterName = "S21(5GHz)", Lower = 8, DividerId = 1, RussianParameterName="Коэффициент передачи, дБ", ParameterNameStat="S21<sub>(5GHz)</sub>(коэффициент передачи)"});
            
            var row8 = new KurbatovXLSViewModel();
            row8.ElementName = "TC6";
            row8.OperationNumber = "560.00.00";
            row8.StageName = "Снятие рабочей пластины с пластины-носителя";
            row8.parameters.Add(new KurbatovParameter{ParameterName = "S21(on)", Lower = -1.5, DividerId = 1, RussianParameterName="Коэффициент передачи в открытом состоянии на частоте 20 ГГц, дБ", ParameterNameStat="S<sub>21ON(20GHz)</sub>"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "S21(off)", Upper = -15, DividerId = 1, RussianParameterName="Коэффициент передачи в закрытом состоянии на частоте 1 ГГц, дБ", ParameterNameStat="S<sub>21OFF(1GHz)</sub>"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.2, Upper = 2.7, Divider=0.6, DividerId = 10, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="R<sub>ds(on)</sub> (сопротивление открытого канала)"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.32, Upper = 0.56, Divider=0.6, DividerId = 10, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)"});
            row8.parameters.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.8, Upper = -1.3, DividerId = 1, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)"});
            
            
            var row9 = new KurbatovXLSViewModel();
            row9.ElementName = "TC8";
            row9.OperationNumber = "560.00.00";
            row9.StageName = "Снятие рабочей пластины с пластины-носителя";
            row9.parameters.Add(new KurbatovParameter{ParameterName = "Risol", Lower = 0.15, DividerId = 1, RussianParameterName="Сопротивление межприборной изоляции, ГОм", ParameterNameStat="Risol"});
            
            var row10 = new KurbatovXLSViewModel();
            row10.ElementName = "TC10";
            row10.OperationNumber = "560.00.00";
            row10.StageName = "Снятие рабочей пластины с пластины-носителя";
            row10.parameters.Add(new KurbatovParameter{ParameterName = "Rgate", Upper = 27, DividerId = 1, RussianParameterName="Сопротивление затвора, Ом", ParameterNameStat="Rgate"});

            var row11 = new KurbatovXLSViewModel();
            row11.ElementName = "TC12";
            row11.OperationNumber = "560.00.00";
            row11.StageName = "Снятие рабочей пластины с пластины-носителя";
            row11.parameters.Add(new KurbatovParameter{ParameterName = "Rc", Upper = 0.35, DividerId = 1, RussianParameterName="Контактное сопротивление омических контактов, Ом*мм", ParameterNameStat="Rc"});
            row11.parameters.Add(new KurbatovParameter{ParameterName = "Rs", Lower = 140, Upper = 180, DividerId = 1, RussianParameterName="Слоевое cопротивление, Ом/кв", ParameterNameStat="Rs"});
                        
            var row12 = new KurbatovXLSViewModel();
            row12.ElementName = "TC13";
            row12.OperationNumber = "560.00.00";
            row12.StageName = "Снятие рабочей пластины с пластины-носителя";
            row12.parameters.Add(new KurbatovParameter{ParameterName = "Rtfr_1", Lower = 42, Upper = 58, DividerId = 1, RussianParameterName="Удельное поверхностное сопротивление тонкопленочного резистора, Ом/кв", ParameterNameStat="TFR1_1sq"});

            var row13 = new KurbatovXLSViewModel();
            row13.ElementName = "TC14";
            row13.OperationNumber = "560.00.00";
            row13.StageName = "Снятие рабочей пластины с пластины-носителя";
            row13.parameters.Add(new KurbatovParameter{ParameterName = "Rtfr_2", Lower = 485, Upper = 715, DividerId = 1, RussianParameterName="Удельное поверхностное сопротивление тонкопленочного резистора, Ом/кв", ParameterNameStat="TFR2_1sq"});

            var row14 = new KurbatovXLSViewModel();
            row14.ElementName = "TC19";
            row14.OperationNumber = "560.00.00";
            row14.StageName = "Снятие рабочей пластины с пластины-носителя";
            row14.parameters.Add(new KurbatovParameter{ParameterName = "Rm2", Upper = 7, DividerId = 1, RussianParameterName="Сопротивление элемента 'змейка', Ом", ParameterNameStat="Rline"});

            var row15 = new KurbatovXLSViewModel();
            row15.ElementName = "TC20";
            row15.OperationNumber = "560.00.00";
            row15.StageName = "Снятие рабочей пластины с пластины-носителя";
            row15.parameters.Add(new KurbatovParameter{ParameterName = "Rhole", Upper = 2.7, DividerId = 1, RussianParameterName="Сопротивление металлизированного отверстия, Ом", ParameterNameStat="Rhole"});

            var row16 = new KurbatovXLSViewModel();
            row16.ElementName = "TC21";
            row16.OperationNumber = "560.00.00";
            row16.StageName = "Снятие рабочей пластины с пластины-носителя";
            row16.parameters.Add(new KurbatovParameter{ParameterName = "Rind", Upper = 3.6, DividerId = 1, RussianParameterName="Сопротивление катушки индуктивности, Ом", ParameterNameStat="Rind"});
            
            var row17 = new KurbatovXLSViewModel();
            row17.ElementName = "TC23";
            row17.OperationNumber = "560.00.00";
            row17.StageName = "Снятие рабочей пластины с пластины-носителя";
            row17.parameters.Add(new KurbatovParameter{ParameterName = "Сmim", Lower = 275, Upper = 345, DividerId = 1, RussianParameterName="Удельная емкость МДМ-конденсатора, пФ/мм2", ParameterNameStat="C<sub>MIM</sub> при U=0.06B (удельная ёмкость МДМ-конденсатора)"});
            row17.parameters.Add(new KurbatovParameter{ParameterName = "Ubrc", Lower = 20, DividerId = 1, RussianParameterName="Пробивное напряжение МДМ-конденсатора, В", ParameterNameStat="U<sub>BRC</sub> (пробивное напряжение МДМ-конденсатора)"});
          
            xlsList.Add(row1);
            xlsList.Add(row2);
            xlsList.Add(row3);
            xlsList.Add(row4);
            xlsList.Add(row5);
            xlsList.Add(row6);
            xlsList.Add(row7);
            xlsList.Add(row8);
            xlsList.Add(row9);
            xlsList.Add(row10);
            xlsList.Add(row11);
            xlsList.Add(row12);
            xlsList.Add(row13);
            xlsList.Add(row14);
            xlsList.Add(row15);
            xlsList.Add(row16);
            xlsList.Add(row17);

            return Ok(xlsList);
        }
       
        private ExcelPackage createExcelPackage(List<KurbatovXLS> xlsList, string waferId, string mslNumber, string date)
        {       
         
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Report";
            package.Workbook.Properties.Author = "Roma Molchanov";

            var commonWorksheet = package.Workbook.Worksheets.Add("Сводная");
            commonWorksheet.Cells[1, 1].Value = "№ тестовой структуры";
            commonWorksheet.Cells[1, 2].Value = "Порядковый номер и наименование параметра, единица измерения";
            commonWorksheet.Cells[1, 3].Value = "Буквенное обозначение параметра";
            commonWorksheet.Cells[1, 4].Value = "Результат измерений";
            commonWorksheet.Cells[1, 5].Value = "Процент годных ТС";
            commonWorksheet.Cells[1, 6].Value = $"Приложение 1 к МСЛ №{mslNumber}";
            commonWorksheet.Cells[1, 6, 1, 8].Merge = true;
            commonWorksheet.Cells[2, 6].Value = $"Пластина {waferId}";
            commonWorksheet.Cells[2, 6, 2, 8].Merge = true;
            commonWorksheet.Cells[3, 6].Value = $"Дата: {date}";
            commonWorksheet.Cells[3, 6, 3, 8].Merge = true;
            commonWorksheet.Row(1).Height = 20;
            for (int i = 1; i < 6; i++)
            {
                commonWorksheet.Column(i).Width = 20;
                commonWorksheet.Column(i).Style.WrapText = true;
                commonWorksheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                commonWorksheet.Column(i).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            commonWorksheet.Column(2).Width = 30;
           
            var currentCursor = 2;

            foreach (var kurbatovXLS in xlsList.Select((value, i) => new { i, value }))
            {             
                  
                var worksheet = package.Workbook.Worksheets.Add(kurbatovXLS.value.OperationNumber + "_" + kurbatovXLS.value.ElementName);
               
           
                worksheet.Cells[6, 1, kurbatovXLS.value.kpList[0].advList.Count + 9, 2 + kurbatovXLS.value.kpList.Count].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[6, 1, kurbatovXLS.value.kpList[0].advList.Count + 9, 2 + kurbatovXLS.value.kpList.Count].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[6, 1, kurbatovXLS.value.kpList[0].advList.Count + 9, 2 + kurbatovXLS.value.kpList.Count].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[6, 1, kurbatovXLS.value.kpList[0].advList.Count + 9, 2 + kurbatovXLS.value.kpList.Count].Style.Border.Left.Style = ExcelBorderStyle.Thin;
             
                worksheet.Cells["A1:C5"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["A1:C5"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["A1:C5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["A1:C5"].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                worksheet.Cells[1, 1].Value = "Номер операции:";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 3].Value = kurbatovXLS.value.OperationNumber;
                worksheet.Cells[1, 1, 1, 2].Merge = true;

                worksheet.Cells[1, 4].Value = $"Приложение {kurbatovXLS.i + 2} к МСЛ №{mslNumber}";
                worksheet.Cells[1, 4, 1, 6].Merge = true;
                worksheet.Cells[2, 4].Value = $"Пластина {waferId}";
                worksheet.Cells[2, 4, 2, 6].Merge = true;
                worksheet.Cells[3, 4].Value = $"Дата: {date}";
                worksheet.Cells[3, 4, 3, 6].Merge = true;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 20;
                worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             
                worksheet.Cells[2, 1].Value = "Элемент:";
                worksheet.Cells[2, 1].Style.Font.Bold = true;
                worksheet.Cells[2, 3].Value = kurbatovXLS.value.ElementName;
                worksheet.Cells[2, 1, 2, 2].Merge = true;
                
                worksheet.Cells[3, 1].Value = "Количество кристаллов:";
                worksheet.Cells[3, 1].Style.Font.Bold = true;
                worksheet.Cells[3, 3].Value = kurbatovXLS.value.DieQuantity;
                worksheet.Cells[3, 1, 3, 2].Merge = true;
                
                worksheet.Cells[4, 1].Value = "Количество годных:";
                worksheet.Cells[4, 1].Style.Font.Bold = true;
                worksheet.Cells[4, 3].Value =  kurbatovXLS.value.DieQuantity - kurbatovXLS.value.DirtyCodesList.Count;
                worksheet.Cells[4, 1, 4, 2].Merge = true;
            
                worksheet.Cells[5, 1].Value = "Процент годных:";
                worksheet.Cells[5, 1].Style.Font.Bold = true;
                worksheet.Cells[5, 3].Value =  100 - kurbatovXLS.value.DirtyPercentage + "%";
                worksheet.Cells[5, 1, 5, 2].Merge = true;
            
                worksheet.Column(1).Width = 13;
                worksheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                
                worksheet.Column(2).Width = 20;

                worksheet.Cells[6, 2].Value = "Контролируемый параметр, ед. изм.";
                worksheet.Cells[6, 2].Style.Font.Bold = true;
                worksheet.Cells[6, 2, 6, 1 + kurbatovXLS.value.kpList.Count].Merge = true;
             
                List<AtomicDieValue> list = kurbatovXLS.value.kpList.FirstOrDefault()?.advList;
                worksheet.Cells[9, 1].Value = "№ кристалла";
                worksheet.Cells[9, 1].Style.Font.Bold = true;

                worksheet.Cells[9, 2 + kurbatovXLS.value.kpList.Count].Value = "Соответствие";
                worksheet.Cells[9, 2 + kurbatovXLS.value.kpList.Count].Style.Font.Bold = true;
                worksheet.Column(2 + kurbatovXLS.value.kpList.Count).Width = 13;
                worksheet.Column(2 + kurbatovXLS.value.kpList.Count).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(2 + kurbatovXLS.value.kpList.Count).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                for (int i = 0; i < list.Count; i++)
                {
                    AtomicDieValue item = (AtomicDieValue)list[i];
                    worksheet.Cells[10 + i, 1].Value = item.DieCode;
                
                    if(kurbatovXLS.value.DirtyCodesList.Contains(i + 1))
                    {
                        worksheet.Cells[10 + i, 2 + kurbatovXLS.value.kpList.Count].Value = "Брак";
                    }
                    else
                    {
                        worksheet.Cells[10 + i, 2 + kurbatovXLS.value.kpList.Count].Value = "Годен";
                    }
                
                }
                for (int i = 0; i < kurbatovXLS.value.kpList.Count; i++)
                {
                    if(kurbatovXLS.value.IsAddedToCommonWorksheet)
                    {
                        if(i == 0)
                        {
                            commonWorksheet.Cells[currentCursor, 1].Value = kurbatovXLS.value.ElementName;
                            commonWorksheet.Cells[currentCursor, 5].Value = 100.0 - kurbatovXLS.value.DirtyPercentage;
                            commonWorksheet.Cells[currentCursor, 1, currentCursor + kurbatovXLS.value.kpList.Count - 1, 1].Merge = true;
                            commonWorksheet.Cells[currentCursor, 5, currentCursor + kurbatovXLS.value.kpList.Count - 1, 5].Merge = true;
                        }                   
                        commonWorksheet.Cells[currentCursor, 2].Value = (i+1) + " " + kurbatovXLS.value.kpList[i].RussianParameterName;
                        commonWorksheet.Cells[currentCursor, 3].Value = kurbatovXLS.value.kpList[i].ParameterName;
                        var averageGood = kurbatovXLS.value.kpList[i].AverageGood;
                        if ((Math.Abs(averageGood) >= 10000 || Math.Abs(averageGood) < 1E-2))
                        {
                            commonWorksheet.Cells[currentCursor, 4].Value = averageGood.ToString("0.00E0");
                        }
                        else
                        {
                            commonWorksheet.Cells[currentCursor, 4].Value = averageGood.ToString("0.00");
                        }                        
                        currentCursor++;
                    }
                    
                    worksheet.Cells[9, 2 + i].Value = "Измеренное значение";
                    worksheet.Column(2 + i).Width = 20;
                    worksheet.Column(2 + i).Style.WrapText = true;
                    worksheet.Column(2 + i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Column(2 + i).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[9, 2 + i].Style.Font.Bold = true;
                    worksheet.Cells[7, 2 + i].Value = kurbatovXLS.value.kpList[i].RussianParameterName;
                    var bounds = String.Empty;
                    if(!Double.IsNaN(kurbatovXLS.value.kpList[i].Lower))
                        bounds = bounds + "не менее " + kurbatovXLS.value.kpList[i].Lower;
                    if(!Double.IsNaN(kurbatovXLS.value.kpList[i].Upper))
                    {
                        if(!String.IsNullOrWhiteSpace(bounds))
                              bounds = bounds + ";" + Environment.NewLine;
                        bounds = bounds + "не более " + kurbatovXLS.value.kpList[i].Upper;
                    }
                      
                    worksheet.Cells[8, 2 + i].Value = bounds;
                    for (int j = 0; j < kurbatovXLS.value.kpList[i].advList.Count; j++)
                    {
                         var value = kurbatovXLS.value.kpList[i].advList[j].Value;
                         if ((Math.Abs(value) >= 10000 || Math.Abs(value) < 1E-2))
                         {
                            worksheet.Cells[10 + j, 2 + i].Value = value.ToString("0.00E0");
                         }
                         else
                         {
                            worksheet.Cells[10 + j, 2 + i].Value = value.ToString("0.00");
                         }
                      
                        
                    } 
                }
            }

            commonWorksheet.Cells[1, 1, currentCursor - 1, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            commonWorksheet.Cells[1, 1, currentCursor - 1, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            commonWorksheet.Cells[1, 1, currentCursor - 1, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            commonWorksheet.Cells[1, 1, currentCursor - 1, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            
            return package;
        }
    }
}