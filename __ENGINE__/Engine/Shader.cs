﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace __ENGINE__.Engine
{
    // taken from https://github.com/opentk/LearnOpenTK/blob/master/Common/Shader.cs
    public class Shader
    {

        private static int _active; 
        private readonly int _handle;
        private readonly Dictionary<string, int> _uniformLocations;
        
        public Shader(string vertPath, string fragPath)
        {
            var shaderSource = File.ReadAllText(vertPath);

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShader, shaderSource);

            compileShader(vertexShader);

            shaderSource = File.ReadAllText(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            compileShader(fragmentShader);
            
            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);

            linkProgram(_handle);
            
            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
            
            GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(_handle, i, out _, out _);
                var location = GL.GetUniformLocation(_handle, key);
                _uniformLocations.Add(key, location);
            }
        }

        private static void compileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code == (int)All.True) return;
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }

        private static void linkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code == (int)All.True) return;
            string infoLog = GL.GetProgramInfoLog(program);
            throw new Exception($"Error occurred whilst linking Program({program}) \n\n{infoLog}");
        }

        public void bind()
        {
            if (_handle == _active)
            {
                return;
            }
            GL.UseProgram(_handle);
            _active = _handle;
        }

        public static void unbind()
        {
            GL.UseProgram(0);
            _active = 0;
        }
        
        public int getAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(_handle, attribName);
        }

        /// <summary>
        /// Set a uniform int on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void setInt(string name, int data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            bind();
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform float on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void setFloat(string name, float data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            bind();
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void setMatrix4(string name, Matrix4 data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            bind();
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        /// <summary>
        /// Set a uniform Vector3 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void setVector3(string name, Vector3 data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            bind();
            GL.Uniform3(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform Vector2 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void setVector2(string name, Vector2 data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            bind();
            GL.Uniform2(_uniformLocations[name], data);
        }
    }
}