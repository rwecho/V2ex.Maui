using System;

namespace V2ex.Api;

public class CreateTopicException: Exception
{
    public CreateTopicException(Problem problem) {
        this.Problem = problem;
    }

    public Problem Problem { get; }
}
