using System.Drawing;

namespace Study
{
    public class CoorinateFloat
    {
        private CoorinateFloat _startCoordinates;

        public CoorinateFloat(float x, float y)
        {
            X = x;
            Y = y;
        }

        public CoorinateFloat(CoorinateFloat startCoordinates)
        {
            _startCoordinates = startCoordinates;
        }

        public CoorinateFloat Coordinate(float x, float y,bool isBadPoint=false)
        {
            var coord =
                new CoorinateFloat(x,y) {ForDrawX = x + _startCoordinates.X,ForDrawY = (-1) * y + _startCoordinates.Y,BadPoint = isBadPoint};

            return coord;
        }

        public PointF Pointf()
        {
            return new PointF(X,Y);
        }
        public float X { get; set; }
        public float Y { get; set; }
        public float ForDrawX { get; set; }
        public float ForDrawY { get; set; }
        public bool BadPoint { get; set; } 
    }
}