using Client.View;
using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
     public class MainWindowViewModel : BinableBase
     {
          public MyICommand<string> NavCommand { get; private set; }

          //views
          private static GetValuesView GetValuesView = new GetValuesView();
          private static GetRelatedValuesView GetRelatedValuesView = new GetRelatedValuesView();
          private static GetExtendedValuesView GetExtendedValuesView = new GetExtendedValuesView();

          //view model
          private GetValuesViewModel GetValuesViewModel = new GetValuesViewModel(GetValuesView);
          private GetRelatedValuesViewModel GetGetRelatedValuesViewModel = new GetRelatedValuesViewModel(GetRelatedValuesView);
          private GetExtendedValuesViewModel GetExtendedValuesViewModel = new GetExtendedValuesViewModel(GetExtendedValuesView);

          private BinableBase currentViewModel;

          public MainWindowViewModel()
          {
               NavCommand = new MyICommand<string>(SetViewModel);
               CurrentViewModel = GetValuesViewModel;
          }

          private void SetViewModel(string parameter)
          {
               switch (parameter)
               {
                    case "values":
                         CurrentViewModel = GetValuesViewModel;
                         break;
                    case "related":
                         CurrentViewModel = GetGetRelatedValuesViewModel;
                         break;
                    case "extended":
                         CurrentViewModel = GetExtendedValuesViewModel;
                         break;
               }
          }

          public BinableBase CurrentViewModel
          {
               get { return currentViewModel; }
               set
               {
                    SetProperty(ref currentViewModel, value);
               }
          }
     }
}
