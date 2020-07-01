using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace TypeShark2.Client.JsInterop
{
    /// <summary>
    /// Proxy for receiving key press events from JavaScript.
    /// Adapted from https://github.com/aesalazar/AsteroidsWasm
    /// </summary>
    public static class InteropKeyPress
    {
        public static event EventHandler<string> KeyPress;

        [JSInvokable]
        public static Task<bool> JsKeyPress(string key)
        {
            KeyPress?.Invoke(null, key);

            return Task.FromResult(true);
        }
    }
}