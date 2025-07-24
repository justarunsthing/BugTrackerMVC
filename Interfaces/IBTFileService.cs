﻿namespace BugTrackerMVC.Interfaces
{
    public interface IBTFileService
    {
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        string ConvertByteArrayToFile(byte[] fileData, string extension);
        string GetFileIcon(string file);
        string FormatFileSize(long bytes);
    }
}