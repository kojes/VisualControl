using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LazyCache;
using Newtonsoft.Json;

namespace VueExample.Color {
    public class ColorService : IColorService {

        private readonly IAppCache cache;

        public ColorService (IAppCache cache) {
            this.cache = cache;
        }
        public string GetRandomHexColor () {
            var random = new Random ();
            var color = String.Format ("#{0:X6}", random.Next (0x1000000));
            return color;
        }

        public string GetHexColorByDieId (long? dieId) {
            if (dieId == null) {
                return "#FFFFFF";
            } else {
                var hex = String.Empty;
                Func<List<Color>> cachedcolorList = () => GetColorList ();
                var colorList = cache.GetOrAdd ("COLORS", cachedcolorList);
                hex = colorList[Convert.ToInt32(dieId % 1000)].Hex;
                return hex;
            }

        }

        private List<Color> GetColorList () {
            var colorList = new List<Color> ();
            using (StreamReader r = new StreamReader (Directory.GetCurrentDirectory() + "//Color//colors.json")) {
                string json = r.ReadToEnd ();
                Dictionary<string, string> colorsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
                foreach (var colorItem in colorsDictionary) {
                    var color = new Color { Hex = colorItem.Key, Name = colorItem.Value };
                    colorList.Add (color);
                }

            }
            return colorList;

        }

    }
}