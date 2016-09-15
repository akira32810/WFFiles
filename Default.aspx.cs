using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.IO.Compression;
using System.Threading.Tasks;


namespace WFFiles
{
    public partial class Default : System.Web.UI.Page
    {
        

        BusinessVariables QueryID = new BusinessVariables();

        private string connstring = ConfigurationManager.ConnectionStrings["Navigator2013ConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
    
            //manipulate string to not use "?" in query parameter and just use /default.aspx/ID instead
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            
            string resultID = url.Substring(url.LastIndexOf('/')+1);


            if (!String.IsNullOrEmpty(resultID) && (!resultID.Contains("fileid")))
            {
                Response.Redirect("?fileid=" + resultID);
                resultID = string.Empty;
            }
       

            if (!string.IsNullOrEmpty(getQueryID()))
            {
                 getFile();
            }

            else {
                Response.Write("Please append ID after the url to download a file");
            }

       
        }

        protected string getQueryID()
        {
            
            QueryID.QueryStringID = Request.QueryString["fileid"];
            return QueryID.QueryStringID;
        }

       
       


        //bind gridview

      //private void BindGrid()
      //{
      //    //tring constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
      //    using (SqlConnection con = new SqlConnection(connstring))
      //    {
      //        using (SqlCommand cmd = new SqlCommand())
      //        {
      //            cmd.CommandText = "select top 20 * from [Comptroller$File Attachment]  order by [Date Created] desc";
      //            cmd.Connection = con;
      //            con.Open();
      //            GridView1.DataSource = cmd.ExecuteReader();
      //            GridView1.DataBind();
                  
      //            con.Close();
      //        }
      //    }
      //}


        //download from gridview

        //download file by redirect the link to fileid
     // protected void DownloadFile(object sender, EventArgs e)
     // {
     //     DataTable result = new DataTable();
     //     string id = (sender as LinkButton).CommandArgument;
     ////     byte[] bytes;
     //   //  string fileName, contentType;
     //     // string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
     //     using (SqlConnection con = new SqlConnection(connstring))
     //     {
     //         using (SqlCommand cmd = new SqlCommand())
     //         {
     //             cmd.CommandText = "select No_ from [Comptroller$File Attachment] where No_=@Id";
     //             cmd.Parameters.AddWithValue("@Id", id.ToString());
     //             cmd.Connection = con;
     //             con.Open();

     //             using (SqlDataAdapter da = new SqlDataAdapter(cmd))
     //             {
     //                 da.Fill(result);
     //                 da.Dispose();

     //                 string ID = result.Rows[0]["No_"].ToString();

     //                 if (result.Rows.Count > 0)
     //                 {
     //                     Response.Redirect("?fileid=" + ID);
     //                 }
     //             }

     //             con.Close();
     //         }
     //     }

     // }

        //get bytes and remove first 4 bytes from bytes array
      protected byte[] getBytesfromFile(string queryID)
      {

          byte[] MyFilebytes = null;
          string filename = string.Empty;

            try
            {


                        using (DataClassesFilesDataContext dcontext = new DataClassesFilesDataContext())
                        {

                            var myfile = (from file in dcontext.Comptroller_File_Attachments
                                          where file.No_ == queryID || file.Primary_Key_Value_1 == queryID
                                          || file.Primary_Key_Value_2 == queryID || file.Primary_Key_Value_3 == queryID
                                          select file).First();

                            if (myfile.Table_ID.ToString().Length > 0 && myfile.Attachment != null)
                            {

                                MyFilebytes = myfile.Attachment.ToArray().Skip(4).ToArray();
                                QueryID.filename = myfile.FileName.ToString();
                            }


                            else
                                Response.Write("No File to download");
                        }

              
  
            }
          catch 
         {
             Response.Write("No File to download. <br/>");
         }

            return MyFilebytes;
            

      }

        //after getting the remaining bytes (after removing 4 first byte) deflate the byte and then store it in a memory steam and get the result back.
      protected void getFile()
      {
          try
          {
              string Filename = string.Empty;
              byte[] myfile = getBytesfromFile(getQueryID());

              byte[] result;

              using (Stream input = new DeflateStream(new MemoryStream(myfile),
                                            CompressionMode.Decompress))
              {
                  using (MemoryStream output = new MemoryStream())
                  {
                      input.CopyTo(output);
                      result = output.ToArray();
                  }
              }

              Filename = QueryID.filename;


              Response.Clear();
              Response.ContentType = "application/octet-stream";
              Response.AddHeader("Content-Disposition", "attachment; filename=" + Filename);
              Response.BinaryWrite(result);
              Response.End();
          }

          catch 
          {
              Response.Write("Cannot download file");
          }

      }

  
        
    }
}