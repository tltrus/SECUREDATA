using System;
using System.Windows.Forms;
using System.IO;

namespace SecureInfoProg
{
    public partial class Form1 : Form
    {
        string currentFileName;
        const string SharedSecret = "dsfsf/saasde/vfsfEd A physics simu%lation wo!rks 4522e02l!4d933FAV4dby making many smal%l predi()ctions based on the laws of physics(!)4Fsl6NXeap";

        public Form1()
        {
            InitializeComponent();

            openFileDialog1.Filter = "SecureData files(*.sec)|*.sec|All files(*.*)|*.*";
            saveFileDialog1.Filter = "SecureData files(*.sec)|*.sec|All files(*.*)|*.*";
        }


        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            currentFileName = openFileDialog1.FileName;
            btnDecrypt.Enabled = true;
            btnSave.Enabled = true;
            tbPassw.Enabled = true;

            rtb.Clear();
            using (FileStream fstream = File.OpenRead(currentFileName))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);

                rtb.AppendText(textFromFile);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var encryptedStringAES = Crypto.EncryptStringAES(rtb.Text, SharedSecret);

                using (FileStream fstream = new FileStream(currentFileName, FileMode.Create))
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(encryptedStringAES);
                    fstream.Write(array, 0, array.Length);
                }
            }
            catch 
            {
                MessageBox.Show("Error!");
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            // шифрование
            var encryptedStringAES = Crypto.EncryptStringAES(rtb.Text, SharedSecret);

            // сохранить как... в файл
            using (FileStream fstream = new FileStream(saveFileDialog1.FileName, FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(rtb.Text);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }
        }

        private void btnNewFile_Click(object sender, EventArgs e) => rtb.Clear();

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (tbPassw.Text == "") return;
            string pattern = Crypto.DecryptStringAES("EAAAAKd9zzKYMavc4LeCN916UJMte6VXdfjf4nlqFqcUwQBV", SharedSecret); // password is qwrty

            if (pattern == tbPassw.Text)
            {
                string text = Crypto.DecryptStringAES(rtb.Text, SharedSecret);

                tbPassw.Text = "";
                rtb.Clear();
                rtb.AppendText(text);

                btnDecrypt.Enabled = false;
                tbPassw.Enabled = false;
            }
        }

        private void tbPassw_MouseClick(object sender, MouseEventArgs e) => tbPassw.Text = "";
    }
}
