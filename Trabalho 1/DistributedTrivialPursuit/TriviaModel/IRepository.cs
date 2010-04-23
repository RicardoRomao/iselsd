using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaModel
{
    public interface IRepository
    {
        List<String> GetThemes();
        String GetAnswer(List<String> keyWords, String theme);
    }
}
