using System.Drawing;
using OpenTK.Mathematics;
using __ENGINE__.Shared;
using __ENGINE__.Shared.Components;
using OpenTK.Graphics.OpenGL4;

namespace __ENGINE__.Engine
{
    public static class RenderSystem
    {

        private static readonly Shader john = new("Resource/Shader/john.vert", "Resource/Shader/john.frag");
        private static readonly Shader pixel = new("Resource/Shader/postprocess.vert", "Resource/Shader/pixelate.frag");
        private static Matrix4 _projection;
        private static Matrix4 _lookAt;
        private static Matrix4[] _model = new Matrix4[7];
        public static Matrix4 model;
        private static int _modelIdx;
        public static bool renderingRed;
        
        static RenderSystem()
        {
            Array.Fill(_model, Matrix4.Identity);
        }

        public static void push()
        {
            _model[_modelIdx + 1] = model;
            _modelIdx++;
            model = _model[_modelIdx];
        }

        public static void pop()
        {
            _modelIdx--;
            model = _model[_modelIdx];
        }

        public static readonly Mesh mesh = new(Mesh.DrawMode.triangle, john, Vao.Attrib.float3, Vao.Attrib.float3, Vao.Attrib.float2, Vao.Attrib.float4);
        public static readonly Mesh line = new(Mesh.DrawMode.line, john, Vao.Attrib.float3, Vao.Attrib.float3, Vao.Attrib.float2, Vao.Attrib.float4);
        public static readonly Mesh post = new(Mesh.DrawMode.triangle, null, Vao.Attrib.float2);
        public static readonly Fbo frame = new(__ENGINE__.instance.Size.X, __ENGINE__.instance.Size.Y, true);
        public static bool rendering3d;
        private static FloatPos _camera;

        public static Vector2i size => __ENGINE__.instance.Size;

        public static void set_defaults(this Shader shader)
        {
            shader.set_matrix4("_proj", _projection);
            shader.set_matrix4("_lookAt", _lookAt);
            shader.set_vector2("_screenSize", new(__ENGINE__.instance.Size.X, __ENGINE__.instance.Size.Y));
            shader.set_int("_rendering3d", rendering3d ? 1 : 0);
            shader.set_int("_renderingRed", renderingRed ? 1 : 0);
            shader.set_vector3("lightPos", new(_camera.x + 5, _camera.y + 12, _camera.z + 5));
        }

        public static void render_pixelation(float pixWidth, float pixHeight)
        {
            pixel.bind();
            frame.bind_color(TextureUnit.Texture0);
            pixel.set_int("_tex0", 0);
            pixel.set_vector2("_screenSize", new Vector2(size.X, size.Y));
            pixel.set_vector2("_pixSize", new Vector2(pixWidth, pixHeight));
            post.begin();
            int i1 = post.float2(0, 0).next();
            int i2 = post.float2(size.X, 0).next();
            int i3 = post.float2(size.X, size.Y).next();
            int i4 = post.float2(0, size.Y).next();
            post.quad(i1, i2, i3, i4);
            post.render();
            Shader.unbind();
        }

        public static void update_projection()
        {
            Matrix4.CreateOrthographic(__ENGINE__.instance.Size.X, __ENGINE__.instance.Size.Y, -1000, 3000, out _projection);
        }

        public static void update_look_at(__ENGINE__Obj cameraObj, bool rendering3d = true)
        {
            if (!cameraObj.has<FloatPos>())
            {
                return;
            }

            _camera = cameraObj.get<FloatPos>();
            RenderSystem.rendering3d = rendering3d;
            if (!RenderSystem.rendering3d)
            {
                _lookAt = Matrix4.Identity;
                return;
            }

            Camera comp = cameraObj.get<Camera>();
            _lookAt = comp.get_camera_matrix();
        }
        
    }
}