using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaModel
{
    class Tester
    {
        public static void Main(String[] args)
        {
            IRepository _rep = XMLRepository.GetInstance("./rep.xml");
            List<String> keyWords = new List<String>();

            Console.WriteLine("Available themes in XML document:");
            foreach (String t in _rep.GetThemes())
                Console.WriteLine("\t{0}", t);

            //Get theme
            Console.WriteLine("Enter a theme...");
            String theme = Console.ReadLine();

            //Get keywords
            Console.WriteLine("Type a query for our expert...");
            String input = Console.ReadLine();
                
            keyWords.AddRange(input.Split(' '));

            //Show answer
            Console.WriteLine("Answer is: {0}",_rep.GetAnswer(keyWords, theme));
        }
    }
}
