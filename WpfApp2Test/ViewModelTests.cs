using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WpfApp;

namespace WpfApp2Test
{
    [TestClass]
    public class ViewModelTests
    {
        private const string TestFilePath = "TestFile";

        [TestCleanup]
        public void TestCleanup()
        {
            // テスト実行後、テスト用のファイルを削除する
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TestMethod]
        public void Search_FileExistAndEmptySearchString_CannotExecute()
        {
            // Arrange
            File.WriteAllLines(
                TestFilePath,
                new string[] { });
            var target = new ViewModel();
            target.FilePath = TestFilePath;
            target.SearchString = string.Empty;

            // Act
            var result = target.SearchCommand.CanExecute();

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Search_FileNotExistAndFilledSearchString_CannotExecute()
        {
            // Arrange
            var target = new ViewModel();
            target.FilePath = TestFilePath;
            target.SearchString = "test";

            // Act
            var result = target.SearchCommand.CanExecute();

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Search_FileExistAndFilledSearchString_CanExecute()
        {
            // Arrange
            File.WriteAllLines(
                TestFilePath,
                new string[] { });
            var target = new ViewModel();
            target.FilePath = TestFilePath;
            target.SearchString = "test";

            // Act
            var result = target.SearchCommand.CanExecute();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Search_MultiLinesInFile_ReturnMatchedLines()
        {
            // Arrange
            File.WriteAllLines(
                TestFilePath,
                new string[] { "test1", "test2", "teest" });
            var target = new ViewModel();
            target.FilePath = TestFilePath;
            target.SearchString = "test";

            // Act
            target.SearchCommand.Execute();
            var result = target.SearchResult;

            // Assert
            Assert.AreEqual("test1" + Environment.NewLine + "test2", result);
        }
    }
}
