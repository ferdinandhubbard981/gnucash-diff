using CommandLine;
using NC = NetCash;
using GNCDiff;


class Program
{

    public class Options
    {
        [Option('a', "file_1", Required = true, HelpText = "first gnucash file")]
        public required string file_1 {get; set;}
        [Option('b', "file_2", Required = true, HelpText = "second gnucash file")]
        public required string file_2 {get; set;}
    }
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
        {

            NC.GnuCashEngine.Initialize();
            NC.Book  oldBook = NC.Book .OpenRead(o.file_1);
            NC.Book  newBook = NC.Book .OpenRead(o.file_2);
            Diff diff = Diff.FromBooks(oldBook, newBook);
            Console.WriteLine("file1: {0}", o.file_1);
        });
    }
}
