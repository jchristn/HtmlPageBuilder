using Touchstone.Cli;
using Test.Shared;

string resultsPath = null;

for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--results" && i + 1 < args.Length)
    {
        resultsPath = args[i + 1];
        break;
    }
}

return await ConsoleRunner.RunAsync(HtmlPageBuilderSuites.All, resultsPath: resultsPath);
