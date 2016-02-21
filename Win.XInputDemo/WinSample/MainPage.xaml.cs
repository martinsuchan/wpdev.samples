using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GameController;
using Windows.UI.Xaml.Navigation;

namespace WinSample
{
    public class Game
    {
        public const int Border = 100;
        public const int PlayerSize = 100;
        public const int ShotSpeed = 25;
        public static Size Size;
    }

    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            SizeChanged += MainPage_SizeChanged;
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Game.Size = e.NewSize;
            if (model == null) return;
            model.Controller1.PlayerLeft = Game.Border;
            model.Controller1.PlayerTop = Game.Border;
            model.Controller2.PlayerLeft = Game.Size.Width - (Game.Border + Game.PlayerSize);
            model.Controller2.PlayerTop = Game.Size.Height - (Game.Border + Game.PlayerSize);
            Bg.Width = Game.Size.Width - 2 * Game.Border;
            Bg.Height = Game.Size.Height - 2 * Game.Border;
            Canvas.SetLeft(Bg, Game.Border);
            Canvas.SetTop(Bg, Game.Border);
        }

        private ControllersModel model;
        private Controller controller1;
        private Controller controller2;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // get the instance of the Xbox controller
            controller1 = new Controller(0);
            controller2 = new Controller(1);
            model = new ControllersModel
            {
                Controller1 = new StateModel(),
                Controller2 = new StateModel()
            };

            DataContext = model;

            // start simple game loop here, 33ms = 30FPS
            while (true)
            {
                Loop();
                await Task.Delay(33);
            }
        }

        private void Loop()
        {
            // Update the controller Model with data from native controller
            model.Controller1.Update(controller1.GetState());
            model.Controller2.Update(controller2.GetState());

            // move the shots
            UpdateShot(model.Controller1, model.Controller2);
            UpdateShot(model.Controller2, model.Controller1);
        }

        private void UpdateShot(StateModel m, StateModel m2)
        {
            // calculate collision with shot and player
            if (m.ShotActive && m.PlayerAlive)
            {
                m.ShotLeft = m.ShotLeft + Math.Sin(m.ShotAngle / StateModel.Rad) * Game.ShotSpeed;
                m.ShotTop = m.ShotTop - Math.Cos(m.ShotAngle / StateModel.Rad) * Game.ShotSpeed;

                // is the shot in player position?
                if (Math.Abs(m.ShotLeft - m2.PlayerLeft) < 50 && Math.Abs(m.ShotTop - m2.PlayerTop) < 50)
                {
                    m2.PlayerAlive = false;
                    m.ShotActive = false;
                    return;
                }

                // is the shot near the border?
                if (m.ShotLeft < Game.Border || m.ShotLeft + Game.Border + Game.PlayerSize > Game.Size.Width
                    || m.ShotTop < Game.Border || m.ShotTop + Game.Border + Game.PlayerSize > Game.Size.Height)
                {
                    m.ShotActive = false;
                }
            }
            // restart the game
            if (!m.PlayerAlive && m.Start || m2.Start)
            {
                m.ShotActive = m2.ShotActive = false;
                m.PlayerAlive = m2.PlayerAlive = true;
            }
        }
    }

    public class ControllersModel : ObservableObject
    {
        public StateModel Controller1
        {
            get { return controller1; }
            set { Set(ref controller1, value); }
        }
        private StateModel controller1;

        public StateModel Controller2
        {
            get { return controller2; }
            set { Set(ref controller2, value); }
        }
        private StateModel controller2;
    }

    public class StateModel : ObservableObject
    {
        public const double Rad = 57.2957795;
        public const double ForceThreshold = 10;

        public void Update(State state)
        {
            Connected = state.connected;
            if (!Connected) return;

            // Data for triggers are provied as BYTE value - how much is the trigger pressed
            LeftTrigger = state.LeftTrigger/4;
            RightTrigger = state.RightTrigger/4;
            LeftBumper = state.left_shoulder;
            RightBumper = state.right_shoulder;
            Start = state.start;
            Back = state.back;
            A = state.a;
            B = state.b;
            X = state.x;
            Y = state.y;
            DpadUp = state.dpad_up;
            DpadDown = state.dpad_down;
            DpadLeft = state.dpad_left;
            DpadRight = state.dpad_right;

            // Thumb values are provided as two coordinates, both with values in range (-32768,32767)
            LeftThumb = Math.Atan2(state.LeftThumbX, state.LeftThumbY)*Rad;
            RightThumb = Math.Atan2(state.RightThumbX, state.RightThumbY)*Rad;
            double leftForce = Math.Sqrt(state.LeftThumbX*state.LeftThumbX + state.LeftThumbY*state.LeftThumbY)/512;
            double rightForce = Math.Sqrt(state.RightThumbX*state.RightThumbX + state.RightThumbY*state.RightThumbY)/512;
            // we use custom threshold to detect thumb position larger than X
            LeftThumbForce = leftForce > ForceThreshold ? leftForce : 0;
            RightThumbForce = rightForce > ForceThreshold ? rightForce : 0;

            // move the player only if the vector is larger than move threshold
            if (LeftThumbForce != 0)
            {
                PlayerLeft = Math.Max(Game.Border, Math.Min(Game.Size.Width - (Game.Border + Game.PlayerSize), PlayerLeft + state.LeftThumbX / 2000));
                PlayerTop = Math.Max(Game.Border, Math.Min(Game.Size.Height - (Game.Border + Game.PlayerSize), PlayerTop - state.LeftThumbY / 2000));
            }
            // is user pressed the fire trigger
            if (RightTrigger > 32 && !ShotActive)
            {
                ShotActive = true;
                ShotLeft = PlayerLeft;
                ShotTop = PlayerTop;
                ShotAngle = RightThumb;
            }
        }

        public bool Connected
        {
            get { return connected; }
            set { Set(ref connected, value); }
        }
        private bool connected;

        public bool A
        {
            get { return a; }
            set { Set(ref a, value); }
        }
        private bool a;

        public bool B
        {
            get { return b; }
            set { Set(ref b, value); }
        }
        private bool b;

        public bool X
        {
            get { return x; }
            set { Set(ref x, value); }
        }
        private bool x;

        public bool Y
        {
            get { return y; }
            set { Set(ref y, value); }
        }
        private bool y;

        public bool DpadUp
        {
            get { return dpadUp; }
            set { Set(ref dpadUp, value); }
        }
        private bool dpadUp;

        public bool DpadDown
        {
            get { return dpadDown; }
            set { Set(ref dpadDown, value); }
        }
        private bool dpadDown;

        public bool DpadLeft
        {
            get { return dpadLeft; }
            set { Set(ref dpadLeft, value); }
        }
        private bool dpadLeft;

        public bool DpadRight
        {
            get { return dpadRight; }
            set { Set(ref dpadRight, value); }
        }
        private bool dpadRight;

        public double LeftTrigger
        {
            get { return leftTrigger; }
            set { Set(ref leftTrigger, value); }
        }
        private double leftTrigger;

        public bool LeftBumper
        {
            get { return leftBumper; }
            set { Set(ref leftBumper, value); }
        }
        private bool leftBumper;

        public double RightTrigger
        {
            get { return rightTrigger; }
            set { Set(ref rightTrigger, value); }
        }
        private double rightTrigger;

        public bool RightBumper
        {
            get { return rightBumper; }
            set { Set(ref rightBumper, value); }
        }
        private bool rightBumper;

        public bool Start
        {
            get { return start; }
            set { Set(ref start, value); }
        }
        private bool start;

        public bool Back
        {
            get { return back; }
            set { Set(ref back, value); }
        }
        private bool back;

        public double LeftThumb
        {
            get { return leftThumb; }
            set { Set(ref leftThumb, value); }
        }
        private double leftThumb;

        public double LeftThumbForce
        {
            get { return leftThumbForce; }
            set
            {
                if (Set(ref leftThumbForce, value))
                    RaisePropertyChanged(() => LeftThumbMargin);
            }
        }
        private double leftThumbForce;

        public double RightThumb
        {
            get { return rightThumb; }
            set { Set(ref rightThumb, value); }
        }
        private double rightThumb;

        public double RightThumbForce
        {
            get { return rightThumbForce; }
            set
            {
                if (Set(ref rightThumbForce, value))
                    RaisePropertyChanged(() => RightThumbMargin);
            }
        }
        private double rightThumbForce;

        #region ViewModel stuff

        public double PlayerLeft
        {
            get { return playerLeft; }
            set { Set(ref playerLeft, value); }
        }
        private double playerLeft = 200;

        public double PlayerTop
        {
            get { return playerTop; }
            set { Set(ref playerTop, value); }
        }
        private double playerTop = 200;

        public Thickness LeftThumbMargin => new Thickness(0, 200 - LeftThumbForce, 0, 0);

        public Thickness RightThumbMargin => new Thickness(0, 200 - RightThumbForce, 0, 0);

        public bool ShotActive
        {
            get { return shotActive; }
            set { Set(ref shotActive, value); }
        }
        private bool shotActive;

        public double ShotLeft
        {
            get { return shotLeft; }
            set { Set(ref shotLeft, value); }
        }
        private double shotLeft;

        public double ShotTop
        {
            get { return shotTop; }
            set { Set(ref shotTop, value); }
        }
        private double shotTop;

        public double ShotAngle
        {
            get { return shotAngle; }
            set { Set(ref shotAngle, value); }
        }
        private double shotAngle;

        public bool PlayerAlive
        {
            get { return playerAlive; }
            set { Set(ref playerAlive, value); }
        }
        private bool playerAlive = true;

        #endregion
    }
}