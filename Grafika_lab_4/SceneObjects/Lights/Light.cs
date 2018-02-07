using OpenTK;

namespace Grafika_lab_4.Lights
{
    public class Light
    {
        public Vector3 Position=Vector3.UnitZ;
        public Vector3 LightColor=Vector3.One;
        public string Name;

        public Light(string name)
        {
            Name = name;
        }

    }
}
