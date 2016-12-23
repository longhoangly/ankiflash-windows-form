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
    class LacViet
    {
        private CommonFunctions commonFunctions = new CommonFunctions();

        public CQ GetDom(string word, string proxy, string language)
        {
            string url = string.Empty;
            if (language.Contains(Languages.VN2EN))
            {
                url = commonFunctions.LookUpUrl(Dictionary.LACVIET_DOMAIN, Dictionary.LACVIET_URL_VN_EN, word);
            }
            else if (language.Contains(Languages.VN2FR))
            {
                url = commonFunctions.LookUpUrl(Dictionary.LACVIET_DOMAIN, Dictionary.LACVIET_URL_VN_FR, word);
            }
            else if (language.Contains(Languages.EN_SRC))
            {
                url = commonFunctions.LookUpUrl(Dictionary.LACVIET_DOMAIN, Dictionary.LACVIET_URL_EN_VN, word);
            }
            else
            {
                url = commonFunctions.LookUpUrl(Dictionary.LACVIET_DOMAIN, Dictionary.LACVIET_URL_FR_VN, word);
            }

            return commonFunctions.GetDomPage(url, proxy);
        }

        public string GetDomResult(CQ dom)
        {
            return dom["div[class=i p10]"].Text();
        }

        public string GetMeaning(CQ dom, string word)
        {
            IDomObject domContent = commonFunctions.GetDomElement(dom, "#ctl00_ContentPlaceHolderMain_cnt_dict", 0);
            string htmlContent = "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"home.css\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                                 "<div class=\"responsive_entry_center_wrap\">" + domContent.OuterHTML +
                                 "</div>" + "</html>";

            htmlContent = htmlContent.Replace(Constant.TAB, string.Empty);
            htmlContent = htmlContent.Replace(Constant.CR, string.Empty);
            htmlContent = htmlContent.Replace(Constant.LF, string.Empty);
            htmlContent = Regex.Replace(htmlContent, "<div class=\"p5l fl\".*?</div>", string.Empty);
            htmlContent = Regex.Replace(htmlContent, "<div class=\"p3l fl m3t\">.*?</div>", string.Empty);
            htmlContent = htmlContent.Replace("<div class=\"cgach p5lr fl\">|</div>", string.Empty);

            htmlContent = htmlContent.Replace("<div id=\"firstHeading\"> </div>", "<div id=\"firstHeading\">" + word + "</div>");

            return htmlContent;
        }

        public string GetTitle(CQ dom)
        {
            return dom["title"].Text();
        }

        public string GetWord(CQ dom)
        {
            return commonFunctions.GetTextElement(dom, "div[class=w fl]", 0);
        }

        public string GetPhonetic(CQ dom)
        {
            string phonetic = commonFunctions.GetTextElement(dom, "div[class=p5l fl cB]", 0);

            //return phonetic == string.Empty ? "There is no phonetic for this word!" : phonetic
            return phonetic;
        }

        public string GetExamples(CQ dom, string word)
        {
            IDomObject exampleElements = commonFunctions.GetDomElement(dom, "div[class=e]", 0);
            if (exampleElements == null)
                return Dictionary.NO_EXAMPLE;

            string examples = exampleElements.OuterHTML + "<br>";
            for (int i = 1; i < 2; i++)
            {
                try
                {
                    examples += commonFunctions.GetDomElement(dom, "div[class=e]", i).OuterHTML + "<br>";
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
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"home.css\">" + examples;

            return examples;
        }

        public string GetPronunciation(CQ dom, string proxyStr, string selector)
        {
            string pro_link = commonFunctions.GetElementAttribute(dom, selector, 0, "flashvars");

            if (pro_link == string.Empty)
            {
                return string.Empty;
            }

            pro_link = pro_link.Replace("file=", string.Empty).Replace("&autostart=false", string.Empty);
            string pro_name = pro_link.Split('/')[pro_link.Split('/').Length - 1];

            Stream input = commonFunctions.GetStream(pro_link, proxyStr).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\sounds\" + pro_name);
            commonFunctions.CopyStream(input, output);
            output.Close();

            return "[sound:" + pro_name + "]";
        }

        public string GetImages(CQ dom, string word, string proxyStr, string selector, string attr)
        {
            string img_link = commonFunctions.GetElementAttribute(dom, selector, 0, attr);

            if (img_link == string.Empty)
                return "<a href=\"https://www.google.com.vn/search?biw=1280&bih=661&tbm=isch&sa=1&q=" + word + "\" style=\"font-size: 15px; color: blue\">Images for this word</a>";

            string img_name = img_link.Split('/')[img_link.Split('/').Length - 1];
            if (attr.Equals(string.Empty))
                img_name = "fullsize_" + img_name;

            Stream input = commonFunctions.GetStream(img_link, proxyStr).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\images\" + img_name);
            commonFunctions.CopyStream(input, output);

            return "<img src=\"" + img_name + "\"/>";
        }
    }
}
