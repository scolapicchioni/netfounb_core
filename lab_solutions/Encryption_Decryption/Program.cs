using System;
using System.IO;
using System.Security.Cryptography;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            menu();

        }

        private static void menu(){
            string choice = "";
            while(choice.ToUpper() != "X"){
                Console.Clear();
                System.Console.WriteLine("1. Encrypt");
                System.Console.WriteLine("2. Decrypt");
                System.Console.WriteLine("X. Exit");
                System.Console.Write("Please enter your choice then press Enter: ");
                choice = Console.ReadLine();
                switch(choice){
                    case "1":
                        encrypting();
                        break;
                    case "2":
                        decrypting();
                        break;
                    default:
                        break;
                }
            }
        }

        private static void decrypting()
        {
            System.Console.WriteLine("DECRYPTION.");
            System.Console.Write("Please enter your password: ");
            string password = Console.ReadLine();
            System.Console.Write("Please enter the path of the file to decrypt: ");
            string filename = Console.ReadLine();
            decrypt(password,filename);
            Console.ReadLine();
        }

        private static void encrypting()
        {
            System.Console.WriteLine("ENCRYPTION.");
            System.Console.Write("Please enter your password: ");
            string password = Console.ReadLine();
            System.Console.Write("Please enter the path of the file to encrypt: ");
            string filename = Console.ReadLine();
            encrypt(password,filename);
            Console.ReadLine();
        }

        private static void encrypt(string password, string fileToEncrypt){
            using(Aes algorithm = Aes.Create()){
                using(RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create()){
                    // salt has to be as big as the blocksize. Blocksize is in bits, salt size is in bytes!
                    byte[] salt = new byte[algorithm.BlockSize / 8];
                    randomNumberGenerator.GetBytes(salt);
                    using(Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, salt)){
                        //key size is in bits!
                        byte[] key = keyGenerator.GetBytes(algorithm.KeySize / 8);
                        byte[] iv = algorithm.IV;
                        algorithm.Key = key;

                        using(ICryptoTransform cryptoTransformer = algorithm.CreateEncryptor()){
                            byte[] data = File.ReadAllBytes(fileToEncrypt);
                            using(FileStream fStream = File.Open($"{fileToEncrypt}.encrypted", FileMode.OpenOrCreate)){
                                using(BinaryWriter bWriter = new BinaryWriter(fStream)){
                                    bWriter.Write(salt.Length);
                                    bWriter.Write(salt);
                                    bWriter.Write(iv.Length);
                                    bWriter.Write(iv);

                                    using(CryptoStream cStream = new CryptoStream(fStream, cryptoTransformer, CryptoStreamMode.Write)){
                                        using(BinaryWriter sWriter = new BinaryWriter(cStream)){
                                            sWriter.Write(data);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Encryption completed.\r\n{fileToEncrypt}.encrypted has been created.");
        }

        private static void decrypt(string password, string filename){
            using(FileStream fStream = File.OpenRead(filename)){
                using(BinaryReader reader = new BinaryReader(fStream)){
                    int saltLength = reader.ReadInt32();
                    byte[] salt = reader.ReadBytes(saltLength);
                    int ivLength = reader.ReadInt32();
                    byte[] iv = reader.ReadBytes(ivLength);

                    using(Aes algorithm = Aes.Create()){
                        using(Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, salt)){
                            byte[] key = keyGenerator.GetBytes(algorithm.KeySize / 8);

                            using(ICryptoTransform cryptoTransformer = algorithm.CreateDecryptor(key, iv)){
                                using(CryptoStream cStream = new CryptoStream(fStream, cryptoTransformer, CryptoStreamMode.Read)){
                                    using(BinaryReader sReader = new BinaryReader(cStream)){
                                        byte[] data = sReader.ReadBytes((int)fStream.Length - 4 - saltLength - 4 - ivLength);
                                        using(FileStream outStream = File.Open(filename.Replace("encrypted", "decrypted"), FileMode.OpenOrCreate)){
                                            outStream.Write(data, 0, data.Length);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            System.Console.WriteLine($"Decryption completed.\r\n {(filename.Replace("encrypted", "decrypted"))} has been created");
        }
    }
}
