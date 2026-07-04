using ChaosFramework.Math.Vectors;
using OpenTK.GLControl;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChaosFramework.Platform.WinForms
{
    public class WinFormsWindow : Window
    {
        public readonly Form form;
        readonly GLControl control;

        public uint width { get => (uint)form.Width; set => form.Width = (int)value; }
        public uint height { get => (uint)form.Height; set => form.Height = (int)value; } 

        public Vector2i position { get => form.Location; set => form.Location = value; }

        public string title { get => form.Name; set => form.Name = value; }

        Vector2i PresentationContext.position => Vector2i.EMPTY;

        public WinFormsWindow(Form form)
        {
            this.form = form;
            form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            control = new GLControl();
            control.Bounds = form.ClientRectangle;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            form.Controls.Add(control);
        }

        void PresentationContext.SetIcon(ApplicationIcon icon)
        {
            switch (icon.format)
            {
                case ApplicationIcon.IconFormat.ico:
                    using (Stream str = icon.getStream())
                        SetIcon(new Icon(str));
                    return;
                default:
                    throw new ArgumentException($"Unsupported format {icon.format}", nameof(icon));
            }
        }

        public void SetIcon(Icon icon)
            => form.Icon = icon;

        void PresentationContext.Present()
        {
            control.Context.MakeCurrent();
            control.SwapBuffers();
        }
    }
}
