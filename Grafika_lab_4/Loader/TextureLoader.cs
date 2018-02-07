﻿using Grafika_lab_4.Configuration;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Grafika_lab_4.Loader
{
    public static class TextureLoader
    {
        public static Texture LoadTexture2D(string fileName)
        {
            Bitmap image;
            try
            {
                image = new Bitmap(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.LoadTextureError) + fileName);
                return null;
            }
            int texID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(texID, 2);
        }

        private static TextureData GetTextureData(string fileName)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static Texture LoadCubeMap(string[] FileNames)
        {
            int textId = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, textId);
            int texID = GL.GenTexture();

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            for (int i = 0; i < FileNames.Length; i++)
            {
                Bitmap image = null;
                try
                {
                    image = new Bitmap(FileNames[i]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Errors.GetErrorMessage(ErrorType.LoadTextureError) + FileNames[i]);
                    continue;
                }

                BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                image.UnlockBits(data);
            }
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

            return new Texture(texID, 3);
        }
    }
}