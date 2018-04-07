using aplimat_core.utilities;
using aplimat_final_exam.Models;
using aplimat_final_exam.Utilities;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aplimat_final_exam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Vector3 mousePos = new Vector3();

        #region Initialization
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;

            gl.Enable(OpenGL.GL_DEPTH_TEST);

            float[] global_ambient = new float[] { 0.5f, 0.5f, 0.5f, 1.0f };
            float[] light0pos = new float[] { 0.0f, 5.0f, 10.0f, 1.0f };
            float[] light0ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] light0diffuse = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] light0specular = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };

            float[] lmodel_ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);

            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light0specular);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_LIGHT0);

            gl.ShadeModel(OpenGL.GL_SMOOTH);
        }

        #endregion

        #region Mouse Func
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            mousePos.x = (float)position.X - (float)Width / 2.0f;
            mousePos.y = -((float)position.Y - (float)Height / 2.0f);
        }
        #endregion


        private void ManageKeyPress()
        {

        }

        private List<CubeMesh> myRocks = new List<CubeMesh>();
        private CubeMesh Rock = new CubeMesh(-75.0f, -8.0f, 0);
        private CubeMesh myPerson = new CubeMesh(-75.0f,-10,0);
        private CubeMesh Platform = new CubeMesh(-76.0f, -15.0f, 0);
        private Randomizer randMass = new Randomizer(1, 2);
        private int counter = 0;
        private float speed = 0.1f;
        private float bulletSpeed = 3.0f;


        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            this.Title = "APLIMAT Final Exam";
            OpenGL gl = args.OpenGL;

            // Clear The Screen And The Depth Buffer
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            // Move Left And Into The Screen
            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, -100.0f);

            counter++;

            gl.Color(1.0, 1.0, 0.0);
            myPerson.Draw(gl);
            myPerson.Scale = new Vector3(3.0f, 5.0f, 0);

            gl.Color((byte)153,(byte) 76,(byte) 0);
            Platform.Draw(gl);
            Platform.Scale = new Vector3(5.0f, 2.0f, 0);

            gl.Color((byte)192, (byte)192, (byte)192);
            Rock.Mass = (float)Gaussian.Generate(randMass.Generate(), randMass.Generate());
            //Rock.Scale = new Vector3(Rock.Mass, Rock.Mass, 0);
            Rock.Draw(gl);
            myRocks.Add(Rock);

            if (counter % 10 == 0)
            {
                Rock.ApplyForce(Vector3.Right * 0.01f);
                Rock.ApplyForce(Vector3.Up * 0.01f);
                Rock.ApplyGravity();
            }


            //gl.Color(1.0, 1.0, 1.0);
            //bullet.Draw(gl);

            //if (Keyboard.IsKeyDown(Key.Space))
            //{
            //    Rock.Draw(gl);
            //}

            //if (Keyboard.IsKeyDown(Key.D))
            //{
            //    myCube.ApplyForce(Vector3.Right * speed);
            //}

            //if (Keyboard.IsKeyDown(Key.A))
            //{
            //    myCube.ApplyForce(Vector3.Left * speed);
            //}
            //if (Keyboard.IsKeyDown(Key.S))
            //{
            //    myCube.ApplyForce(Vector3.Down * speed);
            //}

            //if (Mouse.LeftButton == MouseButtonState.Pressed)
            //{

            //}

            //myCube.ApplyGravity();

            //if (myCube.Position.y <= -50)
            //{
            //    myCube.Position.y = -50;
            //    myc
            //}
        }
    }
}
