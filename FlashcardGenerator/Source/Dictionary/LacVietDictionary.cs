using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsQuery;
using FlashcardGenerator.Source.Utility;
using FlashcardGenerator.Utility;

namespace FlashcardGenerator.Source.DictionaryModel
{
    public class LacVietDictionary : Dictionary
    {
        public override bool IsConnectionEstablished(string word, string proxy, string language)
        {
            Word = word;
            Proxy = proxy;
            Language = language;

            var isConnectionEstablished = false;
            string url = string.Empty;
            if (language.Contains(CardOptions.VN2EN))
            {
                url = ContentRoller.LookupUrl(DictConst.LACVIET_URL_VN_EN, word);
            }
            else if (language.Contains(CardOptions.VN2FR))
            {
                url = ContentRoller.LookupUrl(DictConst.LACVIET_URL_VN_FR, word);
            }
            else if (language.Contains(CardOptions.EN_SRC))
            {
                url = ContentRoller.LookupUrl(DictConst.LACVIET_URL_EN_VN, word);
            }
            else
            {
                url = ContentRoller.LookupUrl(DictConst.LACVIET_URL_FR_VN, word);
            }

            Dom = ContentRoller.GetDom(url, proxy);
            if (Dom != null)
            {
                isConnectionEstablished = true;
            }

            return isConnectionEstablished;
        }

        public override bool IsWordingCorrect()
        {
            var isWordingCorrect = false;
            var title = Dom["title"].Text();
            if (title.Contains(DictConst.LACVIET_SPELLING_WRONG))
            {
                isWordingCorrect = true;
            }

            var word = ContentRoller.GetText(Dom, "div[class=w fl]", 0);
            if (word.Equals(string.Empty))
            {
                isWordingCorrect = true;
            }

            //ToDo: Check which one we use to check correct word??? lacResult or title???
            var lacResult = Dom["div[class=i p10]"].Text();
            if (lacResult.Contains(DictConst.LACVIET_SPELLING_WRONG))
            {
                isWordingCorrect = true;
            }

            return isWordingCorrect;
        }

        public override string GetWordType()
        {
            throw new NotImplementedException();
        }

        public override string GetExample()
        {
            IDomObject exampleElements = ContentRoller.GetIDomObject(Dom, "div[class=e]", 0);
            if (exampleElements == null)
                return DictConst.NO_EXAMPLE;

            string examples = exampleElements.OuterHTML + "<br>";
            for (int i = 1; i < 2; i++)
            {
                try
                {
                    examples += ContentRoller.GetIDomObject(Dom, "div[class=e]", i).OuterHTML + "<br>";
                }
                catch (Exception e)
                {
                    Console.WriteLine(MsgBoxProps.ERROR_OCCUR + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                    break;
                }
            }

            examples = examples.ToLower();

            string pattern = string.Format(@"({0})([^\W_]*?[<>/\]*?[^\W_]*?[<>/\]*?)([\s.])", Word);
            examples = Regex.Replace(examples, pattern, m => "{{c1::" + m.Groups[1].Value + "}}" + m.Groups[2].Value + m.Groups[3].Value);
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"home.css\">" + examples;

            return examples;
        }

        public override string GetPhonetic()
        {
            string phonetic = ContentRoller.GetText(Dom, "div[class=p5l fl cB]", 0);

            //return phonetic == string.Empty ? "There is no phonetic for this word!" : phonetic;
            return phonetic;
        }

        public override string GetImage(string selector, string attr)
        {
            string img_link = ContentRoller.GetAttribute(Dom, selector, 0, attr);

            if (img_link == string.Empty)
                return "<a href=\"https://www.google.com.vn/search?biw=1280&bih=661&tbm=isch&sa=1&q=" + Word + "\" style=\"font-size: 15px; color: blue\">Images for this word</a>";

            string img_name = img_link.Split('/')[img_link.Split('/').Length - 1];
            if (attr.Equals(string.Empty))
                img_name = "fullsize_" + img_name;

            Stream input = ContentRoller.GetStream(img_link, Proxy).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\images\" + img_name);
            ContentRoller.CopyStream(input, output);

            return "<img src=\"" + img_name + "\"/>";
        }

        public override string GetPron(string selector)
        {
            string pro_link = ContentRoller.GetAttribute(Dom, selector, 0, "flashvars");

            if (pro_link == string.Empty)
            {
                return string.Empty;
            }

            pro_link = pro_link.Replace("file=", string.Empty).Replace("&autostart=false", string.Empty);
            string pro_name = pro_link.Split('/')[pro_link.Split('/').Length - 1];

            Stream input = ContentRoller.GetStream(pro_link, Proxy).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\sounds\" + pro_name);
            ContentRoller.CopyStream(input, output);
            output.Close();

            return "[sound:" + pro_name + "]";
        }

        public override string GetMeaning()
        {
            IDomObject domContent = ContentRoller.GetIDomObject(Dom, "#ctl00_ContentPlaceHolderMain_cnt_dict", 0);
            string htmlContent = "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"home.css\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                                 "<div class=\"responsive_entry_center_wrap\">" + domContent.OuterHTML +
                                 "</div>" + "</html>";

            htmlContent = htmlContent.Replace(ConstVars.TAB, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.CR, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.LF, string.Empty);
            htmlContent = Regex.Replace(htmlContent, "<div class=\"p5l fl\".*?</div>", string.Empty);
            htmlContent = Regex.Replace(htmlContent, "<div class=\"p3l fl m3t\">.*?</div>", string.Empty);
            htmlContent = htmlContent.Replace("<div class=\"cgach p5lr fl\">|</div>", string.Empty);

            htmlContent = htmlContent.Replace("<div id=\"firstHeading\"> </div>", "<div id=\"firstHeading\">" + Word + "</div>");

            return htmlContent;
        }

        public override string GetDictionaryName()
        {
            return "Lac Viet Dictionary";
        }
    }
}
