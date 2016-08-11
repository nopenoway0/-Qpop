using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class Image_Manipulator
{
    public static float THRESHOLD = 65;
    public static int DEFAULT_RESIZE = 150;
    public static bool Compare_Image(Bitmap imgA, Bitmap imgB)
    {
        return Compare_Image(imgA, imgB, DEFAULT_RESIZE);
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
        //Console.Write(string.Concat("Pixel Match: ", total_pixels - diff, "/", total_pixels," Pixels\n", perc_matched, "%\n"));
        if (perc_matched >= THRESHOLD) return true;
        else return false;
    }

    public static bool Compare_Image(byte[,,] pixelValues, Bitmap imgB, int rescale_size)
    {
        int total_pixels = rescale_size * rescale_size;
        Bitmap reducedB = new Bitmap(imgB, rescale_size, rescale_size);
        //reducedB.Save("reducedB.bmp"); // del
        float diff = 0;
        // Courtesy of https://rosettacode.org/wiki/Percentage_difference_between_images#C.23
        for (int y = 0; y < reducedB.Height; y++)
        {
            for (int x = 0; x < reducedB.Width; x++)
            {
                diff += (float)Math.Abs(pixelValues[x, y, 0] - reducedB.GetPixel(x, y).R) / 255;
                diff += (float)Math.Abs(pixelValues[x, y, 1] - reducedB.GetPixel(x, y).G) / 255;
                diff += (float)Math.Abs(pixelValues[x, y, 2] - reducedB.GetPixel(x, y).B) / 255;
            }
        }
        float perc_matched = ((total_pixels - diff) / total_pixels) * 100;
        Console.WriteLine("\n" + perc_matched);
        //Console.Write(string.Concat("Pixel Match: ", total_pixels - diff, "/", total_pixels," Pixels\n", perc_matched, "%\n"));
        if (perc_matched >= THRESHOLD) return true;
        else return false;
    }

    public static byte[,,] Store_Image_Value(Bitmap imgA, int rescale_size)
    {
        Bitmap reducedA = new Bitmap(imgA, rescale_size, rescale_size);
        byte[,,] pixelValues = new byte[reducedA.Height, reducedA.Width, 3];
        for (int y = 0; y < reducedA.Height; y++)
         {
            for (int x = 0; x < reducedA.Width; x++)
         {
            pixelValues[x, y, 0] = reducedA.GetPixel(x, y).R;
            pixelValues[x, y, 1] = reducedA.GetPixel(x, y).G;
            pixelValues[x, y, 2] = reducedA.GetPixel(x, y).B;
        }
        }
        return pixelValues;
    }

    public static bool Compare_Image_Section(Bitmap imgA, Bitmap imgB, Rectangle section_imgA, Rectangle section_imgB)
    {
        Bitmap croppedA = (Bitmap)imgA.Clone(section_imgA, imgA.PixelFormat);
        //croppedA.Save("CroppedA.bmp"); // del
        Bitmap croppedB = (Bitmap)imgB.Clone(section_imgB, imgB.PixelFormat);
        //croppedB.Save("CroppedB.bmp"); // del
        return Compare_Image(croppedA, croppedB);
    }

    public static bool Compare_Image_Section(Bitmap imgA, Rectangle section_imgA, byte[,,] pixelValues)
    {
        Bitmap croppedA = (Bitmap)imgA.Clone(section_imgA, imgA.PixelFormat);
        return Compare_Image(pixelValues, croppedA, Image_Manipulator.DEFAULT_RESIZE);
    }

    // Returns the dimensions of the filename, no error handling, x,y format, or width then height
    public static int[] Get_Image_Dimensions(string filename)
    {
        System.Drawing.Image dimen;
        int[] dimensions = new int[2];
        try {
            dimen = System.Drawing.Image.FromFile(filename);
        } catch(Exception e)
        {
            dimensions[0] = 0;
            dimensions[1] = 0;
            return dimensions;
            throw;
        }
        dimensions[0] = dimen.Width;
        dimensions[1] = dimen.Height;
        return dimensions;
    }
}
