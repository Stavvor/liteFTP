﻿using System.Net;
using System.Security;


namespace liteFTP.ViewModels
{
    public class FTPcredentialsModel
    {
        public string ServerName{ get; set; }
        public string Username{ get; set; }
        public SecureString Password { get; set; }

        public NetworkCredential credentials;

        public FTPcredentialsModel(string servername, string username, SecureString password)
        {
            ServerName = servername;
            Username = username;
            Password = password;

            credentials = new NetworkCredential(username, password);
        }

    }
}
