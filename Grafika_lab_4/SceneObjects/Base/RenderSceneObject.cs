using Grafika_lab_4.Renderers;
using Grafika_lab_4.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Grafika_lab_4.SceneObjects.Base
{
    public abstract class RenderSceneObject
    {
        #region Buffers
        protected int vertexArray;
        protected int vertexBuffer;
        protected int elementsBuffer;
        protected int textureBuffer;
        protected int normalsBuffer;

        #endregion

        #region Fields
        public abstract Renderer Renderer { get;}
        public Matrix4 ModelMatrix { get; private set; }
        public Texture Texture { get; set; }

        #endregion
        public RenderSceneObject()
        {
            ModelMatrix = Matrix4.Identity;
            GenerateBuffers();
        }

        protected virtual void  GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out textureBuffer);
            GL.GenBuffers(1, out normalsBuffer);
        }

        protected void SetVerticesBuffer(Vector3[] vertices)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.PositionDataLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        protected void SetIndicesBuffer(int[] indices)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
        }
        protected void SetTextureBuffer(Vector2[] textcoord)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, textcoord.Length * Vector2.SizeInBytes, textcoord, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.TextCoordLocation, 2, VertexAttribPointerType.Float, true, 0, 0);
        }

        protected void SetNormalsBuffer(Vector3[] normals)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * Vector3.SizeInBytes, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.NormalsDataLocation, 3, VertexAttribPointerType.Float, true, 0, 0);
        }

        public void Bind()
        {
            GL.BindVertexArray(vertexArray);
        }

        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Scale(float scale)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateScale(scale);
        }
        public void Scale(Vector3 scale)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateScale(scale);
        }

        public void RotateByX(float angle)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateRotationX(angle);
        }
        public void RotateByY(float angle)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateRotationY(angle);
        }
        public void RotateByZ(float angle)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateRotationZ(angle);
        }

        public void Translate(Vector3 translate)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateTranslation(translate);
        }

        public abstract void Render();

        public abstract void Update(float deltatime);

        
    }
}
