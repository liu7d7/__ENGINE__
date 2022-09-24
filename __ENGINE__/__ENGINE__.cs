﻿using System.ComponentModel;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using __ENGINE__.Engine;
using __ENGINE__.Shared;
using __ENGINE__.Shared.Components;
using __ENGINE__.Shared.Tweens;
using NAudio.Vorbis;
using NAudio.Wave;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Font = __ENGINE__.Engine.Font;

namespace __ENGINE__
{
    public class __ENGINE__ : GameWindow
    {
        private static int _ticks;
        public static __ENGINE__ instance;

        public __ENGINE__(GameWindowSettings windowSettings, NativeWindowSettings nativeWindowSettings) : base(windowSettings, nativeWindowSettings)
        {
            instance = this;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Ticker.init();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            if (e.Size == Vector2i.Zero)
                return;

            RenderSystem.updateProjection();
            GL.Viewport(new Rectangle(0, 0, Size.X, Size.Y));
            Fbo.resize(Size.X, Size.Y);
        }
        
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            RenderSystem.frame.bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwapBuffers();
        }
        
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            int i = Ticker.update();
            
            for (int j = 0; j < Math.Min(10, i); j++)
            {
                _ticks++;
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
    }
}