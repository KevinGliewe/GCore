using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

using GCore.Extensions.StreamEx;

namespace GCore.Networking {
    public class NetworkingHelper {
        public static string FTPUpload(string ftpServer, string filename) {
            return FTPUpload(ftpServer, filename, "anonymous", "");
        }
        public static string FTPUpload(string ftpServer, string filename, string userName, string password) {

            using (System.Net.WebClient client = new System.Net.WebClient()) {
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                client.UploadFile(ftpServer + "/" + new FileInfo(filename).Name, "STOR", filename);
            }

            return "OK";
        }


        public static string FTPUploadS(string ftpServerFile, string data) {
            return FTPUploadS(ftpServerFile, data, "anonymous", "");
        }
        public static string FTPUploadS(string ftpServerFile, string data, string userName, string password) {

            using (System.Net.WebClient client = new System.Net.WebClient()) {
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                //client.UploadFile(ftpServerFile, "STOR", filename);
                client.UploadString(ftpServerFile, data);
            }

            return "OK";
        }



        public static string FTPDownload(string ftpServerFile, string filename) {
            return FTPDownload(ftpServerFile, filename, "anonymous", "");
        }
        public static string FTPDownload(string ftpServerFile, string filename, string userName, string password) {

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerFile);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(userName, password);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            responseStream.StreamToFile(filename, FileMode.OpenOrCreate);

            return "OK";
        }


        public static string FTPDownloadS(string ftpServerFile, string filename) {
            return FTPDownloadS(ftpServerFile, filename, "anonymous", "");
        }

        public static string FTPDownloadS(string ftpServerFile, string filename, string userName, string password) {

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerFile);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(userName, password);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);
            string tmp = reader.ReadToEnd();
            reader.Close();

            return tmp;
        }

        public static string wget(string url, string dst) {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, dst);
            return "OK";
        }

        public static string wget(string url) {
            WebClient client = new WebClient();
            return client.DownloadString(url);
        }

    }
}
