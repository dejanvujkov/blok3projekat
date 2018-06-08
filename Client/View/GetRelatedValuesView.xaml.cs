using FTN.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TelventDMS.Services.NetworkModelService.TestClient.Tests;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for GetRelatedValuesView.xaml
    /// </summary>
    public partial class GetRelatedValuesView
    {
        private readonly ObservableCollection<string> _item2 = new ObservableCollection<string>();
        private readonly ObservableCollection<PopertyView> _item3 = new ObservableCollection<PopertyView>();
        private readonly ObservableCollection<string> _item4 = new ObservableCollection<string>();

        public GetRelatedValuesView()
        {
            InitializeComponent();
            DataContext = this;
            Combo.ItemsSource = _item2;
            ComboType.ItemsSource = _item4;
            dataGrid4.ItemsSource = _item3;
        }

        #region bGetRef_Click
        private void bGetRef_Click(object sender, RoutedEventArgs e)
        {
            var id = InputGlobalId(tbGid.Text);

            if (id == -1) return;
            var model = new ModelResourcesDesc();
            var sub = tbGid.Text.Substring(6, 4);
            ModelCodeHelper.GetDMSTypeFromString(sub, out var dms);
            var lista = model.GetAllPropertyIds(dms);

            _item2.Clear();

            foreach (var mc in lista)
            {
                var intValue = (int)mc;
                var hex = intValue.ToString("X");
                if (hex.EndsWith("09"))
                {
                    _item2.Add(mc.ToString()); //dodavanje u listu referenci
                }
                else if (hex.EndsWith("19"))
                {
                    _item2.Add(mc.ToString());
                }
            }
        }
        #endregion

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Enum.TryParse(Combo.SelectedItem.ToString(), true, out ModelCode mc);

                _item4.Clear();

                //sve single reference
                if (mc == ModelCode.PSR_OUTAGESCHEDULE)
                {
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }
                else if (mc == ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE)
                {
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }
                else if (mc == ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE)
                {
                    _item4.Add(ModelCode.OUTAGESCHEDULE.ToString());
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }
                else if (mc == ModelCode.CURVEDATA_CURVE)
                {
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }

                //sve liste referenci
                else if(mc == ModelCode.CURVE_CURVEDATAS)
                {
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }
                else if(mc == ModelCode.IRREGULARINTERVALSCHEDULE_TIMEPOINTS)
                {
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }
                else if(mc == ModelCode.REGULARINTERVALSCHEDULE_TIMEPOINTS)
                {
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }
                else if(mc == ModelCode.OUTAGESCHEDULE_PSRS)
                {
                    _item4.Add(ModelCode.DISCONNECTOR.ToString());
                    _item4.Add(ModelCode.PROTSWITCH.ToString());
                    _item4.Add($"0 - IDENTIFY OBJECT");
                }
                
            }
            catch (Exception)
            {
                // ignored
            }
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

                    return Convert.ToInt64(long.Parse(text, System.Globalization.NumberStyles.HexNumber));
                }
                else
                {
                    MessageBox.Show($"Format of global id{text} is not valid", "Error");
                    CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering globalId successfully ended.");
                    //return Convert.ToInt64(strId);
                    return -1;
                }
            }
            catch (FormatException)
            {
                const string message = "Entering entity id failed. Please use hex (0x) or decimal format.";
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                Console.WriteLine(message);
                throw;
            }
        }
        #endregion

        #region ComboType_selectionChanged
        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _item3.Clear();
                var modelResourcesDesc = new ModelResourcesDesc();
                if (ComboType.SelectedItem.ToString().Equals("0 - IDENTIFY OBJECT"))
                {
                    var props = modelResourcesDesc.GetAllPropertyIds(ModelCode.IDOBJ);
                    foreach (var v in props)
                    {
                        _item3.Add(new PopertyView(v.ToString(), true));
                    }
                }
                else
                {
                    Enum.TryParse(ComboType.SelectedItem.ToString(), true, out ModelCode mc);
                    
                    var props = modelResourcesDesc.GetAllPropertyIds(mc);


                    //popunjavanje polja za cekiranje
                    foreach (var v in props)
                    {
                        _item3.Add(new PopertyView(v.ToString(), true));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        } 
        #endregion

        private void bGetRelValues_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbGid.Text))
            {
                MessageBox.Show("Enter valid GID first", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Combo.SelectedItem == null)
            {
                MessageBox.Show("Please choose reference for all fields", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (ComboType.SelectedItem == null)
            {
                MessageBox.Show("Reference type must be selected first. If there is nothing to select from, try loading references fist", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            
            //gid
            var gid = InputGlobalId(tbGid.Text);
            if (gid == -1) return;

            //za popunjavanje propertija
            var props = new List<ModelCode>();
            foreach (var item in _item3)
            {
                if (!item.IsChecked) continue;
                var myStatus = (ModelCode)Enum.Parse(typeof(ModelCode), item.Name, true);
                props.Add(myStatus);
            }

            var asc = new Association();
            try
            {
                //asocijacija
                
                Enum.TryParse(Combo.SelectedItem.ToString(), true, out ModelCode mc);
                asc.PropertyId = mc;
                Enum.TryParse(ComboType.SelectedItem.ToString(), true, out mc);
                asc.Type = mc;
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect field selection for reference or reference type", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
            }
                

            var gda = new TestGda();
            if (gda.GetRelatedValues(gid, props, asc) != null)
            {
                var path = Directory.GetCurrentDirectory();
                path = Path.GetFullPath(Path.Combine(path, @"..\..\..\Results\GetRelatedValues_Results.xml"));
                TextBoxRel.Text = string.Empty;
                TextBoxRel.Text = File.Exists(path) ? File.ReadAllText(path) : "File does not exists";
            }
            else
            {
                MessageBox.Show("Unkown error in GetRelatedValues");
            }
        }
    }
}
