using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;

namespace WFFiles
{
    /// <summary>
    /// Summary description for FileHandler
    /// </summary>
    public class FileHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
             if (!String.IsNullOrEmpty(context.Request.QueryString["fileID"]))
               {
                   int id = Int32.Parse(context.Request.QueryString["fileID"]);
    
                   Image image = GetImage(id.ToString());
    
                   context.Response.ContentType = "image/jpeg";
                   image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
               }
               else
               {
                   context.Response.ContentType = "text/html";
                   context.Response.Write("<p>Need a valid id</p>");
              }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private Image GetImage(string queryID)
        {
            MemoryStream memorySteam = new MemoryStream();

            string connString = ConfigurationManager.ConnectionStrings["Navigator2013ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("select * from [Comptroller$File Attachment] where No_=@ID", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = queryID;

                    conn.Open();

                    SqlDataReader dreader = cmd.ExecuteReader();

                    if (dreader.HasRows)
                    {
                        dreader.Read();
                        byte[] btfile = (byte[])dreader["Attachment"];
                        memorySteam = new MemoryStream(btfile, false);

                    }

                }

                conn.Close();


            }

            return Image.FromStream(memorySteam);

        }

    }
}