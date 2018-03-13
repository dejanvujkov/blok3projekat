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

          //kod njih je GetTypeProps
          private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
          {
               try
               {
                    ModelCode mc;
                    Enum.TryParse(Combo.SelectedItem.ToString(), true, out mc);

                    item4.Clear();

                    if (mc == ModelCode.PSR_OUTAGESCHEDULE)
                    {
                         item4.Add(ModelCode.PSR_OUTAGESCHEDULE.ToString());
                    }
                    else if (mc == ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE)
                    {
                         item4.Add(ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE.ToString());
                    }
                    else if (mc == ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE)
                    {
                         item4.Add(ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE.ToString());
                    }
                    else if (mc == ModelCode.CURVEDATA_CURVE)
                    {
                         item4.Add(ModelCode.CURVEDATA_CURVE.ToString());
                    }
                    item4.Add("0");
               }
               catch (Exception) { }
          }

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
                              item2.Add(mc.ToString());
                         }
                         else if (hex.ToString().EndsWith("19"))
                         {
                              item2.Add(mc.ToString());
                         }
                    }
               }
          }

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
          
          private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
          {
               try
               {
                    ModelCode mc;
                    ModelResourcesDesc model = new ModelResourcesDesc();
                    if (!ComboType.SelectedItem.ToString().Equals("0"))
                    {
                         Enum.TryParse(ComboType.SelectedItem.ToString(), true, out mc);

                         List<DMSType> klase = new List<DMSType>();

                         //proveri konkretne klase
                         if (mc == ModelCode.DISCONNECTOR)
                         {
                              klase.Add(DMSType.DISCONNECTOR);
                         }
                         else if(mc == ModelCode.PROTSWITCH)
                         {
                              klase.Add(DMSType.PROTSWITCH);
                         }
                         else if(mc == ModelCode.CURVE)
                         {
                              klase.Add(DMSType.CURVE);
                         }
                         else if(mc == ModelCode.REGULARTIMEPOINT)
                         {
                              klase.Add(DMSType.REGULARTIMEPOINT);
                         }
                         else if(mc == ModelCode.IRREGULARTIMEPOINT)
                         {
                              klase.Add(DMSType.IRREGULARTIMEPOINT);
                         }
                         else if(mc == ModelCode.OUTAGESCHEDULE)
                         {
                              klase.Add(DMSType.OUTAGESCHEDULE);
                         }
                         else if(mc == ModelCode.CURVEDATA)
                         {
                              klase.Add(DMSType.CURVEDATA);
                         }
                         else if(mc == ModelCode.REGULARINTERVALSCHEDULE)
                         {
                              klase.Add(DMSType.REGULARINTERVALSCHEDULE);
                         }

                         item3.Clear();

                         foreach (var dms in klase)
                         {
                              if (dms != DMSType.MASK_TYPE)
                              {
                                   List<ModelCode> list = new List<ModelCode>();
                                   list = model.GetAllPropertyIds(dms);

                                   foreach(var v in list)
                                   {
                                        if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                             item3.Add(new PopertyView(v.ToString(), true));
                                   }
                              }
                         }
                    }
                    else
                    {
                         item3.Clear();
                         List<ModelCode> lista = new List<ModelCode>();
                         lista = model.GetAllPropertyIds(ModelCode.IDOBJ);

                         foreach(var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.PSR);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.EQUIPMENT);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.CONDEQ);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.SWITCH);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.PROTSWITCH);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.DISCONNECTOR);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.CURVEDATA);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.CURVE);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.BINTERVALSCHEDULE);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.IRREGULARINTERVALSCHEDULE);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.OUTAGESCHEDULE);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.REGULARINTERVALSCHEDULE);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.REGULARTIMEPOINT);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();

                         lista = model.GetAllPropertyIds(ModelCode.IRREGULARTIMEPOINT);

                         foreach (var v in lista)
                         {
                              if (!item3.Any(x => x.Name.Equals(v.ToString())))
                                   item3.Add(new PopertyView(v.ToString(), true));
                         }
                         lista.Clear();
                    }
               }
               catch (Exception) { }
          }

          private void bGetRelValues_Click(object sender, RoutedEventArgs e)
          {
               long gid = InputGlobalId(tbGid.Text);
               if(gid != -1)
               {
                    List<ModelCode> props = new List<ModelCode>();
                    foreach(var item in item3)
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
                    ModelCode moc;
                    
                    if(asc.PropertyId == ModelCode.CURVEDATA_CURVE)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }
                    else if (asc.PropertyId == ModelCode.CURVE_CURVEDATAS)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }
                    else if (asc.PropertyId == ModelCode.IRREGULARINTERVALSCHEDULE_TIMEPOINTS)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }
                    else if (asc.PropertyId == ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }
                    else if (asc.PropertyId == ModelCode.OUTAGESCHEDULE_PSRS)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }
                    else if (asc.PropertyId == ModelCode.PSR_OUTAGESCHEDULE)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }
                    else if (asc.PropertyId == ModelCode.REGULARINTERVALSCHEDULE_TIMEPOINTS)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }
                    else if (asc.PropertyId == ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE)
                    {
                         if (ComboType.SelectedItem.ToString().Equals("0"))
                         {
                              asc.Type = 0;
                         }
                         else
                         {
                              Enum.TryParse(ComboType.SelectedItem.ToString(), true, out moc);
                              asc.Type = moc;
                         }
                    }

                    TestGda gda = new TestGda();
                    if(gda.GetRelatedValues(gid, props, asc) != null)
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
