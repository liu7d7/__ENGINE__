﻿using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Mathematics;
using StbImageSharp;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace __ENGINE__.Engine
{
    // taken from https://github.com/opentk/LearnOpenTK/blob/master/Common/Texture.cs
    public class Texture
    {
        private static int _active;
        private static readonly Dictionary<int, Texture> textures = new();
        private readonly int _handle;

        public readonly float width;
        public readonly float height;

        public static Texture load_from_file(string path)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);
            
            StbImage.stbi_set_flip_vertically_on_load(1);
            using Stream stream = File.OpenRead(path);
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                image.Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle, image.Width, image.Height);
        }
        
        public static Texture load_from_buffer(byte[] buffer, int width, int height, PixelFormat format, PixelInternalFormat internalFormat, TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);
            
#pragma warning disable CA1416
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                internalFormat,
                width,
                height,
                0,
                format,
                PixelType.UnsignedByte,
                buffer);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle, width, height);
#pragma warning restore CA1416
        }

        private Texture(int glHandle, int width, int height)
        {
            _handle = glHandle;
            this.width = width;
            this.height = height;
            textures[glHandle] = this;
        }
        
        public void bind(TextureUnit unit)
        {
            if (_handle == _active)
            {
                return;
            }
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
            _active = _handle;
        }
        
        public static void unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            _active = 0;
        }

        public static Vector2 current_bounds()
        {
            if (!textures.ContainsKey(_active))
            {
                return new Vector2(1, 1);
            }
            Texture current = textures[_active];
            return new Vector2(current.width, current.height);
        }
    }
}