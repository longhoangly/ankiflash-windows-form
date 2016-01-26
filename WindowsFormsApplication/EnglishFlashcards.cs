using System;
using System.IO;
using System.Threading;
using CsQuery;
using Mono.Web;

namespace FlashcardsGeneratorApplication
{
    class EnglishFlashcards
    {
        private CQ _oxfDocument = "";
        private CQ _sohaDocument = "";

        private string _wrd = "";
        private string _wordType = "";
        private string _phonetic = "";
        private string _example = "";
        private string _proUk = "";
        private string _proUs = "";
        private string _oxfContent = "";
        private string _sohaContent = "";
        private string _thumb = "";
        private string _img = "";
        private string _ankiCard = "";
        private string _copyRight = "";
        private char _tag;

        private readonly BasicFunctions _basicFunctions = new BasicFunctions();

        public string GenerateEnglishFlashCards(string word, string proxyStr, string language)
        {
            #region Get Input & Check Condition
            if (language.Equals(FlashcardsGenerator.enToEng))
            {
                if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.GenOk))
                {
                    _oxfContent = GetOxfordContent(_oxfDocument);
                }
                else if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.ConNotOk))
                {
                    return FlashcardsGenerator.ConNotOk;
                }
                else
                {
                    return FlashcardsGenerator.GenNotOk + " - " + word + "\r\n";
                }
            }
            else
            {
                if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.GenOk) && checkSohaContent(word, proxyStr).Equals(FlashcardsGenerator.GenOk))
                {
                    _oxfContent = GetOxfordContent(_oxfDocument);
                    _sohaContent = GetSohaContent(_sohaDocument);
                    _sohaContent = _sohaContent.Replace("<div id=\"firstHeading\"> </div>", "<div id=\"firstHeading\">" + word + "</div>");
                }
                else if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.ConNotOk) || checkSohaContent(word, proxyStr).Equals(FlashcardsGenerator.ConNotOk))
                {
                    return FlashcardsGenerator.ConNotOk;
                }
                else
                {
                    return FlashcardsGenerator.GenNotOk + " - " + word + "\r\n";
                }
            }
            #endregion

            _wordType = "(" + _basicFunctions.GetElementText(_oxfDocument, "span[class=pos]", 0) + ")";
            _phonetic = GetOxfordPhonetic(_oxfDocument);
            _example = GetOxfordExamples(_oxfDocument, _wrd);
            _proUk = GetOxfordPronunciation(_oxfDocument, proxyStr, "div.pron-uk");
            _proUs = GetOxfordPronunciation(_oxfDocument, proxyStr, "div.pron-us");
            _thumb = GetOxfordImages(_oxfDocument, proxyStr, "img.thumb", "src");
            _img = GetOxfordImages(_oxfDocument, proxyStr, "a[class=topic]", "href");
            _tag = _wrd[0];
            _copyRight = "This flashcard's content is get from the Oxford Advanced Learner's & Soha Online Dictionaries.<br>Thanks Oxford & Soha Dictionaries! Thanks for using!";

            #region build string _ankiCard
            if (language.Equals(FlashcardsGenerator.enToEng))
            {
                _copyRight = "This flashcard's content is get from the Oxford Advanced Learner's Dictionary.<br>Thanks Oxford Dictionary! Thanks for using!";
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _oxfContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.enToViet))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _sohaContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.enToEngViet))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _oxfContent + "\t" + _sohaContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.enToVietEng))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _sohaContent + "\t" + _oxfContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            #endregion

            //_ankiCard = HttpUtility.HtmlDecode(_ankiCard);
            return _ankiCard;
        }

        private string checkOxfordContent(string word, string proxyStr)
        {
            string oxfordUrl = "";
            if (word.Contains("www.oxfordlearnersdictionaries.com"))
            {
                oxfordUrl = word;
            }
            else
            {
                word = word.Replace(" ", "%20");
                oxfordUrl = "http://www.oxfordlearnersdictionaries.com/search/english/direct/?q=" + word;
            }

            StreamReader st = _basicFunctions.HttpGetRequestViaProxy(oxfordUrl, proxyStr);
            if (st == null)
            {
                return FlashcardsGenerator.ConNotOk;
            }

            string doc = st.ReadToEnd();
            CQ dom = CsQuery.CQ.Create(doc);

            string title = dom["title"].Text();
            if (title.Contains("Did you spell it correctly?") || title.Contains("Oxford Learner's Dictionaries | Find the meanings"))
            {
                return FlashcardsGenerator.SpelNotOk;
            }

            _wrd = _basicFunctions.GetElementText(dom, "h2", 0);
            if (_wrd.Equals(""))
            {
                return FlashcardsGenerator.GenNotOk;
            }

            _oxfDocument = dom;

            return FlashcardsGenerator.GenOk;
        }

        private string checkSohaContent(string word, string proxyStr)
        {
            string sohaUrl = "";
            if (word.Contains("tratu.soha.vn"))
            {
                sohaUrl = word;
            }
            else
            {
                word = word.Replace(" ", "+");
                sohaUrl = "http://tratu.soha.vn/index.php?search=" + word + "&dict=en_vn&btnSearch=&chuyennganh=&tenchuyennganh=";
            }

            CQ sohaDom = "";
            int count = 0;
            while (_basicFunctions.GetElementObject(sohaDom, "#content", 0) == null)
            {
                StreamReader sohaSt = _basicFunctions.HttpGetRequestViaProxy(sohaUrl, proxyStr);
                if (sohaSt == null)
                {
                    return FlashcardsGenerator.ConNotOk;
                }

                string sohaDoc = sohaSt.ReadToEnd();
                sohaDom = CsQuery.CQ.Create(sohaDoc);

                string sohaTitle = sohaDom["title"].Text();
                if (sohaTitle.Contains("Kết quả tìm"))
                {
                    return FlashcardsGenerator.SpelNotOk;
                }

                if (count < 20)
                {
                    count++;
                    Thread.Sleep(1000); //1 second
                }
                else
                {
                    return FlashcardsGenerator.GenNotOk;
                }
            }

            _sohaDocument = sohaDom;
            _sohaDocument = _sohaDocument.Select("h2 > span[class=mw-headline]").Before("<img src=\"minus_section.jpg\"> &nbsp;");
            _sohaDocument = _sohaDocument.Select("h3 > span[class=mw-headline]").After("&nbsp; <img src=\"minus_section.jpg\">");

            return FlashcardsGenerator.GenOk;
        }

        private string GetOxfordPhonetic(CQ dom)
        {
            string phoneticBrE = _basicFunctions.GetElementText(dom, "span[class=phon]", 0);
            string phoneticNAmE = _basicFunctions.GetElementText(dom, "span[class=phon]", 1);

            //if (phoneticBrE == "" && phoneticNAmE == "")
            //    return "There is no phonetic for this word!";

            string phonetic = phoneticBrE + phoneticNAmE;

            phonetic = phonetic.Replace("<span class=\"separator\">/</span>", "");
            phonetic = phonetic.Replace("<span class=\"bre\">BrE</span><span class=\"wrap\">/</span>", "BrE /");
            phonetic = phonetic.Replace("<span class=\"wrap\">/</span><span class=\"name\">NAmE</span><span class=\"wrap\">/</span>", "/  &nbsp; NAmE /");
            phonetic = "<span class=\"phon\">" + phonetic + "</span>";

            return phonetic;
        }

        private string GetOxfordExamples(CQ dom, string word)
        {
            IDomObject exampleElements = _basicFunctions.GetElementObject(dom, "span[class=x-g]", 0);
            if (exampleElements == null)
                return "There is no example for this word.";

            string examples = exampleElements.OuterHTML;

            for (int i = 1; i < 4; i++)
            {
                try
                {
                    examples += _basicFunctions.GetElementObject(dom, "span[class=x-g]", i).OuterHTML;
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
            }

            examples = examples.Replace(word, "{{c1::" + word + "}}");
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"oxford.css\">" + examples;

            return examples;
        }

        private string GetOxfordPronunciation(CQ dom, string proxyStr, string selector)
        {
            string pro_link = _basicFunctions.GetElementAttributeValue(dom, selector, 0, "data-src-mp3");

            if (pro_link == "")
                return "";

            string pro_name = pro_link.Split('/')[pro_link.Split('/').Length - 1];

            Stream input = _basicFunctions.HttpGetRequestViaProxy(pro_link, proxyStr).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\sounds\" + pro_name);
            _basicFunctions.CopyStream(input, output);

            return "[sound:" + pro_name + "]";
        }

        private string GetOxfordImages(CQ dom, string proxyStr, string selector, string attr)
        {
            string img_link = _basicFunctions.GetElementAttributeValue(dom, selector, 0, attr);
            string word = _basicFunctions.GetElementText(dom, "h2", 0);

            if (img_link == "")
                return "<a href=\"https://www.google.com.vn/search?biw=1280&bih=661&tbm=isch&sa=1&q=" + word + "\" style=\"font-size: 15px; color: blue\">Images for this word</a>";

            string img_name = img_link.Split('/')[img_link.Split('/').Length - 1];
            if (attr.Equals("href"))
                img_name = "fullsize_" + img_name;

            Stream input = _basicFunctions.HttpGetRequestViaProxy(img_link, proxyStr).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\images\" + img_name);
            _basicFunctions.CopyStream(input, output);

            return "<img src=\"" + img_name + "\"/>";
        }

        private string GetOxfordContent(CQ dom)
        {
            IDomObject oxfContentElement = _basicFunctions.GetElementObject(dom, "#entryContent", 0);
            string oxfContent =
                "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"interface.css\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"oxford.css\">" +
                "<div class=\"responsive_entry_center_wrap\">" + oxfContentElement.OuterHTML + "</div>" + "</html>";

            oxfContent = oxfContent.Replace("\t", "");
            oxfContent = oxfContent.Replace("\n", "");
            oxfContent = oxfContent.Replace("class=\"unbox\"", "class=\"unbox is-active\"");
            return oxfContent;
        }

        private string GetSohaContent(CQ dom)
        {
            IDomObject sohaContentElement = _basicFunctions.GetElementObject(dom, "#content", 0);

            string sohaContent =
            "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"main_min.css\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
            "<div class=\"responsive_entry_center_wrap\">" + sohaContentElement.OuterHTML + "</div>" + "</html>";

            sohaContent = sohaContent.Replace("\t", "");
            sohaContent = sohaContent.Replace("\r", "");
            sohaContent = sohaContent.Replace("\n", "");

            return sohaContent;
        }
    }
}
