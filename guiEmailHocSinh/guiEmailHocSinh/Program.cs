using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;
using System.Collections;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using guiEmailHocSinh;
using Newtonsoft.Json.Linq;

namespace JogetEmail
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();
            string acc = "khanhcaolambao@gmail.com";
            string pw = "pngavxqzysecvwma";

            Console.OutputEncoding = Encoding.UTF8;
            client.BaseAddress = new Uri("https://khanhcao.cloud.joget.com/jw/api");
            client.Timeout = TimeSpan.FromSeconds(30);

            //connect use API HTTP
            var request = new HttpRequestMessage(HttpMethod.Get, "https://khanhcao.cloud.joget.com/jw/api/list/list_student_temp");
            request.Headers.Add("api_id", "API-aa2652f7-9eb5-41ff-bd52-feb5bf99f814");
            request.Headers.Add("api_key", "cf55893c4b274c8eaafa15f7de0f25e4");

            //connect use API HTTP
            var request1 = new HttpRequestMessage(HttpMethod.Get, "https://khanhcao.cloud.joget.com/jw/api/list/list_student_Info");
            request1.Headers.Add("api_id", "API-aa2652f7-9eb5-41ff-bd52-feb5bf99f814");
            request1.Headers.Add("api_key", "cf55893c4b274c8eaafa15f7de0f25e4");

        
            var request2 = new HttpRequestMessage(HttpMethod.Get, "https://khanhcao.cloud.joget.com/jw/api/list/listClass_Info");
            request2.Headers.Add("api_id", "API-aa2652f7-9eb5-41ff-bd52-feb5bf99f814");
            request2.Headers.Add("api_key", "cf55893c4b274c8eaafa15f7de0f25e4");
            //ArrayList listSendEmailStudent = new ArrayList();
            Dictionary<string, string> infoSendEmail = new Dictionary<string, string>();

            using var response = await client.SendAsync(request);
            using var response1 = await client.SendAsync(request1);
            using var response2 = await client.SendAsync(request2);
            /*Issue: 1. How to get email of object? */
            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine(currentDateTime);

            //Lấy ngày hiện tại của hệ thống
            DateTime date = DateTime.Now;
            string day = date.ToString("dd MMM yyyy");
            string dayOfWeek = date.DayOfWeek.ToString().Substring(0,3);
            string dayTemp = dayOfWeek + ", " +day ;
            Console.WriteLine(dayTemp);


            if (response.IsSuccessStatusCode && response1.IsSuccessStatusCode)
            {
                //in ra danh sach du lieu json
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                //in ra danh sach du lieu json
                var content1 = await response1.Content.ReadAsStringAsync();
                Console.WriteLine(content1);
                //in ra danh sach du lieu json
                var content2 = await response2.Content.ReadAsStringAsync();
                Console.WriteLine(content2);

                EmailData root = JsonConvert.DeserializeObject<EmailData>(content);
                EmailData1 root1 = JsonConvert.DeserializeObject<EmailData1>(content1);
                EmailData2 root2 = JsonConvert.DeserializeObject<EmailData2>(content2);
                Console.WriteLine(root);

                string noiDungGuiEmail = "";

                int ind = 0;

                foreach (data1_ dat in root1.data)
                {
                    foreach (data_ dt in root.data)
                    {
                        if (dat.id == dt.student_id)
                        {
                            infoSendEmail.Add("Name" + ind, dat.studentName);
                            infoSendEmail.Add("Class" + ind, dat.studentClass);
                            infoSendEmail.Add("Mail" + ind, dat.studentEmail);

                            infoSendEmail.Add("Temperature" + ind, dt.tempReading);
                            infoSendEmail.Add("Time" + ind, dt.tempTime);
                            infoSendEmail.Add("Status" + ind, dt.status);
                            infoSendEmail.Add("Date" + ind, dt.tempDate);
                            infoSendEmail.Add("Remark" + ind, dt.tempRemarks);

                            ind++;
                        }
                    }
                }


                Console.WriteLine("size of list " + ind);
                Console.WriteLine("Key and Value pairs from my Dictionary:");
                int sizedata = ind;
                foreach (KeyValuePair<string, string> ele1 in infoSendEmail)
                {
                    Console.WriteLine("key:{0},value:{1} ", ele1.Key, ele1.Value);
                }
                //result: chi lay duoc thoi gian, nhiet do, trang thai - 
                //khong lay duoc ten, lop, email. vi khong lay duoc email nen khong co dia chi de gui
                //tip: join hai bang csdl.

                for (ind = 0; ind < sizedata; ind++)
                {
                    string from = acc;
                    //Send to
                    //khong lay ra duoc dia chi email
                    string to = Convert.ToString(infoSendEmail["Mail" + ind]);
                    //Sub: tieu de
                    string sub = "Temperature Tracking Notification";
                    string ss = "<div style=\"color:red;\">ATTENTION</div>";
                   
                    //chỉ lấy chuỗi con Attention thôi
                    if (infoSendEmail["Status" + ind].ToString() == ss)
                    {
                        
                        int startIndex = 24;
                        int length = 9;
                        String substring = infoSendEmail["Status" + ind].Substring(startIndex, length);
                        infoSendEmail["Status" + ind] = substring;
                        infoSendEmail["Remark" + ind] += " - Quickly declare your health status and limit contact with everyone around you";
                    }
                    
                    //Content Email
                    noiDungGuiEmail = "Name: " + infoSendEmail["Name" + ind] + "\n";//+ lay ten hoc sinh tu data 
                    //noiDungGuiEmail += "Class: " + infoSendEmail["Class" + ind] + "\n";//+ lay ten lop
                    noiDungGuiEmail += "Temperature: " + infoSendEmail["Temperature" + ind] + "°C"+"\n";//+ lay nhiet do
                    noiDungGuiEmail += "Time: " + infoSendEmail["Time" + ind] + "\n";//+lay thoi gian tai luc nap du lieu
                    noiDungGuiEmail += "Status: " + infoSendEmail["Status" + ind] + "\n";//+lay trang thai
                    noiDungGuiEmail += "Date: " + infoSendEmail["Date" + ind] + "\n";// +lay ngay
                    noiDungGuiEmail += "Remark: " + infoSendEmail["Remark" + ind] + "\n";// +lay ngay


                    //gửi mail theo ngày hiện tại của hệ thống
                    if (infoSendEmail["Date" + ind] == dayTemp)
                    {
                        if(infoSendEmail["Status" + ind] == "ATTENTION")
                        {
                            sub = "Temperature Tracking Warning";
                        }
                        //send mail action
                        if (sendEmail(from, to, sub, noiDungGuiEmail, acc, pw))
                        {
                            Console.WriteLine("Gửi thành công cho: " + to);
                        }
                        else
                        {
                            Console.WriteLine("Gửi không thành công cho: " + to);
                        }
                    }
                }
                //check
            }
            else
            {
                Console.WriteLine($"Request failed with status code {response.StatusCode}");
            }
        }

        //tool send mail
        static bool sendEmail(string from, string to, string sub, string message, string ac, string maxacthuc)
        {
            try
            {
                SmtpClient clinet = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mess = new MailMessage(from, to, sub, message);
                clinet.EnableSsl = true;
                clinet.UseDefaultCredentials = false;
                clinet.DeliveryMethod = SmtpDeliveryMethod.Network;
                clinet.Credentials = new NetworkCredential(ac, maxacthuc);
                clinet.Send(mess);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
