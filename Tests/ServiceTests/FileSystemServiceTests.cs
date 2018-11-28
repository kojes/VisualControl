using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VueExample.Services;
using Xunit;


namespace VueExample.Tests.ServiceTests
{
    public class FileSystemServiceTests
    {
        [Fact]
        public void FindFolderInTemporaryFolder_Folder_ShouldFindAndReturnFolder()
        {
            var folderName = FileSystemService.FindFolderInTemporaryFolder("VS");
            var directories = Directory.GetDirectories(Path.GetTempPath());
            Assert.Equal("VS", folderName);
        }

    }
}
