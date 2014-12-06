using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Titan
{
    internal static class ImageUtil
    {
        public static Image ConvertToGrayscale(Image original)
        {
            if (original == null)
                throw new ArgumentNullException("original");

            // From http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale.

            //create a blank bitmap the same size as original

            var result = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image

            using (var g = Graphics.FromImage(result))
            {
                //create the grayscale ColorMatrix

                var colorMatrix = new ColorMatrix(
                    new float[][] 
                    {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    }
                    );

                //create some image attributes

                var attributes = new ImageAttributes();

                //set the color matrix attribute

                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix

                g.DrawImage(
                    original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes
                    );

                //dispose the Graphics object
            }

            return result;
        }
    }
}
