using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;

namespace VueExample.Providers
{
    public class PhotoProvider : IPhotoProvider
    {
        public string InsertPhoto(Photo photo)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                visualControlContext.Add(photo);
                visualControlContext.SaveChanges();
            }

            return photo.Guid;
        }

        public List<Photo> GetPhotosByDefectId(int defectId)
        {
            List<Photo> photosList;

            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                photosList = visualControlContext.Photos.Where(x => x.DefectId == defectId).ToList();
            }

            return photosList;
        }
    }
}
