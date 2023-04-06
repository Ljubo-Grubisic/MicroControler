using MicroController.Game.Entities;
using MicroController.InputOutput;
using MicroController.InputOutput.PortComunication;
using MicroController.Game.RayCasting;
using MicroController.MainLooping;
using MicroController.Mathematics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using MicroController.GUI;
using MicroController.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MicroController.Game.Entities.Sensors;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Runtime;
using System.Threading.Tasks;
using System.Management.Instrumentation;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace MicroController.Game
{
    public class Game : MainLoop
    {
        private uint WindowWidth = 1080;
        private uint WindowHeight = 720;
        private string WindowTitle;
        public static Color WindowFillColor = new Color(MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f));

        public RayCaster rayCaster;
        
        public Map map;
        public Camera camera;

        private SerialPort SerialPort;
        private Serial Serial;

        private Text PortText;
        private TextBox SelectTextBox;
        private Button SelectButton;

        private Button StartButton;
        private Button StopButton;

        private Button OnButton;
        private Button OffButton;

        private Text InBluetoothText;
        private Button ButtonRead;

        private DropBox DropBox;

        private int WindowState = 0;
        public bool IsGamePaused = false;

        public Game(uint windowWidth, uint windowHeight, string title) : base(windowWidth, windowHeight, title, WindowFillColor)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowTitle = title;
        }

        protected override void LoadContent()
        {
            MessegeManager.LoadContent();
            Window.Resized += Window_Resized;
        }

        protected override void Initialize()
        {
            map = new Map(Map.GenerateMapWithWallRandom(1000, 1000), 20, new Window
            {
                Position = new Vector2i(50, 50),
                Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100)
            }, this);

            rayCaster = new RayCaster(fov: 60, angleSpacingRay: 0.5f, depthOffFeild: 100, windowPosition: new Vector2i(0, 0),
                windowSize: new Vector2i((int)WindowWidth, (int)WindowHeight), rayMapColor: Color.Red, horizontalColor: new Color(150, 0, 0),
                verticalColor: new Color(255, 10, 10), drawMapRays: false);
            camera = new Camera(new Vector2f(100f, 100f), this);

            PauseMenu.Init(this);
            Scale.Create(1, map.SquareSize);

            SerialPort = new SerialPort();

            PortText = new Text("PORT", MessegeManager.Arial, 15) { Position = new Vector2f(10, 25), Color = Color.Black };
            SelectTextBox = new TextBox(new Vector2f(60, 25), new Vector2f(200, 50), MessegeManager.Arial, 15);
            SelectButton = new Button(new Vector2f(285, 25), new Vector2f(100, 50), "SELECT");

            StartButton = new Button(new Vector2f(10, 90), new Vector2f(150, 50), "START");
            StopButton = new Button(new Vector2f(175, 90), new Vector2f(150, 50), "STOP");

            OnButton = new Button(new Vector2f(10, 155), new Vector2f(150, 50), "ON");
            OffButton = new Button(new Vector2f(175, 155), new Vector2f(150, 50), "OFF");

            InBluetoothText = new Text("OUTTEXT: ", MessegeManager.Arial, 15) { Position = new Vector2f(10, 285), Color = Color.Black };
            ButtonRead = new Button(new Vector2f(10, 220), new Vector2f(200, 50), "READ");

            DropBox = new DropBox(new Vector2f(400, 20), new Vector2f(200, 50), new List<string>() { "Test1", "Test2", "Test3" });
            

            SelectButton.ButtonClicked += SelectButton_ButtonClicked;

            StartButton.ButtonClicked += StartButton_ButtonClicked;
            StopButton.ButtonClicked += StopButton_ButtonClicked;

            OnButton.ButtonClicked += OnButton_ButtonClicked;
            OffButton.ButtonClicked += OffButton_ButtonClicked;

            ButtonRead.ButtonClicked += ButtonRead_ButtonClicked;
        }

        private void ButtonRead_ButtonClicked(object source, EventArgs args)
        {
            try
            {
                byte[] buffer = new byte[10];
                string text = "";
                SerialPort.Read(buffer, 0, 10);

                for (int i = 0; i < buffer.Length; i++)
                {
                    text += char.ConvertFromUtf32(buffer[i]);
                }

                InBluetoothText.DisplayedString += text;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OffButton_ButtonClicked(object source, EventArgs args)
        {
            if (SerialPort.IsOpen)
            {
                SerialPort.WriteLine("OFF");
            }
        }

        private void OnButton_ButtonClicked(object source, EventArgs args)
        {
            if (SerialPort.IsOpen)
            {
                SerialPort.WriteLine("ON");
            }
        }

        private void StopButton_ButtonClicked(object source, EventArgs args)
        {
            SerialPort.Close();
        }

        private void StartButton_ButtonClicked(object source, EventArgs args)
        {
            try
            {
                if (!SerialPort.IsOpen)
                    SerialPort.Open();
            }
            catch
            {
                Console.WriteLine("FAILED TO OPEN PORT");
            }
        }

        private void SelectButton_ButtonClicked(object source, EventArgs args)
        {
            try
            {
                SerialPort.PortName = SelectTextBox.DisplayedString;
                SerialPort.BaudRate = 9600;
                SerialPort.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsGamePaused)
            {
                camera.Update(gameTime, map);
                map.Window.Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100);
                map.Update(camera, GameTime);
                OpenCloseMap();
                Scale.Update(map);
            }
            else
            {
                PauseMenu.Update(this);
            }
            PauseMenu.OpenClosePauseMenu(this);

            //SelectTextBox.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet);
            //SelectButton.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            //StartButton.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            //StopButton.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            //OnButton.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            //OffButton.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            //ButtonRead.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            //DropBox.Update(Mouse.GetPosition() - Window.Position - MouseManager.MouseOffSet);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!PauseMenu.IsSettingsOpen)
            {
                switch (WindowState)
                {
                    case 0:
                        rayCaster.Draw(this.Window, ref this.map, camera);
                        break;
                    case 1:
                        map.DrawMap(this.Window);
                        rayCaster.Draw(this.Window, ref this.map, camera);
                        camera.Draw(this.Window);
                        break;
                }
            }
            if (IsGamePaused)
            {
                PauseMenu.Draw(this.Window);
            }

            //Window.Draw(PortText);
            //SelectTextBox.Draw(Window);
            //SelectButton.Draw(Window);

            //StartButton.Draw(Window);
            //StopButton.Draw(Window);

            //OnButton.Draw(Window);
            //OffButton.Draw(Window);

            //Window.Draw(InBluetoothText);
            //ButtonRead.Draw(Window);

            //DropBox.Draw(Window);

            string[] ports = SerialPort.GetPortNames();

            for (int i = 0; i < ports.Length; i++)
            {
                //MessegeManager.Message(this.Window, ports[i], new Vector2f(Window.Size.X - 40, 5 + 25f * i));
            }

            //MessegeManager.DrawPerformanceData(this, Color.Red);
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            if(Window.Size.X < 765)
            {
                Window.Size = new Vector2u(765, Window.Size.Y);
            }
            else if(Window.Size.Y < 308)
            {
                Window.Size = new Vector2u(Window.Size.X, 308);
            }
            else
            {
                WindowHeight = Window.Size.Y;
                WindowWidth = Window.Size.X;
                View view = new View(new FloatRect(0, 0, WindowWidth, WindowHeight));
                rayCaster.WindowSize = new Vector2i((int)WindowWidth, (int)WindowHeight);
                Window.SetView(view);
            }
        }

        private void OpenCloseMap()
        {
            if (KeyboardManager.OnKeyPress(Keyboard.Key.M, 0))
            {
                if (WindowState == 0)
                {
                    WindowState++;
                    rayCaster.DrawMapRays = true;
                    rayCaster.Draw3D = false;
                }
                else if (WindowState == 1)
                {
                    WindowState--;
                    rayCaster.DrawMapRays = false;
                    rayCaster.Draw3D = true;
                }
            }
        }
    }
}
