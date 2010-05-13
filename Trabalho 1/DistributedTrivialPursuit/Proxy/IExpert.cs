using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
	public delegate void QuestionHandler(IExpert sender, String keywords, String answer);

    public interface IExpert
    {
        event QuestionHandler OnQuestionAnswered;

        String GetTheme();
        String Ask(List<String> keyWords);
        IAsyncResult BeginAsk(List<String> keyWords, AsyncCallback callback, Object state);
        String EndAsk(IAsyncResult iaR);

        ITriviaSponsor GetSponsor();
    }
}