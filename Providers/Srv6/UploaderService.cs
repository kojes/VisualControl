using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Entities;
using VueExample.Models.SRV6.Uploader;
using VueExample.Providers.Abstract;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Providers.Srv6 
{
    public class UploaderService : IUploaderService 
    {
        private readonly IStandartWaferService _standartWaferService;
        private readonly IDieProvider _dieProvider;
        private readonly IDieValueService _dieValueService;
        private readonly IShortLinkProvider _shortLinkProvider;
        private readonly MeasurementRecordingService _measurementRecordingService = new MeasurementRecordingService();
        public UploaderService (IStandartWaferService standartWaferService, IDieProvider dieProvider, IDieValueService dieValueService, IShortLinkProvider shortLinkProvider) 
        {
            _standartWaferService = standartWaferService;
            _dieProvider = dieProvider;
            _shortLinkProvider = shortLinkProvider;
            _dieValueService = dieValueService;
        }
        public async Task<string> Uploading (UploadingFile uploadingFile, int type) 
        {
            DateTime modifieddate = DateTime.Now;
            var diegraphicList = new List<DieGraphics>();
            var dieList = new List<string>();
            foreach (var data in uploadingFile.Data) 
            {
                string code = data.Key;
                if (!String.IsNullOrEmpty (uploadingFile.Map)) 
                {
                    code = await _standartWaferService.GetCodeFromStandartWafer(code, uploadingFile.Map);
                }
                var die = _dieProvider.GetByWaferIdAndCode(uploadingFile.WaferId, code);
                if (die != null) 
                {
                    dieList.Add(Convert.ToString(die.Id));
                    if (uploadingFile.IsNewMeasurement) 
                    {
                        await _dieProvider.CreateDieParameter(die.Id, (int) uploadingFile.MeasurementRecordingId);
                    }

                    if (type == 1) 
                    {
                        for (var i = 0; i < data.Value.ValueLists.FirstOrDefault().Value.Count; i++) 
                        {
                            var ordinateList = new List<string>();
                            var abcissList = new List<string>();
                            abcissList = data.Value.AbscissList.GetRange (0, data.Value.AbscissList.Count);

                            foreach (var point in data.Value.ValueLists) 
                            {
                                if (i <= point.Value.Count - 1) {
                                    ordinateList.Add (point.Value[i]);
                                }
                            }

                            if (double.Parse(abcissList.FirstOrDefault(), CultureInfo.InvariantCulture) > double.Parse(abcissList.Last(), CultureInfo.InvariantCulture)) 
                            {
                                ordinateList.Reverse();
                                abcissList.Reverse();
                            }

                            var graphicstring = abcissList.Aggregate(String.Empty, (current, abc) => current + "*" + abc);
                            graphicstring = graphicstring + "X";
                            graphicstring = ordinateList.Aggregate(graphicstring, (current, ord) => current + "*" + ord);
                            diegraphicList.Add (new DieGraphics 
                            {
                                    DieId = die.Id,
                                    GraphicId = uploadingFile.Graphics[i].Id,
                                    MeasurementRecordingId = uploadingFile.MeasurementRecordingId,
                                    ValueString = graphicstring

                            });
                        }
                    }

                    if (type == 2) 
                    {
                        for (int i = 0; i < uploadingFile.Graphics.Count; i++) 
                        {
                            diegraphicList.Add (new DieGraphics 
                            {
                                    DieId = die.Id,
                                    GraphicId = uploadingFile.Graphics[i].Id,
                                    MeasurementRecordingId = uploadingFile.MeasurementRecordingId,
                                    ValueString = code + "X" + (i == 0 ? Convert.ToString(data.Value.AbscissList[i]) : Convert.ToString(data.Value.ValueLists.FirstOrDefault().Value[i - 1]))
                            });
                        }
                    }
                }
            }

            await _dieValueService.CreateDieGraphics(diegraphicList);
            foreach (var graphic in uploadingFile.Graphics)
            {
                await _measurementRecordingService.CreateFkMrGraphics (new FkMrGraphic 
                {
                        GraphicId = graphic.Id,
                        MeasurementRecordingId = uploadingFile.MeasurementRecordingId,
                        DateFile = modifieddate,
                        DateImport = DateTime.Now,
                        Operator = uploadingFile.UserName,
                        Comment = uploadingFile.Comment
                });
            }

            var dieidlink = dieList.Aggregate(String.Empty, (current, l) => current + "x" + l);
            var shorturl = _shortLinkProvider.Obfuscate(dieidlink.Remove(0,1));
            var fullLink = "srv6.svr.lan/Ns/WaferMapFaster.aspx?wid=" + uploadingFile.WaferId + "&idmrpcm=" + uploadingFile.MeasurementRecordingId + "&islink=t&dies=" +
                shorturl + "&square=" + "0" + "&rddl=" + "0" + "&lg=" + "false";
            var link = await _shortLinkProvider.Create(fullLink);
            return link.ShortLink;
        }
    }
}