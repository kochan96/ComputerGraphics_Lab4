using Grafika_lab_4.Configuration;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Grafika_lab_4.Renderers
{
    public class SkyBoxRenderer : Renderer
    {
        private SkyBoxRenderer() : base(Resources.SkyBoxVertexShader, Resources.SkyBoxFragmentShader)
        { }
        private static volatile SkyBoxRenderer instance;
        private static readonly object _syncRoot = new object();

        public static SkyBoxRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SkyBoxRenderer();
                        }
                    }
                }
                return instance;
            }
        }

        public int PositionAttribute { get; private set; }

        private int ViewMatrixUniform;
        private int ProjectionMatrixUniform;

        public override void EnableVertexAttribArrays()
        {
            GL.EnableVertexAttribArray(PositionAttribute);
        }

        public override void DisableVertexAttribArrays()
        {
            GL.DisableVertexAttribArray(PositionAttribute);
        }

        protected override void SetAttributess()
        {
            PositionAttribute = GetAttrubute(nameof(PositionAttribute));
        }

        protected override void SetUniformss()
        {
            ViewMatrixUniform = GetUniform(nameof(ViewMatrixUniform));
            ProjectionMatrixUniform = GetUniform(nameof(ProjectionMatrixUniform));
        }

        public void SetViewMatrix(Matrix4 viewMatrix)
        {
            GL.UniformMatrix4(ViewMatrixUniform, false, ref viewMatrix);
        }

        public void SetProjectionMatrix(Matrix4 projMatrix)
        {
            GL.UniformMatrix4(ProjectionMatrixUniform, false, ref projMatrix);
        }
    }
}
