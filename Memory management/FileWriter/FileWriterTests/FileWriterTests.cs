﻿using Convestudo.Unmanaged;
using NUnit.Framework;
using System;
using System.IO;

namespace FileWriterTests
{
    [TestFixture]
    public class FileWriterTests
    {
        private const string TestFileName = "test.txt";

        [Test]
        public void DisposeDoesWork()
        {
            var fileWriter = new FileWriter(TestFileName);
            Assert.DoesNotThrow(fileWriter.Dispose);
        }

        [Test]
        public void DisposingCanBeCalledTwise()
        {
            var fileWriter = new FileWriter(TestFileName);

            fileWriter.Dispose();
            Assert.DoesNotThrow(fileWriter.Dispose);
        }

        [Test]
        public void ResourceIsLocked()
        {
            using (var fileWriter1 = new FileWriter(TestFileName))
            {
                fileWriter1.Write("Test");

                Assert.Throws<FileLoadException>(() =>
                {
                    using (var file2 = new FileWriter(TestFileName))
                    {
                        file2.Write("adsf");
                    }
                });
                fileWriter1.Dispose();
            }
        }

        [Test]
        public void WriteFewWordsDoesWork()
        {
            const string testLine = "TestLine";
            var extectedStr = String.Format("{0}{0}{0}{0}", testLine);
            using (var fileWriter = new FileWriter(TestFileName))
            {
                fileWriter.Write(testLine);
                fileWriter.Write(testLine);
                fileWriter.Write(testLine);
                fileWriter.Write(testLine);
            }

            using (var fileStream = File.OpenRead(TestFileName))
            using (var streamReader = new StreamReader(fileStream))
            {
                var str = streamReader.ReadToEnd();
                Assert.AreEqual(extectedStr, str);
            }
        }

        [Test]
        public void WriteLineWritesWithNewLine()
        {
            const string testLine = "TestLine";
            var extectedStr = String.Format("{0}{1}{0}{1}{0}{1}{0}{1}", testLine, Environment.NewLine);

            using (var fileWriter = new FileWriter(TestFileName))
            {
                fileWriter.WriteLine(testLine);
                fileWriter.WriteLine(testLine);
                fileWriter.WriteLine(testLine);
                fileWriter.WriteLine(testLine);
            }

            using (var fileStream = File.OpenRead(TestFileName))
            using (var streamReader = new StreamReader(fileStream))
            {
                var str = streamReader.ReadToEnd();
                Assert.AreEqual(extectedStr, str);
            }
        }
    }
}
