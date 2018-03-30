using Client.ViewModel;
using FTN.Common;
using FTN.Services.NetworkModelService.TestClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelventDMS.Services.NetworkModelService.TestClient.Tests;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for GetRelatedValuesView.xaml
    /// </summary>
    public partial class GetRelatedValuesView : UserControl
    {
        private ObservableCollection<string> item2 = new ObservableCollection<string>();
        private ObservableCollection<PopertyView> item3 = new ObservableCollection<PopertyView>();
        private ObservableCollection<string> item4 = new ObservableCollection<string>();

        public object TestGda { get; private set; }

        public GetRelatedValuesView()
        {
            InitializeComponent();
            DataContext = this;
            Combo.ItemsSource = item2;
            ComboType.ItemsSource = item4;
            dataGrid4.ItemsSource = item3;
        }

        #region bGetRef_Click
        private void bGetRef_Click(object sender, RoutedEventArgs e)
        {
            long id = InputGlobalId(tbGid.Text);

            if (id != -1)
            {
                List<ModelCode> lista = new List<ModelCode>();

                ModelResourcesDesc model = new ModelResourcesDesc();
                DMSType dms;
                string sub = tbGid.Text.Substring(6, 4);
                ModelCodeHelper.GetDMSTypeFromString(sub, out dms);
                lista = model.GetAllPropertyIds(dms);

                item2.Clear();

                foreach (ModelCode mc in lista)
                {
                    int intValue = (int)mc;
                    string hex = intValue.ToString("X");
                    if (hex.ToString().EndsWith("09"))
                    {
                        item2.Add(mc.ToString()); //dodavanje u listu referenci
                    }
                    else if (hex.ToString().EndsWith("19"))
                    {
                        item2.Add(mc.ToString());
                    }
                }
            }
        }
        #endregion

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ModelCode mc;
                Enum.TryParse(Combo.SelectedItem.ToString(), true, out mc);

                item4.Clear();

                //sve single reference
                if (mc == ModelCode.PSR_OUTAGESCHEDULE)
                {
                    item4.Add(ModelCode.PSR.ToString());
                }
                else if (mc == ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE)
                {
                    item4.Add(ModelCode.REGULARTIMEPOINT.ToString());
                }
                else if (mc == ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE)
                {
                    item4.Add(ModelCode.IRREGULARTIMEPOINT.ToString());
                }
                else if (mc == ModelCode.CURVEDATA_CURVE)
                {
                    item4.Add(ModelCode.CURVEDATA.ToString());
                }

                //sve liste referenci
                else if(mc == ModelCode.CURVE_CURVEDATAS)
                {
                    item4.Add(ModelCode.CURVE.ToString());
                }
                else if(mc == ModelCode.IRREGULARINTERVALSCHEDULE_TIMEPOINTS)
                {
                    item4.Add(ModelCode.IRREGULARINTERVALSCHEDULE.ToString());
                }
                else if(mc == ModelCode.REGULARINTERVALSCHEDULE_TIMEPOINTS)
                {
                    item4.Add(ModelCode.REGULARINTERVALSCHEDULE.ToString());
                }
                else if(mc == ModelCode.OUTAGESCHEDULE_PSRS)
                {
                    item4.Add(ModelCode.OUTAGESCHEDULE.ToString());
                }
                
            }
            catch (Exception) { }
        }
        
        #region InputGlobalId
        private long InputGlobalId(string text)
        {
            CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering globalId started.");

            try
            {
                if (text.StartsWith("0x", StringComparison.Ordinal))
                {
                    text = text.Remove(0, 2);
                    CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering globalId successfully ended.");

                    return Convert.ToInt64(Int64.Parse(text, System.Globalization.NumberStyles.HexNumber));
                }
                else
                {
                    MessageBox.Show(string.Format("Format of global id{0} is not valid", text), "Error");
                    CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering globalId successfully ended.");
                    //return Convert.ToInt64(strId);
                    return -1;
                }
            }
            catch (FormatException ex)
            {
                string message = "Entering entity id failed. Please use hex (0x) or decimal format.";
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                Console.WriteLine(message);
                throw ex;
            }
        }
        #endregion

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                item3.Clear();

                Enum.TryParse(ComboType.SelectedItem.ToString(), true, out ModelCode mc);

                List<ModelCode> klase = new List<ModelCode>
                {
                    mc
                };
                
                foreach (var v in klase)
                {
                    if (!item3.Any(x => x.Name.Equals(v.ToString())))
                        item3.Add(new PopertyView(v.ToString(), true));
                }
            }
            catch (Exception) { }
        }

        private void bGetRelValues_Click(object sender, RoutedEventArgs e)
        {
            long gid = InputGlobalId(tbGid.Text);
            if (gid != -1)
            {
                List<ModelCode> props = new List<ModelCode>();
                foreach (var item in item3)
                {
                    if (item.IsChecked)
                    {
                        ModelCode MyStatus = (ModelCode)Enum.Parse(typeof(ModelCode), item.Name, true);
                        props.Add(MyStatus);
                    }
                }

                Association asc = new Association();
                ModelCode mc;
                Enum.TryParse(Combo.SelectedItem.ToString(), true, out mc);
                asc.PropertyId = mc;
                Enum.TryParse(ComboType.SelectedItem.ToString(), true, out mc);
                asc.Type = mc;
                

                TestGda gda = new TestGda();
                if (gda.GetRelatedValues(gid, props, asc) != null)
                {
                    var path = Directory.GetCurrentDirectory();
                    path = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, @"..\..\..\Results\GetRelatedValues_Results.xml"));
                    TextBoxRel.Clear();
                    if (File.Exists(path))
                        TextBoxRel.Text = File.ReadAllText(path);
                    else TextBoxRel.Text = "Fajl ne postoji";
                }
                else
                {
                    MessageBox.Show("Error in GetRelatedValues");
                }
            }
        }
    }
}
