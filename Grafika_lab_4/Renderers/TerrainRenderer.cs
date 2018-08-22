using Grafika_lab_4.Configuration;
using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Grafika_lab_4.Renderers
{
    public class TerrainRenderer : Renderer
    {
        #region Singleton
        private TerrainRenderer() : base(Properties.Resources.terrain1, Properties.Resources.terrainFrag, nameof(TerrainRenderer)) { }
        private static volatile TerrainRenderer instance;
        private static object syncRoot = new Object();

        public static TerrainRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TerrainRenderer();
                        }
                    }
                }
                return instance;
            }
        }



        #endregion

        #region AttributeLocations
        public int PositionLocation { get; private set; }
        public int NormalLocation { get; private set; }
        public int TextureCoordLocation { get; private set; }
        #endregion

        #region UniformsLocation
        int ModelMatrixLocation;
        int ViewMatrixLocation;
        int ProjectionMatrixLocation;
        readonly List<LightLocation> lightsLocation = new List<LightLocation>();
        #endregion

        public override void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositionLocation);
            GL.EnableVertexAttribArray(NormalLocation);
            GL.EnableVertexAttribArray(TextureCoordLocation);
        }

        public override void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositionLocation);
            GL.DisableVertexAttribArray(NormalLocation);
            GL.DisableVertexAttribArray(TextureCoordLocation);
        }

        protected override void SetAttributesLocations()
        {
            PositionLocation = GetAttrubuteLocation("Position");
            NormalLocation = GetAttrubuteLocation("Normal");
            TextureCoordLocation = GetAttrubuteLocation("TextCoord");
        }

        protected override void SetUniformsLocations()
        {
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
