using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace YilanOyunu
{
    public partial class Form1 : Form
    {
        private Label _YilanKafasi;
        private int _YilanParcasiArasiMesafe = 2;
        private int _YilanParcasiSayisi;
        private int _yilanBoyutu = 20;
        private int _YemBoyutu = 20;
        private Label _Yem;
        private Random _random;
        private HareketYonu _Yon;
        public Form1()
        {
            InitializeComponent();
            _random = new Random();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _YilanParcasiSayisi = 0;
            YemOlustur();
            YeminYeriniDegistir();
            YilaniYerlestir();
            _Yon = HareketYonu.Saga;
            timerYilanHareket.Enabled = true;

        }
        private void YenidenBaslat()
        {

            label3.Text = "0";
            label5.Text = "0";

            
            Sifirla();
            
        }



        private void Sifirla()
        {

            this.pnl.Controls.Clear();
            _YilanParcasiSayisi = 0;
            YemOlustur();
            YeminYeriniDegistir();
            YilaniYerlestir();
            _Yon = HareketYonu.Saga;
            timerYilanHareket.Enabled = true;
            timerSaat.Enabled = true;

        }

        private void label3_Click(object sender, EventArgs e)
        {


        }

        private Label YilanParcasiOlustur(int locationX, int locationY)
        {

            _YilanParcasiSayisi++;

            Label lbl = new Label()
            {
                Name = "YilanParca" + _YilanParcasiSayisi,
                BackColor = Color.Red,
                Width = _yilanBoyutu,
                Height = _yilanBoyutu,
                Location = new Point(locationX, locationY)

            };

            this.pnl.Controls.Add(lbl);
            return lbl;

        }

        private void YilaniYerlestir()
        {
            _YilanKafasi = YilanParcasiOlustur(0, 0);
            _YilanKafasi.Text = " : ";
            _YilanKafasi.TextAlign = ContentAlignment.MiddleCenter;
            _YilanKafasi.ForeColor = Color.White;   
            var locationX = (pnl.Width / 2) - (_YilanKafasi.Width / 2);
            var locationy = (pnl.Height / 2) - (_YilanKafasi.Height / 2);
            _YilanKafasi.Location = new Point(locationX, locationy);

        }



        private void YemOlustur()
        {
            Label lbl = new Label()
            {
                Name = "Yem",
                BackColor = Color.Lime,
                Width = _YemBoyutu,
                Height = _YemBoyutu,

            };
            _Yem = lbl;
            this.pnl.Controls.Add(lbl);

        }

        private void YeminYeriniDegistir()
        {
            int locationX, locationY;
            bool durum;
            do
            {
                durum = false;
                locationX = _random.Next(0, pnl.Width - _YemBoyutu);
                locationY = _random.Next(0, pnl.Height - _YemBoyutu);

                var rect1 = new Rectangle(new Point(locationX, locationY), _Yem.Size);
                foreach (Control control in pnl.Controls)
                {
                    if (control is Label && control.Name.Contains("YilanParca"))
                    {
                        var rect2 = new Rectangle(control.Location, control.Size);
                        if (rect1.IntersectsWith(rect2))
                        {
                            durum = true;
                            break;
                        }
                    }
                }
            } while (durum);

            _Yem.Location = new Point(locationX, locationY);
        }


        private enum HareketYonu     //sabit değerleri bir grup altında toplar.
        {   

            Yukari,
            Asagi,
            Sola,   
            Saga
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var keyCode = e.KeyCode;

            if (_Yon == HareketYonu.Sola &&  keyCode == Keys.D
               || _Yon == HareketYonu.Saga && keyCode == Keys.A
               || _Yon == HareketYonu.Yukari && keyCode == Keys.S
               || _Yon == HareketYonu.Asagi && keyCode == Keys.W
                )
            {
                return;
            }

            switch (keyCode)
            {
                case Keys.W:
                    _Yon = HareketYonu.Yukari;
                    break;
                case Keys.S:
                    _Yon = HareketYonu.Asagi;
                    break;
                case Keys.A:
                    _Yon = HareketYonu.Sola;
                    break;
                case Keys.D:
                    _Yon = HareketYonu.Saga;
                    break;
                case Keys.P:
                    timerSaat.Enabled = false;
                    timerYilanHareket.Enabled = false;
                    break;
                case Keys.C:
                    timerSaat.Enabled = true;
                    timerYilanHareket.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void timerYilanHareket_Tick(object sender, EventArgs e)
        {
            YilanKafasiniTakipEt();
            YilaniYurut();
            OyunBittimi();
            YilanYemiYedimi();


        }



        private void YilaniYurut()
        {
            var locationX = _YilanKafasi.Location.X;
            var locationY = _YilanKafasi.Location.Y;

            switch (_Yon)
            {
                case HareketYonu.Yukari:
                    locationY -= (_YilanKafasi.Width + _YilanParcasiArasiMesafe);
                    break;
                case HareketYonu.Asagi:
                    locationY += (_YilanKafasi.Width + _YilanParcasiArasiMesafe);
                    break;
                case HareketYonu.Sola:
                    locationX -= (_YilanKafasi.Width + _YilanParcasiArasiMesafe);
                    break;
                case HareketYonu.Saga:
                    locationX += (_YilanKafasi.Width + _YilanParcasiArasiMesafe);
                    break;
            }

            // Sınır kontrolü
            if (locationX < 0 || locationX + _YilanKafasi.Width > pnl.Width || locationY < 0 || locationY + _YilanKafasi.Height > pnl.Height)
            {
                
                timerYilanHareket.Enabled = false;
                timerSaat.Enabled = false;
                DialogResult sonuc = MessageBox.Show("Puanınız: " + label3.Text, "Oyun Bitti!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (sonuc == DialogResult.OK)
                {
                    YenidenBaslat();
                }
                return;
            }

            _YilanKafasi.Location = new Point(locationX, locationY);
        }

        private void OyunBittimi()
        {
            bool oyunBittimi = false;
            var rect1 = new Rectangle(_YilanKafasi.Location, _YilanKafasi.Size);
            foreach (Control control in pnl.Controls)
            {
                if (control is Label && control.Name.Contains("YilanParca") && control.Name != _YilanKafasi.Name)
                {
                    var rect2 = new Rectangle(control.Location,control.Size);
                    if (rect1.IntersectsWith(rect2))
                    {
                        
                        oyunBittimi |= true;
                        break;

                    }



                }   

            }
            if (oyunBittimi)
            {
                timerYilanHareket.Enabled = false;
                timerSaat.Enabled = false;  
               DialogResult sonuc = MessageBox.Show("Puanınız:" + label3.Text,"Oyun Bitti!", MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                if (sonuc == DialogResult.OK)
                {
                    YenidenBaslat();
                }

            }




                
        }


        private void YilanYemiYedimi()
        {
            var rect1 = new Rectangle(_YilanKafasi.Location, _YilanKafasi.Size);
            var rect2 = new Rectangle(_Yem.Location, _Yem.Size);

            if (rect1.IntersectsWith(rect2))
            {
                label3.Text = (Convert.ToInt32(label3.Text) + 10).ToString();
                YeminYeriniDegistir();
                YilanParcasiOlustur(- _yilanBoyutu,  - _yilanBoyutu);
            }
        }
        private void YilanKafasiniTakipEt()
        {

            if (_YilanParcasiSayisi <= 1) return;
            for (int i = _YilanParcasiSayisi; i > 1; i--)
            {

                var sonrakiParca = (Label)pnl.Controls[i];
                var oncekiParca = (Label)pnl.Controls[i - 1];
                sonrakiParca.Location = oncekiParca.Location;


            }



        }

        private void timerSaat_Tick(object sender, EventArgs e)
        {
            label5.Text = (Convert.ToInt32(label5.Text) +1 ).ToString();
        }
    }
}
