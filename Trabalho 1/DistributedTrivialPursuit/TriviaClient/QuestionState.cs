using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace TriviaExpert
{
	/// <summary>
	/// Encapsulates a question state for
	/// the callback of a question
	/// </summary>
	public class QuestionState
	{
		public String Index { get; set; }
		public String Theme { get; set; }
		public IExpert Expert { get; set; }
	}
}