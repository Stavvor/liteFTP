using System;
using System.IO;
using System.Net;
using System.Text;


namespace liteFTP.Models
{
    public class FTPclientModel
    {
        private string server;
        private string userName;
        private string password;
        private int bufferSize = 1024;

        private NetworkCredential credentials;
        private const string ftp = "ftp://";
        private string uri;

        private string currentDirectory = null;

        public FTPclientModel(string ser, string usr, string pass)
        {
            server = ser;
            userName = usr;
            password = pass;

            credentials = new NetworkCredential(userName, password);

            uri = $"{ftp}{server}";
        }

        public void FtpGetAllFiles(string path)
        {
            try
            {
                FtpWebRequest request = Request(path, WebRequestMethods.Ftp.ListDirectoryDetails);
                FtpWebResponse response = Response(request);

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                try
                {
                    while (reader.Peek() != -1)
                    {
                        string itemInfo = reader.ReadLine();
                        Console.WriteLine(itemInfo);
                        if (itemInfo[0] == 'd')
                            Console.WriteLine("folder");
                        else
                            Console.WriteLine("plik");
                    }
                }

                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                reader.Close();
                response.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

        }

        public void FtpUploadFile(string fileName)
        {
            FtpWebRequest request = Request(fileName, WebRequestMethods.Ftp.UploadFile);

            StreamReader sourceStream = new StreamReader(fileName);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = Response(request);

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
        }

        public void FtpDownloadFile(string ftpFileName, string localFileName)
        {
            try
            {
                FtpWebRequest request = Request(ftpFileName, WebRequestMethods.Ftp.DownloadFile);

                FtpWebResponse response = Response(request);

                Stream ftpStream = response.GetResponseStream();

                FileStream localFileStream = new FileStream(localFileName, FileMode.Create);

                byte[] byteBuffer = new byte[bufferSize];
                int bytes = ftpStream.Read(byteBuffer, 0, bufferSize);
                try
                {
                    while (bytes > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytes);
                        bytes = ftpStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                localFileStream.Close();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void FtpDeleteFile(string fileName)
        {
            FtpWebRequest request = Request(fileName, WebRequestMethods.Ftp.DeleteFile);

            FtpWebResponse response = Response(request);
            Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();
        }

        private FtpWebRequest Request(string requestPath, string method)
        {
            if (requestPath != null) uri = $"{uri}/{requestPath}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Credentials = credentials;

            return request;
        }

        private FtpWebResponse Response(FtpWebRequest request)
        {
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            return response;
        }
    }
}
