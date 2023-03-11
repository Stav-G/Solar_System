using System;
using System.Collections.Generic;
using System.Windows.Forms;

//2
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenGL
{
    class cOGL
    {
        Control p;
        int Width;
        int Height;

        // flag for reflection
        public int intOptionM = 1;
        //flag for planet
        public int intOptionP = 1;
        GLUquadric obj;
       

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric(); //!!!

            ground[0, 0] = 1;
            ground[0, 1] = 1;
            ground[0, 2] = -0.5f;

            ground[1, 0] = 0;
            ground[1, 1] = 1;
            ground[1, 2] = -0.5f;

            ground[2, 0] = 1;
            ground[2, 1] = 0;
            ground[2, 2] = -0.5f;  
            
            ground[2, 0] = 1;
            ground[2, 1] = 0;
            ground[2, 2] = -0.5f;

            isShadow = false;
            isReflection = false;
        }

        ~cOGL()
        {
            GLU.gluDeleteQuadric(obj); //!!!
            WGL.wglDeleteContext(m_uint_RC);
        }

        uint m_uint_HWND = 0;

        public uint HWND
        {
            get { return m_uint_HWND; }
        }

        uint m_uint_DC = 0;

        public uint DC
        {
            get { return m_uint_DC; }
        }
        uint m_uint_RC = 0;

        public uint RC
        {
            get { return m_uint_RC; }
        }


        void DrawOldAxes()
        {
            //for this time
            //Lights positioning is here!!!
            float[] pos = new float[4];
            pos[0] = 10; 
            pos[1] = 10; 
            pos[2] = 10; 
            pos[3] = 1;

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);
            GL.glDisable(GL.GL_LIGHTING);

            //INITIAL axes
            GL.glEnable(GL.GL_LINE_STIPPLE);
            GL.glLineStipple(1, 0xFF00);  //  dotted   
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
            GL.glDisable(GL.GL_LINE_STIPPLE);
        }
        void DrawAxes()
        {
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
        }

        float[] planeCoeff = { 1, 1, 1, 1 };
        float[,] ground = new float[3, 3];//{ { 1, 1, -0.5 }, { 0, 1, -0.5 }, { 1, 0, -0.5 } };

        // Reduces a normal vector specified as a set of three coordinates,
        // to a unit normal vector of length one.
        void ReduceToUnit(float[] vector)
        {
            float length;

            // Calculate the length of the vector		
            length = (float)Math.Sqrt((vector[0] * vector[0]) +
                                (vector[1] * vector[1]) +
                                (vector[2] * vector[2]));

            // Keep the program from blowing up by providing an exceptable
            // value for vectors that may calculated too close to zero.
            if (length == 0.0f)
                length = 1.0f;

            // Dividing each element by the length will result in a
            // unit normal vector.
            vector[0] /= length;
            vector[1] /= length;
            vector[2] /= length;
        }

        const int x = 0;
        const int y = 1;
        const int z = 2;

        // Points p1, p2, & p3 specified in counter clock-wise order
        void calcNormal(float[,] v, float[] outp)
        {
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            // Calculate two vectors from the three points
            v1[x] = v[0, x] - v[1, x];
            v1[y] = v[0, y] - v[1, y];
            v1[z] = v[0, z] - v[1, z];

            v2[x] = v[1, x] - v[2, x];
            v2[y] = v[1, y] - v[2, y];
            v2[z] = v[1, z] - v[2, z];

            // Take the cross product of the two vectors to get
            // the normal vector which will be stored in out
            outp[x] = v1[y] * v2[z] - v1[z] * v2[y];
            outp[y] = v1[z] * v2[x] - v1[x] * v2[z];
            outp[z] = v1[x] * v2[y] - v1[y] * v2[x];

            // Normalize the vector (shorten length to one)
            ReduceToUnit(outp);
        }

        float[] cubeXform = new float[16];

        // Creates a shadow projection matrix out of the plane equation
        // coefficients and the position of the light. The return value is stored
        // in cubeXform[,]
        void MakeShadowMatrix(float[,] points)
        {
            float[] planeCoeff = new float[4];
            float dot;

            // Find the plane equation coefficients
            // Find the first three coefficients the same way we
            // find a normal.
            calcNormal(points, planeCoeff);

            // Find the last coefficient by back substitutions
            planeCoeff[3] = -(
                (planeCoeff[0] * points[2, 0]) + (planeCoeff[1] * points[2, 1]) +
                (planeCoeff[2] * points[2, 2]));


            // Dot product of plane and light position
            dot = planeCoeff[0] * pos[0] +
                    planeCoeff[1] * pos[1] +
                    planeCoeff[2] * pos[2] +
                    planeCoeff[3];

            // Now do the projection
            // First column
            cubeXform[0] = dot - pos[0] * planeCoeff[0];
            cubeXform[4] = 0.0f - pos[0] * planeCoeff[1];
            cubeXform[8] = 0.0f - pos[0] * planeCoeff[2];
            cubeXform[12] = 0.0f - pos[0] * planeCoeff[3];

            // Second column
            cubeXform[1] = 0.0f - pos[1] * planeCoeff[0];
            cubeXform[5] = dot - pos[1] * planeCoeff[1];
            cubeXform[9] = 0.0f - pos[1] * planeCoeff[2];
            cubeXform[13] = 0.0f - pos[1] * planeCoeff[3];

            // Third Column
            cubeXform[2] = 0.0f - pos[2] * planeCoeff[0];
            cubeXform[6] = 0.0f - pos[2] * planeCoeff[1];
            cubeXform[10] = dot - pos[2] * planeCoeff[2];
            cubeXform[14] = 0.0f - pos[2] * planeCoeff[3];

            // Fourth Column
            cubeXform[3] = 0.0f - pos[3] * planeCoeff[0];
            cubeXform[7] = 0.0f - pos[3] * planeCoeff[1];
            cubeXform[11] = 0.0f - pos[3] * planeCoeff[2];
            cubeXform[15] = dot - pos[3] * planeCoeff[3];
        }

        public bool isShadow = false;
        public bool isReflection = false;

        void DrawFigures_2()
        {
            int i;
            //!!!!!!!!!!!!!
            GL.glPushMatrix();
           // GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);
            //!!!!!!!!!!!!!
            //plane grids
            //GL.glDisable(GL.GL_LIGHTING);

            //GL.glEnable(GL.GL_LIGHTING);

            //DrawFloor();
            //GL.glColor4d(1.0, 1.0, 1.0, 0.1);

            //GL.glBegin(GL.GL_QUADS);
            //GL.glVertex3d(-20, -20, ground[0, 2] - 0.05);
            //GL.glVertex3d(-20, 20, ground[0, 2] - 0.05);
            //GL.glVertex3d(20, 20, ground[0, 2] - 0.05);
            //GL.glVertex3d(20, -20, ground[0, 2] - 0.05);
            //GL.glEnd();

            // must be in scene to be reflected too
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);

            //Draw Light Source
            GL.glDisable(GL.GL_LIGHTING);
            GL.glTranslatef(pos[0], pos[1], pos[2]);
            //Yellow Light source
            GL.glColor3f(1, 1, 0);
            GLUT.glutSolidSphere(0.05, 8, 8);
            GL.glTranslatef(-pos[0], -pos[1], -pos[2]);
            //projection line from source to plane
            GL.glBegin(GL.GL_LINES);
            GL.glColor3d(0.5, 0.5, 0);
            GL.glVertex3d(pos[0], pos[1], 0);
            GL.glVertex3d(pos[0], pos[1], pos[2]);
            GL.glEnd();

            //main System draw
            GL.glEnable(GL.GL_LIGHTING);

            DrawPlanet(false);


            //end of regular show
            //!!!!!!!!!!!!!
            GL.glPopMatrix();
            //!!!!!!!!!!!!!

            //SHADING begin
            //we'll define cubeXform matrix in MakeShadowMatrix Sub
            // Disable lighting, we'll just draw the shadow
            //else instead of shadow we'll see stange projection of the same objects
            GL.glDisable(GL.GL_LIGHTING);

            if(isShadow)
            {
                // floor shadow
                //!!!!!!!!!!!!!
                GL.glPushMatrix();
                //!!!!!!!!!!!!    		
                MakeShadowMatrix(ground);
                GL.glMultMatrixf(cubeXform);
                DrawPlanet(true);
                //!!!!!!!!!!!!!
                GL.glPopMatrix();
                //!!!!!!!!!!!!!
            }
           
        }
        void DrawPlanet(bool isForShades)
        {
           

            if (isForShades == false)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);

                switch (intOptionP)
                {
                    case 1:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
                        break;
                    case 2:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
                        break;
                    case 3:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
                        break;
                    case 4:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
                        break;
                    case 5:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
                        break;
                    case 6:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
                        break;
                    case 7:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
                        break;
                    case 8:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
                        break;
                    case 9:
                        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
                        break;
                }
                GLU.gluQuadricTexture(obj, 1);
                GL.glColor3f(1, 1, 1);
            }
            else
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
                GL.glColor3d(0.5, 0.5, 0.5);
            }

       

            GL.glRotated(intOptionB, 0, 0, 1); //rotate both
            //GL.glRotated(coinyRot, 0, 0, 1); //rotate both

          
            // GL.glTranslated(0, -0.5, 2);
            GL.glTranslated(0, 0.2, 2);
            //GL.glRotated(intOptionC, 1, 1, 1);
            GL.glRotated(intOptionC, 0, 1, 0);
            //GLUT.glutSolidCube(1);
            GLU.gluSphere(obj, 2, 32, 32);
            //GLUT.gluSphere(obj, 2.0, 50, 50);
            //.glRotated(-intOptionC, 1, 1, 1);
            GL.glRotated(-intOptionC, 0, 1, 0);
            //GL.glTranslated(0, -0.5, -2);
            GL.glTranslated(0, -0.2, -2);
            GL.glRotated(intOptionB, 0, 0, 1); //rotate both
            GL.glPopMatrix();

        }


        void DrawMoon()
        {

            // must be in scene to be reflected too
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);

            //Draw Light Source
            GL.glDisable(GL.GL_LIGHTING);
            GL.glTranslatef(pos[0], pos[1], pos[2]);
            //Yellow Light source
            GL.glColor3f(1, 1, 0);
            GLUT.glutSolidSphere(0.05, 8, 8);
            GL.glTranslatef(-pos[0], -pos[1], -pos[2]);
            //projection line from source to plane
            GL.glBegin(GL.GL_LINES);
            GL.glColor3d(0.5, 0.5, 0);
            GL.glVertex3d(pos[0], pos[1], 0);
            GL.glVertex3d(pos[0], pos[1], pos[2]);
            GL.glEnd();


            //main System draw
            GL.glEnable(GL.GL_LIGHTING);

            //GL.glEnable(GL.GL_TEXTURE_2D);
            // GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            // GLU.gluQuadricTexture(obj, 1);

            GL.glRotated(intOptionB, 0, 0, 1); //rotate both
            //GL.glRotated(coinyRot, 0, 0, 1); //rotate both

            GL.glColor3f(1, 1, 1);
            // GL.glTranslated(0, -0.5, 2);
            GL.glTranslated(0, 0.2, 2);
            //GL.glRotated(intOptionC, 1, 1, 1);
            GL.glRotated(intOptionC, 0, 1, 0);
            //GLUT.glutSolidCube(1);
            GLU.gluSphere(obj, 2, 32, 32);
            //GLUT.gluSphere(obj, 2.0, 50, 50);
            //.glRotated(-intOptionC, 1, 1, 1);
            GL.glRotated(-intOptionC, 0, 1, 0);
            //GL.glTranslated(0, -0.5, -2);
            GL.glTranslated(0, -0.2, -2);
            GL.glRotated(intOptionB, 0, 0, 1); //rotate both
            GL.glPopMatrix();
        }

        void DrawFloor()
        {
            GL.glEnable(GL.GL_LIGHTING);
            GL.glBegin(GL.GL_QUADS);
            //!!! for blended REFLECTION 
            GL.glColor4d(0, 0, 1, 0.1);
            GL.glVertex3d(-15, -15, 0);
            GL.glVertex3d(-15, 15, 0);
            GL.glVertex3d(15, 15, 0);
            GL.glVertex3d(15, -15, 0);
            //GL.glVertex3d(-3, -3, 0);
            //GL.glVertex3d(-3, 3, 0);
            //GL.glVertex3d(3, 3, 0);
            //GL.glVertex3d(3, -3, 0);
            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);
        }

        void DrawStars()
        {
            for (int i = 0; i < 1000; i++)
            {
                float x = (float)rand.NextDouble() * 200.0f - 100.0f;
                float y = (float)rand.NextDouble() * 200.0f - 100.0f;
                float z = (float)rand.NextDouble() * 200.0f - 100.0f;

                GL.glVertex3f(x, y, z);

                //Draw Light Source
                GL.glDisable(GL.GL_LIGHTING);
                GL.glTranslatef(x, y, z);
                //Yellow Light source
                GL.glColor3f(1, 1, 1);
                GLUT.glutSolidSphere(0.05, 8, 8);
                GL.glTranslatef(-x, -y, -z);
                //projection line from source to plane
                GL.glBegin(GL.GL_LINES);
                GL.glColor3f(0.5f, 0.5f, 0.5f);
                GL.glVertex3f(x, y, z);
                GL.glVertex3f(x, y, z);
                GL.glEnd();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
        }


        void DrawFigures()
        {
            GL.glPushMatrix();

            // must be in scene to be reflected too
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);

            //Draw Light Source
            GL.glDisable(GL.GL_LIGHTING);
            GL.glTranslatef(pos[0], pos[1], pos[2]);
            //Yellow Light source
            GL.glColor3f(1, 1, 0);
            GLUT.glutSolidSphere(0.05, 8, 8);
            GL.glTranslatef(-pos[0], -pos[1], -pos[2]);
            //projection line from source to plane
            //GL.glBegin(GL.GL_LINES);
            //GL.glColor3d(0.5, 0.5, 0);
            //GL.glVertex3d(pos[0], pos[1], 0);
            //GL.glVertex3d(pos[0], pos[1], pos[2]);
            //GL.glEnd();

            /***************************************************************************************/

            for (int i = 0; i < 1000; i++)
            {
                float x = (float)rand.NextDouble() * 200.0f - 100.0f;
                float y = (float)rand.NextDouble() * 200.0f - 100.0f;
                float z = (float)rand.NextDouble() * 200.0f - 100.0f;

                GL.glVertex3f(x, y, z);

                //Draw Light Source
                GL.glDisable(GL.GL_LIGHTING);
                GL.glTranslatef(x, y, z);
                //Yellow Light source
                GL.glColor3f(1, 1, 1);
                GLUT.glutSolidSphere(0.05, 8, 8);
                GL.glTranslatef(-x, -y, -z);
                //projection line from source to plane
                GL.glBegin(GL.GL_LINES);
                GL.glColor3f(0.5f, 0.5f, 0.5f);
                GL.glVertex3f(x, y, z);
                GL.glVertex3f(x, y, z);
                GL.glEnd();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            /***************************************************************************************/
            //main System draw
            // GL.glEnable(GL.GL_LIGHTING);

            // //GL.glEnable(GL.GL_TEXTURE_2D);
            //// GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            //// GLU.gluQuadricTexture(obj, 1);

            // GL.glRotated(intOptionB, 0, 0, 1); //rotate both
            // //GL.glRotated(coinyRot, 0, 0, 1); //rotate both

            // GL.glColor3f(1, 1, 1);
            // GL.glTranslated(0, -0.5, 1);
            // //GL.glRotated(intOptionC, 1, 1, 1);
            // GL.glRotated(intOptionC, 0, 1, 0);
            // //GLUT.glutSolidCube(1);
            // GLU.gluSphere(obj, 2, 32, 32);
            // //GLUT.gluSphere(obj, 2.0, 50, 50);
            // //.glRotated(-intOptionC, 1, 1, 1);
            // GL.glRotated(-intOptionC, 0, 1, 0);
            // GL.glTranslated(0, -0.5, -1);

            // GL.glRotated(intOptionB, 0, 0, 1); //rotate both
            // GL.glPopMatrix();

            //  GL.glDisable(GL.GL_TEXTURE_2D);
            //GL.glDisable(GL.GL_TEXTURE_2D);
            // DrawAxes();


            //GL.glColor3f(1, 1, 1);
            //GL.glTranslated(0, -2, 0);
            //GL.glRotated(coinyRot, 0, 1, 0);
            //GLUT.gluDisk(obj, 0, 2, 30, 30);
            //GL.glRotated(-coinyRot, 0, 1, 0);
            //GL.glTranslated(0, -2, 0);
            //GL.glColor3f(1, 1, 1);

            // GL.glColor3f(1, 1, 0);
            // GL.glTranslated(0, -0.5, 1);
            // GL.glRotated(coinyRot, 1, 1, 1);
            //// GLUT.gluDisk(obj, 0, 2, 30, 30);
            // GLUT.gluCylinder(obj, 1.0, 1.0, 2.0, 50,10);
            // GL.glRotated(-coinyRot, 1, 1, 1);
            // GL.glTranslated(0, -0.5, -1);
            // GL.glColor3f(1, 1, 1);


            // GL.glTranslated(1, 2, 1.5);
            // GL.glRotated(90, 1, 0, 0);
            // GL.glColor3d(0, 1, 1);
            // GL.glRotated(intOptionB, 1, 0, 0);
            // GLUT.gluDisk(obj, 0, 2, 30, 30);
            // GL.glRotated(-intOptionB, 1, 0, 0); //not neccessary
            // GL.glRotated(-90, 1, 0, 0);     //not neccessary
            // GL.glTranslated(-1, -2, -1.5);  //


            //GL.glTranslated(1, 2, 1.5);
            //GL.glRotated(90, 1, 0, 0);
            //GL.glColor3d(0, 1, 1);
            //GL.glRotated(intOptionB, 1, 0, 0);
            //GLUT.glutSolidTeapot(1);
            //GL.glRotated(-intOptionB, 1, 0, 0); //not neccessary
            //GL.glRotated(-90, 1, 0, 0);     //not neccessary
            //GL.glTranslated(-1, -2, -1.5);  //not neccessary 

            // GL.glRotated(intOptionB, 0, 0, 1); //rotate both not neccessary
            // GL.glRotated(coinyRot, 0, 0, 1);

            //GL.glColor3f(1, 1, 1);
            //GL.glTranslatef(0, 2, 0);
            //GL.glRotatef(1, 0, 1, 0);
            //GLU.gluDisk(obj, 0, 2, 30, 30);
            //GL.glRotated(-1, 0, 1, 0); 
            //GL.glTranslatef(0, -2 , 0);    
            //GL.glColor3f(1, 1, 1);  

            // GL.glPopMatrix();

            //GL.glPushMatrix();
            //GL.glRotated(coinyRot, 0, 0, 1); //rotate both

            //GL.glColor3f(1.0f, 1.0f, 1.0f);
            //GL.glTranslatef(0.0f, 0.0f, 1.0f);
            //GL.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            //GLUT.gluSphere(obj, 2.0, 50, 50);
            ////  GL.glRotatef(-90.0f, 1.0f, 0.0f, 0.0f);
            //GL.glRotatef(-90.0f, 1.0f, 0.0f, 0.0f);
            //GL.glTranslatef(0.0f, 0.0f, -1.0f);
            //GLUT.gluCylinder(obj, 1.0, 1.0, 4.0, 50, 50);


        }

        public float[] pos = new float[4];
        public int intOptionB = 1;
        Random rand = new Random();

        public float[] ScrollValue = new float[14];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;
        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        //public int intOptionM = 0;
        public Boolean intOptionMi = false;
        double[] AccumulatedRotationsTraslations = new double[16];

        

        public void Draw()
        {

            //Shadows
            pos[0] = ScrollValue[10];
            pos[1] = ScrollValue[11];
            pos[2] = ScrollValue[12];
            pos[3] = 1.0f;

            /***/
            ground[0, 2] = ground[1, 2] = ground[2, 2] = ScrollValue[9];
            /***/

            //Shadows

            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //                                                           see InitializeGL also
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

            GL.glLoadIdentity();

            // not trivial
            double[] ModelVievMatrixBeforeSpecificTransforms = new double[16];
            double[] CurrentRotationTraslation = new double[16];

            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2],
                       ScrollValue[3], ScrollValue[4], ScrollValue[5],
                       ScrollValue[6], ScrollValue[7], ScrollValue[8]);
            GL.glTranslatef(0.0f, 0.0f, -1.0f);


            //save current ModelView Matrix values
            //in ModelVievMatrixBeforeSpecificTransforms array
            //ModelView Matrix ========>>>>>> ModelVievMatrixBeforeSpecificTransforms
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            //ModelView Matrix was saved, so
            GL.glLoadIdentity(); // make it identity matrix

            //make transformation in accordance to KeyCode
            float delta;
            if (intOptionC != 0)
            {
                delta = 5.0f * Math.Abs(intOptionC) / intOptionC; // signed 5

                switch (Math.Abs(intOptionC))
                {
                    case 1:
                        GL.glRotatef(delta, 1, 0, 0);
                        break;
                    case 2:
                        GL.glRotatef(delta, 0, 1, 0);
                        break;
                    case 3:
                        GL.glRotatef(delta, 0, 0, 1);
                        break;
                    case 4:
                        GL.glTranslatef(delta / 20, 0, 0);
                        break;
                    case 5:
                        GL.glTranslatef(0, delta / 20, 0);
                        break;
                    case 6:
                        GL.glTranslatef(0, 0, delta / 20);
                        break;
                }
            }


            //as result - the ModelView Matrix now is pure representation
            //of KeyCode transform and only it !!!

            //save current ModelView Matrix values
            //in CurrentRotationTraslation array
            //ModelView Matrix =======>>>>>>> CurrentRotationTraslation
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, CurrentRotationTraslation);

            //The GL.glLoadMatrix function replaces the current matrix with
            //the one specified in its argument.
            //The current matrix is the
            //projection matrix, modelview matrix, or texture matrix,
            //determined by the current matrix mode (now is ModelView mode)
            GL.glLoadMatrixd(AccumulatedRotationsTraslations); //Global Matrix

            //The GL.glMultMatrix function multiplies the current matrix by
            //the one specified in its argument.
            //That is, if M is the current matrix and T is the matrix passed to
            //GL.glMultMatrix, then M is replaced with M • T
            GL.glMultMatrixd(CurrentRotationTraslation);

            //save the matrix product in AccumulatedRotationsTraslations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            //replace ModelViev Matrix with stored ModelVievMatrixBeforeSpecificTransforms
            GL.glLoadMatrixd(ModelVievMatrixBeforeSpecificTransforms);
            //multiply it by KeyCode defined AccumulatedRotationsTraslations matrix
            GL.glMultMatrixd(AccumulatedRotationsTraslations);

            //REFLECTION//DrawAxes();

            //REFLECTION b    	
            intOptionB += 10; //for rotation
            intOptionC += 2; //for rotation

            // without REFLECTION was only DrawAll(); 
            // now

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);


            //only floor, draw only to STENCIL buffer
            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF); // draw floor always
            GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
            GL.glDisable(GL.GL_DEPTH_TEST);

            if (isReflection)
            {
                DrawFloor();
            }


            // DrawFloor();

            // restore regular settings
            GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
            GL.glEnable(GL.GL_DEPTH_TEST);

            // reflection is drawn only where STENCIL buffer value equal to 1
            GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            GL.glEnable(GL.GL_STENCIL_TEST);

            // draw reflected scene
            GL.glPushMatrix();
            GL.glScalef(1, 1, -1); //swap on Z axis
            GL.glEnable(GL.GL_CULL_FACE);
            GL.glCullFace(GL.GL_BACK);//back and forth
                                      // DrawFigures();
            DrawStars();
            //GL.glEnable(GL.GL_TEXTURE_2D);

            //switch (intOptionP)
            //{
            //    case 1:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            //        break;
            //    case 2:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            //        break;
            //    case 3:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            //        break;
            //    case 4:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            //        break;
            //    case 5:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            //        break;
            //    case 6:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            //        break;
            //    case 7:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
            //        break;
            //    case 8:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
            //        break;
            //    case 9:
            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
            //        break;

            //}

            // GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            //GLU.gluQuadricTexture(obj, 1);

            //DrawFigures();
            /*********/
            //DrawMoon();
            /*********/
            DrawFigures_2();

            // GLU.gluSphere(obj, 2, 32, 32);

            GL.glDisable(GL.GL_TEXTURE_2D);
            // DrawMoon();
            GL.glCullFace(GL.GL_FRONT);//back and forth
                                       //DrawFigures();
            DrawStars();
            GL.glEnable(GL.GL_TEXTURE_2D);

            //GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            switch (intOptionP)
            {
                case 1:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
                    break;
                case 2:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
                    break;
                case 3:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
                    break;
                case 4:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
                    break;
                case 5:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
                    break;
                case 6:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
                    break;
                case 7:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
                    break;
                case 8:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
                    break;
                case 9:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
                    break;

            }

            GLU.gluQuadricTexture(obj, 1);

            //DrawFigures();
            /*********/
            //DrawMoon();
            /*********/
            DrawFigures_2();

            // GLU.gluSphere(obj, 2, 32, 32);

            GL.glDisable(GL.GL_TEXTURE_2D);
            //  DrawMoon();
            GL.glDisable(GL.GL_CULL_FACE);
            GL.glPopMatrix();


            // really draw floor 
            //( half-transparent ( see its color's alpha byte)))
            // in order to see reflected objects 
            GL.glDepthMask((byte)GL.GL_FALSE);

            DrawFloor();

            GL.glDepthMask((byte)GL.GL_TRUE);
            // Disable GL.GL_STENCIL_TEST to show All, else it will be cut on GL.GL_STENCIL
            GL.glDisable(GL.GL_STENCIL_TEST);

            /****/
            GL.glEnable(GL.GL_TEXTURE_2D);

            //GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            switch (intOptionP)
            {
                case 1:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
                    break;
                case 2:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
                    break;
                case 3:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
                    break;
                case 4:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
                    break;
                case 5:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
                    break;
                case 6:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
                    break;
                case 7:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
                    break;
                case 8:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
                    break;
                case 9:
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
                    break;

            }

            GLU.gluQuadricTexture(obj, 1);

            //DrawFigures();
            /*********/
            //DrawMoon();
            /*********/
            DrawFigures_2();

            // GLU.gluSphere(obj, 2, 32, 32);

            GL.glDisable(GL.GL_TEXTURE_2D);

            DrawStars();
            //DrawFigures();
            //REFLECTION e    

            //DrawAxes();
            GL.glFlush();

            WGL.wglSwapBuffers(m_uint_DC);

            //switch (intOptionM)
            //{
            //    case 1:
            //        GL.glEnable(GL.GL_BLEND);
            //        GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);


            //        //only floor, draw only to STENCIL buffer
            //        GL.glEnable(GL.GL_STENCIL_TEST);
            //        GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            //        GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF); // draw floor always
            //        GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
            //        GL.glDisable(GL.GL_DEPTH_TEST);

            //        if (isReflection)
            //        {
            //            DrawFloor();
            //        }


            //        // DrawFloor();

            //        // restore regular settings
            //        GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
            //        GL.glEnable(GL.GL_DEPTH_TEST);

            //        // reflection is drawn only where STENCIL buffer value equal to 1
            //        GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
            //        GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            //        GL.glEnable(GL.GL_STENCIL_TEST);

            //        // draw reflected scene
            //        GL.glPushMatrix();
            //        GL.glScalef(1, 1, -1); //swap on Z axis
            //        GL.glEnable(GL.GL_CULL_FACE);
            //        GL.glCullFace(GL.GL_BACK);//back and forth
            //       // DrawFigures();
            //        DrawStars();
            //        //GL.glEnable(GL.GL_TEXTURE_2D);

            //        //switch (intOptionP)
            //        //{
            //        //    case 1:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            //        //        break;
            //        //    case 2:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            //        //        break;
            //        //    case 3:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            //        //        break;
            //        //    case 4:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            //        //        break;
            //        //    case 5:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            //        //        break;
            //        //    case 6:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            //        //        break;
            //        //    case 7:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
            //        //        break;
            //        //    case 8:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
            //        //        break;
            //        //    case 9:
            //        //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
            //        //        break;

            //        //}

            //        // GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            //        //GLU.gluQuadricTexture(obj, 1);

            //        //DrawFigures();
            //        /*********/
            //        //DrawMoon();
            //        /*********/
            //        DrawFigures_2();

            //        // GLU.gluSphere(obj, 2, 32, 32);

            //        GL.glDisable(GL.GL_TEXTURE_2D);
            //        // DrawMoon();
            //        GL.glCullFace(GL.GL_FRONT);//back and forth
            //        //DrawFigures();
            //        DrawStars();
            //        GL.glEnable(GL.GL_TEXTURE_2D);

            //        //GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            //        switch (intOptionP)
            //        {
            //            case 1:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            //                break;
            //            case 2:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            //                break;
            //            case 3:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            //                break;
            //            case 4:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            //                break;
            //            case 5:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            //                break;
            //            case 6:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            //                break;
            //            case 7:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
            //                break;
            //            case 8:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
            //                break;
            //            case 9:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
            //                break;

            //        }

            //        GLU.gluQuadricTexture(obj, 1);

            //        //DrawFigures();
            //        /*********/
            //        //DrawMoon();
            //        /*********/
            //        DrawFigures_2();

            //        // GLU.gluSphere(obj, 2, 32, 32);

            //        GL.glDisable(GL.GL_TEXTURE_2D);
            //        //  DrawMoon();
            //        GL.glDisable(GL.GL_CULL_FACE);
            //        GL.glPopMatrix();


            //        // really draw floor 
            //        //( half-transparent ( see its color's alpha byte)))
            //        // in order to see reflected objects 
            //        GL.glDepthMask((byte)GL.GL_FALSE);

            //        DrawFloor();

            //        GL.glDepthMask((byte)GL.GL_TRUE);
            //        // Disable GL.GL_STENCIL_TEST to show All, else it will be cut on GL.GL_STENCIL
            //        GL.glDisable(GL.GL_STENCIL_TEST);

            //        /****/
            //        GL.glEnable(GL.GL_TEXTURE_2D);

            //        //GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            //        switch (intOptionP)
            //        {
            //            case 1:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            //                break;
            //            case 2:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            //                break;
            //            case 3:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            //                break;
            //            case 4:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            //                break;
            //            case 5:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            //                break;
            //            case 6:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            //                break;
            //            case 7:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
            //                break;
            //            case 8:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
            //                break;
            //            case 9:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
            //                break;

            //        }

            //        GLU.gluQuadricTexture(obj, 1);

            //        //DrawFigures();
            //        /*********/
            //        //DrawMoon();
            //        /*********/
            //        DrawFigures_2();

            //        // GLU.gluSphere(obj, 2, 32, 32);

            //        GL.glDisable(GL.GL_TEXTURE_2D);

            //        DrawStars();
            //        //DrawFigures();
            //        //REFLECTION e    

            //        //DrawAxes();
            //        GL.glFlush();

            //        WGL.wglSwapBuffers(m_uint_DC);
            //        break;
            //    case 2:
            //        GL.glEnable(GL.GL_BLEND);
            //        GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);


            //        //only floor, draw only to STENCIL buffer
            //        GL.glEnable(GL.GL_STENCIL_TEST);
            //        GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            //        GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF); // draw floor always
            //        GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
            //        GL.glDisable(GL.GL_DEPTH_TEST);

            //        //DrawFloor();


            //        // DrawFloor();

            //        // restore regular settings
            //        GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
            //        GL.glEnable(GL.GL_DEPTH_TEST);

            //        // reflection is drawn only where STENCIL buffer value equal to 1
            //        GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
            //        GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            //        GL.glEnable(GL.GL_STENCIL_TEST);

            //        // draw reflected scene
            //        GL.glPushMatrix();
            //        GL.glScalef(1, 1, -1); //swap on Z axis
            //        GL.glEnable(GL.GL_CULL_FACE);
            //        GL.glCullFace(GL.GL_BACK);//back and forth
            //        //DrawFigures();
            //        DrawStars();
            //        GL.glEnable(GL.GL_TEXTURE_2D);

            //        GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            //        GLU.gluQuadricTexture(obj, 1);

            //        //DrawFigures();
            //        /*********/
            //        //DrawMoon();
            //        /*********/
            //        DrawFigures_2();

            //        // GLU.gluSphere(obj, 2, 32, 32);

            //        GL.glDisable(GL.GL_TEXTURE_2D);
            //        // DrawMoon();
            //        GL.glCullFace(GL.GL_FRONT);//back and forth
            //       // DrawFigures();
            //        DrawStars();
            //        GL.glEnable(GL.GL_TEXTURE_2D);

            //        //GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            //        switch (intOptionP)
            //        {
            //            case 1:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            //                break;
            //            case 2:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            //                break;
            //            case 3:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            //                break;
            //            case 4:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            //                break;
            //            case 5:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            //                break;
            //            case 6:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            //                break;
            //            case 7:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
            //                break;
            //            case 8:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
            //                break;
            //            case 9:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
            //                break;

            //        }

            //        GLU.gluQuadricTexture(obj, 1);

            //        //DrawFigures();
            //        /*********/
            //        //DrawMoon();
            //        /*********/
            //        DrawFigures_2();

            //        // GLU.gluSphere(obj, 2, 32, 32);

            //        GL.glDisable(GL.GL_TEXTURE_2D);
            //        //  DrawMoon();
            //        GL.glDisable(GL.GL_CULL_FACE);
            //        GL.glPopMatrix();


            //        // really draw floor 
            //        //( half-transparent ( see its color's alpha byte)))
            //        // in order to see reflected objects 
            //        GL.glDepthMask((byte)GL.GL_FALSE);

            //        //DrawFloor();

            //        GL.glDepthMask((byte)GL.GL_TRUE);
            //        // Disable GL.GL_STENCIL_TEST to show All, else it will be cut on GL.GL_STENCIL
            //        GL.glDisable(GL.GL_STENCIL_TEST);

            //        /****/
            //        GL.glEnable(GL.GL_TEXTURE_2D);

            //        //GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            //        switch (intOptionP)
            //        {
            //            case 1:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            //                break;
            //            case 2:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            //                break;
            //            case 3:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            //                break;
            //            case 4:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            //                break;
            //            case 5:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            //                break;
            //            case 6:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            //                break;
            //            case 7:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
            //                break;
            //            case 8:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
            //                break;
            //            case 9:
            //                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[8]);
            //                break;

            //        }

            //        GLU.gluQuadricTexture(obj, 1);

            //        //DrawFigures();
            //        /*********/
            //        //DrawMoon();
            //        /*********/
            //        DrawFigures_2();

            //        // GLU.gluSphere(obj, 2, 32, 32);

            //        GL.glDisable(GL.GL_TEXTURE_2D);

            //        DrawStars();
            //        //DrawFigures();
            //        //REFLECTION e    

            //        //DrawAxes();
            //        GL.glFlush();

            //        WGL.wglSwapBuffers(m_uint_DC);
            //        break;
            //}

        }

        protected virtual void InitializeGL()
        {
            m_uint_HWND = (uint)p.Handle.ToInt32();
            m_uint_DC = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(WGL.PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)(WGL.PFD_MAIN_PLANE);

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //for Stencil support 

            pfd.cStencilBits = 32;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            int pixelFormatIndex = 0;
            pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }
            //Create rendering context
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }


            initRenderingGL();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            Draw();
        }

        protected virtual void initRenderingGL()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;

            GL.glShadeModel(GL.GL_SMOOTH);
            GL.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            GL.glClearDepth(1.0f);


            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);

            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);
            GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_Hint, GL.GL_NICEST);


            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            //nice 3D
            GLU.gluPerspective(45.0, 1.0, 0.4, 100.0);


            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            GenerateTextures();


            //save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);



        }
        //! TEXTURE b
        public uint[] Textures = new uint[9];
       
        void GenerateTextures()
        {
            
            GL.glGenTextures(9, Textures);

            string[] imagesName = { "earth.bmp", "moon.bmp" ,"sun.bmp" , "mercury.bmp", "venus.bmp" 
                    , "mars.bmp" , "jupiter.bmp" , "uranus.bmp" , "saturn.bmp"};

            for (int i = 0; i <  9 ; i++)
            {
                Bitmap image = new Bitmap(imagesName[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
                System.Drawing.Imaging.BitmapData bitmapdata;
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

                bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                //2D for XYZ
                //The level - of - detail number.Level 0 is the base image level
                //The number of color components in the texture.
                //Must be 1, 2, 3, or 4, or one of the following
                //    symbolic constants: 
                //                GL_ALPHA, GL_ALPHA4,
                //                GL_ALPHA8, GL_ALPHA12, GL_ALPHA16, GL_LUMINANCE, GL_LUMINANCE4,
                //                GL_LUMINANCE8, GL_LUMINANCE12, GL_LUMINANCE16, GL_LUMINANCE_ALPHA,
                //                GL_LUMINANCE4_ALPHA4, GL_LUMINANCE6_ALPHA2, GL_LUMINANCE8_ALPHA8,
                //                GL_LUMINANCE12_ALPHA4, GL_LUMINANCE12_ALPHA12, GL_LUMINANCE16_ALPHA16,
                //                GL_INTENSITY, GL_INTENSITY4, GL_INTENSITY8, GL_INTENSITY12,
                //                GL_INTENSITY16, GL_R3_G3_B2, GL_RGB, GL_RGB4, GL_RGB5, GL_RGB8,
                //                GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1,
                //                GL_RGBA8, GL_RGB10_A2, GL_RGBA12, or GL_RGBA16.


                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                                          //The width of the border.Must be either 0 or 1.
                                                          //The format of the pixel data
                                                          //The data type of the pixel data
                                                          //A pointer to the image data in memory
                                                          0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }
    }
}
