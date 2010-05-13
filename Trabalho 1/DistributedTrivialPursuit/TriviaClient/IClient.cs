using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace TriviaExpert
{
    interface IClient
    {

        // TODO:
        // Define the separation between Experts that belong to the Client
        // and Experts that were given to the Client by the Server

        List<String> GetThemes();
        
        IExpert CreateExpert(String theme);
        List<IExpert> CreateExperts(List<String> themes);
        
        List<IExpert> GetExperts();
        IExpert GetExpert(String theme);
        
        void RegisterExpert(String theme);
        void UnregisterExpert(String theme);


    }
}
