using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
	public delegate void QuestionHandler(IExpert sender, String answer);

    public interface IExpert
    {
        event QuestionHandler OnQuestionAnswered;

        String Ask(List<String> keyWords);
        IAsyncResult BeginAsk(List<String> keyWords, AsyncCallback callback, Object state);
        String EndAsk(IAsyncResult iaR);
    }
}