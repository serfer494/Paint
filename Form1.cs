using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Topicos___Ejercicio1
{
    public partial class frmPrincipal : Form
    {

        #region AtributosClase
        public enum TipoAccion
        {
            Linea,
            ManoAlzada,
            Circulo,
            CirculoconFondo,
            Cuadrado,
            CuadradoconFondo,
            Borrador,
            Rectangulo,
            RectanguloconFondo,
            Elipse,
            ElipseconFondo,
        }

        private TipoAccion tipoAccionActivo;
        private Color colorActivo;
        private Pen miLapiz;
        private Bitmap imagenGuardar;
        private Point[] arregloPuntos;
        private bool botonAplastado;
        private int grosorLapiz;
        private Point primerPunto;
        private Point segundoPunto;
        #endregion

        #region Eventos
        public frmPrincipal()
        {
            botonAplastado = false;
            grosorLapiz = 1;
            colorActivo = Color.Black;
            
            arregloPuntos = new Point[0];
            InitializeComponent();
        }

        private void menuAcercade_Click(object sender, EventArgs e)
        {
            frmAcercaDe miVentAcercaDe = new frmAcercaDe();
            miVentAcercaDe.Show();
        }

        private void btmPaleta_Click(object sender, EventArgs e)
        {
            if (colorPaleta.ShowDialog() == DialogResult.OK)
            {
                btmPaleta.BackColor = colorPaleta.Color;
                colorActivo = colorPaleta.Color;
            }
        }

        private void btmLapiz_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.ManoAlzada;
        }
#endregion

        private void pnlBannerDerecho_MouseMove(object sender, MouseEventArgs e)
        {
            lblCoordenadas.Text = "@X: " + e.Location.X.ToString() + "@Y: " + e.Location.Y.ToString();

            if (!botonAplastado) return;

            agregarPunto(new Point(e.Location.X, e.Location.Y));


                switch (tipoAccionActivo)
                {
                    case TipoAccion.ManoAlzada:
                        DibujarManoAlzada();
                        break;
                    case TipoAccion.Circulo:
                        break;
                    case TipoAccion.Linea:
                        break;
                    case TipoAccion.Cuadrado:
                        break;
                    case TipoAccion.Borrador:
                        break;
                    default:
                        break;
                }
            }
        private void agregarPunto(Point punto)
        {
            Point[] temporalPoints = new Point[arregloPuntos.Length + 1];
            arregloPuntos.CopyTo(temporalPoints,0);
            arregloPuntos = temporalPoints;
            arregloPuntos[arregloPuntos.Length-1] = punto;

        }
        private void DibujarManoAlzada()
        {
            if (arregloPuntos.Length <= 1) return;

            Graphics g1Graphics = pnlBannerDerecho.CreateGraphics();
            g1Graphics.DrawLines(miLapiz, arregloPuntos);
            g1Graphics.Dispose();
        }

        private void InicializarLapiz() 
        {
            miLapiz = new Pen(colorActivo, grosorLapiz);
            miLapiz.StartCap = LineCap.Round;
            miLapiz.EndCap = LineCap.Round;
            miLapiz.LineJoin = LineJoin.Round;
            arregloPuntos = new Point[0];
            IntPtr intptr = Properties.Resources.pencil.GetHicon();
            pnlBannerDerecho.Cursor = new Cursor(intptr);
        }

        private void InicializarBorrador()
        {
            miLapiz = new Pen(colorActivo, grosorLapiz);
            miLapiz.StartCap = LineCap.Round;
            miLapiz.EndCap = LineCap.Round;
            miLapiz.LineJoin = LineJoin.Round;
            arregloPuntos = new Point[0];
            IntPtr intptr = Properties.Resources.descarga.GetHicon();
            pnlBannerDerecho.Cursor = new Cursor(intptr);
        }

        private void pnlBannerDerecho_MouseDown(object sender, MouseEventArgs e)
        {
            botonAplastado = true;
            InicializarLapiz();
            switch (tipoAccionActivo)
            {
                case TipoAccion.Linea:
                    primerPunto = new Point(e.X, e.Y);
                    break;
                case TipoAccion.Rectangulo:
                    primerPunto = new Point(e.X, e.Y);
                    break;
                case TipoAccion.Cuadrado:
                    primerPunto = new Point(e.X, e.Y);
                    break;
                case TipoAccion.Elipse:
                    primerPunto = new Point(e.X, e.Y);
                    break;
                case TipoAccion.RectanguloconFondo:
                    primerPunto = new Point(e.X, e.Y);
                    break;
                case TipoAccion.CuadradoconFondo:
                    primerPunto = new Point(e.X, e.Y);
                    break;
                case TipoAccion.ElipseconFondo:
                    primerPunto = new Point(e.X, e.Y);
                    break;

            }
            
        }

        private void pnlBannerDerecho_MouseUp(object sender, MouseEventArgs e)
        {
            botonAplastado = false;
            segundoPunto = new Point(e.X, e.Y);
            Graphics g1Graphics = pnlBannerDerecho.CreateGraphics();
            SolidBrush sb = new SolidBrush(colorActivo);
            switch (tipoAccionActivo)
            {
                case TipoAccion.Linea:

                    miLapiz.StartCap = LineCap.ArrowAnchor;
                    miLapiz.EndCap = LineCap.ArrowAnchor;
                    g1Graphics.DrawLine(miLapiz, primerPunto.X, primerPunto.Y, segundoPunto.X, segundoPunto.Y);
                    g1Graphics.Dispose();
                    break;

                case TipoAccion.Elipse:
                    int longitudx = Math.Abs(segundoPunto.X - primerPunto.X);
                    int longitudy = Math.Abs(segundoPunto.Y - primerPunto.Y);
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X < segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y < segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                    }
                    g1Graphics.DrawEllipse(miLapiz, primerPunto.X, primerPunto.Y, longitudx, longitudy);
                    g1Graphics.Dispose();
                    break;

                case TipoAccion.Cuadrado:
                    segundoPunto = new Point(e.X, e.Y);
                    longitudx = Math.Abs(segundoPunto.X - primerPunto.X);
                    longitudy = Math.Abs(segundoPunto.Y - primerPunto.Y);
                    int lado;
                    if (longitudx >= longitudy)
                    {
                        lado = longitudx;
                    }
                    else
                    {
                        lado = longitudy;
                    }
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if((primerPunto.X < segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if((primerPunto.X > segundoPunto.X) && (primerPunto.Y < segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                    }
                    g1Graphics.DrawRectangle(miLapiz, primerPunto.X, primerPunto.Y, lado, lado);
                    g1Graphics.Dispose();
                    break;

                case TipoAccion.Rectangulo:
                    longitudx = Math.Abs(segundoPunto.X - primerPunto.X);
                    longitudy = Math.Abs(segundoPunto.Y - primerPunto.Y);
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X < segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y < segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                    }
                    g1Graphics.DrawRectangle(miLapiz, primerPunto.X, primerPunto.Y, longitudx, longitudy);
                    g1Graphics.Dispose();
                    break;

                case TipoAccion.ElipseconFondo:
                    longitudx = Math.Abs(segundoPunto.X - primerPunto.X);
                    longitudy = Math.Abs(segundoPunto.Y - primerPunto.Y);
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X < segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y < segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                    }
                    g1Graphics.FillEllipse(sb, primerPunto.X, primerPunto.Y, longitudx, longitudy);
                    g1Graphics.Dispose();
                    break;

                case TipoAccion.CuadradoconFondo:
                    longitudx = Math.Abs(segundoPunto.X - primerPunto.X);
                    longitudy = Math.Abs(segundoPunto.Y - primerPunto.Y);
                    if (longitudx >= longitudy)
                    {
                        lado = longitudx;
                    }
                    else
                    {
                        lado = longitudy;
                    }
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X < segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y < segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                    }
                    g1Graphics.FillRectangle(sb, primerPunto.X, primerPunto.Y, lado, lado);
                    g1Graphics.Dispose();
                    break;

                case TipoAccion.RectanguloconFondo:
                    longitudx = Math.Abs(segundoPunto.X - primerPunto.X);
                    longitudy = Math.Abs(segundoPunto.Y - primerPunto.Y);
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X < segundoPunto.X) && (primerPunto.Y > segundoPunto.Y))
                    {
                        primerPunto.Y = segundoPunto.Y;
                    }
                    if ((primerPunto.X > segundoPunto.X) && (primerPunto.Y < segundoPunto.Y))
                    {
                        primerPunto.X = segundoPunto.X;
                    }
                    g1Graphics.FillRectangle(sb, primerPunto.X, primerPunto.Y, longitudx, longitudy);
                    g1Graphics.Dispose();
                    break;
            }
            
            
            
        }

        private void nudGrosor_ValueChanged(object sender, EventArgs e)
        {
            grosorLapiz = (int) nudGrosor.Value;
        }

        private void btmBorrador_Click(object sender, EventArgs e)
        {
            botonAplastado = true;
            colorActivo = Color.White;
            InicializarBorrador();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            btmPaleta.BackColor = colorActivo;
        }

        private void btmLR_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.Linea;
        }


        private void btmCD_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.Cuadrado;
        }

        private void btmRA_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.Rectangulo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.Elipse;
        }

        private void btmCDOFONDO_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.CuadradoconFondo;
        }

        private void btmRAFONDO_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.RectanguloconFondo;
        }

        private void btmELFONDO_Click(object sender, EventArgs e)
        {
            tipoAccionActivo = TipoAccion.ElipseconFondo;
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int width = (pnlBannerDerecho.Width);
            int height = (pnlBannerDerecho.Height);
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = pnlBannerDerecho.RectangleToScreen(pnlBannerDerecho.ClientRectangle);
            g.CopyFromScreen(rect.Location, Point.Empty, pnlBannerDerecho.Size);
            g.Dispose();
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Png files|*.png|jpeg files|*jpg|bitmaps|*.bmp";
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(s.FileName))
                {
                    File.Delete(s.FileName);
                }
                if (s.FileName.Contains(".jpg"))
                {
                    bmp.Save(s.FileName, ImageFormat.Jpeg);
                }
                else if (s.FileName.Contains(".png"))
                {
                    bmp.Save(s.FileName, ImageFormat.Png);
                }
                else if (s.FileName.Contains(".bmp"))
                {
                    bmp.Save(s.FileName, ImageFormat.Bmp);
                }
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlBannerDerecho.Refresh();
            
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Png files|*.png|jpeg files|*jpg|bitmaps|*.bmp";
            if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pnlBannerDerecho.BackgroundImage = (Image)Image.FromFile(o.FileName).Clone();
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btmRojo_Click(object sender, EventArgs e)
        {
            colorActivo = Color.Red;
            btmPaleta.BackColor = colorActivo;
        }

        private void btmAzul_Click(object sender, EventArgs e)
        {
            colorActivo = Color.Blue;
            btmPaleta.BackColor = colorActivo; 
        }

        private void btmVerde_Click(object sender, EventArgs e)
        {
            colorActivo = Color.Green;
            btmPaleta.BackColor = colorActivo;
        }

        private void btmNegro_Click(object sender, EventArgs e)
        {
            colorActivo = Color.Black;
            btmPaleta.BackColor = colorActivo;
        }
    }
}
