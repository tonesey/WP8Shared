using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SBSimpleSSL;
using SBX509;
using SBUtils;
using SBSSLCommon;
using System.Threading;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;

namespace Wp7Shared.Net
{
    public class SmtpClient : BaseDisposable
    {
        // Fields
        private List<string> attachments = new List<string>();
        public string BodyMessage;
        public bool busy;
        public List<string> CCn = new List<string>();
        public string From;
        private Regex isEmailAddress = new Regex("^(([^<>()[\\]\\\\.,;:\\s@\\\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$", RegexOptions.Compiled);
        private string lastError = string.Empty;
        private string lastRequest = string.Empty;
        private string lastResponse = string.Empty;
        public string Password;
        private EventHandler<ValueEventArgs<bool>> MailSent;
        private EventHandler<ValueEventArgs<int>> Progress;
        public string ServerHost;
        public int ServerPort;
        public string Subject;
        public string To;
        public string UserName;
        public bool useSSL = true;

        //// Events
        //public event EventHandler<ValueEventArgs<bool>> MailSent
        //{
        //    add
        //    {
        //        EventHandler<ValueEventArgs<bool>> handler2;
        //        EventHandler<ValueEventArgs<bool>> mailSent = this.MailSent;
        //        do
        //        {
        //            handler2 = mailSent;
        //            EventHandler<ValueEventArgs<bool>> handler3 = (EventHandler<ValueEventArgs<bool>>)Delegate.Combine(handler2, value);
        //            mailSent = Interlocked.CompareExchange<EventHandler<ValueEventArgs<bool>>>(ref this.MailSent, handler3, handler2);
        //        }
        //        while (mailSent != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<ValueEventArgs<bool>> handler2;
        //        EventHandler<ValueEventArgs<bool>> mailSent = this.MailSent;
        //        do
        //        {
        //            handler2 = mailSent;
        //            EventHandler<ValueEventArgs<bool>> handler3 = (EventHandler<ValueEventArgs<bool>>)Delegate.Remove(handler2, value);
        //            mailSent = Interlocked.CompareExchange<EventHandler<ValueEventArgs<bool>>>(ref this.MailSent, handler3, handler2);
        //        }
        //        while (mailSent != handler2);
        //    }
        //}

        //public event EventHandler<ValueEventArgs<int>> Progress
        //{
        //    add
        //    {
        //        EventHandler<ValueEventArgs<int>> handler2;
        //        EventHandler<ValueEventArgs<int>> progress = this.Progress;
        //        do
        //        {
        //            handler2 = progress;
        //            EventHandler<ValueEventArgs<int>> handler3 = (EventHandler<ValueEventArgs<int>>)Delegate.Combine(handler2, value);
        //            progress = Interlocked.CompareExchange<EventHandler<ValueEventArgs<int>>>(ref this.Progress, handler3, handler2);
        //        }
        //        while (progress != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<ValueEventArgs<int>> handler2;
        //        EventHandler<ValueEventArgs<int>> progress = this.Progress;
        //        do
        //        {
        //            handler2 = progress;
        //            EventHandler<ValueEventArgs<int>> handler3 = (EventHandler<ValueEventArgs<int>>)Delegate.Remove(handler2, value);
        //            progress = Interlocked.CompareExchange<EventHandler<ValueEventArgs<int>>>(ref this.Progress, handler3, handler2);
        //        }
        //        while (progress != handler2);
        //    }
        //}

