using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    public enum CreditType
    {
        Personal,Mortgage,Bussines,Other
    }
    //I've made the class public in order to be possible for me to make all my references possible
    public class Credit :IComparable<Credit>,ICloneable
    {
        private int creditID;
        private int idClient;
        private CreditType type;
        private float amount;
        private int period; //the number of months over which the credit will be repaid
        private float interestRate; //it will be introduced as a percentage ex:5 (%) and it's monthly
        public Credit()
        {
            creditID = -1;
            idClient = -1 ;
            creditID = -1;
            type = CreditType.Other;
            amount = 0;
            period = 0;
            interestRate = 5;
        }

        public Credit(int creditID,int idClient,CreditType type,float amount,int period)
        {
            this.creditID = creditID;   
            this.idClient = idClient;
            this.type= type;
            this.amount = amount;
            this.period = period;

            if (type == CreditType.Other)
            {
                interestRate = 5;
            }
            else if (type == CreditType.Personal)
            {
                interestRate = 10.82f;
            }
            else if(type== CreditType.Mortgage)
            {
                interestRate = 6.88f;
            }
            else if(type == CreditType.Bussines)
            {
                InterestRate = 6.25f;
            }
           
        }

        public int CreditID
        {
            get { return creditID; }
        } 
        public CreditType Type
        {
            get { return type; }
            set { type = value; }
        }

        public float Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        public float InterestRate
        {
            get { return interestRate; }
            set { interestRate = value; }
        }

        public int IdClient { get => idClient;  }

        public float monthlyPayment()
        {
            return amount / period + amount * (interestRate / 100);
        }



        public int CompareTo(Credit c)
        {
            if (c == null)
            {
                return 1;
            }
            return amount.CompareTo(c.amount);
        }

        public Object Clone()
        {
            Credit copy = new Credit(creditID,idClient,type,amount,period);
            return copy;
        }

        public override string ToString()
        {
            return $"{creditID},{idClient},{type},{amount},{period} monthss,{interestRate}";
        }

    }
}
