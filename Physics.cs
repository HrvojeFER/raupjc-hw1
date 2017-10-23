using System;

namespace Pong
{
    public interface IPhysicalObject2D
    {
        float X { get; set; }
        float Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }

    public class CollisionDetector
    {
        public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
        {
            return a.X + a.Width > b.X &&
                   a.Y < b.Y + b.Height &&
                   a.X < b.X + b.Width &&
                   a.Y + a.Height > b.Y;
        }
    }
}
