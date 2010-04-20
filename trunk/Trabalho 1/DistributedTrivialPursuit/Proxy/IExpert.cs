using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface IExpert
    {
        event EventHandler OnQuestionAnswered;

        String Ask(String question);
    }
}
