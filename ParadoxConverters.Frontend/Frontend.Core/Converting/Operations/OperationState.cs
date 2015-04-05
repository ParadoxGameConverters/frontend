﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Core.Converting.Operations
{
    public enum OperationState
    {
        NotStarted,

        InProgress,

        Success,

        CompleteWithWarnings,

        CompleteWithErrors,

        Cancelled
    }
}
