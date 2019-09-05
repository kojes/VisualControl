using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Models.SRV6.Export;
using VueExample.StatisticsCore.Abstract;
using VueExample.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using OfficeOpenXml.Style;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]

    public class ExportController : Controller
    {
        private readonly IExportProvider _exportProvider;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ExportController(IExportProvider exportProvider, IHostingEnvironment hostingEnvironment)
        {
            _exportProvider = exportProvider;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpGet]
        [Route(("get/kurbxls"))]
        public IActionResult GetKurbatovXLS()
        {
            var xlsList = new List<KurbatovXLS>();
            var row1 = new KurbatovXLS();
            row1.Element = "TC16";
            row1.OperationNumber = "180.50.00";
            row1.kpList.Add(new KurbatovParameter{ParameterName = "Ro-m1", Upper = 17, RussianParameterName="Сопротивление цепочки, Ом", ParameterNameStat="Rchain_OK_RAZV", MeasurementRecordingId=25653});
            
            var row2 = new KurbatovXLS();
            row2.Element = "TC17";
            row2.OperationNumber = "180.55.00";
            row2.kpList.Add(new KurbatovParameter{ParameterName = "Rg-m1", Upper = 12, RussianParameterName="Сопротивление цепочки, Ом", ParameterNameStat="Rchain_GATE_RAZV", MeasurementRecordingId=25653});
            
            var row3 = new KurbatovXLS();
            row3.Element = "TC18";
            row3.OperationNumber = "295.65.00";
            row3.kpList.Add(new KurbatovParameter{ParameterName = "Rm1-m2", Upper = 7, RussianParameterName="Сопротивление цепочки, Ом", ParameterNameStat="Rchain_RAZV_GALV", MeasurementRecordingId=25786});
           
            var row4 = new KurbatovXLS();
            row4.Element = "TC8";
            row4.OperationNumber = "317.10.00";
            row4.kpList.Add(new KurbatovParameter{ParameterName = "Risol", Lower = 0.15, RussianParameterName="Сопротивление межприборной изоляции, ГОм", ParameterNameStat="Risol", MeasurementRecordingId=25852});
            
            var row5 = new KurbatovXLS();
            row5.Element = "TC5";
            row5.OperationNumber = "317.25.00";
            row5.kpList.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.25, Upper = 2.1, Divider = 1000/75, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="r<sub>DS(on)</sub> (сопротивление открытого канала при Uси = 0.02В)", MeasurementRecordingId=25851});
            row5.kpList.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.8, Upper = -1.33, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)", MeasurementRecordingId=25851});
            row5.kpList.Add(new KurbatovParameter{ParameterName = "gmax", Lower = 0.32, Divider = 0.075, RussianParameterName="Максимум крутизны, См/мм", ParameterNameStat="g<sub>max</sub> (максимум крутизны)", MeasurementRecordingId=25851});
            row5.kpList.Add(new KurbatovParameter{ParameterName = "Igss(-3V)", Lower = -2E-5, Divider = 0.075, RussianParameterName="Ток утечки затвора, А/мм", ParameterNameStat="I<sub>GSS(-3V)</sub> (ток утечки затвора при Uзи=-3В)", MeasurementRecordingId=25851});
            row5.kpList.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.32, Upper = 0.56, Divider = 0.075, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)", MeasurementRecordingId=25851});
            row5.kpList.Add(new KurbatovParameter{ParameterName = "Ubr", Lower = 11.0, RussianParameterName="Напряжение пробоя сток-исток, В", ParameterNameStat="U<sub>BR</sub> (напряжение пробоя сток-исток)", MeasurementRecordingId=25851});
            row5.kpList.Add(new KurbatovParameter{ParameterName = "S21(5GHz)", Lower = 8.0, RussianParameterName="Коэффициент передачи, дБ", ParameterNameStat="S21<sub>(5GHz)</sub>(коэффициент передачи)", MeasurementRecordingId=26323});
         
            var row6 = new KurbatovXLS();
            row6.Element = "TC1";
            row6.OperationNumber = "560.00.00";
            row6.kpList.Add(new KurbatovParameter{ParameterName = "Cmicap", Lower = 0.1E-12, Upper = 0.6E-12, RussianParameterName="Емкость элемента, Ф", ParameterNameStat="C при U=0.06В", MeasurementRecordingId=26252});

            var row7 = new KurbatovXLS();
            row7.Element = "TC5";
            row7.OperationNumber = "560.00.00";
            row7.kpList.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.2, Upper = 2.3, Divider = 1000/75, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="r<sub>DS(on)</sub> (сопротивление открытого канала при Uси = 0.02В)", MeasurementRecordingId=26238});
            row7.kpList.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.9, Upper = -1.3, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)", MeasurementRecordingId=26238});
            row7.kpList.Add(new KurbatovParameter{ParameterName = "gmax", Lower = 0.3, Divider = 0.075, RussianParameterName="Максимум крутизны, См/мм", ParameterNameStat="g<sub>max</sub> (максимум крутизны)", MeasurementRecordingId=26238});
            row7.kpList.Add(new KurbatovParameter{ParameterName = "Igss(-3V)", Lower = -3E-5, Divider = 0.075, RussianParameterName="Ток утечки затвора, А/мм", ParameterNameStat="I<sub>GSS(-3V)</sub> (ток утечки затвора при Uзи=-3В)", MeasurementRecordingId=26238});
            row7.kpList.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.3, Upper = 0.6, Divider = 0.075, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)", MeasurementRecordingId=26238});
            row7.kpList.Add(new KurbatovParameter{ParameterName = "Ubr", Lower = 10.0, RussianParameterName="Напряжение пробоя сток-исток, В", ParameterNameStat="U<sub>BR</sub> (напряжение пробоя сток-исток)", MeasurementRecordingId=26238});
            row7.kpList.Add(new KurbatovParameter{ParameterName = "S21(5GHz)", Lower = 7.8, RussianParameterName="Коэффициент передачи, дБ", ParameterNameStat="S21<sub>(5GHz)</sub>(коэффициент передачи)", MeasurementRecordingId=26323});
            
            var row8 = new KurbatovXLS();
            row8.Element = "TC6";
            row8.OperationNumber = "560.00.00";
            row8.kpList.Add(new KurbatovParameter{ParameterName = "S21(on)", Lower = -1.5, RussianParameterName="Коэффициент передачи в открытом состоянии на частоте 20 ГГц, дБ", ParameterNameStat="S<sub>21ON(20GHz)</sub>", MeasurementRecordingId=26320});
            row8.kpList.Add(new KurbatovParameter{ParameterName = "S21(off)", Upper = -15, RussianParameterName="Коэффициент передачи в закрытом состоянии на частоте 1 ГГц, дБ", ParameterNameStat="S<sub>21OFF(1GHz)</sub>", MeasurementRecordingId=26320});
            row8.kpList.Add(new KurbatovParameter{ParameterName = "rDS(on)", Lower = 1.2, Upper = 3.0, RussianParameterName="Сопротивление открытого канала, Ом*мм", ParameterNameStat="R<sub>ds(on)</sub> (сопротивление открытого канала)", MeasurementRecordingId=26320});
            row8.kpList.Add(new KurbatovParameter{ParameterName = "Idss(3V)", Lower = 0.3, Divider = 0.6, RussianParameterName="Начальный ток стока, A/мм", ParameterNameStat="I<sub>DSS(3V)</sub> (начальный ток стока)", MeasurementRecordingId=26320});
            row8.kpList.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.8, Upper = -1.33, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)", MeasurementRecordingId=26320});
            row8.kpList.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.8, Upper = -1.33, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)", MeasurementRecordingId=26319});
            row8.kpList.Add(new KurbatovParameter{ParameterName = "Ugs(off)", Lower = -1.8, Upper = -1.33, RussianParameterName="Напряжение отсечки, В", ParameterNameStat="U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)", MeasurementRecordingId=26326});
            
            var row9 = new KurbatovXLS();
            row9.Element = "TC8";
            row9.OperationNumber = "560.00.00";
            row9.kpList.Add(new KurbatovParameter{ParameterName = "Risol", Lower = 0.1, RussianParameterName="Сопротивление межприборной изоляции, ГОм", ParameterNameStat="Risol", MeasurementRecordingId=26246});
            
            var row10 = new KurbatovXLS();
            row10.Element = "TC10";
            row10.OperationNumber = "560.00.00";
            row10.kpList.Add(new KurbatovParameter{ParameterName = "Rgate", Upper = 30, RussianParameterName="Сопротивление затвора, Ом", ParameterNameStat="Rgate", MeasurementRecordingId=26242});

            var row11 = new KurbatovXLS();
            row11.Element = "TC12";
            row11.OperationNumber = "560.00.00";
            row11.kpList.Add(new KurbatovParameter{ParameterName = "Rc", Upper = 0.4, RussianParameterName="Контактное сопротивление омических контактов, Ом*мм", ParameterNameStat="Rc", MeasurementRecordingId=26249});
            row11.kpList.Add(new KurbatovParameter{ParameterName = "Rs", Lower = 135, Upper = 185, RussianParameterName="Слоевое cопротивление, Ом/кв", ParameterNameStat="Rs", MeasurementRecordingId=26249});
                        
            var row12 = new KurbatovXLS();
            row12.Element = "TC13";
            row12.OperationNumber = "560.00.00";
            row12.kpList.Add(new KurbatovParameter{ParameterName = "Rtfr_1", Lower = 40, Upper = 60, RussianParameterName="Удельное поверхностное сопротивление тонкопленочного резистора, Ом/кв", ParameterNameStat="TFR1_1sq", MeasurementRecordingId=26250});

            var row13 = new KurbatovXLS();
            row13.Element = "TC14";
            row13.OperationNumber = "560.00.00";
            row13.kpList.Add(new KurbatovParameter{ParameterName = "Rtfr_2", Lower = 480, Upper = 720, RussianParameterName="Удельное поверхностное сопротивление тонкопленочного резистора, Ом/кв", ParameterNameStat="TFR1_1sq", MeasurementRecordingId=26251});

            var row14 = new KurbatovXLS();
            row14.Element = "TC19";
            row14.OperationNumber = "560.00.00";
            row14.kpList.Add(new KurbatovParameter{ParameterName = "Rm2", Upper = 7, RussianParameterName="Сопротивление элемента 'змейка', Ом", ParameterNameStat="Rline", MeasurementRecordingId=26247});

            var row15 = new KurbatovXLS();
            row15.Element = "TC20";
            row15.OperationNumber = "560.00.00";
            row15.kpList.Add(new KurbatovParameter{ParameterName = "Rhole", Upper = 3, RussianParameterName="Сопротивление металлизированного отверстия, Ом", ParameterNameStat="Rhole", MeasurementRecordingId=26244});

            var row16 = new KurbatovXLS();
            row16.Element = "TC21";
            row16.OperationNumber = "560.00.00";
            row16.kpList.Add(new KurbatovParameter{ParameterName = "Rind", Upper = 4, RussianParameterName="Сопротивление катушки индуктивности, Ом", ParameterNameStat="Rind", MeasurementRecordingId=26245});
            
            var row17 = new KurbatovXLS();
            row17.Element = "TC23";
            row17.OperationNumber = "560.00.00";
            row17.kpList.Add(new KurbatovParameter{ParameterName = "Сmim", Lower = 270, Upper = 350, RussianParameterName="Удельная емкость МДМ-конденсатора, пФ/мм2", ParameterNameStat="C<sub>MIM</sub> при U=0.06B (удельная ёмкость МДМ-конденсатора)", MeasurementRecordingId=26253});
            row17.kpList.Add(new KurbatovParameter{ParameterName = "Ubrc", Lower = 20, RussianParameterName="Пробивное напряжение МДМ-конденсатора, В", ParameterNameStat="U<sub>BRC</sub> (пробивное напряжение МДМ-конденсатора)", MeasurementRecordingId=26253});
          
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
            
            foreach (var xls in xlsList)
            {
                _exportProvider.PopulateKurbatovXLSByValues(xls);
            }

            var XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileDownloadName = $"{Guid.NewGuid().ToString()}.xlsx";
            var reportsFolder = "reports";

            using (var package = createExcelPackage(xlsList))
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));
            }

            return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }
        
        private ExcelPackage createExcelPackage(List<KurbatovXLS> xlsList)
        {       
         
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Report";
            package.Workbook.Properties.Author = "Roma Molchanov";

            foreach (var kurbatovXLS in xlsList.Select((value, i) => new { i, value }))
            {             
                  
                var worksheet = package.Workbook.Worksheets.Add(kurbatovXLS.value.OperationNumber + "_" + kurbatovXLS.value.Element);
               
           
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

                worksheet.Cells[1, 4].Value = $"Приложение {kurbatovXLS.i + 1} к МСЛ №";
                worksheet.Cells[1, 4, 1, 6].Merge = true;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 20;
                worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
             
                worksheet.Cells[2, 1].Value = "Элемент:";
                worksheet.Cells[2, 1].Style.Font.Bold = true;
                worksheet.Cells[2, 3].Value = kurbatovXLS.value.Element;
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
            return package;
        }
    }
}