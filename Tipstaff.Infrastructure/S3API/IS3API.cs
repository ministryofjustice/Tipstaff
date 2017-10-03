namespace Tipstaff.Infrastructure.S3API
{
    public interface IS3API
    {
        string Save(string bucketName, string folderName, string fileName, System.IO.Stream file);
        string ReadS3Object(string bucketName, string folderName, string fileName);
    }
}