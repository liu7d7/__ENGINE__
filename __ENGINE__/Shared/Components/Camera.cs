using OpenTK.Mathematics;

namespace __ENGINE__.Shared.Components
{
    public class Camera : __ENGINE__Obj.Component
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

        public override void update(__ENGINE__Obj objIn)
        {
            base.update(objIn);

            if (_pos == null)
            {
                _pos = objIn.get<FloatPos>();
            }
            
            front = new Vector3(MathF.Cos(_pos.pitch.to_radians()) * MathF.Cos(_pos.yaw.to_radians()), MathF.Sin(_pos.pitch.to_radians()), MathF.Cos(_pos.pitch.to_radians()) * MathF.Sin(_pos.yaw.to_radians())).Normalized();
            right = Vector3.Cross(front, up).Normalized();
        }
        
        public Matrix4 get_camera_matrix()
        {
            if (_pos == null)
            {
                return Matrix4.Identity;
            }
            Vector3 pos = new(_pos.lerped_x, _pos.lerped_y, _pos.lerped_z);
            Matrix4 lookAt = Matrix4.LookAt(pos - front, pos, up);
            lookAt.scale(50f);
            return lookAt;
        }
    }
}