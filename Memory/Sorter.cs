using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    //UPPGIFT: Mata tillbaka en sorterad lista
    class Sorter
    {
        public static List<int> QuickSort(List<int> data, int left, int right)
        {
            //Välj det tal som avgör indelningen i "högre" och "lägre"
            int pivot = data[(left + right) / 2];
            //Välj det område som skall bearbetas
            int leftHold = left;
            int rightHold = right;

            //Så länge vi har ett område kvar
            while (leftHold < rightHold)
            {
                //Hitta ett tal på vänster sida som skall ligga i den "högre" delen
                while ((data[leftHold] < pivot) && (leftHold <= rightHold)) leftHold++;
                //Hitta ett tal på höger sida som skall ligga i den "lägre" delen
                while ((data[rightHold] > pivot) && (rightHold >= leftHold)) rightHold--;

                //Om vi nu har ett område kvar så skall talen på 
                //vänster kant och höger kant byta plats
                if (leftHold < rightHold)
                {
                    //Byta plats
                    int tmp = data[leftHold];
                    data[leftHold] = data[rightHold];
                    data[rightHold] = tmp;
                    //Minska området om vi flyttat två pivot-tal
                    if (data[leftHold] == pivot && data[rightHold] == pivot)
                        leftHold++;
                }
            }
            //Nu när området är bearbetat så skall "lägre" delen bearbetas
            //om sådan finns därefter detsamma med en eventuell "högre" del
            if (left < leftHold - 1) QuickSort(data, left, leftHold - 1);
            if (right > rightHold + 1) QuickSort(data, rightHold + 1, right);

            return data;
        }
    }
}
