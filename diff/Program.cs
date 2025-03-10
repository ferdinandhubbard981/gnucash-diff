using CommandLine;
using NetCash;


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

            GnuCashEngine.Initialize();
            Book oldBook = Book.OpenRead(o.file_1);
            Book newBook = Book.OpenRead(o.file_2);
            Diff diff = Diff.FromBooks(oldBook, newBook);
            Console.WriteLine("file1: {0}", o.file_1);
        });
    }
}
