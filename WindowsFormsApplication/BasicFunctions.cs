using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using CsQuery;

namespace FlashcardsGeneratorApplication
{
    class BasicFunctions
    {
        public string GetElementAttributeValue(CQ dom, string selector, int index, string attKey)
        {
            CQ elements = dom.Select(selector);

            try
            {
                return elements.Get(index).GetAttribute(attKey);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                return "";
            }
        }

        public string GetElementText(CQ dom, string selector, int index)
        {
            CQ elements = dom.Select(selector);

            try
            {
                //return elements.Get(index).OuterHTML;
                return elements.Get(index).InnerHTML;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                return "";
            }
        }

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public IDomObject GetElementObject(CQ dom, string selector, int index)
        {
            CQ elements = dom.Select(selector);

            try
            {
                return elements.Get(index);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                return null;
            }
        }

        public StreamReader HttpGetRequestViaProxy(string url, string proxyStr)
        {
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            if (!string.IsNullOrEmpty(proxyStr))
            {
                WebProxy myproxy = new WebProxy(proxyStr, true);
                myproxy.BypassProxyOnLocal = true;
                myWebRequest.Proxy = myproxy;
            }

            myWebRequest.Method = "GET";
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)myWebRequest.GetResponse();
            }
            catch (WebException e)
            {
                //Console.WriteLine(e);
                return null;
            }

            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            return streamReader;
        }
    }
}
