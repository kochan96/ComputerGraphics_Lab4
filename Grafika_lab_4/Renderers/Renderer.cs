using Grafika_lab_4.Lights;
using Grafika_lab_4.Loader;
using OpenTK;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Grafika_lab_4.Renderers
{
    public abstract class Renderer
    {
        #region Fields

        public abstract int MAX_LIGHTS { get; }

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
        protected  string LightsUniName { get { return "Lights"; }  }

        protected string LightningMode { get { return "PhongLightning"; } }

        #endregion

        #region UniformLocations

        protected int ModelMatrixLocation;
        protected int ViewMatrixLocation;
        protected int ProjectionMatrixLocation;
        protected int TextureSamplerLocation;
        protected LightLocation[] LightsLocation;
        protected int LightnigModelLocation;
        #endregion



        #endregion

        #region Properties
        public int ProgramID { get; }
        public int PositionDataLocation { get; protected set; }
        public int TextCoordLocation { get; protected set; }

        public int NormalsDataLocation { get; protected set; }

        #endregion

        #region Constructors

        public Renderer()
        {
            int vertexShader = CreateShader(VERTEX_SHADER, ShaderType.VertexShader);
            int fragmentShader = CreateShader(FRAGMENT_SHADER, ShaderType.FragmentShader);
            ProgramID = LinkProgram(vertexShader, fragmentShader);
            LightsLocation = new LightLocation[MAX_LIGHTS];
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
                Debug.WriteLine($"Shader {ShaderFilePath}: {type} had info log: {info}");

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
            LightnigModelLocation = GetUniformLocation(LightningMode);
            
            for (int i = 0; i < MAX_LIGHTS; i++)
            {
                LightsLocation[i].PositionLocation = GetUniformLocation(LightsUniName + $"[{i}].Position");
                LightsLocation[i].ColorLocation = GetUniformLocation(LightsUniName + $"[{i}].Color");
                LightsLocation[i].DirectionLocation = GetUniformLocation(LightsUniName + $"[{i}].Direction");
                LightsLocation[i].ConeAngleLocation = GetUniformLocation(LightsUniName + $"[{i}].ConeAngle");
                LightsLocation[i].TypeLocation = GetUniformLocation(LightsUniName + $"[{i}].LightType");
                LightsLocation[i].AmbientIntensityLocation = GetUniformLocation(LightsUniName + $"[{i}].AmbientIntensity");
                LightsLocation[i].DiffuseIntensityLocation = GetUniformLocation(LightsUniName + $"[{i}].DiffuseIntensity");
                LightsLocation[i].SpecularIntensityLocation = GetUniformLocation(LightsUniName + $"[{i}].SpecularIntensity");
                LightsLocation[i].AttenuationLocation = GetUniformLocation(LightsUniName + $"[{i}].Attenuation");
            }
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
                if (texture.Dimensions == 2)
                {
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, texture.TextureId);

                    GL.Uniform1(TextureSamplerLocation, 0);
                }
                else if (texture.Dimensions == 3)
                {
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture3D, texture.TextureId);

                    GL.Uniform1(TextureSamplerLocation, 0);
                }

            }
        }

        public void SetLightiningModel(bool model)
        {
            GL.Uniform1(LightnigModelLocation, model?1:0);
        }

        public void SetLights(List<Light> lights)
        {
            for(int i=0;i<MAX_LIGHTS ;i++)
            {
                if (i < lights.Count)
                {
                    GL.Uniform3(LightsLocation[i].PositionLocation, lights[i].Position);
                    GL.Uniform3(LightsLocation[i].ColorLocation, lights[i].Color);
                    GL.Uniform3(LightsLocation[i].DirectionLocation, lights[i].Direction);
                    GL.Uniform1(LightsLocation[i].ConeAngleLocation, lights[i].ConeAngle);
                    GL.Uniform1(LightsLocation[i].TypeLocation, (int)lights[i].LightType);
                    GL.Uniform1(LightsLocation[i].AmbientIntensityLocation, lights[i].AmbientIntensity);
                    GL.Uniform1(LightsLocation[i].DiffuseIntensityLocation, lights[i].DiffuseIntensity);
                    GL.Uniform1(LightsLocation[i].SpecularIntensityLocation, lights[i].SpecularIntensity);
                    GL.Uniform3(LightsLocation[i].AttenuationLocation, lights[i].Attenuation);
                }
                else
                {
                    Light tmp = new Light("");
                    GL.Uniform3(LightsLocation[i].PositionLocation, tmp.Position);
                    GL.Uniform3(LightsLocation[i].ColorLocation, tmp.Color);
                    GL.Uniform3(LightsLocation[i].DirectionLocation, tmp.Direction);
                    GL.Uniform1(LightsLocation[i].ConeAngleLocation, tmp.ConeAngle);
                    GL.Uniform1(LightsLocation[i].TypeLocation, (int)tmp.LightType);
                    GL.Uniform1(LightsLocation[i].AmbientIntensityLocation, tmp.AmbientIntensity);
                    GL.Uniform1(LightsLocation[i].DiffuseIntensityLocation, tmp.DiffuseIntensity);
                    GL.Uniform3(LightsLocation[i].AttenuationLocation, tmp.Attenuation);
                }
            }
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

    public struct LightLocation
    {
        public int PositionLocation;
        public int ColorLocation;
        public int TypeLocation;
        public int DirectionLocation;
        public int ConeAngleLocation;
        public int AmbientIntensityLocation;
        public int DiffuseIntensityLocation;
        public int SpecularIntensityLocation;
        public int AttenuationLocation;
    }

}
