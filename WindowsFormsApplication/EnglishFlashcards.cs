using System;
using System.IO;
using System.Text.RegularExpressions;
using CsQuery;

namespace FlashcardsGeneratorApplication
{
    class EnglishFlashcards
    {
        private CQ _oxfDocument = "";
        private CQ _camDocument = "";
        private CQ _lacVietDocument = "";

        private string _wrd = "";
        private string _wordType = "";
        private string _phonetic = "";
        private string _example = "";
        private string _proUk = "";
        private string _proUs = "";
        private string _oxfContent = "";
        private string _lacVietContent = "";
        private string _camContent = "";
        private string _thumb = "";
        private string _img = "";
        private string _ankiCard = "";
        private string _copyRight = "";
        private char _tag;

        private readonly BasicFunctions _basicFunctions = new BasicFunctions();

        public string GenerateEnglishFlashCards(string word, string proxyStr, string language)
        {
            #region Get Input & Check Conditions
            if (language.Equals(FlashcardsGenerator.EN2EN))
            {
                if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.GENERATING_SUCCESS))
                {
                    _oxfContent = GetOxfordContent(_oxfDocument);
                }
                else if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.CONNECTION_FAILED))
                {
                    return FlashcardsGenerator.CONNECTION_FAILED;
                }
                else
                {
                    return FlashcardsGenerator.GENERATING_FAILED + " - " + word + "\r\n";
                }
            }
            else if (language.Equals(FlashcardsGenerator.EN2CH))
            {
                if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.GENERATING_SUCCESS) && checkCambridgeContent(word, proxyStr).Equals(FlashcardsGenerator.GENERATING_SUCCESS))
                {
                    _oxfContent = GetOxfordContent(_oxfDocument);
                    _camContent = GetCambridgeContent(_camDocument);
                }
                else if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.CONNECTION_FAILED) || checkCambridgeContent(word, proxyStr).Equals(FlashcardsGenerator.CONNECTION_FAILED))
                {
                    return FlashcardsGenerator.CONNECTION_FAILED;
                }
                else
                {
                    return FlashcardsGenerator.GENERATING_FAILED + " - " + word + "\r\n";
                }
            }
            else
            {
                if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.GENERATING_SUCCESS) && checkLacVietContent(word, proxyStr).Equals(FlashcardsGenerator.GENERATING_SUCCESS))
                {
                    _oxfContent = GetOxfordContent(_oxfDocument);
                    _lacVietContent = GetLacVietContent(_lacVietDocument);
                    _lacVietContent = _lacVietContent.Replace("<div id=\"firstHeading\"> </div>", "<div id=\"firstHeading\">" + word + "</div>");
                }
                else if (checkOxfordContent(word, proxyStr).Equals(FlashcardsGenerator.CONNECTION_FAILED) || checkLacVietContent(word, proxyStr).Equals(FlashcardsGenerator.CONNECTION_FAILED))
                {
                    return FlashcardsGenerator.CONNECTION_FAILED;
                }
                else
                {
                    return FlashcardsGenerator.GENERATING_FAILED + " - " + word + "\r\n";
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
            _copyRight = "This flashcard's content is get from the Oxford Advanced Learner's & LacViet Online Dictionaries.<br>Thanks Oxford & LacViet Dictionaries! Thanks for using!";

            #region Build String _ankiCard
            if (language.Equals(FlashcardsGenerator.EN2EN))
            {
                _copyRight = "This flashcard's content is get from the Oxford Advanced Learner's Dictionary.<br>Thanks Oxford Dictionary! Thanks for using!";
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _oxfContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.EN2VI))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _lacVietContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.EN2CH))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _camContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.EN2EN_VI))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _oxfContent + "\t" + _lacVietContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.EN2VI_EN))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _example + "\t" + _proUk + "\t" + _proUs + "\t" + _thumb + "\t" + _img + "\t" + _lacVietContent + "\t" + _oxfContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            #endregion

            //_ankiCard = HttpUtility.HtmlDecode(_ankiCard);
            return _ankiCard;
        }

        private string checkOxfordContent(string word, string proxyStr)
        {
            string oxfordUrl = "";
            if (word.Contains("oxfordlearnersdictionaries.com"))
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
                return FlashcardsGenerator.CONNECTION_FAILED;
            }

            string doc = st.ReadToEnd();
            CQ dom = CQ.Create(doc);

            string title = dom["title"].Text();
            if (title.Contains("Did you spell it correctly?") || title.Contains("Oxford Learner's Dictionaries | Find the meanings"))
            {
                return FlashcardsGenerator.SPELLING_FAILED;
            }

            _wrd = _basicFunctions.GetElementText(dom, "h2", 0);
            if (_wrd.Equals(""))
            {
                return FlashcardsGenerator.GENERATING_FAILED;
            }

            _oxfDocument = dom;

            return FlashcardsGenerator.GENERATING_SUCCESS;
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
                "<div class=\"responsive_entry_center_wrap\">" + oxfContentElement.OuterHTML +
                "</div>" + "</html>";

            oxfContent = oxfContent.Replace("\t", "");
            oxfContent = oxfContent.Replace("\r", "");
            oxfContent = oxfContent.Replace("\n", "");

            oxfContent = oxfContent.Replace("class=\"unbox\"", "class=\"unbox is-active\"");

            return oxfContent;
        }

        private string checkLacVietContent(string word, string proxyStr)
        {
            string lacVietUrl = "";
            if (word.Contains("tratu.coviet.vn"))
            {
                lacVietUrl = word;
            }
            else
            {
                word = word.Replace(" ", "+");
                lacVietUrl = "http://tratu.coviet.vn/tu-dien-lac-viet.aspx?learn=hoc-tieng-anh&t=A-V&k=" + word;
            }

            StreamReader lacVietSt = _basicFunctions.HttpGetRequestViaProxy(lacVietUrl, proxyStr);
            if (lacVietSt == null)
            {
                return FlashcardsGenerator.CONNECTION_FAILED;
            }

            string lacVietDoc = lacVietSt.ReadToEnd();
            CQ lacVietDom = CQ.Create(lacVietDoc);

            string lacVietResult = lacVietDom["div[class=i p10]"].Text();
            if (lacVietResult.Contains("Dữ liệu đang được cập nhật"))
            {
                return FlashcardsGenerator.SPELLING_FAILED;
            }

            _lacVietDocument = lacVietDom;

            return FlashcardsGenerator.GENERATING_SUCCESS;
        }

        private string GetLacVietContent(CQ dom)
        {
            IDomObject lacVietContentElement = _basicFunctions.GetElementObject(dom, "#ctl00_ContentPlaceHolderMain_cnt_dict", 0);

            string lacVietContent =
            "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"home.css\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
            "<div class=\"responsive_entry_center_wrap\">" + lacVietContentElement.OuterHTML +
            "</div>" + "</html>";

            lacVietContent = lacVietContent.Replace("\t", "");
            lacVietContent = lacVietContent.Replace("\r", "");
            lacVietContent = lacVietContent.Replace("\n", "");

            lacVietContent = Regex.Replace(lacVietContent, "<div class=\"p5l fl\".*?</div>", "");
            lacVietContent = Regex.Replace(lacVietContent, "<div class=\"p3l fl m3t\">.*?</div>", "");
            lacVietContent = lacVietContent.Replace("<div class=\"cgach p5lr fl\">|</div>", "");

            return lacVietContent;
        }

        private string checkCambridgeContent(string word, string proxyStr)
        {
            string cambridgeUrl = "";
            if (word.Contains("dictionary.cambridge.org"))
            {
                cambridgeUrl = word;
            }
            else
            {
                word = word.Replace(" ", "%20");
                cambridgeUrl = "http://dictionary.cambridge.org/zhs/%E6%90%9C%E7%B4%A2/english-chinese-simplified/direct/?q=" + word;
            }

            StreamReader st = _basicFunctions.HttpGetRequestViaProxy(cambridgeUrl, proxyStr);
            if (st == null)
            {
                return FlashcardsGenerator.CONNECTION_FAILED;
            }

            string doc = st.ReadToEnd();
            CQ dom = CQ.Create(doc);

            string title = dom["title"].Text();
            if (title.Contains("你拼写正确了吗？") || !title.Contains(word))
            {
                return FlashcardsGenerator.SPELLING_FAILED;
            }

            _wrd = _basicFunctions.GetElementText(dom, "h2", 0);
            if (_wrd.Equals(""))
            {
                return FlashcardsGenerator.GENERATING_FAILED;
            }

            _camDocument = dom;

            return FlashcardsGenerator.GENERATING_SUCCESS;
        }

        private string GetCambridgeContent(CQ dom)
        {
            IDomObject cambridgeContentElement = _basicFunctions.GetElementObject(dom, "div.di.english-chinese-simplified", 0);
            cambridgeContentElement.AddClass("entrybox english-chinese-simplified");

            IDomObject ukSoundIcon = _basicFunctions.GetElementObject(dom, "span.sound.audio_play_button.pron-uk", 0);
            ukSoundIcon.Remove();

            IDomObject usIcon = _basicFunctions.GetElementObject(dom, "img.ussymbol", 0);
            if (usIcon !=null) usIcon.Remove();

            IDomObject shareThisEntry = _basicFunctions.GetElementObject(dom, "div.share-this-entry", 0);
            shareThisEntry.Remove();

            string cambridgeContent =
            "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"common.css\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
            "<div class=\"responsive_entry_center_wrap\">" + cambridgeContentElement.OuterHTML +
            "</div>" + "</html>";

            cambridgeContent = cambridgeContent.Replace("\t", "");
            cambridgeContent = cambridgeContent.Replace("\r", "");
            cambridgeContent = cambridgeContent.Replace("\n", "");
            
            return cambridgeContent;
        }
    }
}
