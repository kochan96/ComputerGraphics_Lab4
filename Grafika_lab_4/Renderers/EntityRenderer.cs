using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Grafika_lab_4.Renderers
{
    public class EntityRenderer : Renderer
    {
        #region Singleton
        private EntityRenderer() : base(Properties.Resources.entityVert, Properties.Resources.entityFrag, "EntityShader") { }
        private static volatile EntityRenderer instance;
        private static object syncRoot = new Object();

        public static EntityRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new EntityRenderer();
                        }
                    }
                }
                return instance;
            }
        }



        #endregion

        #region AtributeLocation
        public int PositonLocation { get; private set; }
        public int TextureCoordLocation { get; private set; }
        public int NormalLocation { get; private set; }
        #endregion

        #region UniformLocations
        int AmbientColorLocation;
        int DiffuseColorLocation;
        int SpecularColorLocation;
        int SpecularExponentLocation;
        int HasTextureLocation;
        int ViewMatrixLocation;
        int ProjectionMatrixLocation;
        int ModelMatrixLocation;
        readonly List<LightLocation> lightsLocation = new List<LightLocation>();
        #endregion


        protected override void SetAttributesLocations()
        {
            PositonLocation = GetAttrubuteLocation("Position");
            TextureCoordLocation = GetAttrubuteLocation("TextCoord");
            NormalLocation = GetAttrubuteLocation("Normal");
        }

        protected override void SetUniformsLocations()
        {
            AmbientColorLocation = GetUniformLocation("AmbientColor");
            DiffuseColorLocation = GetUniformLocation("DiffuseColor");
            SpecularColorLocation = GetUniformLocation("SpecularColor");
            SpecularExponentLocation = GetUniformLocation("SpecularExponent");
            HasTextureLocation = GetUniformLocation("HasTexture");
            ViewMatrixLocation = GetUniformLocation("ViewMatrix");
            ProjectionMatrixLocation = GetUniformLocation("ProjectionMatrix");
            ModelMatrixLocation = GetUniformLocation("ModelMatrix");
            for (int i = 0; i < MaxLight; i++)
            {
                var lightLocation = new LightLocation
                {
                    PositionLocation = GetUniformLocation($"Lights[{i}].Position"),
                    AttenuationLocation = GetUniformLocation($"Lights[{i}].Attenuation"),
                    ColorLocation = GetUniformLocation($"Lights[{i}].Color"),
                    DirectionLocation = GetUniformLocation($"Lights[{i}].Direction"),
                    SpecularIntensityLocation = GetUniformLocation($"Lights[{i}].SpecularIntensity"),
                    AmbientIntensityLocation = GetUniformLocation($"Lights[{i}].AmbientIntensity"),
                    DiffuseIntensityLocation = GetUniformLocation($"Lights[{i}].DiffuseIntensity"),
                    ConeAngleLocation = GetUniformLocation($"Lights[{i}].ConeAngle"),
                    TypeLocation = GetUniformLocation($"Lights[{i}].LightType"),
                };
                lightsLocation.Add(lightLocation);
            }
        }

        public override void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositonLocation);
            GL.EnableVertexAttribArray(TextureCoordLocation);
            GL.EnableVertexAttribArray(NormalLocation);
        }

        public override void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositonLocation);
            GL.DisableVertexAttribArray(TextureCoordLocation);
            GL.DisableVertexAttribArray(NormalLocation);
        }

        public void SetAmbientColor(Vector3 color)
        {
            GL.Uniform3(AmbientColorLocation, ref color);
        }
        public void SetDiffuseColor(Vector3 color)
        {
            GL.Uniform3(DiffuseColorLocation, ref color);
        }
        public void SetSpecularColor(Vector3 color)
        {
            GL.Uniform3(SpecularColorLocation, ref color);
        }
        public void SetSpecularExponent(float exponent)
        {
            GL.Uniform1(SpecularExponentLocation, exponent);
        }
        public void SetHasTexture(bool hasTexture)
        {
            GL.Uniform1(HasTextureLocation, hasTexture ? 1 : 0);
        }

        public void SetViewMatrix(Matrix4 viewMatrix)
        {
            GL.UniformMatrix4(ViewMatrixLocation, false, ref viewMatrix);
        }

        public void SetProjectionMatrix(Matrix4 projMatrix)
        {
            GL.UniformMatrix4(ProjectionMatrixLocation, false, ref projMatrix);
        }

        public void SetModelMatrix(Matrix4 modelMatrix)
        {
            GL.UniformMatrix4(ModelMatrixLocation, false, ref modelMatrix);
        }

        public void SetLights(List<Light> lights)
        {
            for (int i = 0; i < MaxLight; i++)
            {
                var light = new Light(string.Empty);
                if (lights.Count > i)
                {
                    light = lights[i];
                }

                GL.Uniform3(lightsLocation[i].PositionLocation, ref light.Position);
                GL.Uniform3(lightsLocation[i].AttenuationLocation, ref light.Attenuation);
                GL.Uniform3(lightsLocation[i].ColorLocation, ref light.Color);
                GL.Uniform3(lightsLocation[i].DirectionLocation, ref light.Direction);
                GL.Uniform1(lightsLocation[i].SpecularIntensityLocation, light.SpecularIntensity);
                GL.Uniform1(lightsLocation[i].AmbientIntensityLocation, light.AmbientIntensity);
                GL.Uniform1(lightsLocation[i].DiffuseIntensityLocation, light.DiffuseIntensity);
                GL.Uniform1(lightsLocation[i].ConeAngleLocation, light.ConeAngle);
                GL.Uniform1(lightsLocation[i].TypeLocation, (int)light.LightType);
            }
        }
    }
}

