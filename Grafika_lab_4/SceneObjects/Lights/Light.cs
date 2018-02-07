using OpenTK;

namespace Grafika_lab_4.Lights
{
    public class Light
    {

        public Light(string name)
        {
            Name = name;
        }
        public string Name { get; }
        public Vector3 Position;
        public Vector3 LightColor=Vector3.One;

    }


}

