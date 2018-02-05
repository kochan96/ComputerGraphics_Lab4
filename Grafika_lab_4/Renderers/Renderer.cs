using Grafika_lab_4.Textures;
using OpenTK;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.Renderers
{
    public abstract class Renderer
    {
        #region Fields

        

        #region Shaders

        protected abstract string VERTEX_SHADER { get; }
        protected abstract string FRAGMENT_SHADER { get; }

        #endregion

        #region AttributesNames

        protected abstract string PositonAttrName { get; }
        protected abstract string TextCoordAttribName { get; }
        protected abstract string NormalsAttribName { get; }

        #endregion

        #region UniformNames
        protected abstract string ModelMatrixUniName { get; }
        protected abstract string ViewMatrixUniName { get; }
        protected abstract string ProjMatrixUniName { get; }
        protected abstract string TextureSamplerUniName { get; }
        protected abstract string LightPositionUniName { get; }
        protected abstract string LightColorUniName { get; }

        protected abstract string SkyColorUniName { get; }

        #endregion

        #region UniformLocations

        int ModelMatrixLocation;
        int ViewMatrixLocation;
        int ProjectionMatrixLocation;
        int TextureSamplerLocation;
        int LightPositionLocation;
        int LightColorLocation;
        int SkyColorLocation;
        #endregion

        #endregion

        #region Properties
        public int ProgramID { get; }
        public int PositionDataLocation { get; private set; }
        public int TextCoordLocation { get; private set; }

        public int NormalsDataLocation { get; private set; }

        #endregion

        #region Constructors

        public Renderer()
        {
            int vertexShader = CreateShader(VERTEX_SHADER, ShaderType.VertexShader);
            int fragmentShader = CreateShader(FRAGMENT_SHADER, ShaderType.FragmentShader);
            ProgramID = LinkProgram(vertexShader, fragmentShader);
            SetLocations();
        }

        #endregion

        #region CreateProgramMethods
        private int CreateShader(string ShaderFilePath, ShaderType type)
        {
            int shader = GL.CreateShader(type);

            string src = String.Empty;
            if (!File.Exists(ShaderFilePath))
                Debug.WriteLine($"File does not exists: {ShaderFilePath}");
            else
                src = File.ReadAllText(ShaderFilePath);

            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            string info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(info))
                Debug.WriteLine($"Shader: {type} had info log: {info}");

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
                Debug.WriteLine($"Program had info log: {info}");

            return program;
        }
        #endregion

        #region SetLocations
        private void SetLocations()
        {
            SetAttributesLocations();
            SetUniformsLocations();
        }
        protected virtual void SetAttributesLocations()
        {
            PositionDataLocation = GetAttrubuteLocation(PositonAttrName);
            TextCoordLocation = GetAttrubuteLocation(TextCoordAttribName);
            NormalsDataLocation = GetAttrubuteLocation(NormalsAttribName);
        }

        protected int GetAttrubuteLocation(string AttributeName)
        {
            int position = GL.GetAttribLocation(ProgramID, AttributeName);
            if (position == -1)
                Debug.WriteLine($"{this}: Could not find attribute of name {AttributeName}");

            return position;
        }

        protected virtual void SetUniformsLocations()
        {
            ModelMatrixLocation = GetUniformLocation(ModelMatrixUniName);
            ViewMatrixLocation = GetUniformLocation(ViewMatrixUniName);
            ProjectionMatrixLocation = GetUniformLocation(ProjMatrixUniName);
            TextureSamplerLocation = GetUniformLocation(TextureSamplerUniName);
            LightPositionLocation = GetUniformLocation(LightPositionUniName);
            LightColorLocation = GetUniformLocation(LightColorUniName);
            SkyColorLocation = GetUniformLocation(SkyColorUniName);
        }

        protected int GetUniformLocation(string UniformName)
        {
            int position = GL.GetUniformLocation(ProgramID, UniformName);
            if (position == -1)
                Debug.WriteLine($"{this}: Could not find uniform of name {UniformName}");

            return position;
        }
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

        public void SetModelMatrix(Matrix4 model, bool transpose)
        {
            GL.UniformMatrix4(ModelMatrixLocation, transpose, ref model);
        }

        public void SetViewMatrix(Matrix4 view, bool transpose)
        {
            GL.UniformMatrix4(ViewMatrixLocation, transpose, ref view);
        }

        public void SetProjectionMatrix(Matrix4 projection, bool transpose)
        {
            GL.UniformMatrix4(ProjectionMatrixLocation, transpose, ref projection);
        }

        public void SetTexture(Texture texture)
        {
            if (texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.TextureId);

                GL.Uniform1(TextureSamplerLocation, 0);
            }
        }

        public void SetLightPosition(Vector3 position)
        {
            GL.Uniform3(LightPositionLocation, ref position);
        }

        public void SetLightColor(Vector3 color)
        {
            GL.Uniform3(LightColorLocation, ref color);
        }

        public void SetSkyColor(Vector3 color)
        {
            GL.Uniform3(SkyColorLocation, color);
        }

        public virtual void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositionDataLocation);
            GL.EnableVertexAttribArray(TextCoordLocation);
            GL.EnableVertexAttribArray(NormalsDataLocation);
        }

        public virtual void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositionDataLocation);
            GL.DisableVertexAttribArray(TextCoordLocation);
            GL.DisableVertexAttribArray(NormalsDataLocation);
        }
        #endregion
    }
}
