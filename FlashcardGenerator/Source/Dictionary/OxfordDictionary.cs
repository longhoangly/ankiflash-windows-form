using CsQuery;
using FlashcardGenerator.Source.Utility;
using FlashcardGenerator.Utility;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FlashcardGenerator.Source.DictionaryModel
{
    public class OxfordDictionary : Dictionary
    {
        public override bool IsConnectionEstablished(string word, string proxy, string language)
        {
            Word = word;
            Proxy = proxy;
            Language = language;

            var isConnectionEstablished = false;
            var url = ContentRoller.LookupUrl(DictConst.OXFORD_URL_EN_EN, word);
            Dom = ContentRoller.GetDom(url, proxy);
            if (Dom != null)
            {
                isConnectionEstablished = true;
            }

            return isConnectionEstablished;
        }

        public override bool IsWordingCorrect()
        {
            var isWordingCorrect = true;
            var title = Dom["title"].Text();
            if (title.Contains(DictConst.OXFORD_SPELLING_WRONG_1) ||
                title.Contains(DictConst.OXFORD_SPELLING_WRONG_2))
            {
                isWordingCorrect = false;
            }

            var word = ContentRoller.GetText(Dom, "h2", 0);
            if (word.Equals(string.Empty))
            {
                isWordingCorrect = false;
            }

            return isWordingCorrect;
        }

        public override string GetWordType()
        {
            var type = ContentRoller.GetText(Dom, "span[class=pos]", 0);
            return type.Equals(string.Empty) ? string.Empty : "(" + type + ")";
        }

        public override string GetExample()
        {
            IDomObject exampleObjs = ContentRoller.GetIDomObject(Dom, "span[class=x-g]", 0);
            if (exampleObjs == null)
            {
                return DictConst.NO_EXAMPLE;
            }

            string examples = exampleObjs.OuterHTML;
            for (int i = 1; i < 4; i++)
            {
                try
                {
                    examples += ContentRoller.GetIDomObject(Dom, "span[class=x-g]", i).OuterHTML;
                }
                catch (Exception e)
                {
                    Console.WriteLine(MsgBoxProps.ERROR_OCCUR + Environment.NewLine + e.StackTrace);
                    break;
                }
            }

            var pattern = string.Format(@"({0})([^\W_]*?[<>/\]*?[^\W_]*?[<>/\]*?)([\s.])", Word);
            examples = examples.ToLower();
            examples = Regex.Replace(examples, pattern, m => "{{c1::" + m.Groups[1].Value + "}}" + m.Groups[2].Value + m.Groups[3].Value);
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"oxford.css\">" + examples;

            return examples;
        }

        public override string GetPhonetic()
        {
            string phoneticBrE = ContentRoller.GetText(Dom, "span[class=phon]", 0);
            string phoneticNAmE = ContentRoller.GetText(Dom, "span[class=phon]", 1);
            string phonetic = $"{phoneticBrE}{phoneticNAmE}";

            phonetic = phonetic.Replace("<span class=\"separator\">/</span>", string.Empty);
            phonetic = phonetic.Replace("<span class=\"bre\">BrE</span><span class=\"wrap\">/</span>", "BrE /");
            phonetic = phonetic.Replace("<span class=\"wrap\">/</span><span class=\"name\">NAmE</span><span class=\"wrap\">/</span>", "/  &nbsp; NAmE /");
            phonetic = $"<span class=\"phon\">{phonetic}</span>";

            return phonetic;
        }

        public override string GetImage(string selector, string attr)
        {
            var img_link = ContentRoller.GetAttribute(Dom, selector, 0, attr);
            if (img_link.Equals(string.Empty))
            {
                return "<a href=\"https://www.google.com.vn/search?biw=1280&bih=661&tbm=isch&sa=1&q=" + Word + "\" style=\"font-size: 15px; color: blue\">Search Google Image for the word!</a>";
            }

            var img_name = img_link.Split('/')[img_link.Split('/').Length - 1];
            if (attr.Equals("href"))
            {
                img_name = "fullsize_" + img_name;
            }

            var input = ContentRoller.GetStream(img_link, Proxy).BaseStream;
            var output = File.Open(Path.Combine(AnkiDirs.IMAGE, img_name), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
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
            FileStream output = File.Open(Path.Combine(AnkiDirs.SOUND, pro_name), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            ContentRoller.CopyStream(input, output);

            return "[sound:" + pro_name + "]";
        }

        public override string GetMeaning()
        {
            var domContent = ContentRoller.GetIDomObject(Dom, "#entryContent", 0);
            string htmlContent = "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"interface.css\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                                 "<link type=\"text/css\" rel=\"stylesheet\" href=\"oxford.css\">" +
                                 "<div class=\"responsive_entry_center_wrap\">" + domContent.OuterHTML +
                                 "</div>" + "</html>";

            htmlContent = htmlContent.Replace(ConstVars.TAB, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.CR, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.LF, string.Empty);
            htmlContent = htmlContent.Replace("class=\"unbox\"", "class=\"unbox is-active\"");

            return htmlContent;
        }

        public override string GetDictionaryName()
        {
            return "Oxford Advanced Learner's Dictionary";
        }
    }
}
