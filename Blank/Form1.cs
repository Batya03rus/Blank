using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Blank
{
    public partial class Form1 : Form
    {
        // === Печать ===
        private readonly PrintDocument _doc = new PrintDocument();
        private readonly PageSettings _pageSettings = new PageSettings();
        private PrinterSettings _printerSettings = new PrinterSettings();
        private readonly PrintPreviewDialog _preview = new PrintPreviewDialog();
        private readonly PageSetupDialog _pageSetup = new PageSetupDialog();
        private readonly PrintDialog _printDialog = new PrintDialog();

        public Form1()
        {
            InitializeComponent();
            SetupPrintMenu();          // меню «Файл» сверху
            WirePrinting();            // привязка событий печати
        }

        // ====== ТВОИ ОБРАБОТЧИКИ (оставил без изменений) ======
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
            textBox3.Text = textBox1.Text;
            textBox4.Text = textBox1.Text;
            textBox5.Text = textBox1.Text;
            textBox6.Text = textBox1.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBox9.Text = textBox8.Text;
            textBox10.Text = textBox8.Text;
            textBox11.Text = textBox8.Text;
            textBox12.Text = textBox8.Text;
            textBox13.Text = textBox8.Text;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox5.Text = guna2TextBox1.Text;
            guna2TextBox6.Text = guna2TextBox1.Text;
            guna2TextBox7.Text = guna2TextBox1.Text;
            guna2TextBox8.Text = guna2TextBox1.Text;
            guna2TextBox4.Text = guna2TextBox1.Text;
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox3.Text = guna2TextBox2.Text;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox4.Text = comboBox1.Text;
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            comboBox3.Text = comboBox2.Text;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            textBox19.Text = textBox14.Text;
        }

        // ====== ПЕЧАТЬ: меню и логика ======
        private void SetupPrintMenu()
        {
            if (this.MainMenuStrip == null)
            {
                var menu = new MenuStrip();
                var mFile = new ToolStripMenuItem("Файл");
                var miPrint = new ToolStripMenuItem("Печать") { ShortcutKeys = Keys.Control | Keys.P };
                var miPrintDirect = new ToolStripMenuItem("Печать сразу (без диалогов)");
                var miPreview = new ToolStripMenuItem("Предпросмотр");
                var miPage = new ToolStripMenuItem("Параметры страницы");
                var miPrinter = new ToolStripMenuItem("Настройки принтера");
                var miExit = new ToolStripMenuItem("Выход");

                miPrint.Click += (_, __) => PrintWithDialog();
                miPrintDirect.Click += (_, __) => PrintDirect();
                miPreview.Click += (_, __) => ShowPreview();
                miPage.Click += (_, __) => ShowPageSetup();
                miPrinter.Click += (_, __) => ShowPrinterSetup();
                miExit.Click += (_, __) => Close();

                mFile.DropDownItems.AddRange(new ToolStripItem[]
                { miPrint, miPrintDirect, new ToolStripSeparator(),
                  miPreview, miPage, miPrinter, new ToolStripSeparator(), miExit });

                menu.Items.Add(mFile);
                this.MainMenuStrip = menu;
                this.Controls.Add(menu);
            }
        }

        private void WirePrinting()
        {
            _doc.PrintPage += OnPrintPage;
            _doc.BeginPrint += (_, __) => _doc.DefaultPageSettings = _pageSettings;
            _doc.PrinterSettings = _printerSettings;

            _preview.Document = _doc;
            _pageSetup.PageSettings = _pageSettings;
            _pageSetup.PrinterSettings = _printerSettings;

            _printDialog.UseEXDialog = true;
            _printDialog.AllowSomePages = true;
            _printDialog.Document = _doc;

            // === A4 без полей ===
            _pageSettings.PaperSize = new PaperSize("A4", 827, 1169); // сотые дюйма
            _pageSettings.Margins = new Margins(0, 0, 0, 0);          // без отступов
            _pageSettings.Landscape = false;                          // книжная ориентация
        }

        // === Главное изменение: печать строго во весь лист ===
        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            using (var bmp = CaptureClientArea())
            {
                Rectangle m = e.PageBounds; // вся страница A4
                Rectangle dest = new Rectangle(m.Left, m.Top, m.Width, m.Height);

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                // растягиваем картинку на всю страницу
                e.Graphics.DrawImage(bmp, dest);
            }
            e.HasMorePages = false;
        }

        // Захват клиентской области формы без строки меню
        private Bitmap CaptureClientArea()
        {
            int menuH = (this.MainMenuStrip != null) ? this.MainMenuStrip.Height : 0;
            var size = new Size(this.ClientSize.Width, this.ClientSize.Height - menuH);
            if (size.Width <= 0 || size.Height <= 0) size = new Size(1, 1);

            var full = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.DrawToBitmap(full, new Rectangle(Point.Empty, full.Size));

            var crop = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(crop))
            {
                g.DrawImage(full,
                    new Rectangle(0, 0, crop.Width, crop.Height),
                    new Rectangle(0, menuH, crop.Width, crop.Height),
                    GraphicsUnit.Pixel);
            }
            full.Dispose();
            return crop;
        }

        // Действия меню
        private void PrintWithDialog()
        {
            if (_printDialog.ShowDialog(this) == DialogResult.OK)
            {
                _doc.Print();
            }
        }

        private void PrintDirect()
        {
            try
            {
                _doc.PrinterSettings = _printerSettings;
                _doc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка печати: " + ex.Message, "Печать",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowPreview()
        {
            _preview.Width = 1000;
            _preview.Height = 700;
            _preview.ShowDialog(this);
        }

        private void ShowPageSetup()
        {
            _pageSetup.ShowDialog(this);
        }

        private void ShowPrinterSetup()
        {
            if (_printDialog.ShowDialog(this) == DialogResult.OK)
            {
                _printerSettings = _printDialog.PrinterSettings;
                _doc.PrinterSettings = _printerSettings;
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox17.Text;
            textBox3.Text = textBox17.Text;
            textBox4.Text = textBox17.Text;
            textBox5.Text = textBox17.Text;
            textBox6.Text = textBox17.Text;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            textBox15.Text = textBox1.Text;
        }

        private void textBox8_TextChanged_1(object sender, EventArgs e)
        {
            textBox9.Text = textBox8.Text;
            textBox10.Text = textBox8.Text;
            textBox11.Text = textBox8.Text;
            textBox12.Text = textBox8.Text;
            textBox13.Text = textBox8.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
