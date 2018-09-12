using OpenTK;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class Camera
    {
        public bool IsActive { get; set; }

        public Vector3 CameraPosition { get; set; } = Vector3.Zero;

        public Vector3 CameraTarget { get; set; } = Vector3.Zero;

        public Vector3 CameraUp { get; set; } = Vector3.UnitY;

        public Matrix4 GetViewMatrix()
        {
            /*Vector3 zaxis = (CameraPosition - CameraTarget).Normalized();    // The "forward" vector.
            Vector3 xaxis = Vector3.Cross(CameraUp, zaxis).Normalized();// The "right" vector.
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);     // The "up" vector.

            // Create a 4x4 view matrix from the right, up, forward and eye position vectors
            Matrix4 viewMatrix = new Matrix4(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Z, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -Vector3.Dot(xaxis, CameraPosition), -Vector3.Dot(yaxis, CameraPosition), -Vector3.Dot(zaxis, CameraPosition), 1);

            return viewMatrix;*/

            return Matrix4.LookAt(CameraPosition, CameraTarget, CameraUp);
        }

        public virtual void Update()
        { }
    }
}
