using Grafika_lab_4.Configuration;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Grafika_lab_4.Renderers
{
    public class StaticRenderer : Renderer
    {
        #region Shaders

        protected override string VERTEX_SHADER { get { return Resources.StaticVertexShader; } }
        protected override string FRAGMENT_SHADER { get { return Resources.StaticFragmentShader; } }

        #endregion

        #region AtributeNames
        protected override string PositonAttrName { get { return "Position"; } }
        protected override string TextCoordAttribName { get { return "TextCoord"; } }
        protected override string NormalsAttribName { get { return "Normal"; } }
        protected virtual string ColorAttribName { get { return "Color"; } }
        #endregion

        #region UniformNames
        protected override string ModelMatrixUniName { get { return "ModelMatrix"; } }
        protected override string ViewMatrixUniName { get { return "ViewMatrix"; } }
        protected override string ProjMatrixUniName { get { return "ProjectionMatrix"; } }
        protected override string TextureSamplerUniName { get { return "TextureSampler"; } }
        protected override string LightPositionUniName { get { return "LightPosition"; } }
        protected override string LightColorUniName { get { return "LightColor"; } }

        protected override string SkyColorUniName { get { return "SkyColor"; } }

        protected virtual string HasTextureUniName { get { return "HasTexture"; } }

        #endregion

        #region UniformLocation
        int HasTextureLocation;
        #endregion

        #region AttributesLocation
        public int ColorDataLocation { get; private set; }
        #endregion

        #region SetAttributesLocation
        protected override void SetAttributesLocations()
        {
            base.SetAttributesLocations();
            ColorDataLocation = GetAttrubuteLocation(ColorAttribName);
        }
        #endregion

        #region SetUniformsLocation
        protected override void SetUniformsLocations()
        {
            base.SetUniformsLocations();
            HasTextureLocation = GetUniformLocation(HasTextureUniName);
        }

        #endregion

        public override void EnableVertexAttribArrays()
        {
            base.EnableVertexAttribArrays();
            GL.EnableVertexAttribArray(ColorDataLocation);
        }

        public override void DisableVertexAttribArrays()
        {
            base.DisableVertexAttribArrays();
            GL.DisableVertexAttribArray(ColorDataLocation);
        }


        public void SetHasTexture(bool hasTexture)
        {
            GL.Uniform1(HasTextureLocation, hasTexture ? 1 : 0);
        }


        #region Singleton
        private StaticRenderer() { }
        private static volatile StaticRenderer instance;
        private static object syncRoot = new Object();

        public static StaticRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new StaticRenderer();
                    }
                }
                return instance;
            }
        }



        #endregion

    }
}
