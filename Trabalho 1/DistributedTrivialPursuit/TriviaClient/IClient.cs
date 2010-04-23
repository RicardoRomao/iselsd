using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace TriviaClient
{
    interface IClient
    {
        List<String> GetThemes();

        List<IExpert> GetExperts();
        IExpert GetExpert(String theme);
        
        IExpert CreateExpert(String theme);
        List<IExpert> CreateExperts(List<String> themes);
        
        void RegisterExpert(String theme);
        void UnregisterExpert(String theme);
    }
}
