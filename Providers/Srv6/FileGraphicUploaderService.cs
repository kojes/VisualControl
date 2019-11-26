using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Exceptions;
using VueExample.Models.SRV6.Uploader;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6
{
    public class FileGraphicUploaderService : IFileGraphicUploaderService
    {
        public async Task<GraphicName> AddGraphicToFileName(int fileNameId, GraphicNameUploaderViewModel graphicNameUploaderViewModel)
        {
            using (var db = new Srv6Context()) 
            {
                var graphicName = db.GraphicNames.Join(db.FileNameGraphics.Where(x => x.Variant == graphicNameUploaderViewModel.Variant && x.FileNameId == fileNameId), 
                                                       c => c.Id,
                                                       p => p.GraphicNameId,
                                                       (c,p) => p.GraphicName);
                if(graphicName is null)
                {
                    var graphic = new GraphicName{Name = graphicNameUploaderViewModel.Name};
                    db.GraphicNames.Add(graphic);
                    await db.SaveChangesAsync();
                    db.FileNameGraphics.Add(new FileNameGraphic{FileNameId = fileNameId, 
                                                                    GraphicNameId = graphicNameUploaderViewModel.Id, 
                                                                    Variant = graphicNameUploaderViewModel.Variant});
                    await db.SaveChangesAsync();
                    return graphic;
                }
                else
                {
                    throw new DuplicateException("Такой график уже существует");
                }
               
            }
        }

        public async Task<FileName> CreateFileName(FileNameUploaderViewModel fileNameViewModel)
        {
            using (var db = new Srv6Context()) 
            {
                if(db.FileNames.Count(x => x.ProcessId == fileNameViewModel.ProcessId && x.Name == fileNameViewModel.Name) > 0)
                {
                    throw new DuplicateException("Уже существует файл с таким именем");
                }
                var fileName = new FileName{Id = fileNameViewModel.Id, Name = fileNameViewModel.Name, ProcessId = fileNameViewModel.ProcessId};
                db.FileNames.Add(fileName);
                var graphicsList = fileNameViewModel.GraphicNames.Select(x => new GraphicName {Name = x.Name}).ToList();
                db.GraphicNames.AddRange(graphicsList);
                await db.SaveChangesAsync();
                var fileNameGraphics = graphicsList.Select(x => new FileNameGraphic{GraphicNameId = x.Id, FileNameId = fileName.Id, Variant = fileNameViewModel.GraphicNames.FirstOrDefault(g => g.Name == x.Name).Variant});
                db.FileNameGraphics.AddRange(fileNameGraphics);
                await db.SaveChangesAsync();
                return fileName;
            }
        }

        public async Task DeleteGraphicFromFileName(int fileNameId, GraphicNameUploaderViewModel graphicNameUploaderViewModel)
        {
            using (var db = new Srv6Context()) 
            {
                var fileNameGraphic = await db.FileNameGraphics.FirstOrDefaultAsync(x => x.Variant == graphicNameUploaderViewModel.Variant 
                                                                && x.GraphicNameId == graphicNameUploaderViewModel.Id 
                                                                && x.FileNameId == fileNameId);
                db.FileNameGraphics.Remove(fileNameGraphic);
                await db.SaveChangesAsync();
            }
        }       

        public async Task<IList<FileName>> GetAllFileNamesByProcessId(int processId)
        {
            using(var db = new Srv6Context()) 
            {
                return await db.FileNames.Where(x => x.ProcessId == processId).ToListAsync();
            }
        }

        public async Task<IList<GraphicNameUploaderViewModel>> GetGraphicsByFileName(int fileNameId)
        {
            using(var db = new Srv6Context()) 
            {
                return await db.FileNames.Join(db.FileNameGraphics, 
                                               c => c.Id,
                                               p => p.FileNameId,
                                               (c,p) => new GraphicNameUploaderViewModel{Id = p.GraphicNameId, Name = p.GraphicName.Name, Variant = p.Variant})
                                         .Where(x => x.Id == fileNameId)
                                         .ToListAsync();
            }
        }
    }
}