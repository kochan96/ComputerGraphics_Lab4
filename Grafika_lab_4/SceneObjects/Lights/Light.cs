using OpenTK;

namespace Grafika_lab_4.Lights
{
    public class Light
    {

        public Light(string name)
        {
            Name = name;
            LightType = LightTypes.PointLight;
        }
        public string Name { get; }
        public Vector3 Position;
        public Vector3 Color;
        public float DiffuseIntensity;
        public float AmbientIntensity;
        public float SpecularIntensity;

        public LightTypes LightType;
        public Vector3 Direction;
        public float ConeAngle;
        public Vector3 Attenuation=new Vector3(1,0,0);
    }

    public enum LightTypes { PointLight, SpotLight,Directional }

}

