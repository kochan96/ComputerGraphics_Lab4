using Grafika_lab_4.Configuration;
using System;

namespace Grafika_lab_4.Renderers
{
    public class TerrainRenderer : Renderer
    {
        public override int MAX_LIGHTS { get { return 5; } }
        #region Shaders
        protected override string VERTEX_SHADER { get { return Resources.TerrainVertexShader; } }
        protected override string FRAGMENT_SHADER { get { return Resources.TerrainFragmentShader; } }
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

        #endregion



        #region Singleton
        private TerrainRenderer() { }
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
    }
}
