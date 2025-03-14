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
            Book before = Book.FromGNCFile(o.file_1);
            Book after = Book.FromGNCFile(o.file_2);
            Diff diff = Diff.FromBooks(before, after);
            Console.WriteLine(diff.ToDiffString());
        });
    }
}
