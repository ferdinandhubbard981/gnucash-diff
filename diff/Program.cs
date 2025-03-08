using System;
using System.Security.Cryptography.X509Certificates;
using CommandLine;


class Program
{

    public class Options
    {
        [Option('a', "file_1", Required = true, HelpText = "first gnucash file")]
        public string file_1 {get; set;}
        [Option('b', "file_2", Required = true, HelpText = "second gnucash file")]
        public string file_2 {get; set;}
    }
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
        {
            Console.WriteLine("file1: {0}", o.file_1);
        });
    }
}
