using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace Crawler
{
    class Program
    {

        static void Main(string[] args)
        {
            string serverName = string.Empty;

            //if no arguments is passed from console app
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a server name/argument.");
                serverName = Console.ReadLine();
            }
            else
            {
                // get the server name
                serverName = args[0];
            }

            List<string> Faliedurls = new List<string>();

            string passedUrl = $"https://{serverName}/sitemap.xml";
            List<string> urls = new List<string>();

            using (var webClient = new WebClient())
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                using (XmlTextReader reader = new XmlTextReader(passedUrl))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name.ToString() == "loc")
                            {
                                var url = reader.ReadString();

                                urls.Add(url);
                            }
                        }
                    }
                }

                foreach (var url in urls)
                {
                    Console.WriteLine("Current url: " + url);

                    // hit the URL
                    try
                    {
                        webClient.DownloadString(url);
                    }
                    catch (WebException webEx)
                    {
                        Console.WriteLine(webEx.Message);
                        Faliedurls.Add(url);
                    }
                    catch (Exception EX)
                    {
                        Console.WriteLine(EX.Message);
                        Faliedurls.Add(url);
                    }
                }
            }


            if (Faliedurls.Count >= 0)
            {
                Console.WriteLine($"\n**********  Failed to fetch below URLs: **********\n");
                foreach (var failed in Faliedurls)
                {
                    Console.WriteLine(failed);
                }
            }

            Console.ReadLine();
        }
    }
}
