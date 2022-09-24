using OpenTK.Mathematics;

namespace __ENGINE__.Shared.Components
{
    public class Camera : EngineObj.Component
    {
        public Vector3 front;
        public Vector3 right;
        public Vector3 up;
        private FloatPos _pos;

        public Camera()
        {
            front = Vector3.Zero;
            right = Vector3.Zero;
            up = Vector3.UnitY;
        }

        public override void update(EngineObj objIn)
        {
            base.update(objIn);

            if (_pos == null)
            {
                _pos = objIn.get<FloatPos>();
            }
            
            front = new Vector3(MathF.Cos(_pos.pitch.toRadians()) * MathF.Cos(_pos.yaw.toRadians()), MathF.Sin(_pos.pitch.toRadians()), MathF.Cos(_pos.pitch.toRadians()) * MathF.Sin(_pos.yaw.toRadians())).Normalized();
            right = Vector3.Cross(front, up).Normalized();
        }
        
        public Matrix4 getCameraMatrix()
        {
            if (_pos == null)
            {
                return Matrix4.Identity;
            }
            Vector3 pos = new(_pos.lerpedX, _pos.lerpedY, _pos.lerpedZ);
            Matrix4 lookAt = Matrix4.LookAt(pos - front, pos, up);
            lookAt.scale(50f);
            return lookAt;
        }
    }
}