using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon;
using System.IO;
using Amazon.S3.Model;

namespace Tipstaff.Infrastructure.S3API
{
    public class S3API:IS3API
    {
        private AmazonS3Config _awsAmazonS3Config;
        private AmazonS3Client _awsAmazonS3Client;

        public S3API()
        {
            try
            {
                _awsAmazonS3Config.RegionEndpoint = Amazon.RegionEndpoint.EUWest2;
                
                _awsAmazonS3Client = new AmazonS3Client( _awsAmazonS3Config); 
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create an Amazon S3 client; " + ex.Message);
            }
        }

        public string Save(string bucketName, string folderName, string fileName, Stream file)
        {
            string s3URL = string.Empty;
            try
            {
                PutObjectRequest request = new PutObjectRequest();
                request.BucketName = bucketName;
                request.Key = folderName + "/" + fileName;
                request.InputStream = file;

                _awsAmazonS3Client.PutObject(request);
                s3URL = "https://" + bucketName + ".s3.amazonaws.com/" + request.Key;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to upload file to S3 bucket (" + bucketName + "/" + folderName + "/" + fileName + ") : " + ex.Message);
            }
            return s3URL;
        }

        public string ReadS3Object(string bucketName, string folderName, string fileName)
        {
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = bucketName;
            request.Key = folderName + "/" + fileName;

            GetObjectResponse response = _awsAmazonS3Client.GetObject(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            String content = reader.ReadToEnd();

            return content;
        }

    }
}
