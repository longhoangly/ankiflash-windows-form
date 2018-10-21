using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using FlashcardGenerator.Source.Utility;
using FlashcardGenerator.Utility;

namespace FlashcardGenerator.Source.DictionaryModel
{
    public class CambridgeDictionary : Dictionary
    {
        public override bool IsConnectionEstablished(string word, string proxy, string language)
        {
            Word = word;
            Proxy = proxy;
            Language = language;

            var isConnectionEstablished = false;
            var url = ContentRoller.LookupUrl(DictConst.CAMBRIDGE_URL_EN_CN, word);
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
            if (title.Contains(DictConst.CAMBRIDGE_SPELLING_WRONG))
            {
                isWordingCorrect = true;
            }

            var word = ContentRoller.GetText(Dom, "span.headword>span", 0);
            if (word.Equals(string.Empty))
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
            throw new NotImplementedException();
        }

        public override string GetPhonetic()
        {
            throw new NotImplementedException();
        }

        public override string GetImage(string selector, string attr)
        {
            throw new NotImplementedException();
        }

        public override string GetPron(string selector)
        {
            throw new NotImplementedException();
        }

        public override string GetMeaning()
        {
            IDomObject ukSoundIcon = ContentRoller.GetIDomObject(Dom, "span.circle.circle-btn.sound.audio_play_button.uk", 0);
            if (ukSoundIcon != null) ukSoundIcon.Remove();

            IDomObject usSoundIcon = ContentRoller.GetIDomObject(Dom, "span.circle.circle-btn.sound.audio_play_button.us", 0);
            if (usSoundIcon != null) usSoundIcon.Remove();

            IDomObject translations = ContentRoller.GetIDomObject(Dom, "div.clrd.mod.mod--style5.mod--dark.mod-translate", 0);
            translations.Remove();

            IDomObject shareThisEntry = ContentRoller.GetIDomObject(Dom, "div.share.rounded.js-share", 0);
            shareThisEntry.Remove();

            List<IDomObject> scripts = ContentRoller.GetIDomObjects(Dom, "script");
            if (scripts.Any()) scripts.ForEach(x => x.Remove());

            IDomObject contentElement = ContentRoller.GetIDomObject(Dom, "div#entryContent", 0);
            contentElement.AddClass("entrybox english-chinese-simplified entry-body");

            string htmlContent = "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                                      "<link type=\"text/css\" rel=\"stylesheet\" href=\"common.css\">" +
                                      "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                                      "<div class=\"responsive_entry_center_wrap\">" + contentElement.OuterHTML +
                                      "</div>" + "</html>";

            htmlContent = htmlContent.Replace(ConstVars.TAB, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.CR, string.Empty);
            htmlContent = htmlContent.Replace(ConstVars.LF, string.Empty);

            return htmlContent;
        }

        public override string GetDictionaryName()
        {
            return "Cambridge Dictionary";
        }
    }
}
