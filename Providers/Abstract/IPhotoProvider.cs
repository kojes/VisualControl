using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.Providers
{
    public interface IPhotoProvider
    {
        string InsertPhoto(Photo photo);
        List<Photo> GetPhotosByDefectId(int defectId);
    }
}