        // Methods
        public SmtpClient()
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                SBUtils.__Global.SetLicenseKey("6D622CA99FC198587E919CFBF546CA15FF0B27AF58F614AB6934B2ABBBECAF32AD5405556908985D62FEF8D1E241049760F7921EAA4256D1B8074724ACB6152EDE65F03B19DED371F4737687AC7DA2F99E4E5BC53ED2D6DC50D294E15F8198870E96D8B66C1C6E12927A84BCD365138EE080D36E413D3B1F1E3D59DE8A4E4697AF289EE683BC9DE6550C071CF57DE579ABB23BACC34177BFF1EF0DB24223CBDAAC8A2830377286DE01F8989097B4F07790CBF5EEEED591EDFCDC707CA2111A151C62659A11D5B1444C4EC8F0A61F466AB7792D08897B685548F34B59C6A6FDDEC7D4350F919F05774F2F0602B5975C51FC59F7776451869F518AA717352FC4AE");
            });
        }

        public void AddAttachment(string filePath)
        {
            if (!this.attachments.Contains(filePath))
            {
                this.attachments.Add(filePath);
            }
        }

        protected override void DisposeBaseManagedResource()
        {
            if (this.attachments != null)
            {
                this.attachments.Clear();
            }
            this.attachments = null;
            if (this.CCn != null)
            {
                this.CCn.Clear();
            }
            this.CCn = null;
            base.DisposeBaseManagedResource();
        }

        private static string extractFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            int index = path.Replace(@"\", "/").IndexOf("/");
            if (index == -1)
            {
                return path;
            }
            return path.Substring(index + 1, (path.Length - index) - 1);
        }

        private static byte[] GetFileBytes(string filePath)
        {
            byte[] buffer2;
            try
            {
                byte[] buffer = null;
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!file.FileExists(filePath))
                    {
                        using (Stream stream = TitleContainer.OpenStream(filePath))
                        {
                            buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            goto Label_0077;
                        }
                    }
                    using (IsolatedStorageFileStream stream2 = file.OpenFile(filePath, FileMode.Open))
                    {
                        buffer = new byte[stream2.Length];
                        stream2.Read(buffer, 0, buffer.Length);
                    }
                }
            Label_0077:
                buffer2 = buffer;
            }
            catch (Exception)
            {
                buffer2 = null;
            }
            return buffer2;
        }

        public bool IsEmailAddress(string candidate)
        {
            return this.isEmailAddress.IsMatch(candidate);
        }

        private void Log(string message)
        {
            Log(message, 0);
        }

        private void Log(string message, eVersus outBoud)
        {
            // this.RaiseDebugInfo(this, outBoud.ToString() + "  " + message, DebugInfoType.Info_DebugInfo, LayerType.Lay_All);
        }

        private void RaiseMailSent()
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                if (this.MailSent != null)
                {
                    this.MailSent(this, new ValueEventArgs<bool>(string.IsNullOrEmpty(this.LastError)));
                }
            });
        }

        private void RaiseProgress(int perc)
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                if (this.Progress != null)
                {
                    this.Progress(this, new ValueEventArgs<int>(perc));
                }
            });
        }


        //             private bool SendAndWaitResponse(SocketClient client, string command, int successCode, [Optional, DefaultParameterValue(0)] int timeout_ms)
        private bool SendAndWaitResponse(SocketClient client, string command, int successCode)
        {
            return SendAndWaitResponse(client, command, successCode, 0);
        }

        private bool SendAndWaitResponse(SocketClient client, string command, int successCode, int timeout_ms)
        {
            try
            {
                this.Log(command, eVersus.OUT);
                if ((client == null) || !client.Connected)
                {
                    this.LastError = "Not connected.";
                    return false;
                }
                string message = client.Send(command + "\r\n", timeout_ms);
                if (message != "Success")
                {
                    this.LastError = message;
                    return false;
                }
                message = client.Receive(timeout_ms);
                this.Log(message, eVersus.IN);
                if (command != "QUIT")
                {
                    if (message.Substring(0, 3) != successCode.ToString())
                    {
                        this.LastError = message;
                        return false;
                    }
                }
                else if (message.IndexOf("221") == -1)
                {
                    this.LastError = message;
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.LastError = exception.Message;
                return false;
            }
        }

        //public bool SendMailAsync(ref string errorMessage)
        //{
        //    if (((string.IsNullOrEmpty(this.ServerHost) || string.IsNullOrEmpty(this.UserName)) || (string.IsNullOrEmpty(this.Password) || !this.IsEmailAddress(this.To))) || (!this.IsEmailAddress(this.From) || (this.ServerPort == 0)))
        //    {
        //        errorMessage = "Please fill all properties first.";
        //        return false;
        //    }
        //    if (this.busy)
        //    {
        //        errorMessage = "Busy";
        //        return false;
        //    }
        //    this.LastError = "";
        //    this.lastRequest = "";
        //    string errmsg = "";

        //    //new Thread(delegate
        //    //{
        //    //    this.SendMailSync(ref errmsg);
        //    //}).Start();

        //    Thread newThread = new Thread(new ThreadStart(SendMailSync));
        //    newThread.Start();
        //    return true;
        //}

        public bool SendMail(out string errorMessage)
        {
            if ((string.IsNullOrEmpty(this.ServerHost) || string.IsNullOrEmpty(this.UserName)) || ((string.IsNullOrEmpty(this.Password) || !this.IsEmailAddress(this.To)) || !this.IsEmailAddress(this.From)))
            {
               errorMessage = "Please fill all properties first.";
                return false;
            }
            bool flag = false;
            if (!this.useSSL)
            {
                flag = this.sendMailSyncNoSSL();
            }
            else
            {
                flag = this.sendMailSyncSSL();
            }
            errorMessage = this.LastError;
            return flag;
        }

        public bool sendMailSyncNoSSL()
        {
            bool flag;
            try
            {
                if (this.busy)
                {
                    this.LastError = "Busy";
                    return false;
                }
                this.busy = true;
                this.LastError = string.Empty;
                using (SocketClient client = new SocketClient())
                {
                    this.Log("Connecting...", eVersus.OUT);
                    string str = client.Connect(this.ServerHost, this.ServerPort);
                    if (str != "Success")
                    {
                        this.LastError = "Connect error: " + str;
                        return false;
                    }
                    string message = client.Receive(0);
                    this.Log(message, eVersus.IN);
                    if (message.Substring(0, 3) != "220")
                    {
                        this.LastError = "Connect response error: " + message;
                        return false;
                    }
                    if (!string.IsNullOrEmpty(this.UserName))
                    {
                        if (!this.SendAndWaitResponse(client, "EHLO me", 250, 0))
                        {
                            return false;
                        }
                        if (!this.SendAndWaitResponse(client, "AUTH LOGIN", 0x14e, 0))
                        {
                            return false;
                        }
                        string str3 = this.ToBase64String(this.UserName);
                        if (!this.SendAndWaitResponse(client, str3, 0x14e, 0))
                        {
                            return false;
                        }
                        str3 = this.ToBase64String(this.Password);
                        if (!this.SendAndWaitResponse(client, str3, 0xeb, 0))
                        {
                            return false;
                        }
                    }
                    else if (!this.SendAndWaitResponse(client, "HELO me", 250, 0))
                    {
                        return false;
                    }
                    if (!this.SendAndWaitResponse(client, "MAIL FROM:<" + this.From + ">", 250, 0))
                    {
                        return false;
                    }
                    if (!this.SendAndWaitResponse(client, "RCPT TO:<" + this.To + ">", 250, 0))
                    {
                        return false;
                    }
                    foreach (string str4 in this.CCn)
                    {
                        if (!this.SendAndWaitResponse(client, "RCPT TO:<" + str4 + ">", 250, 0))
                        {
                            return false;
                        }
                    }
                    string command = string.Empty;
                    StringBuilder builder = new StringBuilder();
                    builder.Append("From: " + this.From + "\r\n");
                    builder.Append("To: " + this.To + "\r\n");
                    foreach (string str6 in this.CCn)
                    {
                        builder.Append("To: " + str6 + "\r\n");
                    }
                    builder.Append("Subject: " + this.Subject + "\r\n");
                    StringBuilder builder2 = new StringBuilder();
                    if (this.attachments.Count == 0)
                    {
                        builder2.Append(this.BodyMessage + "\r\n");
                    }
                    else
                    {
                        builder.Append("MIME-Version: 1.0\r\n");
                        builder.Append("Content-Type: multipart/mixed; boundary=unique-boundary-1\r\n");
                        builder.Append("\r\n");
                        builder.Append("This is a multi-part message in MIME format.\r\n");
                        builder2.Append("--unique-boundary-1\r\n");
                        builder2.Append("Content-Type: text/plain\r\n");
                        builder2.Append("Content-Transfer-Encoding: 7Bit\r\n");
                        builder2.Append("\r\n");
                        builder2.Append(this.BodyMessage + "\r\n");
                        builder2.Append("\r\n");
                        foreach (string str7 in this.attachments)
                        {
                            byte[] fileBytes = GetFileBytes(str7);
                            if (fileBytes != null)
                            {
                                int num2;
                                string str8 = extractFileName(str7);
                                builder2.Append("--unique-boundary-1\r\n");
                                builder2.Append("Content-Type: application/octet-stream; file=" + str8 + "\r\n");
                                builder2.Append("Content-Transfer-Encoding: base64\r\n");
                                builder2.Append("Content-Disposition: attachment; filename=" + str8 + "\r\n");
                                builder2.Append("\r\n");
                                string str9 = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                                for (int i = 0; i < str9.Length; i += num2)
                                {
                                    num2 = 100;
                                    if ((str9.Length - (i + num2)) < 0)
                                    {
                                        num2 = str9.Length - i;
                                    }
                                    builder2.Append(str9.Substring(i, num2));
                                    builder2.Append("\r\n");
                                }
                                builder2.Append("\r\n");
                            }
                        }
                    }
                    command = builder.ToString() + builder2.ToString();
                    if (!command.EndsWith("\r\n"))
                    {
                        command = command + "\r\n";
                    }
                    command = command + ".";
                    if (!this.SendAndWaitResponse(client, "DATA", 0x162, 0))
                    {
                        return false;
                    }
                    if (!this.SendAndWaitResponse(client, command, 250, 0x7530))
                    {
                        return false;
                    }
                    if (!this.SendAndWaitResponse(client, "QUIT", 0xdd, 0))
                    {
                        client.Close();
                        return false;
                    }
                    client.Close();
                }
                this.LastError = "";
                flag = true;
            }
            catch (Exception exception)
            {
                this.LastError = exception.Message;
                flag = false;
            }
            finally
            {
                this.attachments.Clear();
                this.RaiseMailSent();
                this.busy = false;
            }
            return flag;
        }

        private bool sendMailSyncSSL()
        {
            bool flag;
            try
            {
                if (this.busy)
                {
                    this.LastError = "Busy";
                    return false;
                }
                this.busy = true;
                this.LastError = string.Empty;
                using (TElSimpleSSLClient client = new TElSimpleSSLClient())
                {
                    client.SocketTimeout = 0xea60;
                    client.OnError += new TSBErrorEvent(this.sslClient_OnError);
                    client.OnCertificateValidate += new TSBCertificateValidateEvent(this.sslClient_OnCertificateValidate);
                    this.lastRequest = "-Connect-";
                    this.Log("Connecting", eVersus.OUT);
                    client.Address = this.ServerHost;
                    client.Port = this.ServerPort;
                    this.RaiseProgress(0);
                    client.Enabled = false;
                    client.Open();
                    if (!this.WaitResponse(client, 220, 0x7530))
                    {
                        return false;
                    }
                    if (!this.WriteDataAndWaitResponse(client, "HELO me", 250, 0xea60))
                    {
                        return false;
                    }
                    if (!this.WriteDataAndWaitResponse(client, "STARTTLS", 220, 0x7530))
                    {
                        return false;
                    }
                    this.Log(">>> START SSL AUTH <<<<<<", eVersus.OUT);
                    client.StartTLS();
                    this.Log(">>> SSL AUTH OK <<<<<<", eVersus.OUT);
                    if (!this.WriteDataAndWaitResponse(client, "HELO me", 250, 0x7530))
                    {
                        return false;
                    }
                    if (!this.WriteDataAndWaitResponse(client, "AUTH LOGIN", 0x14e, 0x7530))
                    {
                        return false;
                    }
                    string request = this.ToBase64String(this.UserName);
                    if (!this.WriteDataAndWaitResponse(client, request, 0x14e, 0x7530))
                    {
                        this.lastRequest = string.Empty;
                        this.lastResponse = string.Empty;
                        this.LastError = "Invalid username - " + this.lastError;
                        return false;
                    }
                    request = this.ToBase64String(this.Password);
                    if (!this.WriteDataAndWaitResponse(client, request, 0xeb, 0x7530))
                    {
                        this.lastRequest = string.Empty;
                        this.lastResponse = string.Empty;
                        this.LastError = "Invalid username and/or password - " + this.lastError;
                        return false;
                    }
                    if (!this.WriteDataAndWaitResponse(client, "MAIL FROM:<" + this.From + ">", 250, 0x7530))
                    {
                        this.LastError = "FROM address not valid - " + this.lastError;
                        return false;
                    }
                    if (!this.WriteDataAndWaitResponse(client, "RCPT TO:<" + this.To + ">", 250, 0x7530))
                    {
                        this.LastError = "TO address not valid - " + this.lastError;
                        return false;
                    }
                    foreach (string str2 in this.CCn)
                    {
                        if (!this.WriteDataAndWaitResponse(client, "RCPT TO:<" + str2 + ">", 250, 0x7530))
                        {
                            this.LastError = "TO address not valid - " + this.lastError;
                            return false;
                        }
                    }
                    string str3 = string.Empty;
                    StringBuilder builder = new StringBuilder();
                    builder.Append("From: " + this.From + "\r\n");
                    builder.Append("To: " + this.To + "\r\n");
                    foreach (string str4 in this.CCn)
                    {
                        builder.Append("To: " + str4 + "\r\n");
                    }
                    builder.Append("Subject: " + this.Subject + "\r\n");
                    StringBuilder builder2 = new StringBuilder();
                    if (this.attachments.Count == 0)
                    {
                        builder2.Append(this.BodyMessage + "\r\n");
                    }
                    else
                    {
                        builder.Append("MIME-Version: 1.0\r\n");
                        builder.Append("Content-Type: multipart/mixed; boundary=unique-boundary-1\r\n");
                        builder.Append("\r\n");
                        builder.Append("This is a multi-part message in MIME format.\r\n");
                        builder2.Append("--unique-boundary-1\r\n");
                        builder2.Append("Content-Type: text/plain\r\n");
                        builder2.Append("Content-Transfer-Encoding: 7Bit\r\n");
                        builder2.Append("\r\n");
                        builder2.Append(this.BodyMessage + "\r\n");
                        builder2.Append("\r\n");
                        foreach (string str5 in this.attachments)
                        {
                            byte[] fileBytes = GetFileBytes(str5);
                            if (fileBytes != null)
                            {
                                int num2;
                                string str6 = extractFileName(str5);
                                builder2.Append("--unique-boundary-1\r\n");
                                builder2.Append("Content-Type: application/octet-stream; file=" + str6 + "\r\n");
                                builder2.Append("Content-Transfer-Encoding: base64\r\n");
                                builder2.Append("Content-Disposition: attachment; filename=" + str6 + "\r\n");
                                builder2.Append("\r\n");
                                string str7 = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                                for (int i = 0; i < str7.Length; i += num2)
                                {
                                    num2 = 100;
                                    if ((str7.Length - (i + num2)) < 0)
                                    {
                                        num2 = str7.Length - i;
                                    }
                                    builder2.Append(str7.Substring(i, num2));
                                    builder2.Append("\r\n");
                                }
                                builder2.Append("\r\n");
                            }
                        }
                    }
                    str3 = builder.ToString() + builder2.ToString();
                    if (!str3.EndsWith("\r\n"))
                    {
                        str3 = str3 + "\r\n";
                    }
                    str3 = str3 + ".";
                    if (!this.WriteDataAndWaitResponse(client, "DATA", 0x162, 0x7530))
                    {
                        return false;
                    }
                    string str8 = str3;
                    if (!this.WriteDataAndWaitResponse(client, str8, 250, 0xea60))
                    {
                        return false;
                    }
                    if (!this.WriteDataAndWaitResponse(client, "QUIT", 0xdd, 0x7530))
                    {
                        return false;
                    }
                    client.Close(true);
                    this.RaiseProgress(100);
                }
                this.LastError = "";
                flag = true;
            }
            catch (Exception exception)
            {
                this.LastError = exception.Message;
                flag = false;
            }
            finally
            {
                this.attachments.Clear();
                this.busy = false;
                this.RaiseMailSent();
            }
            return flag;
        }

        private void sslClient_OnCertificateValidate(object Sender, TElX509Certificate X509Certificate, ref TSBBoolean Validate)
        {
            //this.Log("sslClient_OnCertificateValidate", eVersus.OUT);
            //Validate = 1;
        }

        private void sslClient_OnError(object Sender, int ErrorCode, bool Fatal, bool Remote)
        {
            this.LastError = ErrorCode.ToString();
            this.Log("sslClient_OnError", eVersus.OUT);
            //this.RaiseError(Sender, ErrorCode.ToString(), SeverityType.Sev_All, LayerType.Lay_All);
        }

        private string ToBase64String(string toEncode)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(toEncode));
        }

        //private bool WaitResponse(TElSimpleSSLClient sslClient, [Optional, DefaultParameterValue(250)] int expectedResponseCode, [Optional, DefaultParameterValue(0x7530)] int timeout_ms)
        private bool WaitResponse(TElSimpleSSLClient sslClient, int expectedResponseCode, int timeout_ms)
        {
            try
            {
                this.lastResponse = string.Empty;
                string str = string.Empty;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int size = 0x3e8;
                while (true)
                {
                    if ((sslClient == null) || !sslClient.Active)
                    {
                        this.LastError = "Not connected!";
                        return false;
                    }
                    if (sslClient.CanReceive(100))
                    {
                        byte[] buffer = new byte[size];
                        try
                        {
                            sslClient.ReceiveData(ref buffer, ref size, false);
                            if (size > 0)
                            {
                                str = SBUtils.__Global.StringOfBytes(buffer, 0, size);
                                this.lastResponse = this.lastResponse + str;
                                stopwatch.Stop();
                                stopwatch.Reset();
                                stopwatch.Start();
                            }
                        }
                        catch (Exception exception)
                        {
                            if (!exception.Message.Contains("timeout"))
                            {
                                throw exception;
                            }
                            this.lastResponse = "TIMEOUT\n\r";
                            break;
                        }
                        if (this.lastResponse.EndsWith("\r\n"))
                        {
                            break;
                        }
                    }
                    if (stopwatch.ElapsedMilliseconds >= timeout_ms)
                    {
                        if (this.lastResponse.Length > 0)
                        {
                            this.Log("Incomplete message: " + this.lastResponse, eVersus.OUT);
                        }
                        this.LastError = "TIMEOUT";
                        return false;
                    }
                    Thread.Sleep(250);
                }
                stopwatch.Stop();
                stopwatch = null;
                this.Log("IN: " + this.lastResponse.Substring(0, this.lastResponse.Length - 2), eVersus.OUT);
                if (!this.lastResponse.StartsWith(expectedResponseCode.ToString()))
                {
                    this.LastError = "Unexpected response code: expected " + expectedResponseCode.ToString();
                    return false;
                }
                this.lastError = string.Empty;
                return true;
            }
            catch (Exception exception2)
            {
                this.LastError = "ERROR: " + exception2.Message;
                return false;
            }
        }

        //   private bool WriteDataAndWaitResponse(TElSimpleSSLClient sslClient, string request, int expectedResponseCode, [Optional, DefaultParameterValue(0x7530)] int timeout_ms)


        private bool WriteDataAndWaitResponse(TElSimpleSSLClient sslClient, string request, int expectedResponseCode)
        {
            return WriteDataAndWaitResponse(sslClient, request, expectedResponseCode, 0x7530);
        }

        private bool WriteDataAndWaitResponse(TElSimpleSSLClient sslClient, string request, int expectedResponseCode, int timeout_ms)
        {
            if (!string.IsNullOrEmpty(request))
            {
                this.lastResponse = string.Empty;
                this.lastRequest = request;
                this.Log("OUT: " + request, eVersus.OUT);
                if ((sslClient == null) || !sslClient.Active)
                {
                    this.LastError = "Not connected!";
                    return false;
                }
                request = request + "\r\n";
                int length = request.Length;
                bool flag = false;
                if (length > 0x186a0)
                {
                    flag = true;
                }
                int startIndex = 0;
                int num3 = 0xc800;
                do
                {
                    if ((startIndex + num3) > length)
                    {
                        num3 = length - startIndex;
                    }
                    sslClient.SendData(SBUtils.__Global.BytesOfString(request.Substring(startIndex, num3)));
                    startIndex += num3;
                    if (flag)
                    {
                        this.RaiseProgress((100 * startIndex) / length);
                    }
                }
                while (startIndex < length);
            }
            return this.WaitResponse(sslClient, expectedResponseCode, timeout_ms);
        }

        // Properties
        public string LastError
        {
            get
            {
                if (string.IsNullOrEmpty(this.lastError))
                {
                    return string.Empty;
                }
                return (this.lastRequest + this.lastResponse + " " + this.lastError).Trim();
            }
            private set
            {
                this.lastError = value;
            }
        }

        // Nested Types
        private enum eVersus
        {
            OUT,
            IN
        }
    }
}
