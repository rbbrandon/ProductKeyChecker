using System;
using System.Text.RegularExpressions;

namespace ProductKeyChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ProductKeyChecker";
            string key = string.Empty;

            for (int position = 0; position < args.Length; position++)
            {
                switch (args[position].ToUpper())
                {
                    case "/H":
                    case "/HELP":
                    case "/?":
                        // Display ProductKeyChecker help message.
                        string fileName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
                        Console.WriteLine("Retrieves the product description for a specified Windows product key, or a key stored in the local system's firmware.");
                        Console.WriteLine();
                        Console.WriteLine(fileName + " [/? | /H | /HELP | /OEM | /KEY productKey]");
                        Console.WriteLine();
                        Console.WriteLine("  /?          Displays this help message.");
                        Console.WriteLine("  /H          Same as /?.");
                        Console.WriteLine("  /HELP       Same as /?.");
                        Console.WriteLine("  /OEM        Use embedded product key.");
                        Console.WriteLine("  /KEY        Use specified product key.");
                        Console.WriteLine("  productKey  Specifies product key to use.");
                        Console.WriteLine();
                        Console.WriteLine("Icon made by Freepik from www.flaticon.com");
                        Console.WriteLine();
                        return;
                    case "/OEM":
                        // Get MSDM Table Key (UEFI OEM Key):
                        key = ACPIUtils.GetOEMKey();
                        break;
                    case "/KEY":
                        // Use key specified on command line.
                        if (args[position + 1] != null)
                            key = args[position + 1];

                        break;
                }
            }
            
            if (key == string.Empty)
            {
                // No key method was specified at command line, prompt for a key:
                Console.WriteLine("Please enter in a Windows 8-10 product key to test, or leave blank to test the embedded product key if available.");

                // Get MSDM Table Key (UEFI OEM Key):
                string oemKey = ACPIUtils.GetOEMKey();

                if (oemKey == string.Empty)
                    Console.Write("Enter product key to test : ");
                else
                    Console.Write("Enter product key to test [default: " + oemKey + "] : ");

                key = Console.ReadLine();

                if (key == string.Empty)
                {
                    // Use embedded key
                    key = oemKey;

                    if (key == string.Empty)
                    {
                        Console.WriteLine("Reading embedded product key failed. System may not have an embedded key.");
                        return;
                    }
                }
            }
            
            Console.Title = "Testing Key: \"" + key + "\"";
            Regex keyRegex = new Regex(@"^([A-Za-z0-9]{5}-){4}[A-Za-z0-9]{5}$");

            if (!keyRegex.IsMatch(key))
            {
                Console.WriteLine("Invalid Key: \"" + key + "\"");
                return;
            }

            ProductKey productKey = new ProductKey(key);

            string description = productKey.GetProductDescription(ProductKey.Edition.Win10);

            if (description.Contains("Invalid") || description.Contains("not supported"))
                description = productKey.GetProductDescription(ProductKey.Edition.Win8);

            Console.WriteLine(description);
        }
    }
}
