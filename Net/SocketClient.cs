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
using System.Threading;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;

namespace Wp7Shared.Net
{
    public class SocketClient : IDisposable
    {
        // Fields
        private static ManualResetEvent _clientDone = new ManualResetEvent(false);
        private Socket _socket;
        private const int MAX_BUFFER_SIZE = 0x800;
        private const int TIMEOUT_MILLISECONDS = 0x7530;

        // Methods
        public void Close()
        {
            try
            {
                if (this._socket != null)
                {
                    this._socket.Close();
                }
            }
            catch (Exception)
            {
                this._socket = null;
            }
        }

        public string Connect(string hostName, int portNumber)
        {
            string result = string.Empty;
            DnsEndPoint point = new DnsEndPoint(hostName, portNumber);
           // this._socket = new Socket(2, 1, 6);
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = point;
            //args.Completed +=new EventHandler<SocketAsyncEventArgs>(args_Completed); delegate(object s, SocketAsyncEventArgs e)
            //{
            //    result = e.SocketError.ToString();
            //    _clientDone.Set();
            //});

            args.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                result = e.SocketError.ToString();
                _clientDone.Set();
            });


            _clientDone.Reset();
            this._socket.ConnectAsync(args);
            _clientDone.WaitOne(0x7530);
            return result;
        }

        //void args_Completed(object sender, SocketAsyncEventArgs e)
        //{
        //    result = e.SocketError.ToString();
        //    _clientDone.Set();
        //}

        public void Dispose()
        {
            try
            {
                if (this._socket != null)
                {
                    this.Close();
                    this._socket.Dispose();
                    this._socket = null;
                }
            }
            catch (Exception)
            {
                this._socket = null;
            }
        }

        //public string Receive([Optional, DefaultParameterValue(0)] int timeout_ms)

        public string Receive()
        {
            return Receive(0);
        }

        public string Receive(int timeout_ms)
        {
            EventHandler<SocketAsyncEventArgs> handler = null;
            string response = "Operation Timeout";
            if (this._socket != null)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.RemoteEndPoint = (this._socket.RemoteEndPoint);
                args.SetBuffer(new byte[0x800], 0, 0x800);
                if (handler == null)
                {
                    handler = delegate(object s, SocketAsyncEventArgs e)
                    {
                        if (e.SocketError == null)
                        {
                            response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                            response = response.Trim(new char[1]);
                        }
                        else
                        {
                            response = e.SocketError.ToString();
                        }
                        _clientDone.Set();
                    };
                }
                //args.add_Completed(handler);

                args.Completed += handler;
                _clientDone.Reset();
                this._socket.ReceiveAsync(args);
                if (timeout_ms < 0x7530)
                {
                    timeout_ms = 0x7530;
                }
                _clientDone.WaitOne(timeout_ms);
            }
            else
            {
                response = "Socket is not initialized";
            }
            return response;
        }

        // public string Send(string data, [Optional, DefaultParameterValue(0)] int timeout_ms)

        public string Send(string data)
        {
            return Send(data, 0);
        }

        public string Send(string data, int timeout_ms)
        {
            EventHandler<SocketAsyncEventArgs> handler = null;
            string response = "Operation Timeout";
            if (this._socket != null)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.RemoteEndPoint = (this._socket.RemoteEndPoint);
                args.UserToken = null;
                if (handler == null)
                {
                    handler = delegate(object s, SocketAsyncEventArgs e)
                    {
                        response = e.SocketError.ToString();
                        _clientDone.Set();
                    };
                }
               // args.add_Completed(handler);
                args.Completed += handler;
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                args.SetBuffer(bytes, 0, bytes.Length);
                _clientDone.Reset();
                this._socket.SendAsync(args);
                if (timeout_ms <= 0x7530)
                {
                    timeout_ms = 0x7530;
                }
                _clientDone.WaitOne(timeout_ms);
            }
            else
            {
                response = "Socket is not initialized";
            }
            return response;
        }

        // Properties
        public bool Connected
        {
            get
            {
                if (this._socket == null)
                {
                    return false;
                }
                return this._socket.Connected;
            }
        }
    }
}