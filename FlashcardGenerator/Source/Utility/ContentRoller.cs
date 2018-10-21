using CsQuery;
using FlashcardGenerator.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace FlashcardGenerator.Source.Utility
{
    public class ContentRoller
    {
        public static string LookupUrl(string dictUrl, string word)
        {
            word = word.Replace(" ", "%20");
            return string.Format(dictUrl, word);
        }

        public static StreamReader GetStream(string url, string proxyAdd)
        {
            StreamReader stream = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var request = WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            if (!string.IsNullOrEmpty(proxyAdd))
            {
                var proxy = new WebProxy(proxyAdd, true)
                {
                    BypassProxyOnLocal = true
                };

                request.Proxy = proxy;
            }

            try
            {
                var response = request.GetResponse();
                stream = new StreamReader(response.GetResponseStream());
            }
            catch (WebException e)
            {
                //MessageBox.Show(e.StackTrace, MsgBoxProps.FAILED);
                Console.WriteLine(e.StackTrace);
            }

            return stream;
        }

        public static CQ GetDom(string url, string proxy)
        {
            var stream = GetStream(url, proxy);
            return stream != null ? CQ.Create(stream) : null;
        }

        public static IDomObject GetIDomObject(CQ dom, string selector, int index)
        {
            var subDom = dom.Select(selector);
            if (subDom != null)
            {
                return subDom.Get(index);
            }
            else
            {
                return null;
            }
        }

        public static List<IDomObject> GetIDomObjects(CQ dom, string selector)
        {
            return dom.Select(selector).Get().ToList();
        }

        public static string GetText(CQ dom, string selector, int index)
        {
            var domObj = GetIDomObject(dom, selector, index);
            if (domObj != null)
            {
                return domObj.InnerHTML;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetAttribute(CQ dom, string selector, int index, string attr)
        {
            var domObj = GetIDomObject(dom, selector, index);
            if (domObj != null)
            {
                return domObj.GetAttribute(attr);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            int len;
            byte[] buffer = new byte[8 * 1024];
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
