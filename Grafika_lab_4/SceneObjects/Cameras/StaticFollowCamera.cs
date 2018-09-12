using Grafika_lab_4.SceneObjects.Base;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class StaticFollowCamera : Camera
    {
        private readonly RenderSceneObject _follow;

        public StaticFollowCamera(RenderSceneObject follow)
        {
            _follow = follow;
            CameraTarget = follow.Position;
        }

        public override void Update()
        {
            CameraTarget = _follow.Position;
        }
    }
}
