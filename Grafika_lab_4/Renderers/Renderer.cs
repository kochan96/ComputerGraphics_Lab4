using OpenTK.Graphics.ES20;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Grafika_lab_4.Renderers
{
    public abstract class Renderer
    {
        #region Properties
        int ProgramID;
        string Name;//for debug
        protected const int MaxLight = 5;
        #endregion

        #region Constructors

        public Renderer(byte[] VertexShader, byte[] FragmentShader, string name)
        {
            Name = name;
            int vertexShader = CreateShader(VertexShader, ShaderType.VertexShader);
            int fragmentShader = CreateShader(FragmentShader, ShaderType.FragmentShader);
            ProgramID = LinkProgram(vertexShader, fragmentShader);
            SetLocations();

        }

        #endregion

        #region CreateProgramMethods
        private int CreateShader(byte[] ShaderFile, ShaderType type)
        {
            int shader = GL.CreateShader(type);
            string src = String.Empty;
            if (ShaderFile == null || ShaderFile.Length == 0)
            {
                Debug.WriteLine($"File is null or empty:{Name}");
            }
            else
            {
                using (var reader = new StreamReader(new MemoryStream(ShaderFile)))
                {
                    src = reader.ReadToEnd();
                }
            }

            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            string info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(info))
            {
                Debug.WriteLine($"Shader {Name}: {type} had info log: {info}");
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
        #endregion

        #region SetLocations
        private void SetLocations()
        {
            SetAttributesLocations();
            SetUniformsLocations();
        }

        protected int GetAttrubuteLocation(string AttributeName)
        {
            int position = GL.GetAttribLocation(ProgramID, AttributeName);
            if (position == -1)
            {
                Debug.WriteLine($"{this}: Could not find attribute of name {AttributeName}");
            }

            return position;
        }

        protected int GetUniformLocation(string UniformName)
        {
            int position = GL.GetUniformLocation(ProgramID, UniformName);
            if (position == -1)
            {
                Debug.WriteLine($"{this}: Could not find uniform of name {UniformName}");
            }

            return position;
        }
        #endregion

        #region Abstract Methods

        public abstract void EnableVertexAttribArrays();
        public abstract void DisableVertexAttribArrays();
        protected abstract void SetAttributesLocations();
        protected abstract void SetUniformsLocations();

        #endregion

        #region Methods

        public void Use()
        {
            GL.UseProgram(ProgramID);
        }

        public void Delete()
        {
            GL.DeleteProgram(ProgramID);
        }

        #endregion
    }
}
