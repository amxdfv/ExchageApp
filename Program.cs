using Oracle.ManagedDataAccess.Client;
using System;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;

namespace ExchangeApp2
{
    public class Program
    {
       
        
        static void Main(string[] args)
        {
            Initial In = new Initial();

            String[] ConPath = In.GetPresets();

            

             String lDate = "test_log" + DateTime.Now.ToString("d") + ".txt";
               File.AppendAllText(lDate, DateTime.Now.ToString("F") + " запускаем обмен, работяги.... \n");
                  while (true)
                  {
                   lDate = "test_log" + DateTime.Now.ToString("d") + ".txt";
                   File.AppendAllText(lDate, DateTime.Now.ToString("F") + "\n");
                   try
                      {
                          String TNSTrans = ConPath[1];
                          OracleConnection ConnectionToTR;
                          ConnectionToTR = new OracleConnection(TNSTrans);
                          ConnectionToTR.Open();
                          String TNSclearing = ConPath[0];
                          OracleConnection ConnectionToClearing;
                          ConnectionToClearing = new OracleConnection(TNSclearing);
                          ConnectionToClearing.Open();


                          ExchangeAppClass EA = new ExchangeAppClass();
                          UniExchange UE = new UniExchange();


                       UE.UniExc(ConnectionToClearing, ConnectionToTR, "EXT_SYSTEM", lDate);                       // топливный клиринг
                       UE.UniExc(ConnectionToClearing, ConnectionToTR, "LAST_BALANCE", lDate);
                       UE.UniExc(ConnectionToClearing, ConnectionToTR, "AZS_CLR_TRANS_STATE", lDate);
                       EA.UniversalExhacge3(ConnectionToTR, ConnectionToClearing, lDate);
                       UE.UniExc(ConnectionToClearing, ConnectionToTR, "TRANSACTIONS_CLR_LIST", lDate);
                       UE.UniExc(ConnectionToClearing, ConnectionToTR, "TRANS_CLR", lDate);
                       UE.UniExc(ConnectionToTR, ConnectionToClearing, "AZS_CLR_TRANS", lDate);
                       UE.UniExc(ConnectionToTR, ConnectionToClearing, "AZS_CLR_TRANS_LINK", lDate);


                       UE.UniExc(ConnectionToTR, ConnectionToClearing, "BNK_CLR_TRANS", lDate);           // банковский клиринг
                       UE.UniExc(ConnectionToTR, ConnectionToClearing, "BNK_CLR_PAYMENTS", lDate);
                       UE.UniExc(ConnectionToClearing, ConnectionToTR, "UC_CARDS", lDate);
                       UE.UniExc(ConnectionToClearing, ConnectionToTR, "BNK_CLR_TRANS_STATE", lDate);


                       ConnectionToTR.Close();
                          ConnectionToClearing.Close();
                       File.AppendAllText(lDate, DateTime.Now.ToString("F") + "\n");
                       File.AppendAllText(lDate, DateTime.Now.ToString("F") + " Обмен успешно \n");
                          Thread.Sleep(1000 * 30);
                      }catch(Exception e)
                      {
                          Console.WriteLine(e.Message);
                      }

               }

        }
    }
}
