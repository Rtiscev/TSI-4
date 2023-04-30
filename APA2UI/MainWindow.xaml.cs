using System;
using System.Windows;
using System.Windows.Input;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Path = System.IO.Path;
using Microsoft.Win32;
using System.Collections.Generic;

namespace TSI4
{
    public partial class MainWindow : Window
    {
        UnicodeEncoding ByteConverter = new();
        RSACryptoServiceProvider _RSA = new();
        byte[] encryptedtext;
        byte[] signedHash;
        RSAParameters sharedParameters;
        string roundtrip;
        SHA256 alg = SHA256.Create();
        int typeOfEnc = 0;
        Aes myAes = Aes.Create();
        Aes aes = Aes.Create();
        List<string> keys = new();

        public MainWindow()
        {
            InitializeComponent();
            grido.Width = borda.Width;
        }

        private void SelectFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    keys.Add(Path.GetFullPath(filename));
            }
        }
        private void EncryptFile(object sender, RoutedEventArgs e)
        {
            string password = passwordFiles.Text;
            //string ivString = ByteConverter.GetString(aes.IV);
            //string aaaa = ByteConverter.GetString(aes.Key);

            checkThePass(ref password);
            aes.Key = ByteConverter.GetBytes(password);

            foreach (string fileName in keys)
            {
                string texttt = "";
                texttt = texttt.Remove(0);
                using (FileStream filestream1 = new(fileName, FileMode.Open))
                {
                    string line;
                    using (var sr = new StreamReader(filestream1))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            texttt += line + "\r\n";
                        }
                    }
                }

                using (FileStream fileStream = new(fileName, FileMode.Truncate))
                {
                    aes.GenerateIV();
                    fileStream.Position = 0;
                    fileStream.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cryptoStream = new(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter encryptWriter = new(cryptoStream, ByteConverter))
                        {
                            encryptWriter.Write(texttt);
                        }
                    }
                }
            }
        }


        private void DecryptFile(object sender, RoutedEventArgs e)
        {
            foreach (string fileName in keys)
            {
                string textor = "";
                textor = textor.Remove(0);
                using (FileStream fileStream = new(fileName, FileMode.Open))
                {
                    byte[] iv = new byte[aes.IV.Length];
                    int numBytesToRead = aes.IV.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    //string password = "jinx567891234567";
                    string password = passwordFiles.Text;
                    aes.IV = iv;

                    checkThePass(ref password);
                    aes.Key = ByteConverter.GetBytes(password);

                    using (CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                    {
                        using (StreamReader decryptReader = new(cryptoStream))
                        {
                            textor = decryptReader.ReadToEnd();
                        }
                    }
                }
                using (FileStream filestr = new(fileName, FileMode.Truncate))
                {
                    using (StreamWriter encryptWriter = new(filestr, ByteConverter))
                    {
                        encryptWriter.Write(textor);
                    }
                }
            }
            keys.Clear();
        }
        private void Encrypt(object sender, RoutedEventArgs e)
        {
            typeOfEnc = typeEnc.SelectedIndex;
            switch (typeOfEnc)
            {
                case 0:
                    encryptedtext = EncryptStringToBytes_Rsa(ByteConverter.GetBytes(messageToEnc.Text), _RSA.ExportParameters(false), false);
                    break;
                case 1:
                    encryptedtext = EncryptStringToBytes_Aes(messageToEnc.Text, myAes.Key, myAes.IV);
                    break;
            }
            encryptedText.Text = System.Convert.ToBase64String(encryptedtext);
        }
        private void Decrypt(object sender, RoutedEventArgs e)
        {
            switch (typeOfEnc)
            {
                case 0:
                    byte[] decryptedtex = DecryptionStringToBytes_RSA(encryptedtext, _RSA.ExportParameters(true), false);
                    originalText.Text = ByteConverter.GetString(decryptedtex);
                    break;
                case 1:
                    roundtrip = DecryptStringFromBytes_Aes(encryptedtext, myAes.Key, myAes.IV);
                    originalText.Text = roundtrip;
                    break;
            }
        }
        static public byte[] EncryptStringToBytes_Rsa(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            byte[] encryptedData;
            using (RSACryptoServiceProvider _RSA = new())
            {
                _RSA.ImportParameters(RSAKey);
                encryptedData = _RSA.Encrypt(Data, DoOAEPPadding);
            }
            return encryptedData;
        }
        static public byte[] DecryptionStringToBytes_RSA(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            byte[] decryptedData;
            using (RSACryptoServiceProvider _RSA = new())
            {
                _RSA.ImportParameters(RSAKey);
                decryptedData = _RSA.Decrypt(Data, DoOAEPPadding);
            }
            return decryptedData;
        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
        private void Window_main_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void Window_main_Deactivated(object sender, EventArgs e)
        {
            this.Activate();
        }
        private void CreateSign(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes("Hello, from the .NET Docs!");
            byte[] hash = alg.ComputeHash(data);

            // Generate signature
            RSA rSA = RSA.Create();
            sharedParameters = rSA.ExportParameters(false);

            RSAPKCS1SignatureFormatter rsaFormatter = new(rSA);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));

            signedHash = rsaFormatter.CreateSignature(hash);
            DigSig.Text = System.Convert.ToBase64String(signedHash);
        }

        private void VerifySign(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes("Hello, from the .NET Docs!");
            byte[] hash = alg.ComputeHash(data);

            // Generate signature
            RSA rSA = RSA.Create();
            rSA.ImportParameters(sharedParameters);

            RSAPKCS1SignatureDeformatter rsaDeformatter = new(rSA);
            rsaDeformatter.SetHashAlgorithm(nameof(SHA256));

            DigVer.Text = (rsaDeformatter.VerifySignature(hash, signedHash)) ? "The signature is valid" : "The signature is not valid";
        }
        private void checkThePass(ref string validate)
        {
            if (validate.Length < 16)
            {
                for (int i = validate.Length; i < 16; i++)
                {
                    validate += "0";
                }
            }
            if (validate.Length > 16)
            {
                validate = validate.Remove(16);
            }
        }
    }
}
