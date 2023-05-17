using SkyApm.Common;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public class DiagnosticStrings
    {
        public const string DiagnosticListenerName = "EntityFramework6DiagnosticListener";

        public const string ClientPrefix = "sqlClient ";

        public const string SqlBeforeExecuteCommand = "EntityFramework6.Command.CommandExecuting";
        public const string SqlAfterExecuteCommand = "EntityFramework6.Command.CommandExecuted";
        public const string SqlErrorExecuteCommand = "EntityFramework6.Command.CommandError";
    }

    public class Components
    {
        public static readonly StringOrIntValue ENTITYFRAMEWORK = new StringOrIntValue(4001, "EntityFramework6");
        public static readonly StringOrIntValue ENTITYFRAMEWORK_SQLSERVER = new StringOrIntValue(4002, "EntityFramework6");
    }
}
