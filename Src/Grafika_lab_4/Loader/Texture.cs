namespace Grafika_lab_4.Loader
{
    public class Texture
    {
        public int TextureId { get;}
        public int Dimensions { get;}

        public Texture(int textid,int dimensions)
        {
            TextureId = textid;
            Dimensions = dimensions;
        }
    }
}
