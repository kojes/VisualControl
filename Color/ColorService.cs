using System;
using System.Collections.Generic;
using System.IO;
using LazyCache;
using Newtonsoft.Json;

namespace VueExample.Color
{
    public class ColorService : IColorService 
    {
        private readonly IAppCache _cache;
        public ColorService(IAppCache cache) 
        {
            _cache = cache;
        }
        
        public string GetRandomHexColor() 
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }

        public string GetHexColorByDieId(long? dieId) 
        {
            if (dieId == null) 
            {
                return "#FFFFFF";
            } 
            else 
            {
                var hex = String.Empty;                
                Func<List<Color>> cachedcolorList = () => GetColorList("colors16.json");
                var colorList = _cache.GetOrAdd ("COLORS16", cachedcolorList);
                hex = colorList[Convert.ToInt32(dieId % 16)].Hex;
                return hex;
            }
        }

        public List<Color> GetGradientColors() 
        {
            Func<List<Color>> cachedcolorList = () => GetColorList("colorsGradient.json");
            return _cache.GetOrAdd ("COLORS_GRADIENT", cachedcolorList);
        }

        private List<Color> GetColorList(string jsonName) 
        {
            var colorList = new List<Color>();
            using (StreamReader r = new StreamReader(Directory.GetCurrentDirectory() + "//Color//" + jsonName)) 
            {
                string json = r.ReadToEnd();
                Dictionary<string, string> colorsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
                foreach (var colorItem in colorsDictionary) 
                {
                    var color = new Color { Hex = colorItem.Key, Name = colorItem.Value };
                    colorList.Add (color);
                }
            }
            return colorList;
        }
    }
}