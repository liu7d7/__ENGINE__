using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace __ENGINE__
{
    public static class __ENGINE__Program
    {
        // ReSharper disable once InconsistentNaming
        [STAThread]
        public static void __ENGINE__Main(string[] args)
        {
            NativeWindowSettings nativeWindowSettings = new()
            {
                Size = new Vector2i(1152, 720),
                Title = "__ENGINE__",
                Flags = ContextFlags.ForwardCompatible
            };

            GLFW.Init();
            GLFW.WindowHint(WindowHintBool.Resizable, false);
            using __ENGINE__ window = new(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
        }
    }
}