using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventProcessing.Objects
{
    internal class Circle : BaseObject
    {         
        public int diameter;
        public Action<Circle> OnCircleTimeOut;

        public Color color = Color.LightGreen;

        public Circle(float x, float y, float angle, int diameter, Color color) : base(x, y, angle)
        {            
            this.diameter = diameter;
            this.color = color; 
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(color), diameter / -2, diameter / -2, diameter, diameter);
            
            g.DrawString(
                $"{diameter}",
                new Font("Verdana", 8),
                new SolidBrush(Color.White),
                -8, -4
            );

            if (this.diameter >= 1)
            {
                this.diameter -= 1;                
            }
            else if (diameter == 0)
            {
                OnCircleTimeOut(this);
            }            
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(diameter / -2, diameter / -2, diameter, diameter);
            return path;
        }
    }
}
