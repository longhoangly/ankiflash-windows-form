using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CsQuery;
using Mono.Web;

namespace FlashcardsGeneratorApplication
{
    class FrenchFlashcards
    {
        private CQ _collinsDocument = "";
        private CQ _lacVietDocument = "";

        private string _wrd = "";
        private string _wordType = "";
        private string _phonetic = "";
        private string _collinsExample = "";
        private string _pron = "";
        private string _collinsContent = "";
        private string _lacVietContent = "";
        private string _thumb = "";
        private string _img = "";
        private string _ankiCard = "";
        private string _copyRight = "";
        private char _tag;

        private readonly BasicFunctions _basicFunctions = new BasicFunctions();

        public string GenerateFrenchFlashCards(string word, string proxyStr, string language)
        {
            #region Get Input & Check Condition
            if (language.Equals(FlashcardsGenerator.frToEng))
            {
                if (checkCollinsContent(word, proxyStr).Equals(FlashcardsGenerator.GenOk))
                {
                    _collinsContent = GetCollinsContent(_collinsDocument);
                }
                else if (checkCollinsContent(word, proxyStr).Equals(FlashcardsGenerator.ConNotOk))
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
                if (checkCollinsContent(word, proxyStr).Equals(FlashcardsGenerator.GenOk) && checkLacVietContent(word, proxyStr).Equals(FlashcardsGenerator.GenOk))
                {
                    _collinsContent = GetCollinsContent(_collinsDocument);
                    _lacVietContent = GetLacVietContent(_lacVietDocument);
                }
                else if (checkCollinsContent(word, proxyStr).Equals(FlashcardsGenerator.ConNotOk) || checkLacVietContent(word, proxyStr).Equals(FlashcardsGenerator.ConNotOk))
                {
                    return FlashcardsGenerator.ConNotOk;
                }
                else
                {
                    return FlashcardsGenerator.GenNotOk + " - " + word + "\r\n";
                }
            }
            #endregion

            _wordType = GetCollinsWordType(_collinsDocument);
            _phonetic = GetCollinsPhonetic(_collinsDocument);
            _collinsExample = GetCollinsExamples(_collinsDocument, _wrd);
            _pron = GetCollinsPronunciation(_collinsDocument, proxyStr, "a[class=hwd_sound sound audio_play_button]");
            _thumb = GetCollinsImages(_collinsDocument, proxyStr, "", "");
            _img = _thumb;  // The same result for _thumb and _img.
            _tag = _wrd[0];
            _copyRight = "This flashcard's content is get from the Collins & LacViet Online Dictionaries.<br>Thanks Collins & LacViet Dictionaries! Thanks for using!";

            #region build string _ankiCard
            if (language.Equals(FlashcardsGenerator.frToEng))
            {
                _copyRight = "This flashcard's content is get from the Collins Dictionary.<br>Thanks Collins Dictionary! Thanks for using!";
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _collinsExample + "\t" + _pron + "\t" + _thumb + "\t" + _img + "\t" + _collinsContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.frToViet))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _collinsExample + "\t" + _pron + "\t" + _thumb + "\t" + _img + "\t" + _lacVietContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.frToEngViet))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _collinsExample + "\t" + _pron + "\t" + _thumb + "\t" + _img + "\t" + _collinsContent + "\t" + _lacVietContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            else if (language.Equals(FlashcardsGenerator.frToVietEng))
            {
                _ankiCard = _wrd + "\t" + _wordType + "\t" + _phonetic + "\t" + _collinsExample + "\t" + _pron + "\t" + _thumb + "\t" + _img + "\t" + _lacVietContent + "\t" + _collinsContent + "\t" + _copyRight + "\t" + _tag + "\r\n";
            }
            #endregion

            return _ankiCard;
        }

        private string checkCollinsContent(string word, string proxyStr)
        {
            string collinsUrl = "";
            if (word.Contains("www.collinsdictionary.com"))
            {
                collinsUrl = word;
            }
            else
            {
                word = word.Replace(" ", "%20");
                collinsUrl = "http://www.collinsdictionary.com/search?q=" + word + "&dataset=french-english";
            }

            StreamReader st = _basicFunctions.HttpGetRequestViaProxy(collinsUrl, proxyStr);
            if (st == null)
            {
                return FlashcardsGenerator.ConNotOk;
            }

            string collinsDoc = st.ReadToEnd();
            CQ collinsDom = CsQuery.CQ.Create(collinsDoc);

            string title = collinsDom["title"].Text();
            if (title.Contains("CollinsDictionary.com | Collins Dictionaries - Free Online"))
            {
                return FlashcardsGenerator.SpelNotOk;
            }

            _wrd = _basicFunctions.GetElementText(collinsDom, "h1[class=orth h1_entry]", 0);
            if (_wrd.Equals(""))
            {
                return FlashcardsGenerator.GenNotOk;
            }

            _wrd = _wrd.Replace("\r", "").Replace("\n", "");
            _wrd = Regex.Replace(_wrd, "<span class=\\\"pron\\\">.*</span>", "");
            _collinsDocument = collinsDom;

            return FlashcardsGenerator.GenOk;
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
                lacVietUrl = "http://tratu.coviet.vn/tu-dien-lac-viet.aspx?learn=hoc-tieng-phap&t=F-V&k=" + word;
            }

            CQ lacVietDom = "";

            StreamReader lacVietSt = _basicFunctions.HttpGetRequestViaProxy(lacVietUrl, proxyStr);
            if (lacVietSt == null)
            {
                return FlashcardsGenerator.ConNotOk;
            }

            string lacVietDoc = lacVietSt.ReadToEnd();
            lacVietDom = CsQuery.CQ.Create(lacVietDoc);

            string lacVietTitle = lacVietDom["div[class=i p10]"].Text();
            if (lacVietTitle.Contains("Dữ liệu đang được cập nhật"))
            {
                return FlashcardsGenerator.SpelNotOk;
            }

            _lacVietDocument = lacVietDom;

            return FlashcardsGenerator.GenOk;
        }

        private string GetCollinsWordType(CQ dom)
        {
            IDomObject wordTypeElements = _basicFunctions.GetElementObject(dom, "span[class=pos]", 0);
            if (wordTypeElements == null)
                return "There is no example for this word.";

            string wordTypes = "(" + wordTypeElements.OuterHTML + ")&nbsp;";

            for (int i = 1; i < 4; i++)
            {
                try
                {
                    wordTypes += "(" + _basicFunctions.GetElementObject(dom, "span[class=pos]", i).OuterHTML + ")&nbsp;";
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
            }

            return wordTypes;
        }

        private string GetCollinsPhonetic(CQ dom)
        {
            string phonetic = _basicFunctions.GetElementText(dom, "span[class=pron]", 0);
            phonetic = phonetic.Replace("\r", "").Replace("\n", "");
            phonetic = Regex.Replace(phonetic, "<span class=\\\"hwd_sound\\\">.*</span>", "");
            phonetic = phonetic.Replace("(", "/").Replace(")", "/");

            return phonetic == "" ? "There is no phonetic for this word!" : phonetic;
        }

        private string GetCollinsExamples(CQ dom, string word)
        {
            IDomObject exampleElements = _basicFunctions.GetElementObject(dom, "span[class=phr]", 0);
            if (exampleElements == null)
                return "There is no example for this word.";

            string examples = exampleElements.OuterHTML + "<br>";

            for (int i = 1; i < 4; i++)
            {
                try
                {
                    examples += _basicFunctions.GetElementObject(dom, "span[class=phr]", i).OuterHTML + "<br>";
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
            }

            examples = examples.Replace(word, "{{c1::" + word + "}}");
            examples = "<link type=\"text/css\" rel=\"stylesheet\" href=\"main.css\">" + examples;

            return examples;
        }

        private string GetCollinsPronunciation(CQ dom, string proxyStr, string selector)
        {
            string pro_link = _basicFunctions.GetElementAttributeValue(dom, selector, 0, "data-src-mp3");

            if (pro_link == "")
            {
                return "";
            }
            else
            {
                pro_link = "http://www.collinsdictionary.com" + pro_link;
            }

            string pro_name = pro_link.Split('/')[pro_link.Split('/').Length - 1];

            Stream input = _basicFunctions.HttpGetRequestViaProxy(pro_link, proxyStr).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\sounds\" + pro_name);
            _basicFunctions.CopyStream(input, output);
            output.Close();

            return "[sound:" + pro_name + "]";
        }

        private string GetCollinsImages(CQ dom, string proxyStr, string selector, string attr)
        {
            string img_link = _basicFunctions.GetElementAttributeValue(dom, selector, 0, attr);

            if (img_link == "")
                return "<a href=\"https://www.google.com.vn/search?biw=1280&bih=661&tbm=isch&sa=1&q=" + _wrd + "\" style=\"font-size: 15px; color: blue\">Images for this word</a>";

            string img_name = img_link.Split('/')[img_link.Split('/').Length - 1];
            if (attr.Equals(""))
                img_name = "fullsize_" + img_name;

            Stream input = _basicFunctions.HttpGetRequestViaProxy(img_link, proxyStr).BaseStream;
            FileStream output = File.OpenWrite(@".\AnkiFlashcards\images\" + img_name);
            _basicFunctions.CopyStream(input, output);

            return "<img src=\"" + img_name + "\"/>";
        }

        private string GetCollinsContent(CQ dom)
        {
            IDomObject collinsContentElement = _basicFunctions.GetElementObject(dom, "div.homograph-entry", 0);
            string collinsContent =
                "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"main.css\">" +
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                "<div class=\"responsive_entry_center_wrap\">" + collinsContentElement.OuterHTML + "</div>" + "</html>";

            collinsContent = collinsContent.Replace("\t", "");
            collinsContent = collinsContent.Replace("\r", "");
            collinsContent = collinsContent.Replace("\n", "");
            return collinsContent;
        }

        private string GetLacVietContent(CQ dom)
        {
            IDomObject lacVietContentElement = _basicFunctions.GetElementObject(dom, "#ctl00_ContentPlaceHolderMain_cnt_dict", 0);

            string lacVietContent =
            "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"main.css\">" +
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
            "<div class=\"responsive_entry_center_wrap\">" + lacVietContentElement.OuterHTML + "</div>" + "</html>";

            lacVietContent = lacVietContent.Replace("\t", "");
            lacVietContent = lacVietContent.Replace("\r", "");
            lacVietContent = lacVietContent.Replace("\n", "");

            lacVietContent = HttpUtility.HtmlDecode(lacVietContent);
            return lacVietContent;
        }
    }
}
