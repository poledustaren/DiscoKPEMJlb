using System.Collections.Generic;

namespace Study
{
    public class CustomArrow
    {
        public List<CoorinateFloat> Coordinates { get; set; }

        public CustomArrow(CoorinateFloat centerPoint)
        {
            var dot = new CoorinateFloat(centerPoint);

            Coordinates =new List<CoorinateFloat>()
            {
                dot.Coordinate(0 , 10),
                dot.Coordinate(220, 10),
                dot.Coordinate(220, 20),
                dot.Coordinate(240, 0),
                dot.Coordinate(220,-20),
                dot.Coordinate(220, -10),
                dot.Coordinate(0 ,-10),
                dot.Coordinate(0 , 10)
            };
           
        }
    }
}