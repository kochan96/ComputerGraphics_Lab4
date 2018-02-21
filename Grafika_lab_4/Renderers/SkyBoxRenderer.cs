using Grafika_lab_4.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.Renderers
{
    public class SkyBoxRenderer : Renderer
    {
        public override int MAX_LIGHTS { get { return 4; } }
        #region Shaders
        protected override string VERTEX_SHADER { get { return Resources.SkyBoxVertexShader; } }
        protected override string FRAGMENT_SHADER { get { return Resources.SkyBoxFragmentShader; } }
        #endregion

        #region AttributesNames
        protected override string PositonAttrName { get { return "Position";}}
        protected override string TextCoordAttribName { get { return "TextCoord"; } }
        protected override string NormalsAttribName { get { return "Normal"; } }

        #endregion

        #region UniformNames

        protected override string ModelMatrixUniName { get { return "ModelMatrix"; } }
        protected override string ViewMatrixUniName { get { return "ViewMatrix"; } }
        protected override string ProjMatrixUniName { get { return "ProjectionMatrix"; } }
        protected override string TextureSamplerUniName { get { return "CubeMap"; } }
        #endregion


        #region Singleton
        private SkyBoxRenderer() { }
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

    }
}
