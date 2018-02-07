using System;

namespace Grafika_lab_4.Loader
{
    public class TextureData
    {
        public byte[] Buffer { get;}
        public int Width { get; }
        public int Height { get; }

        public TextureData(byte[] buffer,int width,int height)
        {
            Buffer = buffer;
            Width = width;
            Height = height;
        }
    }
}
