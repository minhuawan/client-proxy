using System;
using UnityEngine;

namespace Infra.Log
{
    public class Logger : UnityEngine.Logger
    {
        public readonly static Logger GameLogger = new Logger("game");
        public readonly static Logger NetworkLogger = new Logger("network");

        public Logger(string namespace_) : base(new MyLogHandler(namespace_))
        {
        }
    }

    public class MyLogHandler : ILogHandler
    {
        public string namespace_ { get; private set; }

        public MyLogHandler(string namespace_)
        {
            this.namespace_ = namespace_;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            throw new NotImplementedException();
        }
    }
}