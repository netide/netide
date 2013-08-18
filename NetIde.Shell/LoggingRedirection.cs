using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using log4net.Appender;
using log4net.Config;
using log4net.Core;

namespace NetIde.Shell
{
    public static class LoggingRedirection
    {
        private static readonly object _syncRoot = new object();
        private static Interop.INiLogger _logger;

        public static void Install(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            lock (_syncRoot)
            {
                if (_logger != null)
                    return;

                _logger = (Interop.INiLogger)serviceProvider.GetService(typeof(Interop.INiLogger));

                SetupLogging(Level.Debug);
            }
        }

        private static void SetupLogging(Level threshold)
        {
            var configuration = BuildConfiguration(threshold);

            using (var stream = new MemoryStream())
            using (var writer = XmlWriter.Create(stream))
            {
                configuration.WriteTo(writer);

                writer.Flush();

                stream.Position = 0;

                XmlConfigurator.Configure(stream);
            }
        }

        private static XmlDocument BuildConfiguration(Level threshold)
        {
            var document = new XmlDocument();

            var configurationElement = AppendElement(document, "configuration");

            var configSectionsElement = AppendElement(configurationElement, "configSections");

            var sectionElement = AppendElement(configSectionsElement, "section");

            AppendAttribute(sectionElement, "name", "log4net");
            AppendAttribute(sectionElement, "type", typeof(Log4NetConfigurationSectionHandler).AssemblyQualifiedName);

            var log4netElement = AppendElement(configurationElement, "log4net");

            AppendAttribute(log4netElement, "debug", "false");

            var appenderElement = AppendElement(log4netElement, "appender");

            AppendAttribute(appenderElement, "name", typeof(RedirectAppender).Name);
            AppendAttribute(appenderElement, "type", typeof(RedirectAppender).AssemblyQualifiedName);

            AppendElement(appenderElement, "threshold", threshold.Name);

            var rootElement = AppendElement(log4netElement, "root");

            var priorityElement = AppendElement(rootElement, "priority");

            AppendAttribute(priorityElement, "value", Level.Debug.Name);

            var appenderRefElement = AppendElement(rootElement, "appender-ref");

            AppendAttribute(appenderRefElement, "ref", typeof(RedirectAppender).Name);

            return document;
        }

        private static XmlElement AppendElement(XmlNode node, string name)
        {
            return AppendElement(node, name, null);
        }

        private static XmlElement AppendElement(XmlNode node, string name, string text)
        {
            var element = (node as XmlDocument ?? node.OwnerDocument).CreateElement(name);
            node.AppendChild(element);

            if (text != null)
                element.InnerText = text;

            return element;
        }

        private static void AppendAttribute(XmlNode node, string name, string value)
        {
            var attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            node.Attributes.Append(attribute);
        }

        public class RedirectAppender : AppenderSkeleton
        {
            protected override void Append(LoggingEvent loggingEvent)
            {
                var logEvent = new NiLogEvent
                {
                    Message = loggingEvent.RenderedMessage,
                    Severity = GetSeverity(loggingEvent.Level),
                    Source = loggingEvent.LoggerName,
                    TimeStamp = loggingEvent.TimeStamp
                };

                if (loggingEvent.ExceptionObject != null)
                {
                    var sb = new StringBuilder();

                    GetExceptionLog(loggingEvent.ExceptionObject, sb);

                    logEvent.Content = sb.ToString();
                }

                ErrorUtil.ThrowOnFailure(_logger.Append(logEvent));
            }

            private static int GetSeverity(Level level)
            {
                if (level >= Level.Fatal)
                    return (int)NiConstants.Severity.Fatal;
                if (level >= Level.Error)
                    return (int)NiConstants.Severity.Error;
                if (level >= Level.Warn)
                    return (int)NiConstants.Severity.Warn;
                if (level >= Level.Info)
                    return (int)NiConstants.Severity.Info;

                return (int)NiConstants.Severity.Debug;
            }

            private static void GetExceptionLog(Exception exception, StringBuilder stringBuilder)
            {
                if (exception == null)
                    throw new ArgumentNullException("exception");
                if (stringBuilder == null)
                    throw new ArgumentNullException("stringBuilder");

                LogException(stringBuilder, exception);
            }

            private static void LogException(StringBuilder sb, Exception exception)
            {
                if (exception.InnerException != null)
                {
                    LogException(sb, exception.InnerException);

                    sb.AppendLine();
                    sb.AppendLine("=== Caused by ===");
                    sb.AppendLine();
                }

                sb.AppendLine(String.Format("{0} ({1})", exception.Message, exception.GetType().FullName));

                if (exception.StackTrace != null)
                {
                    sb.AppendLine();
                    sb.AppendLine(exception.StackTrace.TrimEnd());
                }
            }
        }
    }
}
