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
            if (IsSQLiteDatabase(filePath))
                Console.WriteLine("The file is a valid SQLite database.");
            else
                Console.WriteLine("The file is NOT a SQLite database.");
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

    static bool IsSQLiteDatabase(string filePath)
    {
        byte[] expectedHeader = Encoding.ASCII.GetBytes("SQLite format 3\0");

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            byte[] headerBytes = new byte[expectedHeader.Length];
            fs.Read(headerBytes, 0, expectedHeader.Length);

            return headerBytes.SequenceEqual(expectedHeader);
        }
    }
}
