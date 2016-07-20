using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class Image_Manipulator
{
    static float THRESHOLD = 60;
    public static bool Compare_Image(Bitmap imgA, Bitmap imgB)
    {
        return Compare_Image(imgA, imgB, 100);
    }

    static void SET_THRESHOLD(float a)
    {
        THRESHOLD = a;
    }

    public static bool Compare_Image(Bitmap imgA, Bitmap imgB, int rescale_size)
    {
        int total_pixels = rescale_size * rescale_size;
        Bitmap reducedA = new Bitmap(imgA, rescale_size, rescale_size);
        //reducedA.Save("reducedA.bmp"); // del
        Bitmap reducedB = new Bitmap(imgB, rescale_size, rescale_size);
        //reducedB.Save("reducedB.bmp"); // del
        float diff = 0;
        // Courtesy of https://rosettacode.org/wiki/Percentage_difference_between_images#C.23
        for (int y = 0; y < reducedA.Height; y++)
        {
            for (int x = 0; x < reducedA.Width; x++)
            {
                diff += (float)Math.Abs(reducedA.GetPixel(x, y).R - reducedB.GetPixel(x, y).R) / 255;
                diff += (float)Math.Abs(reducedA.GetPixel(x, y).G - reducedB.GetPixel(x, y).G) / 255;
                diff += (float)Math.Abs(reducedA.GetPixel(x, y).B - reducedB.GetPixel(x, y).B) / 255;
            }
        }
        float perc_matched = ((total_pixels - diff) / total_pixels) * 100;
        Console.Write(string.Concat("Pixel Match: ", total_pixels - diff, "/", total_pixels," Pixels\n", perc_matched, "%\n"));
        if (perc_matched >= THRESHOLD) return true;
        else return false;
    }

    public static bool Compare_Image_Section(Bitmap imgA, Bitmap imgB, Rectangle section_imgA, Rectangle section_imgB)
    {
        Bitmap croppedA = (Bitmap)imgA.Clone(section_imgA, imgA.PixelFormat);
        //croppedA.Save("CroppedA.bmp"); // del
        Bitmap croppedB = (Bitmap)imgB.Clone(section_imgB, imgB.PixelFormat);
        //croppedB.Save("CroppedB.bmp"); // del
        return Compare_Image(croppedA, croppedB);
    }
}
