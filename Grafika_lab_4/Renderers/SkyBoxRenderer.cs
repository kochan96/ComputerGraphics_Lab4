using OpenTK.Graphics.OpenGL4;
using System;

namespace Grafika_lab_4.Renderers
{
    public class SkyBoxRenderer : Renderer
    { 
        #region Singleton
        private SkyBoxRenderer():base(Properties.Resources.SkyboxVert,Properties.Resources.skyboxFrag,nameof(SkyBoxRenderer)) { }
        private static volatile SkyBoxRenderer instance;
        private static object syncRoot = new Object();

        public static SkyBoxRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SkyBoxRenderer();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region AttributeLocations
        public int PositionLocation { get; private set; }
        #endregion

        #region UniformLocations
        int ModelMatrixLocation;
        int ViewMatrixLocation;
        int ProjectionMatrixLocation;
        int SamplerCubeLocation;
        #endregion


        public override void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositionLocation);
        }

        public override void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositionLocation);
        }

        protected override void SetAttributesLocations()
        {
            PositionLocation = GetAttrubuteLocation("Position");
        }

        protected override void SetUniformsLocations()
        {
            ModelMatrixLocation = GetUniformLocation("ModelMatrix");
            ViewMatrixLocation = GetUniformLocation("ViewMatrix");
            ProjectionMatrixLocation = GetUniformLocation("ProjectionMatrix");
            SamplerCubeLocation = GetUniformLocation("CubeMap");
        }

    }
}
