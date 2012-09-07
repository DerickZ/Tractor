using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;


namespace Kuaff.TractorFere
{
    
    internal partial class Fere : Form
    {
        private Bitmap backImage = null;

        internal Fere()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                srcFolderName.Text = folderBrowserDialog1.SelectedPath;
                cardsName.Text = Path.GetFileName(srcFolderName.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String errorMessage = "";


            if (cardsName.Text.Length == 0)
            {
                errorMessage = "�˿��Ƶ����Ʋ���Ϊ�գ�" + Environment.NewLine + Environment.NewLine;
            }

            if (srcFolderName.Text.Length == 0)
            {
                errorMessage += "����ѡ��ͼƬԴ�ļ��У�";
            }
            else
            {
                if (!Directory.Exists(srcFolderName.Text))
                {
                    errorMessage += "ͼƬԴ�ļ��в����ڣ�";
                }
            }

            if (errorMessage.Length>0)
            {
                MessageBox.Show(errorMessage,"����",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

           
            String pathName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), cardsName.Text);
            if (Directory.Exists(pathName))
            {
                Directory.Delete(pathName,true);
            }
            DirectoryInfo dir = Directory.CreateDirectory(pathName);
            
            //����Ŀ¼�Ѿ�������ϣ���һ�����ζ�ȡԴ�ļ����µ�ͼƬ
            //��ͼƬ���и���,������Ŀ���ļ�����

            if (CreateCards(srcFolderName.Text, pathName, cardsName.Text))
            {
                ZipCards(pathName, cardsName.Text);
            }

            progressBar1.Value = 75;

            if (Directory.Exists(pathName))
            {
                Directory.Delete(pathName, true);
            }

            progressBar1.Value = 80;
        }

        //47*76
        private bool CreateCards(String sourceFolder, String destFolder,String name)
        {
            string[] files = Directory.GetFiles(sourceFolder,"*",SearchOption.AllDirectories);

            for (int i = 0; i < 54; i++)
            {
                Image cardImage = GetSourceImage(files,i);

                if (cardImage == null)
                {
                    return false;
                }

                pictureBox1.BackgroundImage = cardImage;
                pictureBox1.Refresh();
                cardImage.Save(Path.Combine(destFolder,i + ".png"));
                progressBar1.Value = i;
            }

            return true;
        }

        private Image GetSourceImage(string[] files, int number)
        {
            Bitmap src = GetBlankCard(number);

            if (number >= files.Length)
            {
                number -= files.Length;
            }

            
            Bitmap bmp = null;

           
            int k = 0;
            while (true)
            {
                try
                {
                    bmp = new Bitmap(files[number]);
                    break;
                }
                catch (Exception ex)
                {
                    number++;
                    if (number >= files.Length)
                    {
                        number -= files.Length;
                    }

                    k++;
                    if (k >= files.Length)
                    {
                        return null; //ÿ���ļ�����������֤������û��һ��ʱͼƬ�ļ�
                    }
                }
            }

            //�ʵ��ü���ȡ����λ��
            if (checkBox1.Checked)
            {
                int height = 0, width = 0;
                int x = 0, y = 0;
                if (bmp.Height * 47 >= bmp.Width * 76) //����
                {
                    width = bmp.Width;
                    height = width * 76 / 47;
                    x = 0;
                    y = (bmp.Height - height) / 2;
                }
                else
                {
                    height = bmp.Height;
                    width = height * 47 / 76;
                    x = (bmp.Width - width) / 2;
                    y = 0;
                }

                Rectangle destRect = new Rectangle(12, 10, 47, 76);
                Rectangle srcRect = new Rectangle(x, y, width, height);

                
                using ( Graphics g = Graphics.FromImage(src))
                {
                    g.DrawImage(bmp, destRect,srcRect,GraphicsUnit.Pixel);
                }
            }

            else
            {
                //�õ�ͼƬ������ͼ
                Image img = bmp.GetThumbnailImage(47, 76, null, IntPtr.Zero);

                Graphics g = Graphics.FromImage(src);
                g.DrawImage(img, 12, 10, 47, 76);
                g.Dispose();
            }

            return src;
           
        }


