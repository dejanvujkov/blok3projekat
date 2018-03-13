using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Client.View;
using FTN.Common;
using FTN.ServiceContracts;

namespace Client.ViewModel
{
     public class GetValuesViewModel : BinableBase
     {
          public MyICommand GetValues { get; set; }
          private GetValuesView getValuesView;

          public GetValuesViewModel(GetValuesView getValuesView)
          {
               this.getValuesView = getValuesView;
               GetValues = new MyICommand(GetResult);
               
          }
          
          private string gid;

          public string Gid
          {
               get { return gid; }
               set
               {
                    if(gid != value)
                    {
                         gid = value;
                         OnPropertyChanged("Gid");
                    }
               }
          }

          private string result;

          public string Result
          {
               get { return result; }
               set
               {
                    if (result != value)
                    {
                         result = value;
                         OnPropertyChanged("Result");
                    }
               }
          }


          private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

          private NetworkModelGDAProxy gdaQueryProxy = null;
          private NetworkModelGDAProxy GdaQueryProxy
          {
               get
               {
                    if (gdaQueryProxy != null)
                    {
                         gdaQueryProxy.Abort();
                         gdaQueryProxy = null;
                    }

                    gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                    gdaQueryProxy.Open();

                    return gdaQueryProxy;
               }
          }

          private void GetResult()
          {
               try
               {
                    long g;
                    var path = Directory.GetCurrentDirectory();
                    path = Path.GetFullPath(Path.Combine(path, @"..\..\..\Results\"));

                    if (Gid.StartsWith("0x", StringComparison.Ordinal))
                    {
                         Gid = Gid.Remove(0, 2);

                         g = Convert.ToInt64(Int64.Parse(Gid, System.Globalization.NumberStyles.HexNumber));
                    }
                    else
                    {
                         g = Convert.ToInt64(Gid);
                    }
                    //napravi XML fajl
                    short type = ModelCodeHelper.ExtractTypeFromGlobalId(g);
                    List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds((DMSType)type);

                    var rd = GdaQueryProxy.GetValues(g, properties);
                    if (rd != null)
                    {
                         var xmlWriter = new XmlTextWriter(path + "GetValues_Results.xml", Encoding.Unicode);
                         xmlWriter.Formatting = Formatting.Indented;
                         rd.ExportToXml(xmlWriter);
                         xmlWriter.Flush();
                         xmlWriter.Close();
                    }

                    try
                    {
                         using (var reader = new StreamReader(path + "GetValues_Results.xml"))
                         {
                              Result = reader.ReadToEnd();
                              return;
                         }
                    }
                    catch (IOException)
                    {
                         MessageBox.Show("IO Exception");
                         Result = "Fajl nije pronadjen";
                         return;
                    }
               }
               catch (FormatException)
               {
                    MessageBox.Show("Unesite validan gid", "Upozorenje");
                    getValuesView.tbGid.Focus();
               }
          }

     }
}
