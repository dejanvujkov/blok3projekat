using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.View;

namespace Client.ViewModel
{
     public class GetRelatedValuesViewModel : BinableBase
     {
          private GetRelatedValuesView getRelatedValuesView;

          public GetRelatedValuesViewModel(GetRelatedValuesView getRelatedValuesView)
          {
               this.getRelatedValuesView = getRelatedValuesView;
          }
     }
}
