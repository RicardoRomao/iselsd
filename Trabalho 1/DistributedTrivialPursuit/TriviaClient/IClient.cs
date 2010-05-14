using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace TriviaExpert
{
    interface IClient
    {
        event ErrorHandler OnError;
        event QuestionHandler OnQuestionAnswered;
        event ThemeHandler OnExpertsGetComplete;
        event ResponseHandler OnAnswerReceived;

        void Connect();
        void Disconnect();

        void RegisterAll();
        void UnregisterAll();

        int GetQuestionCount();
        string GetServerUrl();
        bool IsConnected();

        void AddLocalExpert(String theme);
        void Ask(String theme, List<string> keyWords);
        void GetExperts(String theme);
    }
}
