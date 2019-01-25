using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;

namespace VueExample.Services
{
    public class ImageManipulationService
    {
        public string ResizeImage(int height, int width, int quality, string inputPath, string outputPath)
        {
            using (var image = new MagickImage(inputPath))
            {
                image.Resize(height, width);
                image.Strip();
                image.Quality = quality;
                image.Write(outputPath);
            }

            return outputPath;
        }
    }
}
