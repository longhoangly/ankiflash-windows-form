using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsQuery;
using FlashcardGenerator.Source.Utility;
using FlashcardGenerator.Utility;

namespace FlashcardGenerator.Source.DictionaryModel
{
    public class CollinsDictionary : Dictionary
    {
        public override bool IsConnectionEstablished(string word, string proxy, string language)
        {
            Word = word;
            Proxy = proxy;
            Language = language;

            var isConnectionEstablished = false;
            var url = ContentRoller.LookupUrl(DictConst.COLLINS_URL_FR_EN, word);
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
            if (title.Contains(DictConst.COLLINS_SPELLING_WRONG))
            {
                isWordingCorrect = true;
            }

            var word = ContentRoller.GetText(Dom, "h2[class=h2_entry]>span", 0);
            if (word.Equals(string.Empty))
            {
                isWordingCorrect = true;
            }

            return isWordingCorrect;
        }

        public override string GetWordType()
        {
            IDomObject wordTypeElements = ContentRoller.GetIDomObject(Dom, "span[class=pos]", 0);
            if (wordTypeElements == null)
                return "There is no type for this word.";

            string wordTypes = "(" + wordTypeElements.OuterHTML + ")&nbsp;";

            for (int i = 1; i < 4; i++)
            {
                try
                {
                    wordTypes += "(" + ContentRoller.GetIDomObject(Dom, "span[class=pos]", i).OuterHTML + ")&nbsp;";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }

            return wordTypes;
        }

        public override string GetExample()
        {
            IDomObject exampleElements = ContentRoller.GetIDomObject(Dom, "span[class=phr]", 0);
            if (exampleElements == null)
                return DictConst.NO_EXAMPLE;

            string examples = exampleElements.OuterHTML + "<br>";
            for (int i = 1; i < 4; i++)
            {
                try
                {
                    examples += ContentRoller.GetIDomObject(Dom, "span[class=phr]", i).OuterHTML + "<br>";
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
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"main.css\">" + examples;

            return examples;
        }

        public override string GetPhonetic()
        {
            string phonetic = ContentRoller.GetText(Dom, "span[class=pron]", 0);
            phonetic = phonetic.Replace(ConstVars.CR, string.Empty);
            phonetic = phonetic.Replace(ConstVars.LF, string.Empty);
            phonetic = Regex.Replace(phonetic, "<span class=\\\"hwd_sound\\\">.*</span>", string.Empty);
            phonetic = phonetic.Replace("(", "/").Replace(")", "/");

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
            string pro_link = ContentRoller.GetAttribute(Dom, selector, 0, "data-src-mp3");
            if (pro_link.Equals(string.Empty))
            {
                return string.Empty;
            }

            string pro_name = pro_link.Split('/')[pro_link.Split('/').Length - 1];

            Stream input = ContentRoller.GetStream(pro_link, Proxy).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\sounds\" + pro_name);
            ContentRoller.CopyStream(input, output);
            output.Close();

            return "[sound:" + pro_name + "]";
        }

        public override string GetMeaning()
        {
            IDomObject wordFrequency = ContentRoller.GetIDomObject(Dom, "div.word-frequency-container.res_hos", 0);
            if (wordFrequency != null) wordFrequency.Remove();

            IDomObject socialButtons = ContentRoller.GetIDomObject(Dom, "div.socialButtons", 0);
            if (socialButtons != null) socialButtons.Remove();

            IDomObject domContent = ContentRoller.GetIDomObject(Dom, "div.homograph-entry", 0);

            string htmlContent =
                "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"collins_common.css\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                "<div class=\"responsive_entry_center_wrap\">" + domContent.OuterHTML + "</div>" + "</html>";

            htmlContent = htmlContent.Replace(ConstVars.TAB, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.CR, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.LF, string.Empty);
            htmlContent = Regex.Replace(htmlContent, "<span class=\"hwd_sound\">.*?</span>", string.Empty);

            return htmlContent;
        }

        public override string GetDictionaryName()
        {
            return "Collins Dictionary";
        }
    }
}
