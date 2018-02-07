using Grafika_lab_4.Loader;
using Grafika_lab_4.Renderers;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

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
        public Quaternion Rotation { get; private set; }
        public Vector3 Scale { get; private set;}
        #endregion

        #region Properties
        public string Name { get; set; }
        public abstract Renderer Renderer { get; }
        public Matrix4 ModelMatrix { get; private set; }
        public Texture Texture { get; set; }


        /// <summary>
        /// WorldPosition of object
        /// </summary>
        public Vector3 Position
        {
            get
            {
                Matrix4 tmp = ModelMatrix;
                tmp.Transpose();
                Vector3 result = new Vector3(tmp * new Vector4(0,0,0,1.0f));
                return result;
            }
        }

        /// <summary>
        /// Vector Facing forward from object
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                Vector3 rotated=Rotation* (-Vector3.UnitZ);
                rotated.Normalize();
                return rotated;
            }
        }

        /// <summary>
        /// Vector Facing up from object
        /// </summary>
        public Vector3 Up
        {
            get
            {
                return Rotation* Vector3.UnitY;
            }
        }

        /// <summary>
        /// Vector Facing right from object
        /// </summary>
        public Vector3 Right
        {
            get
            {
                return Rotation* Vector3.UnitX;
            }
        }
        #endregion

        #region Constructors

        public RenderSceneObject(string name,Vector3 position):this(name)
        {
            Translate(position);
        }
        public RenderSceneObject(string name)
        {
            Name = name;
            ModelMatrix = Matrix4.Identity;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
            GenerateBuffers();
        }

        #endregion

        #region SetBuffers

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

        protected void SetTextureBuffer(Vector3[] textcoord)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, textcoord.Length * Vector3.SizeInBytes, textcoord, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.TextCoordLocation, 3, VertexAttribPointerType.Float, true, 0, 0);
        }

        protected void SetNormalsBuffer(Vector3[] normals)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * Vector3.SizeInBytes, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.NormalsDataLocation, 3, VertexAttribPointerType.Float, true, 0, 0);
        }

        #endregion

        public void Bind()
        {
            GL.BindVertexArray(vertexArray);
        }

        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        #region Transformations
        /// <summary>
        /// Scales object by factor
        /// </summary>
        /// <param name="scale"></param>
        public void ScaleObject(float scale)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateScale(scale);
            Scale *= scale;
        }
        /// <summary>
        /// Scales object by vector
        /// </summary>
        /// <param name="scale">scale vedctor</param>
        public void ScaleObject(Vector3 scale)
        {
            ModelMatrix = ModelMatrix * Matrix4.CreateScale(scale);
            Scale *= scale;
        }


        /// <summary>
        /// Translates object by vector
        /// </summary>
        /// <param name="translate">translation vector</param>
        public void Translate(Vector3 translate)
        {

            ModelMatrix = ModelMatrix * Matrix4.CreateTranslation(translate);
        }

        /// <summary>
        /// Creates Quaternion from angle and axis
        /// </summary>
        /// <param name="angle">angle of rotation</param>
        /// <param name="axis">axis of rotation</param>
        /// <returns></returns>
        private Quaternion CreateQuaternion(float angle, Vector3 axis)
        {
            float sin = (float)Math.Sin(angle / 2);

            Quaternion q = new Quaternion();
            q.X = axis.X * sin;
            q.Y = axis.Y * sin;
            q.Z = axis.Z * sin;
            q.W = (float)Math.Cos(angle / 2);

            return q;
        }

        /// <summary>
        /// Rotate object by Forward vector (Changes Up and Right Vectors)
        /// Default Forward vector: (0,0,-1)
        /// Default Up vector: (0,1,0)
        /// Default Right vecotr: (1,0,0)
        /// </summary>
        /// <param name="angle">angle of roll</param>
        public void Roll(float angle)
        {
            Vector3 old = Position;
            Translate(-old);
            Quaternion q = CreateQuaternion(angle, Forward);
            ModelMatrix = ModelMatrix * Matrix4.CreateFromQuaternion(q);
            Translate(old);
            Rotation = q*Rotation;
        }

        /// <summary>
        /// Rotate object by Up vector (Changes Forward and Right Vector)
        /// Default Forward vector: (0,0,-1)
        /// Default Up vector: (0,1,0)
        /// Default Right vecotr: (1,0,0)
        /// </summary>
        /// <param name="angle"></param>
        public void Yaw(float angle)
        {
            Vector3 old = Position;
            Translate(-old);
            Quaternion q = CreateQuaternion(angle, Up);
            ModelMatrix = ModelMatrix * Matrix4.CreateFromQuaternion(q);
            Translate(old);
            Rotation = q*Rotation;
        }

        /// <summary>
        /// Rotate object by Right vector (Changes Forward and Up Vector)
        /// Default Forward vector: (0,0,-1)
        /// Default Up vector: (0,1,0)
        /// Default Right vecotr: (1,0,0)
        /// </summary>
        /// <param name="angle"></param>
        public void Pitch(float angle)
        {
            Vector3 old = Position;
            Translate(-old);
            Quaternion q = CreateQuaternion(angle, Right);
            ModelMatrix = ModelMatrix * Matrix4.CreateFromQuaternion(q);
            Translate(old);
            Rotation = q*Rotation;
        }
        /// <summary>
        /// Rotates object over axis (Does not change Forward,Up and Right vectors)
        /// </summary>
        /// <param name="angle">angle of rotation</param>
        /// <param name="axis">axis of rotation</param>
        public void Rotate(float angle,Vector3 axis )
        {
            Quaternion q = CreateQuaternion(angle, axis);
            ModelMatrix = ModelMatrix * Matrix4.CreateFromQuaternion(q);
        }

        /// <summary>
        /// Rotates object over  world Z-Axis (Does not change Forward,Up and Right vectors)
        /// </summary>
        /// <param name="angle">angle of rotation</param>
        public void RotateByZ(float angle)
        {
            Quaternion q = CreateQuaternion(angle, Vector3.UnitZ);
            ModelMatrix = ModelMatrix * Matrix4.CreateFromQuaternion(q);
        }

        /// <summary>
        /// Rotates object over  world X-Axis (Does not change Forward,Up and Right vectors)
        /// </summary>
        /// <param name="angle">angle of rotation</param>
        public void RotateByX(float angle)
        {
            Quaternion q = CreateQuaternion(angle, Vector3.UnitX);
            ModelMatrix = ModelMatrix * Matrix4.CreateFromQuaternion(q);
        }

        /// <summary>
        /// Rotates object over  world Y-Axis (Does not change Forward,Up and Right vectors)
        /// </summary>
        /// <param name="angle">angle of rotation</param>
        public void RotateByY(float angle)
        {
            Quaternion q = CreateQuaternion(angle, Vector3.UnitY);
            ModelMatrix = ModelMatrix * Matrix4.CreateFromQuaternion(q);
        }

        #endregion


        public abstract void Render();

        public abstract void Update(float deltatime);

        public abstract void Dispose();

        protected abstract void GenerateBuffers();

    }
}
