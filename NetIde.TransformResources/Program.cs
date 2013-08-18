using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.TransformResources
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Net IDE Resource Transformer");

            string fileName = null;
            string targetFileName = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (String.Equals(args[i], "/file", StringComparison.OrdinalIgnoreCase))
                {
                    i++;

                    if (i < args.Length)
                        fileName = args[i];
                }
                else if (String.Equals(args[i], "/target", StringComparison.OrdinalIgnoreCase))
                {
                    i++;

                    if (i < args.Length)
                        targetFileName = args[i];
                }
            }

            if (
                fileName == null ||
                targetFileName == null ||
                !File.Exists(fileName)
            ) {
                Console.Error.WriteLine("Usage: nixfresource.exe /file <resource file name> /target <target file name>");
                return 1;
            }

            try
            {
                return new ResourceTransformer(Path.GetFullPath(fileName), targetFileName).Transform();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Resource could not be transformed: " + ex.Message);

                if (ex.StackTrace != null)
                    Console.Error.WriteLine(ex.StackTrace);

                return 2;
            }
        }
    }
}
