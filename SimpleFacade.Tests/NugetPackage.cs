﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;

namespace SimpleFacade.Tests
{
    public class NugetPackage
    {
        public static IList<string> FindContentFiles(string folder, string name)
        {
            IList<string> contentFiles = null;

            UsingPackage(folder, name, archive =>
            {
                contentFiles = archive.Entries
                    .Select(ae => ae.FullName)
                    .Where(e => e.StartsWith("content/"))
                    .ToList();
            });

            return contentFiles;
        }

        public static void VerifyDependencies(string folder, string name, string[] expectedDependencies)
        {
            var dependencies = FindDependencies(folder, name);

            var extraDependencies = dependencies
                .Where(d => !expectedDependencies.Any(ed => d.Matches(ed)))
                .ToList();

            if (extraDependencies.Count != 0)
                throw new Exception($"Found unexpected dependencies for {name}:\n{string.Join("\n", extraDependencies)}");

            var missingDependencies = expectedDependencies
                .Where(ed => !dependencies.Any(d => d.Matches(ed)))
                .ToList();

            if (missingDependencies.Count != 0)
                throw new Exception($"Missing dependencies for {name}:\n{string.Join("\n", missingDependencies)}");
        }

        public static IList<NugetDependency> FindDependencies(string folder, string name)
        {
            IList<NugetDependency> dependencies = null;

            UsingPackage(folder, name, archive =>
            {
                var nuspecName = name + ".nuspec";
                var entries = archive.Entries;
                var nuspecs = entries.Where(e => e.Name == nuspecName).ToList();

                if (nuspecs.Count != 1)
                    throw new Exception(string.Format("Could not find '{0}' in:\n{1}",
                        nuspecName,
                        string.Join("\n", entries.Select(e => e.FullName))));

                var nuspec = nuspecs.Single();

                using (var stream = nuspec.Open())
                {
                    var doc = new XmlDocument();
                    doc.Load(stream);
                    var dependencyNodes = doc.SelectNodes("//*[local-name()='dependency']");
                    dependencies = dependencyNodes.Cast<XmlElement>().Select(e => new NugetDependency(e)).ToList();
                }
            });

            return dependencies;
        }

        public static void UsingPackage(string folder, string name, Action<ZipArchive> action)
        {
            var regFile = name + "\\.\\d+\\.\\d+\\.\\d+(\\.\\d+)?\\.nupkg";
            var regEx = new Regex(regFile);

            if (folder.StartsWith("."))
                folder = Path.Combine(TestContext.CurrentContext.TestDirectory, folder);

            var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            var archiveFiles = files.Where(f => regEx.IsMatch(f)).ToList();

            if (archiveFiles.Count != 1)
                throw new Exception(string.Format("Expected single archive, but found {0} files:\n{1}\nfrom all files:\n{2}\nusing regex {3}",
                    archiveFiles.Count,
                    string.Join("\n", archiveFiles),
                    string.Join("\n", files),
                    regFile));

            var archiveFile = archiveFiles.Single();

            using (var archive = ZipFile.OpenRead(archiveFile))
                action(archive);
        }

        public class NugetDependency
        {
            public NugetDependency(XmlElement dependency)
            {
                Id = dependency.Attributes["id"].Value;
                Version = dependency.Attributes["version"].Value;
            }

            public string Id        { get; protected set; }
            public string Version   { get; protected set; }

            public override string ToString()
            {
                return $"{Id}:{Version}";
            }

            public bool Matches(string expected)
            {
                if (expected.EndsWith(":*"))
                    return Id == expected.Substring(0, expected.Length - 2);
                else
                    return ToString() == expected;
            }
        }
    }
}
