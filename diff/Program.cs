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
            // NC.GnuCashEngine.Shutdown();
            NC.GnuCashEngine.Initialize();

            // Book book = Book.FromGNCFile("/home/ferdi/gnucash-version-control/diff.Tests/test_data/single_account.gnucash");
            // Diff diff = Diff.FromBooks(oldBook, newBook);
            // Console.WriteLine("file1: {0}", o.file_1);
        });
    }
}
