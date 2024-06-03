using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventProcessing.Objects
{
    internal class DarkArea : BaseObject
    {
        public Action<DarkArea> OnDarkAreaTimeOut;
        public Action<Circle> OnDarkAreaOverlap;
        public DarkArea(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Black), -300, -150, 300, 300);

            if (this.X <= 1000)
            {
                this.X += 3;
            }
            else if (this.X >= 1000)
            {
                OnDarkAreaTimeOut(this);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddRectangle(new Rectangle(-300, -150, 300, 300));
            return path;
        }
        
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if (obj is Circle)
            {
                OnDarkAreaOverlap(obj as Circle);
            }
        }
    }
}
