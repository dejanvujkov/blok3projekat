using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
     public class PopertyView
     {
          private string name;
          private bool isChecked;
          public PopertyView(string name, bool isChecked)
          {
               this.name = name;
               this.isChecked = isChecked;
          }

          public string Name
          {
               get { return name; }
               set { name = value; }
          }

          public bool IsChecked
          {
               get { return isChecked; }
               set { isChecked = value; }
          }
     }
}
