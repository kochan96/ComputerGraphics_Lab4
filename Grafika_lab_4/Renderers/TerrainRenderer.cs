using Grafika_lab_4.Configuration;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Grafika_lab_4.Renderers
{
    public class TerrainRenderer : Renderer
    {
        #region Singleton
        private TerrainRenderer():base(Properties.Resources.terrain1,Properties.Resources.terrainFrag,nameof(TerrainRenderer)) { }
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
                            instance = new TerrainRenderer();
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
            
        }

    }
}
