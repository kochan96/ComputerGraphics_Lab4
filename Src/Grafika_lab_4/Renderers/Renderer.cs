using OpenTK.Graphics.ES20;
using System;
using System.Diagnostics;
using System.IO;

namespace Grafika_lab_4.Renderers
{
    public abstract class Renderer
    {
        private readonly int _programId;
        protected const int MaxLight = 5;

        public Renderer(string vertexShaderFilePath, string fragmentShaderFilePath)
        {
            int vertexShader = CreateShader(vertexShaderFilePath, ShaderType.VertexShader);
            int fragmentShader = CreateShader(fragmentShaderFilePath, ShaderType.FragmentShader);
            _programId = LinkProgram(vertexShader, fragmentShader);
            Sets();
        }

        private int CreateShader(string shaderFilePath, ShaderType type)
        {
            int shader = GL.CreateShader(type);
            string src = String.Empty;
            if (string.IsNullOrWhiteSpace(shaderFilePath))
            {
                Debug.WriteLine($"File is null or empty:{shaderFilePath}");
            }
            else
            {
                src = File.ReadAllText(shaderFilePath);
            }

            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            string info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(info))
            {
                Debug.WriteLine($"Shader {shaderFilePath}: {type} had info log: {info}");
            }

            return shader;
        }

        private int LinkProgram(int vertexShader, int fragmentShader)
        {
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);

            GL.LinkProgram(program);
            string info = GL.GetProgramInfoLog(program);

            if (!string.IsNullOrEmpty(info))
            {
                Debug.WriteLine($"Program had info log: {info}");
            }

            return program;
        }

        private void Sets()
        {
            SetAttributess();
            SetUniformss();
        }

        protected int GetAttrubute(string AttributeName)
        {
            int position = GL.GetAttribLocation(_programId, AttributeName);
            if (position == -1)
            {
                Debug.WriteLine($"{this}: Could not find attribute of name {AttributeName}");
            }

            return position;
        }

        protected int GetUniform(string UniformName)
        {
            int position = GL.GetUniformLocation(_programId, UniformName);
            if (position == -1)
            {
                Debug.WriteLine($"{this}: Could not find uniform of name {UniformName}");
            }

            return position;
        }

        public abstract void EnableVertexAttribArrays();

        public abstract void DisableVertexAttribArrays();

        protected abstract void SetAttributess();

        protected abstract void SetUniformss();

        public void Use()
        {
            GL.UseProgram(_programId);
        }

        public void Delete()
        {
            GL.DeleteProgram(_programId);
        }
    }
}
