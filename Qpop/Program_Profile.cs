using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

    public class Program_Profile
    {
    string name;
    string display_name;
    int offset_x;
    int offset_y;
    int length_x;
    int length_y;
    bool auto_click = false;
    Bitmap comparison_img;
    public Program_Profile(int offset_x, int offset_y, int length_x, int length_y)
    {
        this.offset_x = offset_x;
        this.offset_y = offset_y;
        this.length_x = length_x;
        this.length_y = length_y;
    }
    public Program_Profile(int offset_x, int offset_y, int length_x, int length_y, Bitmap img) :this(offset_x, offset_y, length_x, length_y)
    {
        comparison_img = img;
    }

    public Program_Profile(): this(0, 0, 0, 0, (Bitmap)null)
    {
    }

    public Program_Profile(int offset_x, int offset_y, int length_x, int length_y, string img_name) : this(offset_x, offset_y, length_x, length_y)
    {
        Bitmap img = null; ;
        if (File.Exists(img_name)) img = new Bitmap(img_name);
        comparison_img = img;
    }

    public void Set_OffsetX(int x)
    {
        this.offset_x = x;
    }

    public void Set_OffsetY(int y)
    {
        this.offset_y = y;
    }

    public void Set_LengthX(int x)
    {
        this.length_x = x;
    }

    public void Set_LengthY(int y)
    {
        this.length_y = y;
    }

    public void Set_Image(Bitmap img)
    {
        this.comparison_img = img;
    }

    public void Set_Image(string filename)
    {
        Bitmap img = new Bitmap(filename);
        comparison_img = img;
    }

    public void Set_Name(string name)
    {
        this.name = name;
    }

    public string Get_Name()
    {
        return this.name;
    }

    public int Get_OffsetX()
    {
        return this.offset_x;
    }

    public int Get_OffsetY()
    {
        return this.offset_y;
    }

    public int Get_LengthX()
    {
        return this.length_x;
    }

    public int Get_LengthY()
    {
        return this.length_y;
    }

    public Bitmap Get_Image()
    {
        return this.comparison_img;
    }

    public void Set_DisplayName(string name)
    {
        this.display_name = name;
    }

    public string Get_DisplayName()
    {
        return this.display_name;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
