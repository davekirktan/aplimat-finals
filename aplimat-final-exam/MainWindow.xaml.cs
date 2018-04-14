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
        private Randomizer randMass = new Randomizer(1, 2);
        private Randomizer randPos = new Randomizer(-50,50);
        private Liquid Ocean = new Liquid(0, 0, 100, 50, 0.8f);
        private Randomizer colorRNG = new Randomizer(0, 1);
        private Attractor blackHole = new Attractor();

        private int counter = 0;


        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            this.Title = "APLIMAT Final Exam";
            OpenGL gl = args.OpenGL;

            // Clear The Screen And The Depth Buffer
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            // Move Left And Into The Screen
            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, -100.0f);


            Ocean.Draw(gl);
            gl.Color(1.0f, 1.0f, 1.0f);

            //gl.Color((byte)192, (byte)192, (byte)192);
            //Rock.Mass = (float)Gaussian.Generate(randMass.Generate(), randMass.Generate());
            //Rock.Scale = new Vector3(Rock.Mass, Rock.Mass, 0);
            //Rock.ApplyForce(Vector3.arcRight * 0.01f);
            //Rock.ApplyGravity();

            gl.Color(0, 0, 0);
            blackHole.Position.x = 0;
            blackHole.Position.y = -40;
            blackHole.Mass = 5;
            blackHole.Scale = new Vector3(blackHole.Mass, blackHole.Mass, 0);
            blackHole.Draw(gl);


            if (myRocks.Count <= 20)
            {
                for (int x = 0; x < 20; x++)
                {
                    myRocks.Add(new CubeMesh());
                    myRocks[x].Position = new Vector3(randPos.Generate(), randPos.Generate(), 0);
                    myRocks[x].Mass = (float)Gaussian.Generate(randMass.Generate(), randMass.Generate());
                    myRocks[x].Scale = new Vector3(myRocks[x].Mass, myRocks[x].Mass, 0);
                }
            }

            foreach (var rock in myRocks)
            {
                gl.Color((byte)192, (byte)192, (byte)192);
                rock.ApplyGravity();
                rock.Draw(gl);
                if (Ocean.Contains(rock))
                {
                    var dragForce = Ocean.CalculateDragForce(rock);
                    rock.ApplyForce(dragForce);
                    rock.ApplyForce(blackHole.CalculateAttraction(rock));
                }
                if (rock.Position.y <= -40)
                {
                    rock.Position.y = -40;
                    rock.Velocity.y *= -1;
                }
            }

            counter++;

            //for (int x = 0; x < 10; x++)
            //{
            //    myRocks.Add(new CubeMesh());
            //    myRocks[x].Position = new Vector3(-50 - (x * 10), 30, 0);
            //    gl.Color((byte)192, (byte)192, (byte)192);
            //    myRocks[x].Mass = (float)Gaussian.Generate(randMass.Generate(), randMass.Generate());
            //    myRocks[x].Scale = new Vector3(Rock.Mass, Rock.Mass, 0);
            //    myRocks[x].ApplyGravity();
            //    myRocks[x].Draw(gl);
            //    if (Ocean.Contains(myRocks[x]))
            //    {
            //        var dragForce = Ocean.CalculateDragForce(myRocks[x]);
            //        //myRocks[x].ApplyForce(dragForce);
            //        //myRocks[x].ApplyForce(blackHole.CalculateAttraction(myRocks[x]));
            //    }
            //    if (myRocks[x].Position.y <= -40)
            //    {
            //        myRocks[x].Position.y = -40;
            //        myRocks[x].Velocity.y *= -1;
            //    }
            //}



            //if (counter >= 20)
            //{
            //    Rock.ApplyForce(Vector3.Right * 0.01f);
            //    Rock.ApplyForce(Vector3.Up * 0.01f);
            //    Rock.ApplyGravity();
            //    Rock.Draw(gl);
            //    myRocks.Add(Rock);
            //    if (Ocean.Contains(Rock))
            //    {
            //        var dragForce = Ocean.CalculateDragForce(Rock);
            //        Rock.ApplyForce(dragForce);

            //    }
            //}

            //if(counter == 120)
            //{
            //    counter = 0;
            //    Rock.Position = new Vector3(-75.0f, -8.0f, 0);
            //}


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
