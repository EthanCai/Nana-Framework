using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// 邮件Helper
    /// </summary>
    public class EmailHelper
    {
        private static object _lockPad = new object();
        private static Dictionary<string, DateTime> _RepeateBuffer = new Dictionary<string, DateTime>();

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="smtpServer">smtp服务器；例如，elong：mta1.corp.ebj.elong.com</param>
        /// <param name="port">端口；例如，elong：25</param>
        /// <param name="from">发送方address</param>
        /// <param name="fromDisplayName">发送方显示名</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="toList">接收方address列表</param>
        public static void SendMail(string smtpServer, int port, string from, string fromDisplayName, string subject, string body, params string[] toList)
        {
            System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage();
            mm.From = new MailAddress(from, fromDisplayName);
            foreach (string to in toList.ToList())
            {
                mm.To.Add(to);
            }
            mm.Subject = subject;
            mm.Body = body;
            mm.IsBodyHtml = true;
            System.Net.Mail.SmtpClient st = new SmtpClient(smtpServer, port);
            st.DeliveryMethod = SmtpDeliveryMethod.Network;
            st.Send(mm);
        }

        /// <summary>
        /// Send Mail [应需求重载加入过滤标识]
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="smtpSetting"></param>
        /// <param name="isRepeat"></param>
        /// <returns></returns>
        public static string SendMail(string from, string to, string title, string message, string smtpServer, string smtpAccount, string smtpAccountPassword, int checkRepeateSeconds)
        {
            try
            {
                if (Repeat(title, checkRepeateSeconds) == false)
                {
                    //检查在规定时间内，没有重复发送的邮件
                    string result = String.Empty;

                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
                    client.Host = smtpServer; ;//指定SMTP服务器
                    client.Credentials = new System.Net.NetworkCredential(smtpAccount, smtpAccountPassword);//用户名和密码

                    MailMessage _mailMessage = new MailMessage();
                    _mailMessage.From = new MailAddress(from);

                    string[] toList = to.Split(",".ToCharArray());
                    foreach (string toSingle in toList)
                    {
                        if (toSingle.Trim() != string.Empty)
                        {
                            _mailMessage.To.Add(new MailAddress(toSingle));
                        }
                    }

                    _mailMessage.Subject = title;//主题
                    _mailMessage.Body = message + result;//内容
                    _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//正文编码
                    _mailMessage.IsBodyHtml = true;//设置为HTML格式
                    _mailMessage.Priority = MailPriority.High;//优先级

                    try
                    {
                        client.Send(_mailMessage);
                        Add2RepeatBuffer(title);
                        return "SUCCESS";
                    }
                    catch (Exception ex)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append("Failed\r\n");
                        builder.Append(ex.Message);
                        builder.Append("\r\n");
                        builder.Append(ex.StackTrace);

                        return builder.ToString();
                    }
                }
                else
                {
                    return "RepeateMail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        /// <summary>
        /// 发送Exchange邮件
        /// </summary>
        /// <param name="sSmtpServer"></param>
        /// <param name="sFrom"></param>
        /// <param name="sFromPass"></param>
        /// <param name="sto"></param>
        /// <param name="sSubject"></param>
        /// <param name="sBody"></param>
        /// <returns></returns>
        public static int SendExchangeMail(string sSmtpServer, string sFrom, string sFromPass, string sto, string sSubject, string sBody)
        {
            string sMethod = "SendSMTPEMail";
            string sLog;
            int iRet = 0;

            try
            {

                SmtpClient client = new SmtpClient(sSmtpServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(sFrom, sFromPass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                string[] mailtoList = sto.Split(',');
                foreach (string mailto in mailtoList)
                {
                    if (!String.IsNullOrEmpty(mailto))
                    {
                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(sFrom, mailto, sSubject, sBody);
                        message.Priority = MailPriority.High;
                        message.IsBodyHtml = true;
                        client.Send(message);
                    }
                }

                iRet = 1;
            }
            catch (Exception e)
            {
                sLog = "Exception in " + sMethod + ": " + e.Message;
            }
            finally
            {
                sLog = sMethod + " finally";
            }

            return iRet;

        }

        private static void Add2RepeatBuffer(string title)
        {
            lock (_lockPad)
            {
                if (_RepeateBuffer.ContainsKey(title) == false)
                {
                    _RepeateBuffer.Add(title, DateTime.Now);
                }
                else
                {
                    _RepeateBuffer[title] = DateTime.Now;
                }
            }
        }

        private static bool Repeat(string title, int checkRepeateSeconds)
        {
            lock (_lockPad)
            {
                if (_RepeateBuffer.ContainsKey(title) == false)
                {
                    return false;
                }
                else
                {
                    DateTime p = _RepeateBuffer[title];
                    TimeSpan ts = DateTime.Now - p;
                    if (ts.TotalSeconds < checkRepeateSeconds)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
