using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Windows.Markup;
using System.Xml;
using System.Globalization;

namespace Tema1
{
    public partial class Form1 : Form
    {
        Bitmap btm;
        int pointsLeft = 2;
        int met = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            algorithm.Text = "DDA";
            algorithm.ForeColor = Color.Red;
        }

        private void draw_Click(object sender, EventArgs e)
        {
            float xa, ya, xb, yb;

            if (x1.Text == "" || y1.Text == "" || x2.Text == "" || y2.Text == "")
            {
                MessageBox.Show("Toate campurile trebuie completate!");
            }
            else if (!float.TryParse(x1.Text, out xa) || !float.TryParse(y1.Text, out ya) || !float.TryParse(x2.Text, out xb) || !float.TryParse(y2.Text, out yb))
            {
                MessageBox.Show("Coordonatele trebuie sa fie numere reale!");
            }
            else if (xa < 0 || xa > pictureBox1.Width || ya < 0 || ya > pictureBox1.Height || xb < 0 || xb > pictureBox1.Width || yb < 0 || yb > pictureBox1.Height)
            {
                MessageBox.Show("Coordonatele trebuie sa se afle in interiorul pictureBox-ului!");
            }
            else
            {
                btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                pictureBox1.Image = null;

                if (met == 0)
                {
                    DDA(float.Parse(x1.Text), float.Parse(y1.Text), float.Parse(x2.Text), float.Parse(y2.Text));
                }
                else
                {
                    Bresenham(int.Parse(x1.Text), int.Parse(y1.Text), int.Parse(x2.Text), int.Parse(y2.Text));
                }

                pictureBox1.Image = btm;
            }
            
        }

        private void reset_Click(object sender, EventArgs e)
        {
            btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            pictureBox1.Image = null;

            pointsLeft = 2;

            x1.Text = "";
            y1.Text = "";
            x2.Text = "";
            y2.Text = "";
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (pointsLeft == 2)
            {
                x1.Text = e.X.ToString();
                y1.Text = e.Y.ToString();

                pointsLeft--;
            }
            else if (pointsLeft == 1)
            {
                x2.Text = e.X.ToString();
                y2.Text = e.Y.ToString();
                pointsLeft--;

                draw_Click(null, null);
            }
            else
            {
                reset_Click(null, null);

                x1.Text = e.X.ToString();
                y1.Text = e.Y.ToString();

                pointsLeft--;
            }
        } 

        private void DDA(float x1, float y1, float x2, float y2)
        {
            float dx, dy, totalPixels, xInc, yInc;

            dx = x2 - x1;
            dy = y2 - y1;

            float slope = dy / dx;

            totalPixels = Math.Max(Math.Abs(dx), Math.Abs(dy));

            xInc = dx / totalPixels;
            yInc = dy / totalPixels;

            for (int i = 0; i < totalPixels; i++)
            {
                btm.SetPixel((int)Math.Round(x1), (int)Math.Round(y1), Color.Red);

                // Grosime
                if (int.Parse(thickness.Text) > 1)
                {
                    if (Math.Abs(slope) < 1)
                    {
                        float j = (int)y1;
                        while (j <= y1 + int.Parse(thickness.Text) / 2 && j < pictureBox1.Height)
                        {
                            btm.SetPixel((int)Math.Round(x1), (int)Math.Round(j), Color.Red);
                            j++;
                        }
                    }
                    else
                    {
                        float j = (int)x1;
                        while (j <= x1 + int.Parse(thickness.Text) / 2 && j < pictureBox1.Width)
                        {
                            btm.SetPixel((int)Math.Round(j), (int)Math.Round(y1), Color.Red);
                            j++;
                        }
                    }
                }
                

                x1 += xInc;
                y1 += yInc;
            }
        }

        private void Bresenham(int x, int y, int x2, int y2)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;

            if (w < 0)
            {
                dx1 = -1;
            }
            else if (w > 0)
            {
                dx1 = 1;
            }

            if (h < 0)
            {
                dy1 = -1;
            }
            else if (h > 0)
            {
                dy1 = 1;
            }

            if (w < 0)
            {
                dx2 = -1;
            }
            else if (w > 0)
            {
                dx2 = 1;
            }

            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);

            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);

                if (h < 0)
                {
                    dy2 = -1;
                }
                else if (h > 0)
                {
                    dy2 = 1;
                }

                dx2 = 0;
            }

            float slope = (float)(y2 - y) / (x2 - x);

            int numerator = longest >> 1;

            for (int i = 0; i <= longest; i++)
            {
                btm.SetPixel(x, y, Color.Blue);
                
                if (Math.Abs(slope) < 1)
                {
                    float j = (int)y;
                    while (j <= y + int.Parse(thickness.Text) / 2 && j < pictureBox1.Height)
                    {
                        btm.SetPixel(x, (int)Math.Round(j), Color.Blue);
                        j++;
                    }
                }
                else
                {
                    float j = (int)x;
                    while (j <= x + int.Parse(thickness.Text) / 2 && j < pictureBox1.Width)
                    {
                        btm.SetPixel((int)Math.Round(j), y, Color.Blue);
                        j++;
                    }
                }

                numerator += shortest;

                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            met = 0;

            algorithm.Text = "DDA";
            algorithm.ForeColor = Color.Red;

            reset_Click(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            met = 1;

            algorithm.Text = "Bresenham";
            algorithm.ForeColor = Color.Blue;

            reset_Click(null, null);
        }
    }
}
