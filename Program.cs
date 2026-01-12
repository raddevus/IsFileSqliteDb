using System;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run <file-path>");
            return;
        }

        string filePath = args[0];

        if (File.Exists(filePath))
        {
           var isHeaderValid = IsSQLiteDatabase(filePath);
           if (!isHeaderValid){
              Console.WriteLine("The file header is incorrect. Not a valid sqlite db or file is corrupt.");
              return;
           }
           Console.WriteLine("Sqlite header found.  Possible sqlite db.");
           var isDbIntegrity = CheckDatabaseIntegrity(filePath);
           if (!isDbIntegrity){
              Console.WriteLine("File header seems right, but database may be corrupt. Integrity check failed.");
              return;
           }
           Console.WriteLine("File integrity check succeeded.");
           Console.WriteLine("The file seems to be a valid SQLite database.");
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

   static bool CheckDatabaseIntegrity(string filePath){
      // we can check that the size of the file 
      // is evenly divisible by 4096 (page size)
      long fileSize = new FileInfo(filePath).Length;
      return fileSize % 4096 == 0;
   }

    static bool IsSQLiteDatabase(string filePath)
    {
        byte[] expectedHeader = Encoding.ASCII.GetBytes("SQLite format 3\0");

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            byte[] headerBytes = new byte[expectedHeader.Length];
            fs.Read(headerBytes, 0, expectedHeader.Length);

            return headerBytes.SequenceEqual(expectedHeader);
        }
    }
}
