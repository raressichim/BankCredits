using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    //I've made the class public in order to be possible for me to make all my references possible
    public class Client : ICloneable, IComparable<Client>,ICreaditable
    {
        private int id ;
        private string name;
        private float income; 
        private float debt; 
        private List<Credit> credits = new List<Credit>(); //list of credits that belong to the client


        public Client()
        {
            this.id = -1;
            this.name = "";
            this.income = 0;
            this.debt = 0;
        }

        public Client(int id, string name, float income,List<Credit>credits)
        {
            this.id = id;
            this.name = name;
            this.income = income;
            this.credits = credits;
            credits = new List<Credit>();

        }

        public int ID //ID should not have a setter
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public float Income
        {
            get { return income; }  
            set { income = value; }
        }

        public float Debt
        {
            get { return debt; }
            set { debt = value; }
        }

        public List<Credit> Credits { get => credits; }

        public bool IsCreditworthy()
        {
            if(debt/income<0.05f && income>0)
            return true;

            return false;
        }

        public Credit this[int index]
        {
            get
            {
                return credits[index];
            }
            set
            {
                debt -= credits[index].Amount; //removing the old amount of credit from debt
                credits[index] = value;
                debt += value.Amount;  //adding the new amount
            } 
        }

        public void addCredit(Credit c)
        {
            credits.Add(c);
            debt += c.Amount;
        }
        //considering that a month has passed, the debt has to be lowered
        public static Client operator --(Client c) 
        {
            float sum=0f;
            foreach(Credit i in c.credits)
            {
                sum += i.monthlyPayment();
            }

            c.debt -= sum;
            return c;
        }

        public static float operator -(Client c1,Client c2)
        {
            return Math.Abs(c1.debt - c2.debt);
        }

        public Object Clone()
        {
            Client copy = new Client(id + 1, name, income, credits);
            return copy;
        }

        public int CompareTo(Client c)
        {
            if (c == null)
            {
                return 1;
            }
            else
            {
                return id.CompareTo(c.id);
            }
        }

        public override string ToString()
        {
            return $"{id},{name},{income},{debt}";
        }

    }
}
