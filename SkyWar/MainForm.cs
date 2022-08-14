
using SkyWar;
using System.Drawing;

namespace ScratchGameCore
{
    public partial class MainForm : Form
    {
        Game game;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public MainForm()
        {
            InitializeComponent();

            Image bg = Image.FromFile(@"C:\Users\slime\Pictures\Snipaste_2022-08-07_06-33-54.png");
            Image img = Image.FromFile(@"C:\Users\slime\Pictures\qwq.png");

            game = new Game(gamePanel, 480, 700);
            game.BackgroundColor = Color.Pink;
            game.Background = bg;

            gamePanel.Game = game;


            GameSprite sprite1;
            game.Sprites.Add(sprite1 = new WarplaneSprite(game)
            {
                Sprite = img,
                Scale = 0.3f
            });
            game.Sprites.Add(new ConorAwa(game)
            {
                Position = new PointF(250, 250),
                Sprite = img,
                Scale = 0.2f,
                target = sprite1
            });

            timer.Interval = 30;
            timer.Tick += (s, e) =>
            {
                try
                {
                    Invoke(() =>
                    {
                        game.MainLoopAction();
                        gamePanel.Render();
                    });
                }
                catch { }
            };

            timer.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainForm_Resize(this, null!);
        }

        protected override void DestroyHandle()
        {
            timer.Stop();
            base.DestroyHandle();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Point center = (Point)((ClientSize - gamePanel.Size) / 2);
            gamePanel.Location = center;
        }
    }
}