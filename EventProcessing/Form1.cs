using EventProcessing.Objects;

namespace EventProcessing
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;  
        DarkArea darkArea;
       
        
        int point;

        public Form1()
        {
            InitializeComponent();
            point = 0;

            marker = new Marker(pbMain.Width / 2 + 1, pbMain.Height / 2 + 1, 0);
            objects.Add(marker);

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            objects.Add(player);

                     
                    
            spawnDanger();
            spawnCircle();
            spawnCircle();
            darkArea = spawnDarkArea();

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnCircleOverlap += (m) =>
            {
                objects.Remove(m);

                spawnCircle();

                point += 1;
                points.Text = $"Очки: {point}";
            };

            player.OnDangerOverlap += (m) =>
            {
                objects.Remove(m);

                spawnDanger();

                point -= 1;
                points.Text = $"Очки: {point}";
            };

           
        } 

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            updatePlayer();
            

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }

                if (obj != darkArea && darkArea.Overlaps(obj, g))
                {
                    darkArea.Overlap(obj);
                    obj.Overlap(darkArea);
                }
            }
            /*
            foreach (var obj in objects.ToList())
            {
                foreach (var obj2 in objects.ToList())
                {

                    if (obj.Overlaps(obj2, g))
                    {
                        {
                            obj2.Overlap(obj);
                        }
                    }
                }

            }*/


            foreach (var obj in objects.ToList())
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }

        private Circle spawnCircle()
        {
            var rnd = new Random();
            var w = 13 + rnd.Next() % (pbMain.Width -26);
            var h = 13 + rnd.Next() % (pbMain.Height - 26);            
            var diameter = 50 + rnd.Next() % 100;
            
            var circle = new Circle(w, h, 0, diameter, Color.LightGreen);
            objects.Insert(2, circle);
            circle.OnCircleTimeOut += (m) =>
            {
                objects.Remove(m);
                spawnCircle();
            };
            return circle;
        }

        private Danger spawnDanger()
        {
            var rnd = new Random();
            var w = 13 + rnd.Next() % (pbMain.Width - 26);
            var h = 13 + rnd.Next() % (pbMain.Height - 26);
            var diameter = 150 + rnd.Next() % 100;

            var danger = new Danger(w, h, 0, diameter);
            objects.Insert(1, danger);
            danger.OnDangerTimeOut += (m) =>
            {
                objects.Remove(m);
                spawnDanger();
            };
            return danger;
        }

        private DarkArea spawnDarkArea()
        {
            var rnd = new Random();
            var w = 0;
            var h = 13 + rnd.Next() % (pbMain.Height - 26);
            var diameter = 150 + rnd.Next() % 100;

            var darkArea = new DarkArea(w, h, 0);
            objects.Insert(0, darkArea);

            darkArea.OnDarkAreaOverlap += (m) =>
            {                
                m.color = Color.White;
            };

            darkArea.OnDarkAreaTimeOut += (m) =>
            {
                objects.Remove(m);
                this.darkArea = spawnDarkArea();
            };
            return darkArea;
        }
    }
}
