using Grafika_lab_4.Configuration;
using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Grafika_lab_4.Renderers
{
    public class EntityRenderer : Renderer
    {
        private EntityRenderer() : base(Resources.EntityVertexShader, Resources.EntityFragmentShader)
        { }

        private static volatile EntityRenderer instance;

        private static readonly object syncRoot = new object();

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

        public int PositionAttribute { get; private set; }

        public int TextureCoordAttribute { get; private set; }

        public int NormalAttribute { get; private set; }

        private int AmbientColorUniform;
        private int DiffuseColorUniform;
        private int SpecularColorUniform;
        private int SpecularExponentUniform;
        private int HasTextureUniform;
        private int ViewMatrixUniform;
        private int ProjectionMatrixUniform;
        private int ModelMatrixUniform;
        private int PhongLightningUniform;
        private readonly List<LightUniform> LightsUniform = new List<LightUniform>();
        private int DiscardUniform;

        protected override void SetAttributess()
        {
            PositionAttribute = GetAttrubute(nameof(PositionAttribute));
            TextureCoordAttribute = GetAttrubute(nameof(TextureCoordAttribute));
            NormalAttribute = GetAttrubute(nameof(NormalAttribute));
        }

        protected override void SetUniformss()
        {
            AmbientColorUniform = GetUniform(nameof(AmbientColorUniform));
            DiffuseColorUniform = GetUniform(nameof(DiffuseColorUniform));
            SpecularColorUniform = GetUniform(nameof(SpecularColorUniform));
            SpecularExponentUniform = GetUniform(nameof(SpecularExponentUniform));
            HasTextureUniform = GetUniform(nameof(HasTextureUniform));
            ViewMatrixUniform = GetUniform(nameof(ViewMatrixUniform));
            ProjectionMatrixUniform = GetUniform(nameof(ProjectionMatrixUniform));
            ModelMatrixUniform = GetUniform(nameof(ModelMatrixUniform));
            PhongLightningUniform = GetUniform(nameof(PhongLightningUniform));
            DiscardUniform = GetUniform(nameof(DiscardUniform));
            for (int i = 0; i < MaxLight; i++)
            {
                var light = new LightUniform
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

        public override void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositionAttribute);
            GL.EnableVertexAttribArray(TextureCoordAttribute);
            GL.EnableVertexAttribArray(NormalAttribute);
        }

        public override void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositionAttribute);
            GL.DisableVertexAttribArray(TextureCoordAttribute);
            GL.DisableVertexAttribArray(NormalAttribute);
        }

        public void SetAmbientColor(Vector3 color)
        {
            GL.Uniform3(AmbientColorUniform, ref color);
        }
        public void SetDiffuseColor(Vector3 color)
        {
            GL.Uniform3(DiffuseColorUniform, ref color);
        }
        public void SetSpecularColor(Vector3 color)
        {
            GL.Uniform3(SpecularColorUniform, ref color);
        }
        public void SetSpecularExponent(float exponent)
        {
            GL.Uniform1(SpecularExponentUniform, exponent);
        }
        public void SetHasTexture(bool hasTexture)
        {
            GL.Uniform1(HasTextureUniform, hasTexture ? 1 : 0);
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

        public void SetDiscard(bool discard)
        {
            GL.Uniform1(DiscardUniform, discard ? 1 : 0);
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

