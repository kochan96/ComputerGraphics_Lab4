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
            Vector3 zaxis = (CameraPosition - CameraTarget).Normalized();    // The "forward" vector.
            Vector3 xaxis = Vector3.Cross(CameraUp, zaxis).Normalized();// The "right" vector.
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);     // The "up" vector.

            // Create a 4x4 view matrix from the right, up, forward and eye position vectors
            Matrix4 viewMatrix = new Matrix4(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -Vector3.Dot(xaxis, CameraPosition), -Vector3.Dot(yaxis, CameraPosition), -Vector3.Dot(zaxis, CameraPosition), 1);

            return viewMatrix;

            /*return Matrix4.LookAt(CameraPosition, CameraTarget, CameraUp);

            Vector3 forward = normalize(CameraPosition - CameraTarget);
            Vector3 right = crossProduct(normalize(tmp), forward);
            Vector3 up = crossProduct(forward, right);

            Matrix4 camToWorld = new Matrix4()
            {
                
            }

            camToWorld[0][0] = right.x;
            camToWorld[0][1] = right.y;
            camToWorld[0][2] = right.z;
            camToWorld[1][0] = up.x;
            camToWorld[1][1] = up.y;
            camToWorld[1][2] = up.z;
            camToWorld[2][0] = forward.x;
            camToWorld[2][1] = forward.y;
            camToWorld[2][2] = forward.z;

            camToWorld[3][0] = from.x;
            camToWorld[3][1] = from.y;
            camToWorld[3][2] = from.z;

            return camToWorld;*/
        }

        public virtual void Update()
        { }
    }
}
