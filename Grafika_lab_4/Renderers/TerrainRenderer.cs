using Grafika_lab_4.Configuration;
using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Grafika_lab_4.Renderers
{
    public class TerrainRenderer : Renderer
    {
        private TerrainRenderer() : base(Resources.TerrainVertexShader, Resources.TerrainFragmentShader)
        { }

        private static volatile TerrainRenderer instance;
        private static readonly object _syncRoot = new object();

        public static TerrainRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
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

        public int PositionAttribute { get; private set; }

        public int NormalAttribute { get; private set; }

        public int TextureCoordAttribute { get; private set; }

        private int ModelMatrixUniform;
        private int ViewMatrixUniform;
        private int ProjectionMatrixUniform;
        private int PhongLightningUniform;
        private readonly List<LightUniform> LightsUniform = new List<LightUniform>();

        public override void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositionAttribute);
            GL.EnableVertexAttribArray(NormalAttribute);
            GL.EnableVertexAttribArray(TextureCoordAttribute);
        }

        public override void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositionAttribute);
            GL.DisableVertexAttribArray(NormalAttribute);
            GL.DisableVertexAttribArray(TextureCoordAttribute);
        }

        protected override void SetAttributess()
        {
            PositionAttribute = GetAttrubute(nameof(PositionAttribute));
            NormalAttribute = GetAttrubute(nameof(NormalAttribute));
            TextureCoordAttribute = GetAttrubute(nameof(TextureCoordAttribute));
        }

        protected override void SetUniformss()
        {
            ViewMatrixUniform = GetUniform(nameof(ViewMatrixUniform));
            ProjectionMatrixUniform = GetUniform(nameof(ProjectionMatrixUniform));
            ModelMatrixUniform = GetUniform(nameof(ModelMatrixUniform));
            PhongLightningUniform = GetUniform(nameof(PhongLightningUniform));

            for (int i = 0; i < MaxLight; i++)
            {
                var light = new LightUniform()
                {
                    Position = GetUniform($"{nameof(LightsUniform)}[{i}].Position"),
                    Attenuation = GetUniform($"{nameof(LightsUniform)}[{i}].Attenuation"),
                    Color = GetUniform($"{nameof(LightsUniform)}[{i}].Color"),
                    Direction = GetUniform($"{nameof(LightsUniform)}[{i}].Direction"),
                    SpecularIntensity = GetUniform($"{nameof(LightsUniform)}[{i}].SpecularIntensity"),
                    AmbientIntensity = GetUniform($"{nameof(LightsUniform)}[{i}].AmbientIntensity"),
                    DiffuseIntensity = GetUniform($"{nameof(LightsUniform)}[{i}].DiffuseIntensity"),
                    ConeAngle = GetUniform($"{nameof(LightsUniform)}[{i}].ConeAngle"),
                    LightType = GetUniform($"{nameof(LightsUniform)}[{i}].LightType"),
                };

                LightsUniform.Add(light);
            }
        }

        public void SetViewMatrix(Matrix4 viewMatrix)
        {
            GL.UniformMatrix4(ViewMatrixUniform, false, ref viewMatrix);
        }
        public void SetProjectionMatrix(Matrix4 projMatrix)
        {
            GL.UniformMatrix4(ProjectionMatrixUniform, false, ref projMatrix);
        }

        public void SetModelMatrix(Matrix4 modelMatrix)
        {
            GL.UniformMatrix4(ModelMatrixUniform, false, ref modelMatrix);
        }

        public void SetPhongLightning(bool value)
        {
            GL.Uniform1(PhongLightningUniform, value ? 1 : 0);
        }

        public void SetLights(List<Light> lights)
        {
            for (int i = 0; i < MaxLight; i++)
            {
                Light light = lights.Count > i ? lights[i] : new Light();

                GL.Uniform3(LightsUniform[i].Position, light.Position);
                GL.Uniform3(LightsUniform[i].Attenuation, light.Attenuation);
                GL.Uniform3(LightsUniform[i].Color, light.Color);
                GL.Uniform3(LightsUniform[i].Direction, light.Direction);
                GL.Uniform1(LightsUniform[i].SpecularIntensity, light.SpecularIntensity);
                GL.Uniform1(LightsUniform[i].AmbientIntensity, light.AmbientIntensity);
                GL.Uniform1(LightsUniform[i].DiffuseIntensity, light.DiffuseIntensity);
                GL.Uniform1(LightsUniform[i].ConeAngle, light.ConeAngle);
                GL.Uniform1(LightsUniform[i].LightType, (int)light.LightType);
            }
        }
    }
}
