using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace 手机摄像头
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 服务器状态，如果为false表示服务器暂停,true表示服务器开启
        /// </summary>
        public bool ServerStatus = false;
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerAddress;
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort;
        /// <summary>
        /// 开启服务的线程
        /// </summary>
        private Thread processor;
        /// <summary>
        /// 用于TCP监听
        /// </summary>
        private TcpListener tcpListener;
        /// <summary>
        /// 与客户端连接的套接字接口
        /// </summary>
        private Socket clientSocket;
        /// <summary>
        /// 用于处理客户事件的线程
        /// </summary>
        private Thread clientThread;
        /// <summary>
        /// 手机客户端所有客户端的套接字接口
        /// </summary>
        private Hashtable PhoneClientSockets = new Hashtable();
        /// <summary>
        /// 手机用户类数组
        /// </summary>
        public ArrayList PhoneUsersArray = new ArrayList();
        /// <summary>
        /// 手机用户名数组
        /// </summary>
        public ArrayList PhoneUserNamesArray = new ArrayList();
        /// <summary>
        /// 图像数据流
        /// </summary>
        private ArrayList StreamArray;

        public MainForm()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void phoneslistView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (phoneslistView.SelectedItems != null &&
                phoneslistView.SelectedItems.Count > 0)
            {
                ListViewItem tempItem = phoneslistView.SelectedItems[0];
                string tempUserName = tempItem.SubItems[1].Text;
                int tempIndex = GetPhoneUserIndex(tempUserName);
                if (tempIndex >= 0)
                {
                    UserClass tempUser = (UserClass)(PhoneUsersArray[tempIndex]);
                    if (tempUser != null)
                    {
                        PhoneVideoForm form = new PhoneVideoForm(tempUser);
                        this.AddOwnedForm(form);
                        form.Show();
                    }
                }
            }
        }

        #region 开启服务
        /// <summary>
        /// 开启服务
        /// </summary>
        public void StartServer()
        {
            try
            {
                if (this.ServerStatus)//服务器已经开启
                {
                    MessageBox.Show("服务已经开启!");
                }
                else
                {
                    processor = new Thread(new ThreadStart(StartListening));//建立监听服务器地址及端口的线程
                    processor.Start();
                    processor.IsBackground = true;

                    StreamArray = new ArrayList();
                    PhoneUserNamesArray = new ArrayList();
                }
                停止服务器button.Enabled = true;
                开启服务器button.Enabled = false;

                this.ServerStatus = true;
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message,
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region 停止服务
        /// <summary>
        /// 停止服务
        /// </summary>
        public void StopServer()
        {
            try
            {
                if (this.ServerStatus)
                {
                    tcpListener.Stop();
                    Thread.Sleep(1000);
                    processor.Abort();
                }
                else
                    MessageBox.Show("服务已经停止!");

                停止服务器button.Enabled = false;
                开启服务器button.Enabled = true;
                this.ServerStatus = false;
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message,
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        /// <summary>
        /// 开始监听服务器地址和端口
        /// </summary>
        private void StartListening()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ServerAddress);
                tcpListener = new TcpListener(ipAddress, ServerPort);//建立指定服务器地址和端口的TCP监听

                tcpListener.Start();//开始TCP监听
                while (true)
                {
                    Thread.Sleep(50);
                    try
                    {
                        Socket tempSocket = tcpListener.AcceptSocket();//接受挂起的连接请求
                        clientSocket = tempSocket;
                        clientThread = new Thread(new ThreadStart(ProcessClient));//建立处理客户端传递信息的事件线程
                        //线程于后台运行
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                    catch (Exception e)
                    {                        
                    }
                }
            }
            catch
            {
                this.ServerStatus = false;
                processor.Abort();
            }
        }

        #region 处理客户端传递数据及处理事情
        /// <summary>
        /// 处理客户端传递数据及处理事情
        /// </summary>
        private void ProcessClient()
        {
            Socket client = clientSocket;
            bool keepalive = true;
            while (keepalive)
            {
                Thread.Sleep(50);
                Byte[] buffer = null;
                bool tag = false;
                try
                {
                    buffer = new Byte[1024];//client.Available
                    int count = client.Receive(buffer, SocketFlags.None);//接收客户端套接字数据
                    if (count > 0)//接收到数据
                        tag = true;
                }
                catch (Exception e)
                {
                    keepalive = false;
                    if (client.Connected)
                        client.Disconnect(true);
                    client.Close();
                }
                if (!tag)
                {
                    if (client.Connected)
                        client.Disconnect(true);
                    client.Close();
                    keepalive = false;
                }

                string clientCommand = "";
                try
                {
                    clientCommand = System.Text.Encoding.UTF8.GetString(buffer);//转换接收的数据,数据来源于客户端发送的消息
                    if (clientCommand.Contains("%7C"))//从Android客户端传递部分数据
                        clientCommand = clientCommand.Replace("%7C", "|");//替换UTF中字符%7C为|
                }
                catch
                {
                }
                //分析客户端传递的命令来判断各种操作
                string[] messages = clientCommand.Split('|');
                if (messages != null && messages.Length > 0)
                {
                    string tempStr = messages[0];//第一个字符串为命令
                    if (tempStr == "PHONECONNECT")//手机连接服务器
                    {
                        try
                        {
                            string tempClientName = messages[1].Trim();
                            PhoneClientSockets.Remove(messages[1]);//删除之前与该用户的连接
                            PhoneClientSockets.Add(messages[1], client);//建立与该客户端的Socket连接                        

                            UserClass tempUser = new UserClass();
                            tempUser.UserName = tempClientName;
                            tempUser.LoginTime = DateTime.Now;
                            Socket tempSocket = (Socket)PhoneClientSockets[tempClientName];
                            tempUser.IPAddress = tempSocket.RemoteEndPoint.ToString();

                            int tempIndex = PhoneUserNamesArray.IndexOf(tempClientName);
                            if (tempIndex >= 0)
                            {
                                PhoneUserNamesArray[tempIndex] = tempClientName;
                                PhoneUsersArray[tempIndex] = tempUser;
                                MemoryStream stream2 = (MemoryStream)StreamArray[tempIndex];
                                if (stream2 != null)
                                {
                                    stream2.Close();
                                    stream2.Dispose();
                                }
                            }
                            else//新增加
                            {
                                PhoneUserNamesArray.Add(tempClientName);
                                PhoneUsersArray.Add(tempUser);
                                StreamArray.Add(null);
                            }
                            RefreshPhoneUsers();
                        }
                        catch (Exception except)
                        {
                        }
                    }
                    else if (tempStr == "PHONEDISCONNECT")//某个客户端退出了
                    {
                        try
                        {
                            string tempClientName = messages[1];
                            RemovePhoneUser(tempClientName);

                            int tempPhoneIndex = PhoneUserNamesArray.IndexOf(tempClientName);
                            if (tempPhoneIndex >= 0)
                            {
                                PhoneUserNamesArray.RemoveAt(tempPhoneIndex);
                                MemoryStream memStream = (MemoryStream)StreamArray[tempPhoneIndex];
                                if (memStream != null)
                                {
                                    memStream.Close();
                                    memStream.Dispose();
                                }
                                StreamArray.RemoveAt(tempPhoneIndex);
                            }
                            Socket tempSocket = (Socket)PhoneClientSockets[tempClientName];//第1个为客户端的ID,找到该套接字
                            if (tempSocket != null)
                            {
                                tempSocket.Close();
                                PhoneClientSockets.Remove(tempClientName);
                            }
                            keepalive = false;
                        }
                        catch (Exception except)
                        {
                        }
                        RefreshPhoneUsers();
                    }
                    else if (tempStr == "PHONEVIDEO")//接收手机数据流
                    {
                        try
                        {
                            string tempClientName = messages[1];
                            string tempForeStr = messages[0] + "%7C" + messages[1] + "%7C";
                            int startCount = System.Text.Encoding.UTF8.GetByteCount(tempForeStr);
                            try
                            {
                                MemoryStream stream = new MemoryStream();
                                if (stream.CanWrite)
                                {
                                    stream.Write(buffer, startCount, buffer.Length - startCount);
                                    int len = -1;
                                    while ((len = client.Receive(buffer)) > 0)
                                    {
                                        stream.Write(buffer, 0, len);
                                    }
                                }
                                stream.Flush();

                                int tempPhoneIndex = PhoneUserNamesArray.IndexOf(tempClientName);
                                if (tempPhoneIndex >= 0)
                                {
                                    MemoryStream stream2 = (MemoryStream)StreamArray[tempPhoneIndex];
                                    if (stream2 != null)
                                    {
                                        stream2.Close();
                                        stream2.Dispose();
                                    }
                                    StreamArray[tempPhoneIndex] = stream;

                                    PhoneVideoForm form = GetPhoneVideoForm(tempClientName);
                                    if (form != null)
                                        form.DataStream = stream;
                                }
                            }
                            catch
                            {
                            }
                        }
                        catch (Exception except)
                        {
                        }
                    }
                }
                else//客户端发送的命令或字符串为空,结束连接
                {
                    try
                    {
                        client.Close();
                        keepalive = false;
                    }
                    catch
                    {
                        keepalive = false;
                    }
                }
            }
        }
        #endregion

        #region 刷新手机用户列表
        /// <summary>
        /// 刷新手机用户列表
        /// </summary>
        public void RefreshPhoneUsers()
        {
            phoneslistView.Items.Clear();
            if (PhoneUsersArray != null && PhoneUsersArray.Count > 0
                && PhoneClientSockets != null && PhoneClientSockets.Count > 0)
            {
                int i, count = PhoneUsersArray.Count;
                UserClass tempUser;
                ListViewItem tempItem;
                ListViewItem.ListViewSubItem tempSubItem;
                Color tempColor;
                Socket tempSocket;
                for (i = 0; i < count; i++)
                {
                    tempUser = (UserClass)PhoneUsersArray[i];
                    tempSocket = (Socket)PhoneClientSockets[tempUser.UserName];
                    tempItem = phoneslistView.Items.Add((i + 1).ToString());
                    if (tempUser.Enable)
                        tempColor = Color.Blue;
                    else
                        tempColor = Color.Red;
                    tempItem.ForeColor = tempColor;
                    tempSubItem = tempItem.SubItems.Add(tempUser.UserName);
                    tempSubItem.ForeColor = tempColor;
                    tempSubItem = tempItem.SubItems.Add(tempUser.IPAddress);
                    tempSubItem.ForeColor = tempColor;
                    tempSubItem = tempItem.SubItems.Add(tempUser.LoginTime.ToString());
                    tempSubItem.ForeColor = tempColor;
                }
            }
        }
        #endregion

        #region 获取手机视频窗体
        /// <summary>
        /// 获取手机视频窗体
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public PhoneVideoForm GetPhoneVideoForm(string username)
        {
            PhoneVideoForm form = null;
            foreach (Form tempForm in this.OwnedForms)
            {
                if (tempForm is PhoneVideoForm)
                {
                    PhoneVideoForm tempForm2 = tempForm as PhoneVideoForm;
                    if (tempForm2.UserName == username)
                    {
                        form = tempForm2;
                        break;
                    }
                }
            }
            return form;
        }
        #endregion

        #region 删除手机用户
        /// <summary>
        /// 从当前用户列表中删除指定用户
        /// </summary>
        /// <param name="userName"></param>
        public void RemovePhoneUser(string userName)
        {
            if (PhoneUsersArray != null && PhoneUsersArray.Count > 0)
            {
                int i = 0;
                foreach (UserClass tempUser in PhoneUsersArray)
                    if (tempUser.UserName == userName)
                    {
                        PhoneUsersArray.RemoveAt(i);
                        break;
                    }
                    else
                        i++;
            }
        }
        #endregion

        #region 寻找用户所在序号
        /// <summary>
        /// 寻找用户所在序号
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GetPhoneUserIndex(string userName)
        {
            int result = -1;
            if (PhoneUserNamesArray != null && PhoneUserNamesArray.Count > 0)
            {
                int i = 0;
                foreach (string tempName in PhoneUserNamesArray)
                    if (tempName == userName)
                    {
                        result = i;
                        break;
                    }
                    else
                        i++;
            }
            return result;
        }
        #endregion

        private void 开启服务器button_Click(object sender, EventArgs e)
        {
            ServerAddress = iptextBox.Text;
            int tempPort = 9635;
            if (int.TryParse(porttextBox.Text, out tempPort))
            {
                ServerPort = tempPort;
                StartServer();
            }
            else
            {
                MessageBox.Show("端口设置不正确!");
            }
        }

        private void 停止服务器button_Click(object sender, EventArgs e)
        {
            StopServer();
        }

    }
}
