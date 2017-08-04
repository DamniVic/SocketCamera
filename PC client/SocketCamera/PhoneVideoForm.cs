using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SocketCamera
{
    public partial class PhoneVideoForm : Form
    {
        #region 数据流
        /// <summary>
        /// 数据流
        /// </summary>
        public MemoryStream DataStream
        {
            set
            {
                    if (value != null)
                    {
                        videopictureBox.Image = Bitmap.FromStream(value);
                        value.Close();
                    }
            }
        }
        #endregion

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 用户对象
        /// </summary>
        public UserClass MyUser;

        public PhoneVideoForm(UserClass user)
        {
            this.UserName = user.UserName;
            this.MyUser = user;
            InitializeComponent();
            this.Text = "用户:" + MyUser.UserName + " [" + MyUser.IPAddress + "]" + MyUser.LoginTime.ToString();
        }
    }

    /// <summary>
    /// 用户类
    /// </summary>
    public class UserClass
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 登入时间
        /// </summary>
        public DateTime LoginTime;
        /// <summary>
        /// 登入IP地址
        /// </summary>
        public string IPAddress;
        /// <summary>
        /// 部门
        /// </summary>
        public string PartName;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark;
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable = true;
    }
}
