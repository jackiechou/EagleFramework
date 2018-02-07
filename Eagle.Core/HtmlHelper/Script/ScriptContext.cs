using System;
using System.Collections.Generic;
using System.Web;

namespace Eagle.Core.HtmlHelper.Script
{
    public class ScriptContext : IDisposable
    {
        internal const string ScriptContextItems = "ScriptContexts";
        internal const string ScriptContextItem = "ScriptContext";

        private readonly HttpContextBase _httpContext;
        private readonly IList<string> _scriptBlocks = new List<string>();
        private readonly HashSet<string> _scriptFiles = new HashSet<string>();

        public ScriptContext(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            _httpContext = httpContext;
        }

        public IList<string> ScriptBlocks { get { return _scriptBlocks; } }

        public HashSet<string> ScriptFiles { get { return _scriptFiles; } }

        public void Dispose()
        {
            var items = _httpContext.Items;
            var scriptContexts = items[ScriptContextItems] as Stack<ScriptContext> ?? new Stack<ScriptContext>();

            // remove any script files already the same as the ones we're about to add
            foreach (var scriptContext in scriptContexts)
            {
                scriptContext.ScriptFiles.ExceptWith(ScriptFiles);
            }

            scriptContexts.Push(this);

            items[ScriptContextItems] = scriptContexts;
        }
    }
}
