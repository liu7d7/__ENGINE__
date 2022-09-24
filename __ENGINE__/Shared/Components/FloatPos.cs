using OpenTK.Mathematics;

namespace __ENGINE__.Shared.Components
{
    public class FloatPos : EngineObj.Component
    {
        public float x;
        public float prevX;
        public float y;
        public float prevY;
        public float z;
        public float prevZ;
        public float yaw;
        public float prevYaw;
        public float pitch;
        public float prevPitch;
        public float lerpedX => Util.lerp(prevX, x, Ticker.tickDelta);
        public float lerpedY => Util.lerp(prevY, y, Ticker.tickDelta);
        public float lerpedZ => Util.lerp(prevZ, z, Ticker.tickDelta);

        public FloatPos()
        {
            x = prevX = y = prevY = z = prevZ = yaw = prevYaw = pitch = prevPitch = 0;
        }

        public Vector3 toVec3()
        {
            return new(x, y, z);
        }

        public Vector3 toLerpedVec3(float xOff, float yOff, float zOff)
        {
            return new(lerpedX + xOff, lerpedY + yOff, lerpedZ + zOff);
        }

        public void setPrev()
        {
            prevX = x;
            prevY = y;
            prevZ = z;
            prevYaw = yaw;
            prevPitch = pitch;
        }
    }
}