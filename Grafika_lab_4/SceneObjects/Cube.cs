using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Grafika_lab_4.SceneObjects
{
    public class Cube : RenderSceneObject
    {
        #region Fields
        int indiceCount;
        #endregion

        #region Properties
        StaticRenderer renderer = StaticRenderer.Instance;
        public override Renderer Renderer { get { return renderer; } }
        /*protected override Vector3 DefaultForward { get { return -Vector3.UnitZ; } }
        protected override Vector3 DefaultUp { get { return Vector3.UnitY; } }
        protected override Vector3 DefaultRight { get { return Vector3.UnitX; } }*/
        #endregion

        #region Constructors
        /// <summary>
        /// Cube of size 1
        /// </summary>
        /// <param name="name">name of object</param>
        public Cube(string name) : this(name,Vector3.Zero) { }
        /// <summary>
        /// Cube of size 1
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <param name="position">Initial positon of object</param>
        public Cube(string name, Vector3 position) : base(name, position)
        {
            CreateCube();
        }

        #endregion

        #region CreateCube

        private void CreateCube()
        {
            Vector3[] vertices = CreateVertices();
            Vector3[] normals = CreateNormals(vertices);
            int[] indices = CreateIndices();

            Bind();
            SetVerticesBuffer(vertices);
            SetIndicesBuffer(indices);
            SetNormalsBuffer(normals);
            UnBind();
        }

        private Vector3[] CreateVertices()
        {
            return new Vector3[]
            {
                //front
                new Vector3(-1,-1,1),
                new Vector3(1,-1,1),
                new Vector3(-1,1,1),
                new Vector3 (1,1,1),
                new Vector3(-1,-1,-1),
                new Vector3(1,-1,-1),
                new Vector3(-1,1,-1),
                new Vector3 (1,1,-1)
            };
        }

        private int[] CreateIndices()
        {
            return new int[]
            {
                //front
                0,1,2,
                2,3,1,
                //back
                4,5,6,
                6,7,5,
                //left
                0,4,2,
                2,6,4,
                //right
                1,5,7,
                7,3,1,
                //top
                2,3,6,
                6,7,3,
                //bottom
                0,1,4,
                4,5,1
            };
        }

        private Vector3[] CreateNormals(Vector3[] vertices)
        {
            return new Vector3[0];
        }

        #endregion

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1,out vertexArray);
            GL.GenBuffers(1,out vertexBuffer);
            GL.GenBuffers(1, out normalsBuffer);
            GL.GenBuffers(1, out elementsBuffer);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(normalsBuffer);
            GL.DeleteBuffer(elementsBuffer);
        }

        public override void Render()
        {
            renderer.EnableVertexAttribArrays();
            GL.DrawElements(BeginMode.Triangles, indiceCount, DrawElementsType.UnsignedInt, 0);
            renderer.DisableVertexAttribArrays();
        }
        float time;

        public override void Update(float deltatime)
        {
            time += deltatime;
            Translate(Up * (float)Math.Cos(time));
        }

        
    }
}
