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
     public class GetExtendedValuesViewModel : BinableBase
     {
          private GetExtendedValuesView getExtendedValuesView;
          public MyICommand GetExtendedValues { get; set; }

          private string modelCode;
          private string result;

          public GetExtendedValuesViewModel(GetExtendedValuesView getExtendedValuesView)
          {
               this.getExtendedValuesView = getExtendedValuesView;
               GetExtendedValues = new MyICommand(GetResult);
          }

          #region Properties
          public string ModelCode
          {
               get { return modelCode; }
               set
               {
                    if(modelCode != value)
                    {
                         modelCode = value;
                         OnPropertyChanged("ModelCode");
                    }
               }
          }

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
          #endregion

          #region 4 connection
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
          #endregion

          #region GetResult
          private void GetResult()
          {
               ModelCode mc = 0;
               try
               {
                    if (!ModelCodeHelper.GetModelCodeFromString(ModelCode, out mc))
                    {
                         if (ModelCode.StartsWith("0x", StringComparison.Ordinal))
                         {
                              mc = (ModelCode)long.Parse(ModelCode.Substring(2), System.Globalization.NumberStyles.HexNumber);
                         }
                         else
                         {
                              mc = (ModelCode)long.Parse(ModelCode);
                         }
                    }
               }
               catch (Exception)
               {
                    MessageBox.Show("Unesite validnu vrednost model code-a", "Upozorenje");
                    getExtendedValuesView.tbModelCode.Focus();
                    return;
               }


               int iteratorId = 0;
               List<long> ids = new List<long>();

               int numberOfResources = 2;
               int resourcesLeft = 0;

               List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(mc);

               iteratorId = GdaQueryProxy.GetExtentValues(mc, properties);
               resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

               var path = Directory.GetCurrentDirectory();
               path = Path.GetFullPath(Path.Combine(path, @"..\..\..\Results\"));

               var xmlWriter = new XmlTextWriter(path + "GetExtentValues_Results.xml", Encoding.Unicode);
               xmlWriter.Formatting = Formatting.Indented;

               while (resourcesLeft > 0)
               {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                         ids.Add(rds[i].Id);
                         rds[i].ExportToXml(xmlWriter);
                         xmlWriter.Flush();
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
               }

               GdaQueryProxy.IteratorClose(iteratorId);
               xmlWriter.Close();

               try
               {
                    using (var reader = new StreamReader(path + "GetExtentValues_Results.xml"))
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
          #endregion
     }
}
