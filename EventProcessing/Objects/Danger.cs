using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventProcessing.Objects
{
    internal class Danger : BaseObject
    {
        public int diameter;
        public int nowDiameter;
        public Action<Danger> OnDangerTimeOut;

        public Danger(float x, float y, float angle, int diameter) : base(x, y, angle)
        {
            this.diameter = diameter;
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(30, 255, 0, 0)), nowDiameter / -2, nowDiameter / -2, nowDiameter, nowDiameter);
                
            if (this.nowDiameter < diameter)
            {
                this.nowDiameter += 1;
            }
            else if (nowDiameter == diameter)
            {
                OnDangerTimeOut(this);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(nowDiameter / -2, nowDiameter / -2, nowDiameter, nowDiameter);
            return path;
        }
    }
}