        private Bitmap GetBlankCard(int number)
        {
            return (Bitmap)Kuaff.TractorFere.Properties.Resources.ResourceManager.GetObject("_" + number);
        }

        private static void ZipCards(String destFolder, String name)
        {
            string[] filenames = Directory.GetFiles(destFolder);

            //�ж��ļ����Ƿ����
            String filePath = Path.Combine(Environment.CurrentDirectory,"cards");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }


            Crc32 crc = new Crc32();
            ZipOutputStream s = new ZipOutputStream(File.Create(Path.Combine(filePath, name + ".cds")));

            s.SetLevel(9); 

            foreach (string file in filenames)
            {
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;

                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);

            }

            s.Finish();
            s.Close();
        }

    
        private void button5_Click(object sender, EventArgs e)
        {
            //����

            string pathName = Path.Combine(Environment.CurrentDirectory, "cardbackimgs");
            if (!Directory.Exists(pathName))
            {
                Directory.CreateDirectory(pathName);
            }

            backImage.Save(Path.Combine(pathName,textBox2.Text + ".png"));

            button5.Enabled = false;
            textBox2.Text = "";
            button8.Select();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            string oldPath = Environment.CurrentDirectory;
           
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    bmp = new Bitmap(openFileDialog1.OpenFile());
                    textBox2.Text = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {

                }
            }

            Environment.CurrentDirectory = oldPath;

            if (bmp == null)
            {
                return;
            }

            int height = 0, width = 0;
            int x = 0, y = 0;

            if (checkBox2.Checked)
            {
                if (bmp.Height * 71 >= bmp.Width * 96) //����
                {
                    width = bmp.Width;
                    height = width * 96 / 71;
                    x = 0;
                    y = (bmp.Height - height) / 2;
                }
                else
                {
                    height = bmp.Height;
                    width = height * 71 / 96;
                    x = (bmp.Width - width) / 2;
                    y = 0;
                }
            }
            else
            {
                x = 0;
                y = 0;
                height = bmp.Height;
                width = bmp.Width;
            }
            //�õ�һ��ͼƬ�ļ�
            Bitmap img = new Bitmap(71, 96);
            
            Rectangle destRect = new Rectangle(0,0,71,96);
            Rectangle srcRect = new Rectangle(x, y, width, height);

            using (Graphics g = Graphics.FromImage(img))
            {
                //��ͼ
                g.DrawImage(bmp, destRect, srcRect, GraphicsUnit.Pixel);
                Pen pen = new Pen(Color.Black);
                Point p1 = new Point(0, 0);
                Point p2 = new Point(0, 95);
                Point p3 = new Point(70, 0);
                Point p4 = new Point(70, 95);

                g.DrawLines(pen, new Point[] {p1,p2});
                g.DrawLines(pen, new Point[] { p1, p3 });
                g.DrawLines(pen, new Point[] { p2, p4 });
                g.DrawLines(pen, new Point[] { p3, p4 });

            }



            //���α߽�
            img.SetPixel(0,0,Color.Transparent);
            img.SetPixel(0, 1, Color.Transparent);
            img.SetPixel(1, 0, Color.Transparent);
            img.SetPixel(1, 1, Color.Black);

            img.SetPixel(70, 0, Color.Transparent);
            img.SetPixel(70, 1, Color.Transparent);
            img.SetPixel(69, 0, Color.Transparent);
            img.SetPixel(69, 1, Color.Black);

            img.SetPixel(0, 94, Color.Transparent);
            img.SetPixel(0, 95, Color.Transparent);
            img.SetPixel(1, 95, Color.Transparent);
            img.SetPixel(1, 94, Color.Black);

            img.SetPixel(70, 95, Color.Transparent);
            img.SetPixel(70, 94, Color.Transparent);
            img.SetPixel(69, 95, Color.Transparent);
            img.SetPixel(69, 94, Color.Black);

            backImage = img;
            pictureBox3.Image = img;

            button5.Enabled = true;
        }
    }
}