using Grafika_lab_4.Renderers.Structs;
using OpenTK;

namespace Grafika_lab_4.Lights
{
    public class Light
    {
        public Vector3 Position { get; set; }

        public Vector3 Color { get; set; }

        public float DiffuseIntensity { get; set; }

        public float AmbientIntensity { get; set; }

        public float SpecularIntensity { get; set; }

        public LightTypes LightType { get; set; } = LightTypes.PointLight;

        public Vector3 Direction { get; set; }

        public float ConeAngle { get; set; }

        public Vector3 Attenuation { get; set; } = new Vector3(1, 0, 0);
    }
}

