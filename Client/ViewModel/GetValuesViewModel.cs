using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MyICommand LoadModel { get; set; }
        private GetValuesView getValuesView;
        public ObservableCollection<PopertyView> PropertiesView { get; set; }

        public GetValuesViewModel(GetValuesView getValuesView)
        {
            this.getValuesView = getValuesView;
            GetValues = new MyICommand(GetResult);
            LoadModel = new MyICommand(Load);
            PropertiesView = new ObservableCollection<PopertyView>();
        }
        private long g;

        

        private string gid;

        public string Gid
        {
            get { return gid; }
            set
            {
                if (gid != value)
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

        private void Load()
        {
            PropertiesView.Clear();

            if (string.IsNullOrEmpty(Gid))
            {
                MessageBox.Show("You first have to enter GID", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (Gid.StartsWith("0x", StringComparison.Ordinal))
                {
                    var NewGid = Gid.Remove(0, 2);

                    g = Convert.ToInt64(Int64.Parse(NewGid, System.Globalization.NumberStyles.HexNumber));
                }
                else
                {
                    g = Convert.ToInt64(Gid);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You entered invalid GID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                short type = ModelCodeHelper.ExtractTypeFromGlobalId(g);
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds((DMSType)type);
                foreach (var v in properties)
                {
                    PropertiesView.Add(new PopertyView(v.ToString(), true)); //popuni sa svim propertijima i da su svi cekirani
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void GetResult()
        {
            var path = Directory.GetCurrentDirectory();
            path = Path.GetFullPath(Path.Combine(path, @"..\..\..\Results\"));
            
            //napravi XML fajl
            List<ModelCode> properties = new List<ModelCode>();

            foreach(var item in PropertiesView)
            {
                if (item.IsChecked)
                {
                    properties.Add(modelResourcesDesc.GetModelCodeFromModelCodeName(item.Name));
                }
            }

            try
            {
                //provera da li je validan gid
                short type = ModelCodeHelper.ExtractTypeFromGlobalId(g);
                
                var rd = GdaQueryProxy.GetValues(g, properties);
                if (rd != null)
                {
                    var xmlWriter = new XmlTextWriter(path + "GetValues_Results.xml", Encoding.Unicode);
                    xmlWriter.Formatting = Formatting.Indented;
                    rd.ExportToXml(xmlWriter);
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    string.IsNullOrEmpty(Gid)
                        ? "Please enter valid GID first"
                        : "Entered GID does't exists", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                Result = "File not found";
                return;
            }
            
        }

    }
}
