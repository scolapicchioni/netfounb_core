using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleApplication
{
    public class Program
    {
        static byte[] key = {145,12,32,245,98,132,98,214,6,77,131,44,221,3,9,50};
        static byte[] iv = {15,122,132,5,93,198,44,31,9,39,241,49,250,188,80,7};
            
        const string FILENAME = "./encrypted.bin";
        public static void Main(string[] args)
        {
            //symmetric_encryption();
            //symmetric_decryption();
            //asymmetric_encryption_decryption();
            certificates();
        }

        private static void symmetric_encryption()
        {
            string message = "OH HI! THIS IS A SECRET MESSAGE!";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message); // or we just make up something like {1,2,3,4,5}; 
            using(SymmetricAlgorithm algorithm = Aes.Create()){
                using(ICryptoTransform encriptor = algorithm.CreateEncryptor(key,iv)){
                    using(Stream f = File.Create(FILENAME)){
                        using(Stream c = new CryptoStream(f,encriptor, CryptoStreamMode.Write)){
                            c.Write(data,0,data.Length);
                        }
                    }
                }
            }
            System.Console.WriteLine("String encrypted in " + FILENAME);
        }

        private static void symmetric_decryption(){
            List<byte> decripted = new List<byte>();
            using(SymmetricAlgorithm algo = Aes.Create()){
                using(ICryptoTransform decriptor = algo.CreateDecryptor(key,iv)){
                    using(Stream f = File.OpenRead(FILENAME)){
                        using(Stream c = new CryptoStream(f,decriptor,CryptoStreamMode.Read)){
                            for (int b = 0; (b = c.ReadByte())>-1;){
                                decripted.Add((byte)b);                       
                            }
                        }
                    }
                }
            }
            string decodedMessage = System.Text.Encoding.UTF8.GetString(decripted.ToArray());
            System.Console.WriteLine(decodedMessage);
            System.Console.ReadLine();
        }


        private static void more_complex_symmetric_encryption()
        {
            string password = "Pa$$w0rd";
            string message = "OH HI! THIS IS A SECRET MESSAGE!";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message); 

            using(SymmetricAlgorithm algorithm = Aes.Create()){
                using(RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create()){
                    // salt has to be as big as the blocksize. Blocksize is in bits, salt size is in bytes!
                    byte[] salt = new byte[algorithm.BlockSize / 8];
                    randomNumberGenerator.GetBytes(salt);
                    using(Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, salt)){
                        //key size is in bits!
                        byte[] key = keyGenerator.GetBytes(algorithm.KeySize / 8);
                        byte[] iv = algorithm.IV;
                        algorithm.Key = key;
                    }
                }
            
                using(ICryptoTransform encryptor = algorithm.CreateEncryptor(key,iv)){
                    using(Stream f = File.Create(FILENAME)){
                        using(Stream c = new CryptoStream(f,encryptor, CryptoStreamMode.Write)){
                            c.Write(data,0,data.Length);
                        }
                    }
                }
            }
            System.Console.WriteLine("String encrypted in " + FILENAME);
        }

        private static void asymmetric_encryption_decryption(){
            var rawBytes = Encoding.UTF8.GetBytes("hello world...");
            var decryptedText = string.Empty;

            using (var rsaProvider = System.Security.Cryptography.RSA.Create()) {
            
                var encryptedBytes = rsaProvider.Encrypt(rawBytes, RSAEncryptionPadding.OaepSHA512);

                var decryptedBytes = rsaProvider.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA512);

                decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                
                Console.WriteLine("KeySize: " + rsaProvider.KeySize);
            
                var parameters = rsaProvider.ExportParameters(true);
                
            }
            // decryptedText == hello world..
            Console.WriteLine(decryptedText);
            
            Console.ReadLine();
        }

        private static void certificates(){
            using(var store = new X509Store(StoreName.My, StoreLocation.LocalMachine)){
                store.Open(OpenFlags.ReadOnly);
                foreach (var storeCertificate in store.Certificates)
                {
                    var privateKey = storeCertificate.GetRSAPrivateKey();
                }
            }

        }
    }
}
