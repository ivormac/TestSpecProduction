using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jira = Atlassian.Jira;
using Json = Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Word = Microsoft.Office.Interop.Word;
using TestRail;
//using TestRail = Gurock.TestRail;
using System.Net.NetworkInformation;
using System.Web.UI;
using RestSharp.Extensions;

namespace TestSpecProduction
{
    static class TestSpecProduction
    {
        private static string TestRailServer = "https://novsqskal.testrail.io/";
        private static string userName = "ivor.mccormack@sqs.com";
        private static string trPwd = "19New03Begin";
        // private static string jiraPwd = "19New03Begin";

        private static ulong trProjectId = 1;     // Default to "EMEA PQT"
        private static ulong trSuiteId = 31;      // Default to "WearLenses 3.1 (Including 3.0.1)"
        private static ulong trSectionId = 407;   // Default to "WL30-514 - Tasks to Stories - CIT.SAL.PQ.001.EasyWL3.1.0"
        private static ulong trTestCaseId = 4301; // Degfault to "Set the password maximum length on Wearlenses to 30 characters ( Forgot Your password )"

        static string RemoveBefore( this string value , string character )
        {
            int index = value.IndexOf ( character );
            if ( index > 0 )
            {
                value = value.Substring ( index + 1 );
            }
            return value;
        }


        static void Main(string[] args)
        {

            if ( args.Length == 0 )
            {
                Console.WriteLine ( "You must provide TestRail username, password, project id, suite id, section id to run this process." );
                Console.WriteLine ( "-user <username>" );
                Console.WriteLine ( "-pwd <password>" );
                Console.WriteLine ( "-project <id>" );
                Console.WriteLine ( "-suite <id>" );
                Console.WriteLine ( "-section <id>" );
                Console.WriteLine ( "All parameters must be entered!" );
                return;
            }
            else
            {
                if ( args.Length < 5 )
                {
                    Console.WriteLine ( "Not all params have been supplied." );
                    Console.WriteLine ( "You must provide TestRail username, password, project id, suite id, section id to run this process." );
                    Console.WriteLine ( "-user <username>" );
                    Console.WriteLine ( "-pwd <password>" );
                    Console.WriteLine ( "-project <id>" );
                    Console.WriteLine ( "-suite <id>" );
                    Console.WriteLine ( "-section <id>" );
                    Console.WriteLine ( "All parameters must be entered!" );
                    return;
                }
                else
                {
                    userName = RemoveBefore ( args [ 0 ] , " " );
                    trPwd = RemoveBefore ( args [ 1 ] , " " );
                    trProjectId = Convert.ToUInt64 ( args [ 2 ] );     // Puts project argument into correct variable
                    trSuiteId = Convert.ToUInt64 ( args [ 3 ] );     // Puts suite argument into correct variable
                    trSectionId = Convert.ToUInt64 ( args [ 4 ] );     // Puts section argument into correct variable
                }
            }

            TestRailClient trClient = new TestRailClient ( TestRailServer, userName, trPwd );

             trProject = ( JObject ) trClient.GetProject(trProjectId);
            JObject trSuite = ( JObject ) trClient.GetSuite(trSuiteId);
            JObject trSection = ( JObject ) trClient.GetSection(trSectionId);
            Console.WriteLine ( "The Project: " + trProject [ "name" ] );
            Console.WriteLine ( "The Test Suite: " + trSuite [ "name" ] );
            Console.WriteLine ( "The Test Section: " + trSection [ "name" ] );

            JArray tsTestCases = ( JArray ) trClient.SendGet ( "get_cases/" + trProjectId.ToString ( ) + "&suite_id=" + trSuiteId.ToString ( ) + "&section_id=" + trSectionId.ToString ( ) );
            Console.WriteLine ( "There are "+tsTestCases.Count()+" test cases" );

            for ( int i = 0; i < tsTestCases.Count ( ); i++ )
            {
                Console.WriteLine ( "Test Case "+ i.ToString() + " - " + tsTestCases[i]);
            }


        }

    }

}

