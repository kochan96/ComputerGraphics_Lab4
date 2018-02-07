using Grafika_lab_4.Configuration;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Grafika_lab_4.Renderers
{
    public class AircraftRenderer : Renderer
    {
        #region Shaders
        protected override string VERTEX_SHADER { get { return Resources.AircraftVertexShader; } }
        protected override string FRAGMENT_SHADER { get { return Resources.AircraftFragmentShader; } }
        #endregion

        #region AtributeNames
        protected override string PositonAttrName { get { return "Position"; } }
        protected override string TextCoordAttribName { get { return "TextCoord"; } }
        protected override string NormalsAttribName { get { return "Normal"; } }
        #endregion

        #region UniformNames
        protected override string ModelMatrixUniName { get { return "ModelMatrix"; } }
        protected override string ViewMatrixUniName { get { return "ViewMatrix"; } }
        protected override string ProjMatrixUniName { get { return "ProjectionMatrix"; } }
        protected override string TextureSamplerUniName { get { return "TextureSampler"; } }
        protected override string LightPositionUniName { get { return "LightPosition"; } }
        protected override string LightColorUniName { get { return "LightColor"; } }

        protected virtual string AmbientColorUniName { get { return "AmbientColor"; } }
        protected virtual string DiffuseColorUniName { get { return "DiffuseColor"; } }
        protected virtual string SpecularColorUniName { get { return "SpecularColor"; } }
        protected virtual string SpecularExponenUniName { get { return "SpecularExponent"; } }
        protected override string SkyColorUniName { get { return "SkyColor"; } }
        #endregion


        #region UniformLocations
        int AmbientColorLocation;
        int DiffuseColorLocation;
        int SpecularColorLocation;
        int SpecularExponentLocation;
        #endregion


        #region SetLocations

        protected override void SetUniformsLocations()
        {
            base.SetUniformsLocations();
            AmbientColorLocation = GetUniformLocation(AmbientColorUniName);
            DiffuseColorLocation = GetUniformLocation(DiffuseColorUniName);
            SpecularColorLocation = GetUniformLocation(SpecularColorUniName);
            SpecularExponentLocation = GetUniformLocation(SpecularExponenUniName);
        }
        #endregion

        #region Methods

        public void SetAmbientColor(Vector3 ambient)
        {
            GL.Uniform3(AmbientColorLocation, ambient);
        }

        public void SetDiffuseColor(Vector3 diffuse)
        {
            GL.Uniform3(DiffuseColorLocation, diffuse);
        }

        public void SetSpecularColor(Vector3 specular)
        {
            GL.Uniform3(SpecularColorLocation, specular);
        }

        public void SetSpecularExponenet(float specularExponent)
        {
            GL.Uniform1(SpecularExponentLocation, specularExponent);
        }

        #endregion

        #region Singleton
        private AircraftRenderer() { }
        private static volatile AircraftRenderer instance;
        private static object syncRoot = new Object();

        public static AircraftRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AircraftRenderer();
                    }
                }
                return instance;
            }
        }

        #endregion
    }

}

