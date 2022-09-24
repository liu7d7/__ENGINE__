using OpenTK.Mathematics;

namespace __ENGINE__.Shared.Components
{
    public class IntPos : EngineObj.Component
    {
        public int x;
        public int y;
        public int z;

        public IntPos()
        {
            
        }
        
        public IntPos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public IntPos(Vector3i pos)
        {
            x = pos.X;
            y = pos.Y;
            z = pos.Z;
        }
    }
}