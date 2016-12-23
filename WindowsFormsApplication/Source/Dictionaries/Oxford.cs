using System;
using System.IO;
using CsQuery;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FlashcardsGenerator.Source.Dictionaries
{
    class Oxford
    {
        private CommonFunctions commonFunctions = new CommonFunctions();

        public CQ GetDom(string word, string proxy)
        {
            string url = commonFunctions.LookUpUrl(Dictionary.OXFORD_DOMAIN, Dictionary.OXFORD_URL_EN_EN, word);
            return commonFunctions.GetDomPage(url, proxy);
        }

        public string GetTitle(CQ dom)
        {
            return dom["title"].Text();
        }

        public string GetWord(CQ dom)
        {
            return commonFunctions.GetTextElement(dom, "h2", 0);
        }

        public string GetMeaning(CQ dom)
        {
            IDomObject domContent = commonFunctions.GetDomElement(dom, "#entryContent", 0);
            string htmlContent = "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"interface.css\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"oxford.css\">" +
                                 "<div class=\"responsive_entry_center_wrap\">" + domContent.OuterHTML +
                                 "</div>" + "</html>";

            htmlContent = htmlContent.Replace(Constant.TAB, string.Empty);
            htmlContent = htmlContent.Replace(Constant.CR, string.Empty);
            htmlContent = htmlContent.Replace(Constant.LF, string.Empty);
            htmlContent = htmlContent.Replace("class=\"unbox\"", "class=\"unbox is-active\"");

            return htmlContent;
        }

        public string GetWordType(CQ dom)
        {
            string wordType = commonFunctions.GetTextElement(dom, "span[class=pos]", 0);
            return wordType.Equals(string.Empty) ? string.Empty : "(" + wordType + ")";
        }

        public string GetPhonetic(CQ dom)
        {
            string phoneticBrE = commonFunctions.GetTextElement(dom, "span[class=phon]", 0);
            string phoneticNAmE = commonFunctions.GetTextElement(dom, "span[class=phon]", 1);
            string phonetic = phoneticBrE + phoneticNAmE;

            phonetic = phonetic.Replace("<span class=\"separator\">/</span>", string.Empty);
            phonetic = phonetic.Replace("<span class=\"bre\">BrE</span><span class=\"wrap\">/</span>", "BrE /");
            phonetic = phonetic.Replace("<span class=\"wrap\">/</span><span class=\"name\">NAmE</span><span class=\"wrap\">/</span>", "/  &nbsp; NAmE /");
            phonetic = "<span class=\"phon\">" + phonetic + "</span>";

            return phonetic;
        }

        public string GetExamples(CQ dom, string word)
        {
            IDomObject exampleElements = commonFunctions.GetDomElement(dom, "span[class=x-g]", 0);
            if (exampleElements == null)
            {
                return "There is no example for this word!";
            }

            string examples = exampleElements.OuterHTML;
            for (int i = 1; i < 4; i++)
            {
                try
                {
                    examples += commonFunctions.GetDomElement(dom, "span[class=x-g]", i).OuterHTML;
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
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"oxford.css\">" + examples;

            return examples;
        }

        public string GetPronunciation(CQ dom, string proxy, string selector)
        {
            string pro_link = commonFunctions.GetElementAttribute(dom, selector, 0, "data-src-mp3");
            if (pro_link.Equals(string.Empty))
            {
                return string.Empty;
            }

            string pro_name = pro_link.Split('/')[pro_link.Split('/').Length - 1];

            Stream input = commonFunctions.GetStream(pro_link, proxy).BaseStream;
            FileStream output = File.Open(Path.Combine(DirectoryPath.SOUND, pro_name), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            commonFunctions.CopyStream(input, output);

            return "[sound:" + pro_name + "]";
        }

        public string GetImages(CQ dom, string word, string proxy, string selector, string attr)
        {
            string img_link = commonFunctions.GetElementAttribute(dom, selector, 0, attr);
            if (img_link.Equals(string.Empty))
            {
                return "<a href=\"https://www.google.com.vn/search?biw=1280&bih=661&tbm=isch&sa=1&q=" + word + "\" style=\"font-size: 15px; color: blue\">Images for this word</a>";
            }

            string img_name = img_link.Split('/')[img_link.Split('/').Length - 1];
            if (attr.Equals("href"))
            {
                img_name = "fullsize_" + img_name;
            }

            Stream input = commonFunctions.GetStream(img_link, proxy).BaseStream;
            FileStream output = File.Open(Path.Combine(DirectoryPath.IMAGE, img_name), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            commonFunctions.CopyStream(input, output);

            return "<img src=\"" + img_name + "\"/>";
        }
    }
}
