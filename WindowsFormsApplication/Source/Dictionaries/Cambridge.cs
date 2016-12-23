using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FlashcardsGenerator.Source.Dictionaries
{
    class Cambridge
    {
        private CommonFunctions commonFunctions = new CommonFunctions();

        public CQ GetDom(string word, string proxy)
        {
            string url = commonFunctions.LookUpUrl(Dictionary.CAMBRIDGE_DOMAIN, Dictionary.CAMBRIDGE_URL_EN_CN, word);
            return commonFunctions.GetDomPage(url, proxy);
        }

        public string GetTitle(CQ dom)
        {
            return dom["title"].Text();
        }

        public string GetWord(CQ dom)
        {
            return commonFunctions.GetTextElement(dom, "h2>span", 0);
        }

        public string GetMeaning(CQ dom)
        {
            IDomObject contentElement = commonFunctions.GetDomElement(dom, "div#entryContent", 0);
            contentElement.AddClass("entrybox english-chinese-simplified entry-body");

            IDomObject ukSoundIcon = commonFunctions.GetDomElement(dom, "span.circle.circle-btn.sound.audio_play_button.uk", 0);
            if (ukSoundIcon != null) ukSoundIcon.Remove();

            IDomObject usSoundIcon = commonFunctions.GetDomElement(dom, "span.circle.circle-btn.sound.audio_play_button.us", 0);
            if (usSoundIcon != null) usSoundIcon.Remove();

            IDomObject translations = commonFunctions.GetDomElement(dom, "div.clrd.mod.mod--style5.mod--dark.mod-translate", 0);
            translations.Remove();

            IDomObject shareThisEntry = commonFunctions.GetDomElement(dom, "div.share.rounded.js-share", 0);
            shareThisEntry.Remove();

            string htmlContent = "<html>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                                      "<link type=\"text/css\" rel=\"stylesheet\" href=\"common.css\">" +
                                      "<link type=\"text/css\" rel=\"stylesheet\" href=\"responsive.css\">" +
                                      "<div class=\"responsive_entry_center_wrap\">" + contentElement.OuterHTML +
                                      "</div>" + "</html>";

            htmlContent = htmlContent.Replace(Constant.TAB, string.Empty);
            htmlContent = htmlContent.Replace(Constant.CR, string.Empty);
            htmlContent = htmlContent.Replace(Constant.LF, string.Empty);

            return htmlContent;
        }
    }
}
