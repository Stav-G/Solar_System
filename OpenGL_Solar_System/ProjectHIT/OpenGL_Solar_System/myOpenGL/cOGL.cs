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

        public int moviendo;
     
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
            isLight = false;
            isFloor = false;
            isOrbit = false;

            angelOrbitMercury = r.Next(360);
            speedOrbitMercury = (float)r.NextDouble() * 0.3f;
            angelOrbitVenus = r.Next(360);
            speedOrbitVenus = (float)r.NextDouble() * 0.3f;
            angelOrbitEarth = r.Next(360);
            speedOrbitEarth = (float)r.NextDouble() * 0.3f;
            angelOrbitMars = r.Next(360);
            speedOrbitMars = (float)r.NextDouble() * 0.3f;
            angelOrbitJupiter = r.Next(360);
            speedOrbitJupiter = (float)r.NextDouble() * 0.3f;
            angelOrbitSaturn = r.Next(360);
            speedOrbitSaturn = (float)r.NextDouble() * 0.3f;
            angelOrbitUranus = r.Next(360);
            speedOrbitUranus = (float)r.NextDouble() * 0.3f;
            angelOrbitNeptun = r.Next(360);
            speedOrbitNeptun = (float)r.NextDouble() * 0.3f;
            angelOrbitPluton = r.Next(360);
            speedOrbitPluton = (float)r.NextDouble() * 0.3f;
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
            GL.glVertex3f(-10.0f, 0.0f, 0.0f);
            GL.glVertex3f(10.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -10.0f, 0.0f);
            GL.glVertex3f(0.0f, 10.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -10.0f);
            GL.glVertex3f(0.0f, 0.0f, 10.0f);
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
        public bool isLight = false;
        public bool isFloor = false;
        public bool isOrbit = false;
        public float rotacion = 0;
        public float rotacionP = 0;
        public float objy = 0.0f;


        /**************************************/
      //  #region Camera constants

        const double div1 = Math.PI / 180;
        const double div2 = 180 / Math.PI;

      //  #endregion

     //   #region Private atributes

        static float eyex, eyey, eyez;
        static float centerx, centery, centerz;
        static float forwardSpeed = 0.3f;
        static float yaw, pitch;
        static float rotationSpeed = 0.25f;
        static double i, j, k;


        // #endregion
        /**************************************/


        public void Update(int pressedButton)
        {
            // #region Aim camera
            Point position = new Point();
            //  Cursor.Position
            //Winapi.GetCursorPos(ref position);
            // Cursor.Position = 

            int difX = panelPosX + 512 - position.X;
            int difY = panelPosY + 384 - position.Y;

            if (position.Y < 384)
            {
                pitch -= rotationSpeed * difY;
            }
            else
                if (position.Y > 384)
            {
                pitch += rotationSpeed * -difY;
            }
            if (position.X < 512)
            {
                yaw += rotationSpeed * -difX;
            }
            else
                if (position.X > 512)
            {
                yaw -= rotationSpeed * difX;
            }
            UpdateDirVector();
            CenterMouse();


            if (pressedButton == 1) //the left mouse button was pressed
            {
                eyex -= (float)i * forwardSpeed;
                eyey -= (float)j * forwardSpeed;
                eyez -= (float)k * forwardSpeed;
            }
            else
                if (pressedButton == -1) // the right mouse button was pressed
            {
                eyex += (float)i * forwardSpeed;
                eyey += (float)j * forwardSpeed;
                eyez += (float)k * forwardSpeed;
            }

            // #endregion

            Look();
        }

        static public float AnguloARadian(double pAngle)
        {
            return (float)(pAngle * div1);
        }

        static public float RadianAAngulo(double pAngle)
        {
            return (float)(pAngle * div2);
        }
        public void UpdateDirVector()
        {
            k = Math.Cos(AnguloARadian((double)yaw)); //z-axis
            i = -Math.Sin(AnguloARadian((double)yaw)); // x-axis
            j = Math.Sin(AnguloARadian((double)pitch)); // y-axis     

            centerz = eyez - (float)k; // Calculate where the camera is looking
            centerx = eyex - (float)i;
            centery = eyey - (float)j;
        }

        //public static void CenterMouse()
        //{
        //    int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        //    int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        //    Cursor.Position = new Point(screenWidth / 2, screenHeight / 2);
        //}

        public void CenterMouse()
        {

            this.panelPosX = 500;
            this.panelPosY = 500;
            //this.pointX = 512;            
            //this.panelPosX = 384;

            //pointX = panelPosX + 512;
            //pointY = panelPosY + 384;

        }

        public static float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }

        public static float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }

        public void InitCamara()
        {
            eyex = 0f;
            eyey = 2f;
            eyez = 5f;
            centerx = 0;
            centery = 2;
            centerz = 0;
            Look();
        }

        public void Look()
        {
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
            GLU.gluLookAt(eyex, eyey, eyez, centerx, centery, centerz, 0, 1, 0);
        }

        /**************************************/


        void DrawFigures_2()
        {
            
            int i;
            //!!!!!!!!!!!!!
            GL.glPushMatrix();

          
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
            //GL.glVertex3d(pos[0], pos[1], 0);
            //GL.glVertex3d(pos[0], pos[1], pos[2]);
            GL.glVertex3d(pos[0], pos[1], ground[0, 2] - 0.01);
            GL.glVertex3d(pos[0], pos[1], pos[2]);
            GL.glEnd();

           GL.glTranslatef(pos[0], pos[1], pos[2]);

            //main System draw
            GL.glEnable(GL.GL_LIGHTING);
            

            // DrawPlanet(false);
            DrawEachPlane(false);


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
                //  DrawPlanet(true);
                DrawEachPlane(true);
                //!!!!!!!!!!!!!
                GL.glPopMatrix();
                //!!!!!!!!!!!!!
            }
           
        }
        void DrawEachPlane(Boolean isForShades)
        {

            DrawMyPlanet(2, 3.0f, posXSun, posYSun,posZSun,isForShades);// sun
            DrawMyPlanet(3, 0.5f, posXMercury, posYMercury,posZMercury,isForShades); // mercury
            DrawMyPlanet(4, 0.7f, posXVenus,posYVenus,posZVenus, isForShades);// venus
            DrawMyPlanet(0, 1.0f, posXEarth,posYEarth,posZEarth, isForShades);// earth
            DrawMyPlanet(1, 0.5f, posXMoon,posYMoon,posZMoon ,isForShades);// moon
            DrawMyPlanet(5, 1.0f, posXMars,posYMars, posZMars,isForShades);// mars
            DrawMyPlanet(6, 1.5f, posXJupiter,posYJupiter, posZJupiter,isForShades);// jupiter
            DrawMyPlanet(7, 1.2f, posXUranus,posYUranus, posZUranus,isForShades);// uranus
            DrawMyPlanet(8, 1.2f, posXSaturn,posYSaturn,posZSaturn ,isForShades);// saturn
            DrawMyPlanet(9, 1.2f, posXNeptun,posYNeptun,posZNeptun ,isForShades);// neptune

            GL.glDisable(GL.GL_TEXTURE_2D);  

            //0 = "earth.bmp", 
            //1 = "moon.bmp" ,
            //2 = "sun.bmp" ,
            //3 = "mercury.bmp",
            //4 = "venus.bmp",
            //5 = "mars.bmp" ,
            //6 = "jupiter.bmp" ,
            //7 = "uranus.bmp" ,
            //8 = "saturn.bmp"
            //9 = "neptune.bmp"

        }

        void DrawMyPlanet1(int texture, float radios, float x, Boolean isForShades)
        {

            if (texture == 0) //earth
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glPushMatrix();
               // GL.glTranslatef(15, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitEarth += speedOrbitEarth;
                angelOrbitEarth += 0.6f;
                GL.glRotated(angelOrbitEarth, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-15, 0, 0);
                GL.glRotated(angelOrbitEarth, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if (texture == 1) //moon
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }
                GL.glPushMatrix();
            //    GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                GL.glRotated(angelOrbitEarth, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitEarth, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            } 
            if (texture == 3) //mercury
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }
                GL.glPushMatrix();
             //   GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitMercury += speedOrbitMercury;
                angelOrbitMercury += 0.6f;
                GL.glRotated(angelOrbitMercury, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitMercury, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if (texture == 4) //venus
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glPushMatrix();
               // GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitVenus += speedOrbitVenus;
                angelOrbitVenus += 0.6f;
                GL.glRotated(angelOrbitVenus, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitVenus, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if (texture == 5) //mars
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glPushMatrix();
              //  GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitMars += speedOrbitMars;
                angelOrbitMars += 0.6f;
                GL.glRotated(angelOrbitMars, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitMars, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if (texture == 6) //jupiter
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glPushMatrix();
               // GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitJupiter += speedOrbitJupiter;
                angelOrbitJupiter += 0.6f;
                GL.glRotated(angelOrbitJupiter, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitJupiter, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if (texture == 7) //uranus
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glPushMatrix();
               // GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitUranus += speedOrbitUranus;
                angelOrbitUranus += 0.6f;
                GL.glRotated(angelOrbitUranus, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitUranus, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if (texture == 8) //uranus
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glPushMatrix();
                //GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitSaturn += speedOrbitSaturn;
                angelOrbitSaturn += 0.6f;
                GL.glRotated(angelOrbitSaturn, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitSaturn, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if (texture == 9) //neptune
            {

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glPushMatrix();
              //  GL.glTranslatef(x, 0, 0);
                GL.glRotated(270, 1, 0, 0);
                angelOrbitNeptun += speedOrbitNeptun;
                angelOrbitNeptun += 0.6f;
                GL.glRotated(angelOrbitNeptun, 0, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0, 0);
                GL.glRotated(angelOrbitNeptun, 1, 0, 0);
                GL.glPopMatrix();
                GL.glDisable(GL.GL_TEXTURE_2D);
            }
            if(texture == 2) //sun
            {

                if (isLight)
                {
                    GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);
                    //Draw Light Source
                    GL.glDisable(GL.GL_LIGHTING);
                    //Yellow Light source
                    GL.glColor3f(1, 1, 0);
                }

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glTranslatef(3, 0.0f, 0.0f);
                GL.glRotated(90, 0, 0, 1);
                //   GL.glColor3f(1, 1, 1);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0.0f, 0.0f);
                rotacion += 0.05f;
                GL.glRotatef(rotacion, 0, 1, 0);
                GL.glTranslatef(-x, 0.0f, 0.0f);
            }
        }

        void DrawMyPlanet(int texture, float radios, float posX,float posY,float posZ,  Boolean isForShades)
        {
            if (texture != 2)
            {
               // GL.glPushMatrix();
                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                GL.glTranslatef(posX, posY, posZ);
                GL.glRotated(270, 1, 0, 0);
                
               // GL.glRotated(intOptionB, 1, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-posX, -posY, -posZ);
                // GL.glRotated(intOptionB, 1, 0, 0);

                if (isOrbit)
                {
                    DrawOrbit(posX);
                }
                //
                //GL.glPopMatrix();
            }
            else
            {
             //   GL.glPushMatrix();
                if (isLight)
                {
                    GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);
                    //Draw Light Source
                    GL.glDisable(GL.GL_LIGHTING);
                    //Yellow Light source
                    GL.glColor3f(1, 1, 0);
                }

                if (isForShades == false)
                {
                    GL.glEnable(GL.GL_TEXTURE_2D);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }


                GL.glTranslatef(posX, posY, posZ);
                rotacion += 0.05f;
                GL.glRotatef(rotacion, 0, 0, 1);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-posX, -posY, -posZ);

                //GL.glPopMatrix();
            }
        }


        void DrawMyPlanet22(int texture, float radios, float x , Boolean isForShades , float speedOrbit , float angelOrbit)
        {
           
            if (texture != 2)
            {

                if (isForShades == false)
                {
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                //// GL.glTranslatef(x, 0, 0);
                //GL.glTranslatef(x, 0, 0);
                //GL.glRotated(270, 1, 0, 0);
                //GL.glRotated(intOptionB, 1, 0, 0);
                //GLU.gluSphere(obj, radios, 32, 32);
                //GL.glTranslatef(-x, 0, 0);

                // GL.glTranslatef(x, 0, 0);
                //GL.glPushMatrix();
                GL.glRotated(270, 1, 0, 0);
                GLU.gluSphere(obj, radios, 32, 32);
                //angelOrbit += speedOrbit;
                //angelRotation += 0.6f;
               // GL.glRotatef(intOptionB, 0, 1, 0);
                GL.glRotatef(angelOrbitMercury, 0, 1, 0);
              //  GL.glTranslatef(-posObj.x , -posObj.y , -posObj.z);
                GL.glTranslatef(-x , -0 , -0);
               // GL.glRotatef(intOptionB, 0, 1, 0);
                GL.glRotatef(angelOrbitMercury, 0, 1, 0);
               
                //DrawOrbit(x);

            }
            else
            {
              
                if (isLight)
                {
                    GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);
                    //Draw Light Source
                    GL.glDisable(GL.GL_LIGHTING);
                    //Yellow Light source
                    GL.glColor3f(1, 1, 0);
                }

                if (isForShades == false)
                {
                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[texture]);
                    GLU.gluQuadricTexture(obj, 1);
                    GL.glColor3f(1, 1, 1);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_2D);
                    GL.glColor3f(0.5f, 0.5f, 0.5f);
                }

                //GL.glTranslatef(x, 0, 0);
                //GL.glRotated(90, 1, 0, 0);
                //  rotacion += 0.05f;
                //GL.glTranslatef(x, 0, 0);
                // GL.glTranslatef(x, 0, 0);

                //   GL.glRotatef(rotacion, 0, 0, 1);
                ////   GL.glColor3f(1, 1, 1);
                //   GLU.gluSphere(obj, radios, 32, 32);
                //   GL.glTranslatef(-x, 0.0f, 0.0f);

               
                GL.glRotated(90, 0, 0, 1);
                //   GL.glColor3f(1, 1, 1);
                GLU.gluSphere(obj, radios, 32, 32);
                GL.glTranslatef(-x, 0.0f, 0.0f);
                rotacion += 0.05f;
                GL.glRotatef(rotacion, 0, 1, 0);
              

            }
        }

        void DrawFloor()
        {
            GL.glEnable(GL.GL_LIGHTING);
            GL.glBegin(GL.GL_QUADS);
            //!!! for blended REFLECTION 
            if(isFloor)
            {
                GL.glColor4d(0, 0, 1, 0.5);
            }
            else
            {
                GL.glColor4d(0, 0, 1, 0.1);
            }
            GL.glVertex3d(-15, -15, 0);
            GL.glVertex3d(-15, 15, 0);
            GL.glVertex3d(15, 15, 0);
            GL.glVertex3d(15, -15, 0);
            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);
        }
   
        void DrawStars()
        {
            GL.glPushMatrix();
            for (int i = 0; i < 1500; i++)
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
            GL.glPopMatrix();
        }

        public void DrawOrbit(float x)
        {

            GL.glBegin(GL.GL_LINE_STRIP);

            for (int i = 0; i < 361; i++)
            {
                GL.glVertex3f(x * (float)Math.Sin(i * Math.PI / 180), 0, x * (float)Math.Cos(i * Math.PI / 180));
            }
            GL.glEnd();
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

        public int panelPosX;
        public int panelPosY;

        public float angelRotation;
        public float angelOrbit;

        //speed
        public float speedOrbitMercury;
        public float speedOrbitVenus;
        public float speedOrbitEarth;
        public float speedOrbitMars;
        public float speedOrbitJupiter;
        public float speedOrbitSaturn;
        public float speedOrbitUranus;
        public float speedOrbitNeptun;
        public float speedOrbitPluton;
        //angel
        public float angelOrbitMercury;
        public float angelOrbitVenus;
        public float angelOrbitEarth;
        public float angelOrbitMars;
        public float angelOrbitJupiter;
        public float angelOrbitSaturn;
        public float angelOrbitUranus;
        public float angelOrbitNeptun;
        public float angelOrbitPluton;
       
        static Random r = new Random();
        //3,
        //5,
        //11
        //15
        //15
        //22
        //28
        //35
        //41
        //51
        //X
        public float posXSun = 3;
        public float posXMercury = 5;
        public float posXVenus = 11;
        public float posXEarth = 15;
        public float posXMoon = 15;
        public float posXMars = 22;
        public float posXJupiter = 28;
        public float posXSaturn = 35;
        public float posXUranus = 41;
        public float posXNeptun = 51;
        //public float posXPluton;
        //Y
        public float posYSun = 0;
        public float posYMercury = 5;
        public float posYVenus = 11;
        public float posYEarth = 15;
        public float posYMoon = 15;
        public float posYMars = 22;
        public float posYJupiter = 28;
        public float posYSaturn = 35;
        public float posYUranus = 41;
        public float posYNeptun = 51;
        //  public float posYPluton = 0;
        //Y
        public float posZSun = 0;
        public float posZMercury = 0;
        public float posZVenus = 0;
        public float posZEarth = 0;
        public float posZMoon = 0;
        public float posZMars = 0;
        public float posZJupiter = 0;
        public float posZSaturn = 0;
        public float posZUranus = 0;
        public float posZNeptun = 0;


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
           // intOptionB += 10; //for rotation
            intOptionC += 2; //for rotation
            intOptionB += 1;
            

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
            DrawFigures_2();
            DrawStars();
            GL.glCullFace(GL.GL_FRONT);//back and forth
            DrawFigures_2();
            DrawStars();
            GL.glDisable(GL.GL_CULL_FACE);
            GL.glPopMatrix();


            // really draw floor 
            //( half-transparent ( see its color's alpha byte)))
            // in order to see reflected objects 
            GL.glDepthMask((byte)GL.GL_FALSE);
           // DrawFloor();


            if (isReflection)
            {
                DrawFloor();
            }

            GL.glDepthMask((byte)GL.GL_TRUE);
            // Disable GL.GL_STENCIL_TEST to show All, else it will be cut on GL.GL_STENCIL
            GL.glDisable(GL.GL_STENCIL_TEST);

            DrawFigures_2();
            DrawStars();
            DrawAxes();
            //REFLECTION e    

            GL.glFlush();

            WGL.wglSwapBuffers(m_uint_DC);

           
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
        public uint[] Textures = new uint[10];
       
        void GenerateTextures()
        {
            
            GL.glGenTextures(10, Textures);

            string[] imagesName = { "earth.bmp", "moon.bmp" ,"sun.bmp" , "mercury.bmp", "venus.bmp" 
                    , "mars.bmp" , "jupiter.bmp" , "uranus.bmp" , "saturn.bmp" , "neptune.bmp"};

            for (int i = 0; i <  10 ; i++)
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
