using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGL;
using System.Runtime.InteropServices;



namespace myOpenGL
{
    public partial class Form1 : Form
    {
        cOGL cGL;

      
        public Form1()
        {

            InitializeComponent();
            cGL = new cOGL(panel1);
            //apply the bars values as cGL.ScrollValue[..] properties 
                                         //!!!
            hScrollBarScroll(hScrollBar1, null);
            hScrollBarScroll(hScrollBar2, null);
            hScrollBarScroll(hScrollBar3, null);
            hScrollBarScroll(hScrollBar4, null);
            hScrollBarScroll(hScrollBar5, null);
            hScrollBarScroll(hScrollBar6, null);
            hScrollBarScroll(hScrollBar7, null);
            hScrollBarScroll(hScrollBar8, null);
            hScrollBarScroll(hScrollBar9, null);
            hScrollBarScroll(hScrollBar11, null);
            hScrollBarScroll(hScrollBar12, null);
            hScrollBarScroll(hScrollBar13, null);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            cGL.OnResize();
        }

        public float[] oldPos = new float[7];

        private void numericUpDownValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nUD = (NumericUpDown)sender;
            int i = int.Parse(nUD.Name.Substring(nUD.Name.Length - 1));
            int pos = (int)nUD.Value;
            switch (i)
            {
                case 1:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xShift += 0.25f;
                        cGL.intOptionC = 4;
                    }
                    else
                    {
                        cGL.xShift -= 0.25f;
                        cGL.intOptionC = -4;
                    }
                    break;
                case 2:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yShift += 1.0f;// 0.25f;
                        cGL.intOptionC = 5;
                    }
                    else
                    {
                        cGL.yShift -= 1.0f;// 0.25f;
                        cGL.intOptionC = -5;
                    }
                    break;
                case 3:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zShift += 1.0f;// 0.25f;
                        cGL.intOptionC = 6;
                    }
                    else
                    {
                        cGL.zShift -= 1.0f;//  0.25f;
                        cGL.intOptionC = -6;
                    }
                    break;
                case 4:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xAngle += 5;
                        cGL.intOptionC = 1;
                    }
                    else
                    {
                        cGL.xAngle -= 5;
                        cGL.intOptionC = -1;
                    }
                    break;
                case 5:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yAngle += 5;
                        cGL.intOptionC = 2;
                    }
                    else
                    {
                        cGL.yAngle -= 5;
                        cGL.intOptionC = -2;
                    }
                    break;
                case 6:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zAngle += 5;
                        cGL.intOptionC = 3;
                    }
                    else
                    {
                        cGL.zAngle -= 5;
                        cGL.intOptionC = -3;
                    }
                    break;
            }
            cGL.Draw();
            oldPos[i - 1] = pos;

        }

        private void hScrollBarScroll(object sender, ScrollEventArgs e)
        {
            cGL.intOptionC = 0;
            HScrollBar hb = (HScrollBar)sender;
            int n = int.Parse(hb.Name.Substring(10));
            cGL.ScrollValue[n - 1] = (hb.Value - 100) / 10.0f;
            if (e != null)
                cGL.Draw();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           //GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            cGL.Draw();
           //WGL.wglSwapBuffers(cGL. m_uint_DC);
            //GL.glFlush();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            cGL.isReflection = !cGL.isReflection;
            cGL.Draw();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            string s = ((RadioButton)sender).Name;
            cGL.intOptionM = int.Parse(s.Substring(11));
            cGL.Draw();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            string s = ((RadioButton)sender).Name;
            cGL.intOptionP = int.Parse(s.Substring(11));
            cGL.Draw();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            cGL.isShadow = !cGL.isShadow;
            cGL.Draw();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            cGL.isLight = !cGL.isLight;
            cGL.Draw();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            cGL.isFloor = !cGL.isFloor;
            cGL.Draw();
        } 
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            cGL.isOrbit = !cGL.isOrbit;
            cGL.Draw();
        }


        private void pnlViewPort_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cGL.moviendo = 1;
                //cGL.Update(cGL.moviendo);
               // cGL.Draw();
               

            }
            else
            {
                cGL.moviendo = -1;
               // cGL.Update(cGL.moviendo);
                //cGL.Draw();
               
            }

        }

        private void pnlViewPort_MouseUp(object sender, MouseEventArgs e)
        {
            cGL.moviendo = 0;
            //cGL.Update(cGL.moviendo);
           // cGL.Draw();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            cGL.panelPosX = this.Left;
            cGL.panelPosY = this.Top;
           
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {   
            if (e.KeyCode == Keys.Escape)
            {
                cGL.Draw();
                //this.Close();
               // Close();//                cGL.Draw();
            }
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Enter key pressed");
                cGL.Draw();
                //if (showOrbit == true)
                //    showOrbit = false;
                //else
                //    showOrbit = true;
            }
        }
    }
}