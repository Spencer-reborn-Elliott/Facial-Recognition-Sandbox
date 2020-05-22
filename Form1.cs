using DlibDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facial_Recognition_Sandbox
{
    public partial class Form1 : Form
    {
        string Title = "Facial Recognition Sandbox";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            selectImageFile.Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.tiff, *.png) | *.jpg; *.jpeg; *.bmp; *.tiff; *.png";
            selectImageFile.FileName = "";
            DialogResult result = selectImageFile.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(selectImageFile.FileName))
            {
                originalPicture.Image = Image.FromFile(selectImageFile.FileName);
                ProcessImage(selectImageFile.FileName);
            }
        }


        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = InputFolder.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(InputFolder.SelectedPath))
            {
                var ext = new List<string> { ".jpg", ".gif", ".png", ".jpeg", ".tiff", ".bmp" };
                DirectoryInfo InputFolderDir = new DirectoryInfo(InputFolder.SelectedPath);
                foreach (var file in InputFolderDir.GetFiles("*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s.FullName).ToLowerInvariant())))
                {
                    ProcessImage(file.FullName);
                }
            }
        }


        void ProcessImage(string Photo)
        {
            try
            {
                using (var fd = Dlib.GetFrontalFaceDetector())
                {
                    var img = Dlib.LoadImage<RgbPixel>(Photo);

                    // find all faces in the image
                    var faces = fd.Operator(img);
                    foreach (var face in faces)
                    {
                        Dlib.FillRect(img, face, color: new RgbPixel(204, 6, 12));
                    }
                    Dlib.SaveJpeg(img, "Processed" + Path.GetFileName(Photo));
                }
                processedImage.Image = Image.FromFile("Processed" + Path.GetFileName(Photo));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), Title);
            }
        }

    }
}
