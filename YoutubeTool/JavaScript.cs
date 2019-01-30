using System;
using System.Windows.Controls;

namespace YoutubeTool
{
    public static class JavaScript
    {
        /// <summary>
        /// 関数の呼び出し
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="funcName">関数名</param>
        /// <param name="args">引数</param>
        public static void Call(WebBrowser browser, String funcName, params Object[] args)
        {
            try {
                if (args == null) {
                    // 引数が無ければ引数なしで関数を呼ぶ
                    browser.InvokeScript(funcName);
                }
                else {
                    // 引数があれば引数有で渡す。
                    browser.InvokeScript(funcName, args);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
