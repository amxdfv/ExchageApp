using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Client;

namespace ExchangeApp2
{
    class UniExchange
    {
        public void UniExc(OracleConnection ConnectionToTR, OracleConnection ConnectionToClearing, String TableName,String lDate)
        {
            try
            {
                String Col = IdCol(ConnectionToClearing, TableName);
                OracleCommand CL = new OracleCommand("select " + Col + " from " + TableName, ConnectionToClearing);   // получаем содержание ведущей колонки
                OracleCommand CL1 = new OracleCommand("select count(*) from " + TableName, ConnectionToClearing);
                OracleCommand CL3 = new OracleCommand("select column_name from user_tab_columns where table_name = '" + TableName + "' order by column_id", ConnectionToClearing);     //   получаем имена колонок в таблице
                OracleCommand CL4 = new OracleCommand("select count(*) from user_tab_columns where table_name = '" + TableName + "'", ConnectionToClearing);     //   получаем количество колонок в таблице


                int rn = Convert.ToInt32(CL1.ExecuteScalar());                                                           // считаем количество рядов
                int ColCount = Convert.ToInt32(CL4.ExecuteScalar());

                if (rn > 0)
                {
                    DataTable tbl0 = new DataTable();                             // заполняем таблицу ведущей колонкой
                    OracleDataAdapter adapter = new OracleDataAdapter(CL);
                    adapter.Fill(tbl0);

                    DataTable tbl4 = new DataTable();                             // заполняем таблицу именами колонкой
                    OracleDataAdapter adapter4 = new OracleDataAdapter(CL3);
                    adapter4.Fill(tbl4);

                    for (int k = 0; k < rn; k++)
                    {
                        OracleCommand CL2 = new OracleCommand("select * from " + TableName + " where " + Col + "='" + tbl0.Rows[k][0].ToString() + "'", ConnectionToClearing);  // оригинальный ряд
                        DataTable tbl1 = new DataTable();
                        OracleDataAdapter adapter1 = new OracleDataAdapter(CL2);
                        adapter1.Fill(tbl1);

                        OracleCommand TR = new OracleCommand("select * from " + TableName + " where " + Col + "='" + tbl0.Rows[k][0].ToString() + "'", ConnectionToTR);  // изменяемый ряд
                        DataTable tbl2 = new DataTable();
                        OracleDataAdapter adapter3 = new OracleDataAdapter(TR);
                        adapter3.Fill(tbl2);

                        if (RowMatching(tbl1, tbl2, TableName, ColCount, tbl4) != "matched")
                        {
                            OracleCommand TR1 = new OracleCommand(RowMatching(tbl1, tbl2, TableName, ColCount, tbl4), ConnectionToTR);
                            TR1.ExecuteNonQuery();

                        }

                        //Console.WriteLine(InSert( tbl1,TableName, ColCount));

                    }
                }
            }catch(Exception e)
            {
                File.AppendAllText(lDate, DateTime.Now.ToString("F") + "Неудачно " + TableName + " \n" + e.Message.ToString() + "\n");
            }
        }

        public String IdCol(OracleConnection ConnectionTo, String TableName)   // получаем имя ведущей колонки
        {
            OracleCommand CL = new OracleCommand("select column_name from user_tab_columns where table_name = '"+TableName+"' and column_id = 1 " , ConnectionTo);
            String col = CL.ExecuteScalar().ToString();
            return col;
        }

        public String RowMatching(DataTable tbl1, DataTable tbl2,String TableName, int ColCount, DataTable ColTable)
        {
            String query = "matched";
            
            if (tbl2.Rows.Count.ToString() != "0")
            {

                for (int k = 0; k < ColCount; k++)
                {
                    if (tbl1.Rows[0][k].ToString() == tbl2.Rows[0][k].ToString())
                    {

                    }
                    else
                    {
                        query = UpDate(ColTable, tbl1, TableName, ColCount);
                        break;
                    }
                }
                return query;
            }
            else
            {
                query = InSert(tbl1, TableName, ColCount);
                return query;
            }
        }

        public String UpDate(DataTable ColTable,DataTable ValTable , String TableName, int cc)      // формируем update
        {
            String up = "update " + TableName + " set ";
            for (int k=1; k<cc; k++)
            {
                up = up + ColTable.Rows[k][0].ToString()+"="+CellConverter(ValTable, 0, k);
                if (k < (cc - 1))
                {
                    up = up + ",";
                }
            }
            up = up + " where " + ColTable.Rows[0][0].ToString() + "=" + CellConverter(ValTable, 0, 0);

            return up;
        }

        public String InSert(DataTable ValTable, String TableName, int cc)                  // формируем insert
        {
            String ins = "insert into " + TableName + " values (";
            for (int k = 0; k < cc; k++)
            {
                ins = ins + CellConverter(ValTable, 0, k);
                if (k < (cc - 1))
                {
                    ins = ins + ",";
                }
            }
            ins = ins + ")";
                return ins;
        }

        public String CellConverter(DataTable tbl, int i, int k)
        {
            String shit = tbl.Columns[k].DataType.ToString();
            String val = "null";
            if (shit == "System.DateTime")
            {
                if (tbl.Rows[i][k].ToString() == "") { val = "null"; }
                else { val = "to_date('" + tbl.Rows[i][k].ToString() + "', 'dd-mm-yyyy hh24:mi:ss')"; }
            }
            else
            {
                if (tbl.Rows[i][k].ToString() == "") { val = "null"; }
                else { val = "'" + tbl.Rows[i][k].ToString() + "'"; }
            }
            return val;
        }

    }
}
