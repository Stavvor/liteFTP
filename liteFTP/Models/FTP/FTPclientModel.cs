﻿using liteFTP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace liteFTP.Models
{
    public class FTPclientModel
    {
        private string server;
        private string userName;
        private SecureString password;
        private int bufferSize = 1024;

        private NetworkCredential credentials;
        private const string ftp = "ftp://";

        public string Uri {get; set; }

        //public string CurrentDirectory { get { return "test"; } } //TODO private variable with current path for displaying in local/remote explorer

        public FTPclientModel(string ser, string usr, SecureString pass)
        {
            server = ser;
            userName = usr;
            password = pass;

            credentials = new NetworkCredential(userName, password);

            Uri = $"{ftp}{server}";
        }

        public FTPclientModel(FTPcredentialsVM FTPcredentials)
        {
            server = FTPcredentials.ServerName;
            userName = FTPcredentials.Username;
            password = FTPcredentials.Password;

            credentials = FTPcredentials.credentials;

            Uri = $"{ftp}{server}";
        }

        public async Task<List<string>> FtpGetAllFiles()
        {
            List<string> ftpItemsInfo=new List<string>();

            try
            {
                FtpWebRequest request = Request(null, WebRequestMethods.Ftp.ListDirectoryDetails);
                FtpWebResponse response = await Response(request);

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                try
                {
                    while (reader.Peek() != -1)
                    {
                        ftpItemsInfo.Add(reader.ReadLine());
                      
                    }
                }

                catch (Exception ex)
                {
                    //TODO IoC messageBox
                    String e = ex.ToString();
                }
                reader.Close();
                response.Close();
            }
            catch (Exception ex) {
                //TODO IoC messageBox
                String e = ex.ToString();
            }
            return ftpItemsInfo;
        }

        public async Task FtpUploadFile(string FilePath)
        {
            var name=DirectoryManager.GetNameFromPath(FilePath);

            FtpWebRequest request = Request(name, WebRequestMethods.Ftp.UploadFile);

            StreamReader sourceStream = new StreamReader(FilePath);
            byte[] fileContents = Encoding.UTF8.GetBytes(await sourceStream.ReadToEndAsync()); //TODO buffer
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = await Response(request);

            String r = response.StatusDescription;

            response.Close();
        }

        public async Task FtpDownloadFile(string ftpFileName, string localFileName)
        {
            try
            {

                FtpWebRequest request = Request(ftpFileName, WebRequestMethods.Ftp.DownloadFile);
                FtpWebRequest sizeRequest = Request(ftpFileName, WebRequestMethods.Ftp.GetFileSize);

                FtpWebResponse response = await Response(request);

                Stream ftpStream = response.GetResponseStream();

                FileStream localFileStream = new FileStream(localFileName, FileMode.Create);

                var fileSize = sizeRequest.GetResponse().ContentLength;
                byte[] byteBuffer = new byte[bufferSize];
                int bytes = await ftpStream.ReadAsync(byteBuffer, 0, bufferSize);

                try
                {
                    while (bytes > 0)
                    {
                        
                        var progress = (int)((float)localFileStream.Length / (float)fileSize * 100);
                        TransferProgressControlVM.Instance.ProgressValue = progress;
                        await localFileStream.WriteAsync(byteBuffer, 0, bytes);
                        bytes = await ftpStream.ReadAsync(byteBuffer, 0, bufferSize);
                    }
                    TransferProgressControlVM.Instance.ProgressValue = 0;
                }
                catch (Exception ex)
                {
                    //TODO IoC messageBox
                    String e = ex.ToString();
                }
                localFileStream.Close();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                //TODO IoC messageBox
                String e = ex.ToString();
            }

        }

        public async Task FtpDeleteFile(string fileName)
        {
            FtpWebRequest request = Request(fileName, WebRequestMethods.Ftp.DeleteFile);

            FtpWebResponse response = await Response(request);
            response.Close();
        }

        public async Task<bool> AuthorizeFTPConnection()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Uri);
            request.Credentials = new NetworkCredential(userName, password);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            try
            {
                FtpWebResponse response = await Response(request);
                return true;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private FtpWebRequest Request(string requestPath, string method)
        {
            string requestUri = Uri;

            if (requestPath != null)
            {
                requestUri = $"{Uri}/{requestPath}";
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUri);
            request.Method = method;
            request.Credentials = credentials;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            return request;
        }

        private async Task<FtpWebResponse> Response(FtpWebRequest request)
        {
            FtpWebResponse response = null;
            try
            {
                WebResponse webResponse = await request.GetResponseAsync();
                response = (FtpWebResponse)webResponse;
            }
            catch (WebException e)
            {
                //TODO IoC messageBox
                String status = ((FtpWebResponse)e.Response).StatusDescription;
            }

            return response;
        }
    }
}
