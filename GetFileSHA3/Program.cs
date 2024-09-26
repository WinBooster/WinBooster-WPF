using Org.BouncyCastle.Asn1.Crmf;
using System;
using WinBoosterNative.database.sha3;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Tuple<string, string>> database = new List<Tuple<string, string>>();

            start:

            foreach (Tuple<string, string> element in database)
            {
                Console.WriteLine(element.Item1 + " | " + element.Item2);
            }

            Console.Write("File path: ");
            string path = Console.ReadLine().Replace("\"", "");
            FileInfo file = new FileInfo(path);
            Console.WriteLine("Getting bytes...");
            var bytes = File.ReadAllBytes(file.FullName);
            Console.WriteLine("Getting hash...");
            string hash = SHA3DataBase.GetHashString(SHA3DataBase.GetHash(bytes));
            database.Add(new Tuple<string, string>(file.Name, hash));
            Console.Clear();
            goto start;
        }
    }
}