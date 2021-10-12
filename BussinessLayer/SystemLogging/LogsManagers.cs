using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BussinessLayer.SystemLogging
{
    public class LogsManagers :ILogsManager
    {
        public readonly ILog _logger = LogManager.GetLogger(typeof(LogManager));
        public LogsManagers()
        {
            try
            {
                XmlDocument log4netConfig = new XmlDocument();
                using (var fs = File.OpenRead("log4net.config"))
                {
                    log4netConfig.Load(fs);
                    var repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                    XmlConfigurator.Configure(repository, log4netConfig["log4net"]);
                    _logger.Info("Log system initialized");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error", ex);
            }
        }
        public void Infor(string message)
        {
            _logger.Info(message);
        }
        public void Error(string message)
        {
            _logger.Error(message);
        }
        public void Debug(string message)
        {
            _logger.Debug(message);
        }
        public void Warning(string message)
        {
            _logger.Warn(message);
        }

    }
}
