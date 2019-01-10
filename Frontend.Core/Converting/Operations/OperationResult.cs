using System.Collections.Generic;
using Frontend.Core.Logging;

namespace Frontend.Core.Converting.Operations
{
	public class OperationResult
	{
		private OperationResultState state;

		public OperationResultState State
		{
			get { return state; }
			set { state = value; }
		}

		public OperationResult()
		{
			state = OperationResultState.Success;
			LogEntries = new List<LogEntry>();
		}

		public IList<LogEntry> LogEntries { get; private set; }
	}
}