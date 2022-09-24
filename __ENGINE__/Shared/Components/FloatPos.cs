using OpenTK.Mathematics;

namespace __ENGINE__.Shared.Components
{
    public class FloatPos : __ENGINE__Obj.Component
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
        public float lerped_x => Util.lerp(prevX, x, Ticker.tickDelta);
        public float lerped_y => Util.lerp(prevY, y, Ticker.tickDelta);
        public float lerped_z => Util.lerp(prevZ, z, Ticker.tickDelta);

        public FloatPos()
        {
            x = prevX = y = prevY = z = prevZ = yaw = prevYaw = pitch = prevPitch = 0;
        }

        public Vector3 to_vec3()
        {
            return new(x, y, z);
        }

        public Vector3 to_lerped_vec3(float xOff, float yOff, float zOff)
        {
            return new(lerped_x + xOff, lerped_y + yOff, lerped_z + zOff);
        }

        public void set_prev()
        {
            prevX = x;
            prevY = y;
            prevZ = z;
            prevYaw = yaw;
            prevPitch = pitch;
        }
    }
}