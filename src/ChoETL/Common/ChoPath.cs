﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ChoETL
{
    public static class ChoPath
    {
        public static readonly string EntryAssemblyBaseDirectory = null;
        public static readonly string EntryAssemblyName = null;

        private static readonly string _fileNameCleanerExpression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidFileNameChars(), x => Regex.Escape(x.ToString()))) + "]";
        private static readonly Regex _fileNameCleaner = new Regex(_fileNameCleanerExpression, RegexOptions.Compiled);
        private static readonly string _pathCleanerExpression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidPathChars(), x => Regex.Escape(x.ToString()))) + "]";
        private static readonly Regex _pathCleaner = new Regex(_pathCleanerExpression, RegexOptions.Compiled);

        static ChoPath()
        {
            if (System.Web.HttpContext.Current == null)
            {
                EntryAssemblyBaseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                EntryAssemblyName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            }
            else
            {
                EntryAssemblyBaseDirectory = System.Web.HttpRuntime.AppDomainAppPath;
                EntryAssemblyName = new DirectoryInfo(System.Web.HttpRuntime.AppDomainAppPath).Name;
            }
        }

        public static string CleanPath(string path)
        {
            return _pathCleaner.Replace(path, "_");
        }

        public static string CleanFileName(string fileName)
        {
            return _fileNameCleaner.Replace(fileName, "_");
        }

        public static string GetFullPath(string path, string baseDirectory = null)
        {
            if (path.IsNullOrWhiteSpace())
                return path;

            if (Path.IsPathRooted(path))
                return path;
            else if (!baseDirectory.IsNullOrWhiteSpace())
                return GetFullPath(Path.Combine(baseDirectory, path));
            else if (!EntryAssemblyBaseDirectory.IsNullOrEmpty())
                return GetFullPath(Path.Combine(EntryAssemblyBaseDirectory, path));
            else
                return Path.GetFullPath(path);
        }
    }
}
