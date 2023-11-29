using Org.BouncyCastle.Asn1.Crmf;
using System;
using WinBoosterNative.database.sha3;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileInfo file = new FileInfo(@"C:\Program Files\WinBooster\Scripts\LastActivity Cleaner.cs");
            var bytes = File.ReadAllBytes(file.FullName);

            Console.WriteLine(file.Name + ": " + SHA3DataBase.GetHashString(SHA3DataBase.GetHash(bytes)));
        }
    }
}