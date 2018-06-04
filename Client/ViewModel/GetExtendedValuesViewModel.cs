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
    public class GetExtendedValuesViewModel : BinableBase
    {
        private GetExtendedValuesView getExtendedValuesView;
        public MyICommand GetExtendedValues { get; set; }

        public MyICommand LoadObject { get; set; }

        public ObservableCollection<PopertyView> PropertiesView { get; set; }

        private string modelCode;
        private string result;

        public GetExtendedValuesViewModel(GetExtendedValuesView getExtendedValuesView)
        {
            this.getExtendedValuesView = getExtendedValuesView;
            GetExtendedValues = new MyICommand(GetResult);
            LoadObject = new MyICommand(Load);
            PropertiesView = new ObservableCollection<PopertyView>();
        }

        private ModelCode mc;

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

        public void Load()
        {
            PropertiesView.Clear();
            //model code bude npr system.controls.combobox.nesto: protswitch, pa zato moramo da parsiramo
            try
            {
                var parsedString = ModelCode.Split(':');
                ModelCode = parsedString[1];
            }
            catch (Exception)
            {
                MessageBox.Show("Choose valid Model Code from list first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            mc = 0;
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
                MessageBox.Show("Chosen Model Code is not valid", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                getExtendedValuesView.tbModelCode.Focus();
                return;
            }

            List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(mc);

            foreach(var item in properties)
            {
                PropertiesView.Add(new PopertyView(item.ToString(), true));
            }
        }

        #region Properties
        public string ModelCode
        {
            get { return modelCode; }
            set
            {
                if (modelCode != value)
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

        #region GetResult
        private void GetResult()
        {
            if (string.IsNullOrEmpty(ModelCode))
            {
                MessageBox.Show("You first have to load element!", "Warrning", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                int iteratorId = 0;
                List<long> ids = new List<long>();

                int numberOfResources = 2;
                int resourcesLeft = 0;

                if (mc == 0)
                {
                    MessageBox.Show("You didn't load element", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(mc);

                foreach (var item in PropertiesView)
                {
                    if (!item.IsChecked)
                    {
                        properties.Remove(modelResourcesDesc.GetModelCodeFromModelCodeName(item.Name));
                    }
                }

                iteratorId = GdaQueryProxy.GetExtentValues(mc, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                var path = Directory.GetCurrentDirectory();
                path = Path.GetFullPath(Path.Combine(path, @"..\..\..\Results\"));

                var xmlWriter = new XmlTextWriter(path + "GetExtentValues_Results.xml", Encoding.Unicode);
                xmlWriter.Formatting = Formatting.Indented;

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    foreach (var t in rds)
                    {
                        ids.Add(t.Id);
                        t.ExportToXml(xmlWriter);
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
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("IO Exception");
                    Result = "File was not found";
                    return;
                }
            }
        }
        #endregion
    }
}
