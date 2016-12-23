using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FlashcardsGenerator.Source.Dictionaries
{
    class Collins
    {
        private CommonFunctions commonFunctions = new CommonFunctions();

        public CQ GetDom(string word, string proxy)
        {
            string url = commonFunctions.LookUpUrl(Dictionary.COLLINS_DOMAIN, Dictionary.COLLINS_URL_FR_EN, word);
            return commonFunctions.GetDomPage(url, proxy);
        }

        public string GetTitle(CQ dom)
        {
            return dom["title"].Text();
        }

        public string GetWord(CQ dom)
        {
            return commonFunctions.GetTextElement(dom, "h2[class=orth h1_entry]", 0);
        }

        public string GetWordType(CQ dom)
        {
            IDomObject wordTypeElements = commonFunctions.GetDomElement(dom, "span[class=pos]", 0);
            if (wordTypeElements == null)
                return "There is no type for this word.";

            string wordTypes = "(" + wordTypeElements.OuterHTML + ")&nbsp;";

            for (int i = 1; i < 4; i++)
            {
                try
                {
                    wordTypes += "(" + commonFunctions.GetDomElement(dom, "span[class=pos]", i).OuterHTML + ")&nbsp;";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }

            return wordTypes;
        }

        public string GetPhonetic(CQ dom)
        {
            string phonetic = commonFunctions.GetTextElement(dom, "span[class=pron]", 0);
            phonetic = phonetic.Replace(Constant.CR, string.Empty);
            phonetic = phonetic.Replace(Constant.LF, string.Empty);
            phonetic = Regex.Replace(phonetic, "<span class=\\\"hwd_sound\\\">.*</span>", string.Empty);
            phonetic = phonetic.Replace("(", "/").Replace(")", "/");

            return phonetic;
        }

        public string GetExamples(CQ dom, string word)
        {
            IDomObject exampleElements = commonFunctions.GetDomElement(dom, "span[class=phr]", 0);
            if (exampleElements == null)
                return Dictionary.NO_EXAMPLE;

            string examples = exampleElements.OuterHTML + "<br>";
            for (int i = 1; i < 4; i++)
            {
                try
                {
                    examples += commonFunctions.GetDomElement(dom, "span[class=phr]", i).OuterHTML + "<br>";
                }
                catch (Exception e)
                {
                    Console.WriteLine(MessageBoxProps.ERROR_OCCUR + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                    break;
                }
            }

            examples = examples.ToLower();

            string pattern = string.Format(@"({0})([^\W_]*?[<>/\]*?[^\W_]*?[<>/\]*?)([\s.])", word);
            examples = Regex.Replace(examples, pattern, m => "{{c1::" + m.Groups[1].Value + "}}" + m.Groups[2].Value + m.Groups[3].Value);
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"main.css\">" + examples;

            return examples;
        }

        public string GetPronunciation(CQ dom, string proxy, string selector)
        {
            string pro_link = commonFunctions.GetElementAttribute(dom, selector, 0, "data-src-mp3");

            if (pro_link.Equals(string.Empty))
            {
                return string.Empty;
            }
            else
            {
                pro_link = "http://www.collinsdictionary.com" + pro_link;
            }

            string pro_name = pro_link.Split('/')[pro_link.Split('/').Length - 1];

            Stream input = commonFunctions.GetStream(pro_link, proxy).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\sounds\" + pro_name);
            commonFunctions.CopyStream(input, output);
            output.Close();

            return "[sound:" + pro_name + "]";
        }

        public string GetImages(CQ dom, string word, string proxy, string selector, string attr)
        {
            string img_link = commonFunctions.GetElementAttribute(dom, selector, 0, attr);

            if (img_link == string.Empty)
                return "<a href=\"https://www.google.com.vn/search?biw=1280&bih=661&tbm=isch&sa=1&q=" + word + "\" style=\"font-size: 15px; color: blue\">Images for this word</a>";

            string img_name = img_link.Split('/')[img_link.Split('/').Length - 1];
            if (attr.Equals(string.Empty))
                img_name = "fullsize_" + img_name;

            Stream input = commonFunctions.GetStream(img_link, proxy).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\images\" + img_name);
            commonFunctions.CopyStream(input, output);

            return "<img src=\"" + img_name + "\"/>";
        }

        public string GetMeaning(CQ dom)
        {
            IDomObject domContent = commonFunctions.GetDomElement(dom, "div.homograph-entry", 0);

            IDomObject wordFrequency = commonFunctions.GetDomElement(dom, "div.word-frequency-container.res_hos", 0);
            if (wordFrequency != null) wordFrequency.Remove();

            IDomObject socialButtons = commonFunctions.GetDomElement(dom, "div.socialButtons", 0);
            if (socialButtons != null) socialButtons.Remove();

            string htmlContent =
                "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"collins_common.css\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                "<div class=\"responsive_entry_center_wrap\">" + domContent.OuterHTML + "</div>" + "</html>";

            htmlContent = htmlContent.Replace(Constant.TAB, string.Empty);
            htmlContent = htmlContent.Replace(Constant.CR, string.Empty);
            htmlContent = htmlContent.Replace(Constant.LF, string.Empty);
            htmlContent = Regex.Replace(htmlContent, "<span class=\"hwd_sound\">.*?</span>", string.Empty);

            return htmlContent;
        }
    }
}
