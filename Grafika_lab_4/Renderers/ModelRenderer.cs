using Grafika_lab_4.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.IO;

namespace Grafika_lab_4.Renderers
{
    public class ModelRenderer : Renderer
    {
        #region Shaders
        protected override string VERTEX_SHADER { get { return "Shaders/aircraft.vert"; } }
        protected override string FRAGMENT_SHADER { get { return "Shaders/aircraft.frag"; } }
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
        #endregion

        #region Singleton
        private ModelRenderer() { }
        private static volatile ModelRenderer instance;
        private static object syncRoot = new Object();

        public static ModelRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ModelRenderer();
                    }
                }
                return instance;
            }
        }

        #endregion
    }

}

