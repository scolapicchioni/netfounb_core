using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //File.ReadAllLines
            //readAllLines();

            //File.ReadAllText
            //readAllText();

            //File.WriteAllLines
            //writeAllLines();

            //FileInfo
            //fileInfo();

            //Streams And Friends
            //streams();

            //ZippityZap
            //zipping();
            //unzipping("./zippity.zip",".");

            //gZipCompress
            //gZipCompress(new FileInfo("./target.txt"));
            //gZipDecompress(new FileInfo("./target.txt.gz"));

            //fileSystemWatching();

            //dataContractSerialization();

            //newtonsoft();
            Console.ReadLine();
        }

        private static void newtonsoft()
        {
            string courseUri = "http://www.infosupport.com/ISTraining.External/trainingservice/nl/courses?find=C#";
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string result = client.GetStringAsync(courseUri).Result;
            CourseSummary[] courses = Newtonsoft.Json.JsonConvert.DeserializeObject<CourseSummary[]>(result);
            foreach (var item in courses)
            {
                Console.WriteLine(item.Name);
            }

        }

        private static void dataContractSerialization()
        {
            PersonForDataContractXmlSerialization p = new PersonForDataContractXmlSerialization(){Id = 1, Name="Mario", Surname="Super"};
            DataContractSerializer dcs = new DataContractSerializer(typeof(PersonForDataContractXmlSerialization));
            using(var stream = new FileStream("./personDataContract.xml",FileMode.Create,FileAccess.Write)){
                //XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateTextWriter(stream,Encoding.UTF8 );
                dcs.WriteObject(stream, p);
            }
            System.Console.WriteLine("personDataContract.xml created");
        }

        //not implemented in .net Core!
        private static void binarySerialization()
        {
            // PersonForBinaryFormatter person = new PersonForBinaryFormatter(){Id = 1, Name="Mario", Surname="Super"};
            // IFormatter formatter = new BinaryFormatter();
            // FileStream buffer = File.Create(@"C:\somepath\person.bin");
            // formatter.Serialize(buffer, person);
            // buffer.Close();

        }

        private static void fileSystemWatching()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = "C:\\mymonitoredfolder";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | 	NotifyFilters.DirectoryName;
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories=true;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
            Console.ReadLine();
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            System.Console.WriteLine($"{e.OldFullPath} {e.ChangeType} to {e.Name}");
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            System.Console.WriteLine($"{e.FullPath} {e.ChangeType}");
        }

        public static void gZipDecompress(FileInfo fileToDecompress){
            using (FileStream originalFileStream = fileToDecompress.OpenRead()) {
                using (FileStream decompressedFileStream = File.Create("./ungzippedcontent.txt")) {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress)) {          
                        decompressionStream.CopyTo(decompressedFileStream);
                        System.Console.WriteLine("decompressed to ungzippedcontent.txt");
                    } 
                } 
            } 
        }

        private static void unzipping(string zipFullName, string extractPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFullName)) {
                foreach (ZipArchiveEntry entry in archive.Entries) {
                    System.Console.WriteLine("Found file: " + entry.FullName);
                    if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)) {
                        string destination = Path.Combine(extractPath, entry.FullName);
                        if(File.Exists(destination))
                            File.Delete(destination);
                        entry.ExtractToFile(destination);
                        System.Console.WriteLine("Extracted to " + destination); 
	                } 
                } 
            }
        }

        public static void gZipCompress(FileInfo fileToCompress) {
            using (FileStream originalFileStream = fileToCompress.OpenRead()) {
                string targetFile = $"{fileToCompress.FullName}.gz";
                using (FileStream compressedFileStream = File.Create(targetFile)) {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,CompressionMode.Compress)) {
                        originalFileStream.CopyTo(compressionStream);
                        System.Console.WriteLine($"{fileToCompress.FullName} compressed to {targetFile}");
                    } 
                } 
            } 
        }

        private static void streams(){
            using(FileStream s1 = new FileStream("./target.txt",FileMode.Create,FileAccess.Write) /* File.Open("./target.txt",FileMode.Create,FileAccess.Write); */ ){
                // using(BinaryWriter bw = new BinaryWriter(s1)){
                //     bw.Write(42);
                // }
                    
                using(StreamWriter s2 = new StreamWriter(s1)){
                    s2.WriteLine("Oh Hi!");
                }
            }
            //s1.Flush();
            //s1.Dispose();
            System.Console.WriteLine("done");


        }
        private static void fileInfo(){
            FileInfo f = new FileInfo("./target.txt");
            System.Console.WriteLine($"f.CreationTime:\t{f.CreationTime}\r\nf.DirectoryName:\t{f.DirectoryName}\r\nf.FullName:\t{f.FullName}\r\nf.LastAccessTime:\t{f.LastAccessTime}\r\n"); 
        }
        private static void writeAllLines(){
            bool exists = File.Exists("./target.txt");
            File.WriteAllLines("./target.txt",new string[]{"Dear Student,", "", "With this code you can write multiple lines on a newly created file.", "Sincerely,", "Your Teacher"});
            System.Console.WriteLine($"Written on {(!exists ? "newly created" : "")} target.txt");
        }
        private static void readAllText(){
            string text = File.ReadAllText("./project.lock.json");
            System.Console.WriteLine(text);            
        }
        private static void readAllLines(){
            string[] lines = File.ReadAllLines("./project.lock.json");
            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"Line {i} : {lines[i]}");
            }
        }


    }
}
