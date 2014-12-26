using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;

namespace NetIde.Core
{
    internal static class Checked
    {
        [DebuggerStepThrough]
        public static bool Execute(IServiceProvider serviceProvider, Action action)
        {
            bool success;

            Execute(serviceProvider, action, out success);

            return success;
        }

        [DebuggerStepThrough]
        public static void Execute(IServiceProvider serviceProvider, Action action, out bool success)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            success = false;

            try
            {
                action();

                success = true;
            }
            catch (Exception ex)
            {
                ShowException(serviceProvider, ex);
            }
        }

        [DebuggerStepThrough]
        public static T Execute<T>(IServiceProvider serviceProvider, Func<T> action)
        {
            bool success;

            return Execute(serviceProvider, action, out success);
        }

        [DebuggerStepThrough]
        public static T Execute<T>(IServiceProvider serviceProvider, Func<T> action, out bool success)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            success = false;

            try
            {
                var result = action();

                success = true;

                return result;
            }
            catch (Exception ex)
            {
                ShowException(serviceProvider, ex);

                return default(T);
            }
        }

        private static void ShowException(IServiceProvider serviceProvider, Exception exception)
        {
            serviceProvider
                .CreateTaskDialog()
                .Content(String.Format(Labels.UnexpectedSituationMessage, exception.Message))
                .ExpandedControlText(Labels.UnexpectedSituationShowStackTrace)
                .ExpandedInformation(GetExceptionLog(exception))
                .Alert(serviceProvider as IWin32Window);
        }

        private static string GetExceptionLog(Exception exception)
        {
            var sb = new StringBuilder();

            GetExceptionLog(exception, sb);

            return sb.ToString();
        }

        private static void GetExceptionLog(Exception exception, StringBuilder stringBuilder)
        {
            LogException(stringBuilder, exception);
        }

        private static void LogException(StringBuilder sb, Exception exception)
        {
            if (exception.InnerException != null)
            {
                LogException(sb, exception.InnerException);

                sb.AppendLine();
                sb.AppendLine(Labels.ExceptionCaused);
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
