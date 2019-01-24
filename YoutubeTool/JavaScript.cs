using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace YoutubeTool
{
    public static class JavaScript
    {
        public static void Call(WebBrowser browser, String funcName, params Object[] args)
        {
            try {
                browser.InvokeScript(funcName, args);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
