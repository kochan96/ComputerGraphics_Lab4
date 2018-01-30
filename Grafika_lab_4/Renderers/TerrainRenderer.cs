using Grafika_lab_4.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.IO;

namespace Grafika_lab_4.Renderers
{
    public class TerrainRenderer
    {
        #region Fields

        #region Shaders

        readonly string VERTEX_SHADER = "Shaders/terrain.vert";
        readonly string FRAGMENT_SHADER = "Shaders/terrain.frag";

        #endregion

        #region AttributesNames

        readonly string positonAttrName = "Position";
        readonly string TextCoordAttribName = "TextCoord";
        readonly string NormalsAttribName = "Normal";

        #endregion

        #region UniformNames

        readonly string modelMatrixUniName = "ModelMatrix";
        readonly string viewMatrixUniName = "ViewMatrix";
        readonly string projMatrixUniName = "ProjectionMatrix";
        readonly string TextureSamplerUniName = "TextureSampler";
        readonly string LightPositionUniName = "LightPosition";
        readonly string LightColorUniName = "LightColor";
       
        #endregion
        
        #region UniformLocations

        int ModelMatrixLocation;
        int ViewMatrixLocation;
        int ProjectionMatrixLocation;
        int TextureSamplerLocation;
        int LightPositionLocation;
        int LightColorLocation;

        #endregion

        #endregion

        #region Properties
        public int ProgramID { get; }
        public int PositionDataLocation { get; private set; }
        public int TextCoordLocation { get; private set; }

        public int NormalsDataLocation { get; private set;}
        #endregion

        #region Constructors

        public TerrainRenderer()
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

            if (!File.Exists(ShaderFilePath))
                Debug.WriteLine($"File does not exists: {ShaderFilePath}");

            string src = File.ReadAllText(ShaderFilePath);
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
        private void SetAttributesLocations()
        {
            PositionDataLocation = GetAttrubuteLocation(positonAttrName);
            TextCoordLocation = GetAttrubuteLocation(TextCoordAttribName);
            NormalsDataLocation = GetAttrubuteLocation(NormalsAttribName);
        }

        private int GetAttrubuteLocation(string AttributeName)
        {
            int position = GL.GetAttribLocation(ProgramID, AttributeName);
            if (position == -1)
                Debug.WriteLine($"Could not find attribute of name {AttributeName}");

            return position;
        }

        private void SetUniformsLocations()
        {
            ModelMatrixLocation = GetUniformLocation(modelMatrixUniName);
            ViewMatrixLocation = GetUniformLocation(viewMatrixUniName);
            ProjectionMatrixLocation = GetUniformLocation(projMatrixUniName);
            TextureSamplerLocation = GetUniformLocation(TextureSamplerUniName);
            LightPositionLocation = GetUniformLocation(LightPositionUniName);
            LightColorLocation = GetUniformLocation(LightColorUniName);
        }

        private int GetUniformLocation(string UniformName)
        {
            int position = GL.GetUniformLocation(ProgramID, UniformName);
            if (position == -1)
                Debug.WriteLine($"Could not find attribute of name {UniformName}");

            return position;
        }
        #endregion



        #region Methods
        public void Use()
        {
            GL.UseProgram(ProgramID);
        }

        public void SetModelMatrix(Matrix4 model)
        {
            GL.UniformMatrix4(ModelMatrixLocation, false, ref model);
        }

        public void SetViewMatrix(Matrix4 view)
        {
            GL.UniformMatrix4(ViewMatrixLocation, false, ref view);
        }

        public void SetProjectionMatrix(Matrix4 projection)
        {
            GL.UniformMatrix4(ProjectionMatrixLocation, false, ref projection);
        }

        public void SetTexture(Texture texture)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.TextureId);

            GL.Uniform1(TextureSamplerLocation, 0);
        }

        public void SetLightPosition(Vector3 position)
        {
            GL.Uniform3(LightPositionLocation, ref position);
        }

        public void SetLightColor(Vector3 color)
        {
            GL.Uniform3(LightColorLocation, ref color);
        }

        public void SetModelViewProjectionMatrix(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            SetModelMatrix(model);
            SetViewMatrix(view);
            SetProjectionMatrix(projection);
        }

        public void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositionDataLocation);
            GL.EnableVertexAttribArray(TextCoordLocation);
            GL.EnableVertexAttribArray(NormalsDataLocation);
        }

        public void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositionDataLocation);
            GL.DisableVertexAttribArray(TextCoordLocation);
            GL.DisableVertexAttribArray(NormalsDataLocation);
        }
        #endregion

    }
}
