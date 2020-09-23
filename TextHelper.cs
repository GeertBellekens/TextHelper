using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TextHelper
{
    public class TextHelper
    {
        /// <summary>
        /// adds '" ' (double quote + space) at the front of the each line (except on the first line, there its only a double quote
        /// adds ' " & vbNewLine & _' to each line (with the appropriate amount of spaces, making each line the same length, except for the last line, there the ' & vbNewLine & _ is not added
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string formatVbScriptQuery(string query)
        {
            int maxLineLenght = 0;
            //get max length of the line in the string
            using (StringReader reader = new StringReader(query))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var lineLenght = ExpandTabs("\" " + line).Length + 2; 
                    if (lineLenght > maxLineLenght)
                    {
                        maxLineLenght = lineLenght;
                    }
                }
            }
            string previousLine = null;
            var newString = string.Empty;
            //now loop again over the lines and add 
            using (StringReader reader = new StringReader(query))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (previousLine != null)
                    {
                        newString += formatLine(previousLine, false, newString == string.Empty, maxLineLenght);
                    }
                    //set the previous line
                    previousLine = line;
                }
            }
            //do the last line
            if (previousLine != null)
            {
                newString += formatLine(previousLine, false, true, maxLineLenght);
            }
            //return
            return newString;
        }
        private static string formatLine(string line, bool firstLine, bool lastLine, int maxLineLenght)
        {
            var formattedLine = string.Empty;
            var prefix = "\"";
            if (!firstLine) prefix += " "; //don't extra space for first line
            //get number of spaces to add
            var spacedNeeded = maxLineLenght - ExpandTabs(prefix + line).Length + 1;
            //add the end of the string
            formattedLine = prefix + line + String.Empty.PadLeft(spacedNeeded, ' ') + "\"";
            if (!lastLine) formattedLine += " & vbNewLine & _" + Environment.NewLine;
            return formattedLine;
        }

        public static string ExpandTabs(string input, int tabLength = 4)
        {
            string[] parts = input.Split('\t');
            int count = 0;
            int maxpart = parts.Count() - 1;
            foreach (string part in parts)
            {
                if (count < maxpart)
                    parts[count] = part + new string(' ', tabLength - (part.Length % tabLength));
                count++;
            }
            return (string.Join("", parts));
        }
        public static string TabsToSpaces(string inTxt, int tabLen = 4)
        {
            var outTxt = new List<string>();

            var textValues = inTxt.Split('\t');

            foreach (var val in textValues)
            {
                var lines = val.Split('\r');
                var preTxt = lines[lines.Length - 1];
                preTxt = preTxt.Replace("\n", "");
                var numSpaces = tabLen - preTxt.Length % tabLen;
                if (numSpaces == 0)
                    numSpaces = tabLen;
                outTxt.Add(val + new string(' ', numSpaces));
            }
            return String.Join("", outTxt);
        }
    }
}
