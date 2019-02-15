using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VueExample.Models;
using VueExample.ServiceModels;

namespace VueExample.Services
{
    public class WaferMapFormationService
    {
        private double FieldHeight { get; }
        private double FieldWidth { get; }
        private double StreetSize { get; }
        private int YCoordinateMin { get; }
        private int XCoordinateMin { get; }
        private int YCoordinateMax { get; }
        private int XCoordinateMax { get; }
      
        private readonly List<Die> _dieList;


        public WaferMapFormationService(double fieldHeight, double fieldWidth, double streetSize, List<Die> dieList)
        {
            FieldHeight = fieldHeight;
            FieldWidth = fieldWidth;
            StreetSize = streetSize;
            _dieList = dieList;

            XCoordinateMax = _dieList.Select(x => x.XCoordinate).Max();
            YCoordinateMax = _dieList.Select(x => x.YCoordinate).Max();
            XCoordinateMin = _dieList.Select(x => x.XCoordinate).Min();
            YCoordinateMin = _dieList.Select(x => x.YCoordinate).Min();
        }

        public string GetFormedWaferMap()
        {
            return JsonConvert.SerializeObject(WaferMapFormation());
        }

       

        private List<WaferMapDie> WaferMapFormation()
        {
            var waferMapDieList = new List<WaferMapDie>();
            var xQuantity = XCoordinateMax - XCoordinateMin + 1;
            var yQuantity = YCoordinateMax - YCoordinateMin + 1;
            var xGridCellSize = (FieldWidth - xQuantity * StreetSize) / xQuantity;
            var yGridCellSize = (FieldHeight - yQuantity * StreetSize) / yQuantity;

            foreach (var die in _dieList)
                waferMapDieList.Add(new WaferMapDie(
                    (die.XCoordinate - XCoordinateMin) * xGridCellSize +
                    (die.XCoordinate - XCoordinateMin) * StreetSize,
                    (die.YCoordinate - YCoordinateMin) * yGridCellSize +
                    (die.YCoordinate - YCoordinateMin) * StreetSize, yGridCellSize, xGridCellSize, die.DieId, die.Code));

            return waferMapDieList;
        }
    }
}
