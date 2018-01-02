using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace liteFTP.ViewModels
{
    public class FTPcredentialsVM
    {
        public string ServerName{ get; set; }
        public string Username{ get; set; }
        public string Password { get; set; }

        public NetworkCredential credentials;

        public FTPcredentialsVM(string servername, string username, string password)
        {
            ServerName = servername;
            Username = username;
            Password = password;

            credentials = new NetworkCredential(username, password);
        }

    }
}
