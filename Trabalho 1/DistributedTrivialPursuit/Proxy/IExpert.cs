using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface IExpert
    {
        event EventHandler OnQuestionAnswered;

        String Ask(List<String> keyWords);
        IAsyncResult BeginAsk(
                List<String> keyWords, AsyncCallback callback, Object state);
        String EndAsk(IAsyncResult iaR);
    }
}
