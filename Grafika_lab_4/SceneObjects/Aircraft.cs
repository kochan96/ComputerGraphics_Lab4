using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.SceneObjects
{
    public class Aircraft : RenderSceneObject
    {
        public override Renderer Renderer { get { return ModelRenderer.Instance; } }
        readonly string ModelFilePath = "Resources/Aircraft/Aircraft.obj";
        int indiceCount;
        #region Fields

        #endregion
        public Aircraft()
        {
            if (File.Exists(ModelFilePath))
            {
                CreateAircraft(ModelFilePath);
            }
            else
                throw new FileNotFoundException("Did not find model file for aircraft");
        }

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out normalsBuffer);
        }


        private void CreateAircraft(string filePath)
        {
        }


        public override void Render()
        {
            Renderer.EnableVertexAttribArrays();
            GL.DrawElements(BeginMode.Triangles,indiceCount,DrawElementsType.UnsignedInt,0);
            Renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {
            
        }
    }
}
