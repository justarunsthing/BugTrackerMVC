using BugTrackerMVC.Interfaces;

namespace BugTrackerMVC.Services
{
    public class BTFileService : IBTFileService
    {
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public string ConvertByteArrayToFile(byte[] fileData, string extension)
        {
            try
            {
                var imageBase64Data = Convert.ToBase64String(fileData);

                return string.Format($"data:{extension};base64,{imageBase64Data}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            try
            {
                var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var byteFile = memoryStream.ToArray();

                memoryStream.Close();
                memoryStream.Dispose();

                return byteFile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string FormatFileSize(long bytes)
        {
            throw new NotImplementedException();
        }

        public string GetFileIcon(string file)
        {
            throw new NotImplementedException();
        }
    }
}