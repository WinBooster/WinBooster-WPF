using Org.BouncyCastle.Asn1.Crmf;
using System;
using WinBoosterNative.database.sha3;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("File path: ");
            string path = Console.ReadLine().Replace("\"", "");
            FileInfo file = new FileInfo(path);
            var bytes = File.ReadAllBytes(file.FullName);

            Console.WriteLine(file.Name + ": " + SHA3DataBase.GetHashString(SHA3DataBase.GetHash(bytes)));
            Console.ReadKey();
        }
    }
}