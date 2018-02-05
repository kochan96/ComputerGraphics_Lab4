using Grafika_lab_4.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.Renderers
{
    public  class StaticRenderer:Renderer
    {
        #region Shaders

        protected override string VERTEX_SHADER { get { return Resources.StaticVertexShader; } }
        protected override string FRAGMENT_SHADER { get { return Resources.StaticFragmentShader; } }

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

        protected override string SkyColorUniName { get { return "SkyColor"; } }

        #endregion


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
