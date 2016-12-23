using System;
using System.IO;
using System.Net;
using CsQuery;
using System.Windows.Forms;
using FlashcardsGenerator.Source;

namespace FlashcardsGenerator
{
    public class CommonFunctions
    {
        public StreamReader GetStream(string url, string proxyAddr)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            if (!string.IsNullOrEmpty(proxyAddr))
            {
                WebProxy proxy = new WebProxy(proxyAddr, true);
                proxy.BypassProxyOnLocal = true;
                request.Proxy = proxy;
            }

            try
            {
                WebResponse response = request.GetResponse();
                return new StreamReader(response.GetResponseStream());
            }
            catch (WebException)
            {
                return null;
            }
        }

        public string LookUpUrl(string domain, string urlString, string word)
        {
            if (word.Contains(domain))
            {
                return word;
            }
            else
            {
                word = word.Replace(" ", "%20");
                return string.Format(urlString, word);
            }
        }

        public CQ GetDomPage(string url, string proxy)
        {
            StreamReader stream = GetStream(url, proxy);
            if (stream != null)
            {
                return CQ.Create(stream);
            }
            else
            {
                return null;
            }
        }

        public IDomObject GetDomElement(CQ dom, string selector, int index)
        {
            CQ elements = dom.Select(selector);
            if (elements != null)
            {
                return elements.Get(index);
            }
            else
            {
                return null;
            }
        }

        public string GetTextElement(CQ dom, string selector, int index)
        {
            CQ elements = dom.Select(selector);
            if (elements != null)
            {
                IDomObject element = elements.Get(index);
                if (element != null)
                {
                    return element.InnerHTML;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetElementAttribute(CQ dom, string selector, int index, string attKey)
        {
            CQ elements = dom.Select(selector);
            if (elements != null)
            {
                IDomObject element = elements.Get(index);
                if (element != null)
                {
                    return element.GetAttribute(attKey);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public void CopyStream(Stream input, Stream output)
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
